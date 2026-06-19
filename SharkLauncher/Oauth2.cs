using SharkLauncher.objects;
using System.Diagnostics;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace SharkLauncher
{
    internal class Oauth2
    {
        const string ClientId = "1489903711093395567";
        const string RedirectUri = "http://127.0.0.1:53123/callback/";
        const string Scope = "identify guilds relationships.read";

        private static readonly string AppDataDir = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
            "SharkBotLauncher"
        );
        private static readonly string TokenFilePath = Path.Combine(AppDataDir, "sharkbot_launcher.json");

        public async Task<DiscordLoginResult?> LoginUser()
        {
            var savedToken = LoadToken();

            if (savedToken != null)
            {
                if (DateTime.UtcNow < savedToken.ExpiresAt.AddMinutes(-5))
                {
                    try
                    {
                        return await FetchDiscordData(savedToken.AccessToken);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"既存トークンでのデータ取得に失敗（再認証します）: {ex.Message}");
                        Logout();
                    }
                }
                else
                {
                    Console.WriteLine("トークンの期限が切れているため、再ログインが必要です。");
                    Logout();
                }
            }

            string verifier = Base64Url(RandomNumberGenerator.GetBytes(64));
            string challenge = Base64Url(SHA256.HashData(Encoding.ASCII.GetBytes(verifier)));
            string state = Base64Url(RandomNumberGenerator.GetBytes(32));

            string authUrl =
                "https://discord.com/oauth2/authorize" +
                "?response_type=code" +
                $"&client_id={ClientId}" +
                $"&redirect_uri={Uri.EscapeDataString(RedirectUri)}" +
                $"&scope={Uri.EscapeDataString(Scope)}" +
                $"&state={state}" +
                "&code_challenge_method=S256" +
                $"&code_challenge={challenge}";

            using var listener = new HttpListener();
            listener.Prefixes.Add(RedirectUri);
            listener.Start();

            Process.Start(new ProcessStartInfo(authUrl) { UseShellExecute = true });

            var ctx = await listener.GetContextAsync();
            var query = ctx.Request.QueryString;

            string? code = query["code"];
            string? returnedState = query["state"];

            byte[] html = Encoding.UTF8.GetBytes("認証完了しました。この画面を閉じてOKです。");
            ctx.Response.ContentType = "text/plain; charset=utf-8";
            ctx.Response.OutputStream.Write(html);
            ctx.Response.Close();

            if (returnedState != state) throw new Exception("state mismatch");
            if (string.IsNullOrEmpty(code)) throw new Exception("code not found");

            using var http = new HttpClient();

            var tokenRes = await http.PostAsync(
                "https://discord.com/api/oauth2/token",
                new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    ["client_id"] = ClientId,
                    ["grant_type"] = "authorization_code",
                    ["code"] = code,
                    ["redirect_uri"] = RedirectUri,
                    ["code_verifier"] = verifier
                })
            );

            if (!tokenRes.IsSuccessStatusCode)
                throw new Exception($"トークンの取得に失敗しました: {tokenRes.StatusCode}");

            string tokenJson = await tokenRes.Content.ReadAsStringAsync();

            using var doc = JsonDocument.Parse(tokenJson);
            var root = doc.RootElement;

            string accessToken = root.GetProperty("access_token").GetString() ?? throw new Exception("access_token not found");
            int expiresIn = root.GetProperty("expires_in").GetInt32();
            string? refreshToken = root.TryGetProperty("refresh_token", out var refTokenProp) ? refTokenProp.GetString() : null;

            var tokenInfo = new TokenInfo
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                ExpiresAt = DateTime.UtcNow.AddSeconds(expiresIn)
            };
            SaveToken(tokenInfo);

            return await FetchDiscordData(accessToken);
        }

        private async Task<DiscordLoginResult> FetchDiscordData(string accessToken)
        {
            using var http = new HttpClient();

            var req = new HttpRequestMessage(HttpMethod.Get, "https://discord.com/api/users/@me");
            req.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

            var userRes = await http.SendAsync(req);
            if (!userRes.IsSuccessStatusCode)
                throw new Exception("ユーザー情報の取得に失敗しました（トークンが無効な可能性があります）");

            string userJson = await userRes.Content.ReadAsStringAsync();
            var user = JsonSerializer.Deserialize<DiscordUser>(userJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (user == null)
                throw new Exception("ユーザー情報のデシリアライズに失敗しました");

            var guildReq = new HttpRequestMessage(HttpMethod.Get, "https://discord.com/api/users/@me/guilds?with_counts=true");
            guildReq.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

            string guildJson = await (await http.SendAsync(guildReq)).Content.ReadAsStringAsync();
            var guilds = JsonSerializer.Deserialize<List<DiscordGuild>>(guildJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                         ?? new List<DiscordGuild>();

            return new DiscordLoginResult
            {
                User = user,
                Guilds = guilds
            };
        }
        public void Logout()
        {
            if (File.Exists(TokenFilePath))
            {
                File.Delete(TokenFilePath);
            }
        }

        private void SaveToken(TokenInfo tokenInfo)
        {
            try
            {
                if (!Directory.Exists(AppDataDir))
                {
                    Directory.CreateDirectory(AppDataDir);
                }

                string json = JsonSerializer.Serialize(tokenInfo, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(TokenFilePath, json, Encoding.UTF8);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"トークンの保存に失敗: {ex.Message}");
            }
        }

        private TokenInfo? LoadToken()
        {
            if (!File.Exists(TokenFilePath)) return null;

            try
            {
                string json = File.ReadAllText(TokenFilePath, Encoding.UTF8);
                return JsonSerializer.Deserialize<TokenInfo>(json);
            }
            catch
            {
                return null; 
            }
        }

        static string Base64Url(byte[] bytes)
        {
            return Convert.ToBase64String(bytes)
                .TrimEnd('=')
                .Replace('+', '-')
                .Replace('/', '_');
        }
    }

    public class TokenInfo
    {
        public string AccessToken { get; set; } = "";
        public string? RefreshToken { get; set; }
        public DateTime ExpiresAt { get; set; }
    }

    public class DiscordLoginResult
    {
        public DiscordUser User { get; set; } = null!;
        public List<DiscordGuild> Guilds { get; set; } = new();
    }

    public class DiscordGuild
    {
        public string Id { get; set; } = "";
        public string Name { get; set; } = "";
        public string? Icon { get; set; }
        public bool Owner { get; set; }
        public JsonElement Permissions { get; set; }

        public string? IconUrl =>
            Icon == null ? null : $"https://cdn.discordapp.com/icons/{Id}/{Icon}.png";
    }
}
