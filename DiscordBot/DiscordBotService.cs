using DiscordBot.Commands;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Enums;
using DSharpPlus.Interactivity.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace DiscordBot
{
    public sealed class DiscordBotService : IHostedService
    {
        private readonly IConfiguration configuration;
        private readonly DiscordClient discordClient;

        public DiscordBotService(IConfiguration configuration)
        {
            this.configuration = configuration;
            this.discordClient = new DiscordClient(new DiscordConfiguration()
            {
                Token = configuration["DiscordToken"],
                TokenType = TokenType.Bot,
                Intents = DiscordIntents.AllUnprivileged | DiscordIntents.MessageContents
            });
            var commands = this.discordClient.UseCommandsNext(new CommandsNextConfiguration()
            {
                StringPrefixes = new[] { "!" }
            });

            this.discordClient.UseInteractivity(new InteractivityConfiguration()
            {
                PollBehaviour = PollBehaviour.KeepEmojis,
                Timeout = TimeSpan.FromSeconds(30)
            });

            commands.RegisterCommands<PingModule>();
            commands.RegisterCommands<RockPaperScisorsModule>();
            commands.RegisterCommands<WereWolfModule>();
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await discordClient.ConnectAsync();
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await discordClient.DisconnectAsync();
        }
    }
}
