namespace DiscordBot.Services {
    using System.Reflection;
    using Discord;
    using Discord.Addons.Hosting;
    using Discord.Commands;
    using Discord.WebSocket;
    using DiscordBot.Common;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;

    public class DiscordCommandHandler : DiscordClientService {
        private readonly IServiceProvider provider;
        private readonly DiscordSocketClient client;
        private readonly CommandService service;
        private readonly IConfiguration configuration;

        public DiscordCommandHandler(IServiceProvider provider,
                                DiscordSocketClient client, 
                                CommandService service, 
                                IConfiguration configuration,
                                ILogger<DiscordCommandHandler> logger) : base (client, logger) {
            this.provider = provider;
            this.client = client;
            this.service = service;
            this.configuration = configuration;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
            this.client.MessageReceived += this.OnMessageReceived;
            this.service.CommandExecuted += this.OnCommandExecuted;

            await this.service.AddModulesAsync(Assembly.GetEntryAssembly(), this.provider);
        }

        private async Task OnCommandExecuted(Optional<CommandInfo> commandInfo,
                                            ICommandContext commandContext,
                                            IResult result) {
            var reason = result.ErrorReason;
            if (result.IsSuccess)
                return;

            var embed = new EmbedBuilder()
                .WithColor(new Color(190, 0, 26))
                .WithDescription(reason)
                .Build();
            var message = await commandContext.Channel.SendMessageAsync(null, false, embed);
            ConsoleReport.Error($"ERROR: {reason}");

            await Task.Delay(5000);
            await message.DeleteAsync();
        }

        private async Task OnMessageReceived(SocketMessage socketMessage) {
            if (socketMessage is not SocketUserMessage message)
                return;

            if (message.Source != MessageSource.User)
                return;

            var prefixPosition = 0;
            if (!message.HasStringPrefix(this.configuration["Prefix"], ref prefixPosition)
                && !message.HasMentionPrefix(this.client.CurrentUser, ref prefixPosition))
                return;

            var context = new SocketCommandContext(this.client, message);
            await this.service.ExecuteAsync(context, prefixPosition, this.provider);
        }
    }
}