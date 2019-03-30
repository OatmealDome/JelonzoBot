using System.Threading.Tasks;
using SmashBcatDetector.Scheduler.Job;

namespace JelonzoBot.Scheduler.Job
{
    public class JelonzoBotRecurringHousekeepingJob : RecurringHousekeepingJob
    {
        protected override Task RunAppSpecificRecurringTasks()
        {
            // TODO
            return Task.FromResult(0);
        }
        
    }
}