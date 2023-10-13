using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.Commands
{
    public class WereWolfModule : BaseCommandModule
    {
        [Command("wolf"), Aliases("wf")]

        public async Task WereWolfCommand(CommandContext ctx)
        {
            var emoji = DiscordEmoji.FromName(ctx.Client, ":white_check_mark:");
            var message = await ctx.RespondAsync($"L'usuari {ctx.User.Mention} " +
                $"et repta a jugar a l'home llop, acceptes? Tens 30 segons per acceptar amb {emoji}.");
            await message.CreateReactionAsync(emoji);
            var reaction = (await message.CollectReactionsAsync())
                .First(r => r.Emoji.Equals(emoji));
            
            var strBuilder = new StringBuilder("Els usuaris que juguen són: ");

            foreach ( var user in reaction.Users)
            {
                strBuilder.Append(user.Mention + "\n");
            }

            await ctx.RespondAsync(strBuilder.ToString());

            string[] roles = { "llop", "poble" };

            var random = new Random();

            var users =  new Dictionary<DiscordUser, string>();

            foreach (var user in reaction.Users)
            {
                var value = random.Next(0, 1);
                var role = roles[value];
                
                if(!users.ContainsValue("llop"))
                {
                    users.Add(user, role);
                }
                else
                {
                    role = roles[1];
                    users.Add(user, role);
                }

                await message.RespondAsync(users[user]);
            }
        }
    }
}
