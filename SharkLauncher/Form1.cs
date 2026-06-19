using SharkLauncher.objects;
using System.Diagnostics;
using System.Net.Security;

namespace SharkLauncher
{
    public partial class Form1 : Form
    {
        public List<DiscordGuild> guilds = [];
        public DiscordUser loginUser;

        public Form1()
        {
            InitializeComponent();
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                DiscordLoginResult user = await new Oauth2().LoginUser();

                userName.Text = user.User.Username;
                userId.Text = user.User.Id;

                if (user.User.AvatarUrl != null)
                {
                    using var http = new HttpClient();
                    var bytes = await http.GetByteArrayAsync(user.User.AvatarUrl);

                    using var ms = new MemoryStream(bytes);
                    avatarImage.SizeMode = PictureBoxSizeMode.Zoom;
                    avatarImage.Image = Image.FromStream(ms);
                }
                loginUser = user.User;
                guilds = user.Guilds.ToList();

                loginButton.Text = "🔑再ログイン";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ログインエラー");
            }
        }

        private async void loginButton_Click(object sender, EventArgs e)
        {
            try
            {
                DiscordLoginResult user = await new Oauth2().LoginUser();

                userName.Text = user.User.Username;
                userId.Text = user.User.Id;

                if (user.User.AvatarUrl != null)
                {
                    using var http = new HttpClient();
                    var bytes = await http.GetByteArrayAsync(user.User.AvatarUrl);

                    using var ms = new MemoryStream(bytes);
                    avatarImage.SizeMode = PictureBoxSizeMode.Zoom;
                    avatarImage.Image = Image.FromStream(ms);
                }
                loginUser = user.User;
                guilds = user.Guilds.ToList();

                loginButton.Text = "🔑再ログイン";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ログインエラー");
            }
        }

        private void inviteSharkBot_Click(object sender, EventArgs e)
        {
            if (guilds.Count == 0)
            {
                MessageBox.Show("サーバーを取得するにはログインする必要があります。", "サーバーが見つかりません。");
                return;
            }
            new inviteSharkBot(guilds).ShowDialog();
        }

        private void openOmikuji_Click(object sender, EventArgs e)
        {
            if (loginUser == null)
            {
                MessageBox.Show("この機能の仕様にはログインが必要です。", "ログインが必要です。");
                return;
            }
            new Omikuji(loginUser).ShowDialog();
        }
    }
}
