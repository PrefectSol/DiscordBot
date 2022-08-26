namespace DiscordBot.Common {
    /// <summary>
    /// Themes for embed.
    /// </summary>
    
    using Discord;

    internal class BotEmbedBuilder : EmbedBuilder {
        public BotEmbedBuilder() {
            this.WithColor(new Color(255, 138, 0));
            this.WithDescription("Тут пока ничего нет.");
        }
    }
}