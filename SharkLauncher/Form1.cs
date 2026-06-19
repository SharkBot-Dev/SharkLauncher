using SharkLauncher.objects;
using System.Diagnostics;
using System.Net.Security;

namespace SharkLauncher
{
    public partial class Form1 : Form
    {
        public List<DiscordGuild> guilds = [];

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

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
            new inviteSharkBot(guilds).ShowDialog();
        }
    }
}
