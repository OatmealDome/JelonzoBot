using BcatBotFramework.Core;
using JelonzoBot.Blitz;
using JelonzoBot.Core;

namespace JelonzoBot.Scheduler.Job
{
    public class JelonzoBotShutdown : Shutdown
    {
        protected override void ShutdownAppSpecificItems()
        {
            // Shutdown the RomReasourceLoader
            RomResourceLoader.Dispose();
            
            // Shutdown the FileCache
            FileCache.Dispose();
        }

    }
}