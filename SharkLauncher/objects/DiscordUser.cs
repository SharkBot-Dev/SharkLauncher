namespace SharkLauncher.objects
{
    public class DiscordUser
    {
        public string Id { get; set; } = "";
        public string Username { get; set; } = "";
        public string global_name { get; set; } = "";
        public string? Avatar { get; set; }

        public string AvatarUrl =>
            Avatar == null
                ? "https://cdn.discordapp.com/embed/avatars/0.png"
                : $"https://cdn.discordapp.com/avatars/{Id}/{Avatar}.png?size=128";
    }
}
