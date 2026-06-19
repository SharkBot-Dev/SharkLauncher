using SharkLauncher.objects;

namespace SharkLauncher
{
    public partial class Omikuji : Form
    {
        public string UserGlobalName { get; } = "";
        public string UserAvatarUrl { get; } = "";

        private static readonly HttpClient _httpClient = new HttpClient();

        public Omikuji()
        {
            InitializeComponent();
        }

        public Omikuji(DiscordUser user) : this()
        {
            UserGlobalName = user.global_name ?? "Unknown";
            UserAvatarUrl = user.AvatarUrl ?? "";
        }

        static string GetWeightedRandom(string[] items, int[] weights)
        {
            Random rand = new Random();
            int totalWeight = weights.Sum();
            int randomNumber = rand.Next(0, totalWeight);

            int currentSum = 0;
            for (int i = 0; i < items.Length; i++)
            {
                currentSum += weights[i];
                if (randomNumber < currentSum)
                {
                    return items[i];
                }
            }

            return items[0];
        }

        public async Task GenerateOmikujiAsync()
        {
            try
            {
                string encodedName = Uri.EscapeDataString(UserGlobalName);
                string encodedUrl = Uri.EscapeDataString(UserAvatarUrl);
                string[] results = { "大吉", "中吉", "小吉", "吉", "末吉", "凶", "大凶" };
                int[] weights = { 5, 15, 20, 30, 20, 7, 3 };
                string kuzi = GetWeightedRandom(results, weights);
                int currentHour = DateTime.Now.Hour;
                string url = $"https://api.sharkbot.xyz/images/omikuji?text={kuzi}&username={encodedName}&avatarUrl={encodedUrl}&hour={currentHour}";
                var bytes = await _httpClient.GetByteArrayAsync(url);
                using var ms = new MemoryStream(bytes);

                omikujiResult.SizeMode = PictureBoxSizeMode.Zoom;

                omikujiResult.Image?.Dispose();
                omikujiResult.Image = Image.FromStream(ms);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"おみくじの取得に失敗しました: {ex.Message}", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void Omikuji_Load(object sender, EventArgs e)
        {
            await GenerateOmikujiAsync();
        }

        private async void reOpenButton_Click(object sender, EventArgs e)
        {
            await GenerateOmikujiAsync();
        }
    }
}