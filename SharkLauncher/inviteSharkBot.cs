using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Reflection.Emit;
using System.Text;
using System.Windows.Forms;

namespace SharkLauncher
{
    public partial class inviteSharkBot : Form
    {
        public string botInviteUrl = "https://discord.com/oauth2/authorize?client_id=1322100616369147924&guild_id=";

        public inviteSharkBot()
        {
            InitializeComponent();
        }

        private void inviteSharkBot_Load(object sender, EventArgs e)
        {

        }

        public inviteSharkBot(List<DiscordGuild> guilds)
        {
            InitializeComponent();

            foreach (var guild in guilds)
            {
                selectGuilds.Items.Add($"{guild.Name} ({guild.Id})");
            }
        }

        private void invite_Click(object sender, EventArgs e)
        {
            if (selectGuilds.SelectedItem != null)
            {
                string selectedText = selectGuilds.SelectedItem.ToString();
                string guildId = selectedText.Split("(")[1].Split(")")[0];
                string botInviteUrlGuild = botInviteUrl + guildId;
                Process.Start(new ProcessStartInfo(botInviteUrlGuild) { UseShellExecute = true });
            }
        }

        private void cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
