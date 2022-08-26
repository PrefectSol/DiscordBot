namespace DiscordBot.Modules {
    using System.Diagnostics;
    using Discord;
    using Discord.Commands;
    using Discord.WebSocket;
    using DiscordBot.Common;

    public class Moderation : ModuleBase<SocketCommandContext> {
        [RequireUserPermission(GuildPermission.Administrator)]
        [Command("Clear")]
        [Alias("c")]

        public async Task ClearAsync(int count = 1) {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            var messages = await this.Context.Channel.GetMessagesAsync(count + 1).FlattenAsync();
            await ((SocketTextChannel)this.Context.Channel).DeleteMessagesAsync(messages);

            var embed = new BotEmbedBuilder()
                .WithDescription($"Deleted messages: {messages.Count()}")
                .Build();

            var embedMessage = await this.ReplyAsync(embed: embed);
            await Task.Delay(1500);
            await embedMessage.DeleteAsync();

            sw.Stop();

            ConsoleReport.Report($"Deleted {count + 1} messages in {sw.ElapsedMilliseconds} ms");
        }
    }
}