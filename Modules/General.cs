namespace DiscordBot.Modules {
    using Discord;
    using Discord.Commands;
    using Discord.WebSocket;
    using DiscordBot.Common;
    using DiscordBot.Services;

    public class General : ModuleBase<SocketCommandContext> {
        [Command("help")]
        [Alias("h")]
 
        public async Task HelpAsync(string? user = null) {
            if (user == "dev") {
                var devEmbed = new BotEmbedBuilder()
                    .WithTitle("Commands:")
                    .WithDescription("")
                    .AddField("-RegConsole (-console)", "Entering on behalf of the bot", false)
                    .Build();
            
                await this.ReplyAsync(embed: devEmbed);
                return;
            }

            if (user == "admin") {
                var adminEmbed = new BotEmbedBuilder()
                    .WithTitle("Commands:")
                    .WithDescription("")
                    .AddField("-Clear (-c)", "Deletes n messages", false)
                    .AddField("-RegConsole (-console)", "Entering on behalf of the bot", false)
                    .Build();
            
                await this.ReplyAsync(embed: adminEmbed);
                return;
            }

            var embed = new BotEmbedBuilder()
                .WithTitle("Commands: ")
                .Build();
            
            await this.ReplyAsync(embed: embed);
        }

        [Command("RegConsole")]
        [Alias("console")]
        public async Task RegConsoleAsync() {
            var messages = await this.Context.Channel.GetMessagesAsync(1).FlattenAsync();
            await ((SocketTextChannel)this.Context.Channel).DeleteMessagesAsync(messages);
            await Task.Run(() => ConsoleCommandHandler.InputCommandAsync(this.Context.Channel));

            ConsoleReport.Report($"The console is registered: '{this.Context.Channel}'");
        }

     }
}