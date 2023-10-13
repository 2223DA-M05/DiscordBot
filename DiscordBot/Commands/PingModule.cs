using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

namespace DiscordBot.Commands
{
    public class PingModule : BaseCommandModule
    {
        [Command("ping")]
        public async Task PingCommand(CommandContext context)
        {
            await context.RespondAsync("Pong!");
        }

        [Command("greet")]
        public async Task GreetCommand(CommandContext context, DiscordMember member)
        {
            await member.SendMessageAsync($"Hola {member.Mention}, l'usuari {context.Member.Mention} et saluda!");
        }
    }
}
