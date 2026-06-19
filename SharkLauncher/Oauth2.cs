using SharkLauncher.objects;
using System;
using System.Collections.Generic;
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

        public async Task<DiscordLoginResult> LoginUser()
        {
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

            string tokenJson = await tokenRes.Content.ReadAsStringAsync();
            Console.WriteLine(tokenJson);

            var token = JsonDocument.Parse(tokenJson)
                .RootElement.GetProperty("access_token")
                .GetString();

            var req = new HttpRequestMessage(HttpMethod.Get, "https://discord.com/api/users/@me");
            req.Headers.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            string userJson = await (await http.SendAsync(req)).Content.ReadAsStringAsync();

            var user = JsonSerializer.Deserialize<DiscordUser>(
                userJson,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );

            if (user == null)
                throw new Exception("ユーザー情報の取得に失敗しました");

            var guildReq = new HttpRequestMessage(
                HttpMethod.Get,
                "https://discord.com/api/users/@me/guilds?with_counts=true"
            );

            guildReq.Headers.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            string guildJson = await (await http.SendAsync(guildReq)).Content.ReadAsStringAsync();

            var guilds = JsonSerializer.Deserialize<List<DiscordGuild>>(
                guildJson,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            ) ?? new List<DiscordGuild>();

            return new DiscordLoginResult
            {
                User = user,
                Guilds = guilds
            };
        }

        static string Base64Url(byte[] bytes)
        {
            return Convert.ToBase64String(bytes)
                .TrimEnd('=')
                .Replace('+', '-')
                .Replace('/', '_');
        }
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
