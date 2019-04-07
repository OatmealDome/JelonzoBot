using System.Threading.Tasks;
using BcatBotFramework.Scheduler;
using BcatBotFramework.Social.Discord.Precondition;
using Discord;
using Discord.Commands;
using JelonzoBot.Core;
using JelonzoBot.Scheduler.Job;

namespace JelonzoBot.Social.Discord.Command
{
    public class AdminCommands : ModuleBase<SocketCommandContext>
    {
        [RequireBotAdministratorPrecondition]
        [Command("shutdown"), Summary("Shutdown")]
        public async Task Execute()
        {
            await Context.Channel.SendMessageAsync("**[Admin]** OK, shutting down");

            await QuartzScheduler.ScheduleJob<JelonzoBotShutdownJob>("Request");
        }

    }
}