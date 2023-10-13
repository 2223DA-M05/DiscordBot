using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus;
using DSharpPlus.Interactivity.Extensions;
using DSharpPlus.Interactivity;
using DSharpPlus.EventArgs;
using System.Text;

namespace DiscordBot.Commands
{
    public class RockPaperScisorsModule : BaseCommandModule
    {
        [Command("rockpaperscisors"), Aliases("rps")]
        public async Task RockPaperScisorsCommand(CommandContext ctx, DiscordMember member)
        {
            var emoji = DiscordEmoji.FromName(ctx.Client, ":white_check_mark:");
            var message = await ctx.RespondAsync($"{member.Mention}, l'usuari {ctx.User.Mention} et repta a pedra paper i tisores, acceptes? Tens 30 segons per acceptar amb {emoji}.");
            await message.CreateReactionAsync(emoji);
            var result = await message.WaitForReactionAsync(member, emoji);
            if(result.TimedOut)
            {
                await message.RespondAsync("S'ha acabat el temps");
            } else
            {
                await message.RespondAsync($"L'usuari {member.Mention} ha acceptat el repte");
                var builder = new DiscordMessageBuilder()
                    .WithContent("Selecciona pedra, paper o tisores:")
                    .AddComponents(new DiscordComponent[]
                    {
                        new DiscordButtonComponent(ButtonStyle.Primary, "pedra", null, false, new DiscordComponentEmoji(DiscordEmoji.FromName(ctx.Client, ":rock:"))),
                        new DiscordButtonComponent(ButtonStyle.Primary, "paper", null, false, new DiscordComponentEmoji(DiscordEmoji.FromName(ctx.Client, ":roll_of_paper:"))),
                        new DiscordButtonComponent(ButtonStyle.Primary, "tisores", null, false, new DiscordComponentEmoji(DiscordEmoji.FromName(ctx.Client, ":scissors:"))),
                    });
                var m2 = await builder.SendAsync(ctx.Channel);
                var results = await Task.WhenAll<InteractivityResult<ComponentInteractionCreateEventArgs>>(m2.WaitForButtonAsync(member), m2.WaitForButtonAsync(ctx.User));
                if (results.Any(r => r.TimedOut))
                {
                    await m2.RespondAsync("Un dels jugadors no ha jugat a temps.");
                    return;
                }
                var sb = new StringBuilder($"{member.Mention} ha triat {results[0].Result.Id}\n{ctx.User.Mention} ha triat {results[1].Result.Id}\n");
                switch (results[0].Result.Id)
                {
                    case "pedra":
                        switch(results[1].Result.Id)
                        {
                            case "pedra":
                                sb.Append("Empat");
                                break;
                            case "paper":
                                sb.Append($"El guanyador és {ctx.User.Mention}");
                                break;
                            case "tisores":
                                sb.Append($"El guanyador és {member.Mention}");
                                break;
                        }
                        break;
                    case "paper":
                        switch (results[1].Result.Id)
                        {
                            case "paper":
                                sb.Append("Empat");
                                break;
                            case "tisores":
                                sb.Append($"El guanyador és {ctx.User.Mention}");
                                break;
                            case "pedra":
                                sb.Append($"El guanyador és {member.Mention}");
                                break;
                        }
                        break;
                    case "tisores":
                        switch (results[1].Result.Id)
                        {
                            case "tisores":
                                sb.Append("Empat");
                                break;
                            case "pedra":
                                sb.Append($"El guanyador és {ctx.User.Mention}");
                                break;
                            case "paper":
                                sb.Append($"El guanyador és {member.Mention}");
                                break;
                        }
                        break;
                }
                await m2.RespondAsync(sb.ToString());

            }
        }
    }
}
