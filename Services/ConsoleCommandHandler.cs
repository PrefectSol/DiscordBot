namespace DiscordBot.Services {
    using Discord.Commands;
    using Discord.WebSocket;
    using DiscordBot.Common;

    public class ConsoleCommandHandler : ModuleBase<SocketCommandContext> {
        private static async void InputCommand(ISocketMessageChannel channelCommand) {
            while (true) {
                string? text = Console.ReadLine();
                var embed = new BotEmbedBuilder()
                    .WithDescription(text)
                    .Build();
                await channelCommand.SendMessageAsync(embed: embed);
            }
        }

        public static async void InputCommandAsync(ISocketMessageChannel channelCommand)
            => await Task.Run(() => InputCommand(channelCommand));
    }
}