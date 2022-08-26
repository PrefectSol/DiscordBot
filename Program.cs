namespace DiscordBot {
    using Discord;
    using Discord.Addons.Hosting;
    using Discord.Commands;
    using Discord.WebSocket;
    using DiscordBot.Services;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;

    internal class Program {

        private static async Task Main(string[] args) {
            var builder = new HostBuilder()
                .ConfigureAppConfiguration(x => {
                        var configuration = new ConfigurationBuilder()
                            .SetBasePath(Directory.GetCurrentDirectory())
                            .AddJsonFile("appsettings.json", false, true)
                            .Build();
                        x.AddConfiguration(configuration); 
                    })

                .ConfigureLogging(x => {
                        x.AddConsole();
                        x.SetMinimumLevel(LogLevel.Debug);
                    })

                .ConfigureDiscordHost((context, config) => {
                    config.SocketConfig = new DiscordSocketConfig {
                        LogLevel = LogSeverity.Debug,
                        AlwaysDownloadUsers = false,
                        MessageCacheSize = 10000,
                    };
                    config.Token = context.Configuration["Token"];
                })

                .UseCommandService((context, config) => {
                    config.CaseSensitiveCommands = false;
                    config.LogLevel = LogSeverity.Debug;
                    config.DefaultRunMode = RunMode.Async;
                })
                
                .ConfigureServices((context, services) => {
                    services.AddHostedService<DiscordCommandHandler>();
                })

                .UseConsoleLifetime();

            var host = builder.Build();
            using (host) { await host.RunAsync(); }
        }
    }
}