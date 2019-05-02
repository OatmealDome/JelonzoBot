using BcatBotFramework.Core;
using JelonzoBot.Core;

namespace JelonzoBot.Scheduler.Job
{
    public class JelonzoBotShutdown : Shutdown
    {
        protected override void ShutdownAppSpecificItems()
        {
            // Shutdown the FileCache
            FileCache.Dispose();
        }

    }
}