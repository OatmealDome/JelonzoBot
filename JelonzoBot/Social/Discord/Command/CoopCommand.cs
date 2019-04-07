using System.Threading.Tasks;
using BcatBotFramework.Scheduler;
using BcatBotFramework.Social.Discord;
using Discord;
using Discord.Commands;
using JelonzoBot.Social.Discord.Interactive;
using Nintendo.Bcat;

namespace JelonzoBot.Social.Discord.Command
{
    public class CoopCommand : ModuleBase<SocketCommandContext>
    {
        [Command("sr"), Summary("Shows Salmon Run data")]
        public async Task Execute(string languageStr = null)
        {
            // Get the language
            Language language = DiscordUtil.GetDefaultLanguage(Context.Guild, languageStr);

            // Send the interactive message
            await DiscordBot.SendInteractiveMessageAsync(Context.Channel, new CoopMessage(Context.User, language));
        }

    }
}