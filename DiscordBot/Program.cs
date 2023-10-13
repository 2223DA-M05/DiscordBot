using DiscordBot.Commands;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Interactivity.Extensions;
using DSharpPlus.Interactivity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DiscordBot
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var hostBuilder = Host.CreateApplicationBuilder(args);
            hostBuilder.Configuration.AddUserSecrets<Program>();
            using IHost host = hostBuilder.Build();

            var config = host.Services.GetRequiredService<IConfiguration>();

            var discord = new DiscordClient(new DiscordConfiguration()
            {
                Token = config["DiscordToken"],
                TokenType = TokenType.Bot,
                Intents = DiscordIntents.AllUnprivileged | DiscordIntents.MessageContents
            });

            var commands = discord.UseCommandsNext(new CommandsNextConfiguration()
            {
                StringPrefixes = new[] { "!" }
            });

            discord.UseInteractivity(new InteractivityConfiguration()
            {
                PollBehaviour = DSharpPlus.Interactivity.Enums.PollBehaviour.KeepEmojis,
                Timeout = TimeSpan.FromSeconds(30)
            });

            commands.RegisterCommands<PingModule>();
            commands.RegisterCommands<RockPaperScisorsModule>();
            commands.RegisterCommands<WereWolfModule>();

            await discord.ConnectAsync();
            await host.RunAsync();
            await Task.Delay(-1);
        }
    }
}