using System.Collections.Generic;
using System.IO;
using System.Linq;
using Nintendo.Archive;
using Nintendo.Bcat;
using Nintendo.Blitz;
using Nintendo.Blitz.Mush;

namespace JelonzoBot.Blitz.Internationalization
{
    public static class BlitzLocalizer
    {
        private static Dictionary<Language, MsbtHolder> MsbtHolders;
        private static List<MapInfoEntry> MapInfoEntries;
        
        public static void Initialize()
        {
            // Create a new dictionary
            MsbtHolders = new Dictionary<Language, MsbtHolder>();
            
            // Loop over every language
            foreach (Language language in BlitzUtil.SupportedLanguages)
            {
                // Load the corresponding common szs file
                using (Stream stream = RomResourceLoader.GetRomFile($"/Message/CommonMsg_{language.GetSeadCode()}.release.szs"))
                {
                    // Get the Sarc archive
                    Sarc sarc = new Sarc(stream);

                    // Create a MsbtHolder
                    MsbtHolders.Add(language, new MsbtHolder(sarc));
                }
            }
        }

        public static void Dispose()
        {

        }

    }
}