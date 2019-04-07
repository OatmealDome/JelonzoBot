using System.Threading.Tasks;
using BcatBotFramework.Internationalization;
using Discord;
using Discord.Commands;
using JelonzoBot.Core;
using Nintendo.Bcat;
using Nintendo.Blitz;
using Nintendo.Blitz.Bcat.Festival;

namespace JelonzoBot.Social.Discord.Command
{
    public class DebugCommand : ModuleBase<SocketCommandContext>
    {
        [Command("d"), Summary("debug")]
        public async Task Execute()
        {
            await Context.Channel.SendMessageAsync("**[Debug]** debug message");
        }

    }
}