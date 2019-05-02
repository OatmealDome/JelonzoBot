using BcatBotFramework.Core.Config;

namespace JelonzoBot.Core.Config
{
    public class RomConfig : ISubConfiguration
    {
        public string BaseNcaPath
        {
            get;
            set;
        }

        public string UpdateNcaPath
        {
            get;
            set;
        }

        public void SetDefaults()
        {
            BaseNcaPath = "/home/oatmealdome/base.nca";
            UpdateNcaPath = "/home/oatmealdome/update.nca";
        }
        
    }
}