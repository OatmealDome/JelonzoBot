using System.Threading.Tasks;
using BcatBotFramework.Core.Config;
using BcatBotFramework.Scheduler;
using BcatBotFramework.Scheduler.Job;
using JelonzoBot.Blitz;
using JelonzoBot.Blitz.Internationalization;
using JelonzoBot.Core;

namespace JelonzoBot.Scheduler.Job
{
    public class JelonzoBotBootHousekeepingJob : BootHousekeepingJob
    {
        protected override Task RunAppSpecificBootTasks()
        {
            // Initialize the FileCache if first run is complete
            if (Configuration.LoadedConfiguration.FirstRunCompleted)
            {
                FileCache.Initialize();
            }

            // Initialize the RomResourceLoader
            RomResourceLoader.Initialize();

            // Initialize the BlitzLocalizer
            BlitzLocalizer.Initialize();

            // No async tasks to wait on here
            return Task.FromResult(0);
        }

        protected override async Task SchedulePostBootJobs()
        {
            if (!Configuration.LoadedConfiguration.FirstRunCompleted)
            {
                await QuartzScheduler.ScheduleJob<BcatFirstRunJob>("Normal");
            }
            else if (Configuration.LoadedConfiguration.IsProduction) 
            {
                await QuartzScheduler.ScheduleJob<BcatCheckerJob>("Normal", Configuration.LoadedConfiguration.JobSchedules["Bcat"]);
            }
        }
        
    }
}