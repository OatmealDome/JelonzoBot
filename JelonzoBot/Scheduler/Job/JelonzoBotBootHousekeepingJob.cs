using System.Threading.Tasks;
using BcatBotFramework.Scheduler.Job;

namespace JelonzoBot.Scheduler.Job
{
    public class JelonzoBotBootHousekeepingJob : BootHousekeepingJob
    {
        protected override Task RunAppSpecificBootTasks()
        {
            // TODO
            return Task.FromResult(0);
        }

        protected override Task SchedulePostBootJobs()
        {
            // TODO
            return Task.FromResult(0);
        }
        
    }
}