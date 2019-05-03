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

            // Load Mush.pack
            using (Stream mushStream = RomResourceLoader.GetRomFile("/Pack/Mush.release.pack"))
            {
                // Get the Sarc archive
                Sarc mushSarc = new Sarc(mushStream);

                // Load MapInfo
                MapInfoEntries = RomResourceLoader.GetByamlDeserializedFromLocal<List<MapInfoEntry>>(mushSarc["Mush/MapInfo.release.byml"]);
            }
        }

        public static void Dispose()
        {

        }

        public static string LocalizeMap(Language language, int id)
        {
            // Get the entry from MapInfo
            MapInfoEntry mapInfoEntry = MapInfoEntries.Where(x => x.Id == id).FirstOrDefault();

            // Check if the entry exists
            if (mapInfoEntry == null)
            {
                // Return a generic string
                return $"ID ${id}";
            }

            // Get the position of the final underscore in the filename
            int idx = mapInfoEntry.MapFileName.LastIndexOf('_');

            // Get the map type
            string mapType = mapInfoEntry.MapFileName.Substring(idx + 1, 3);

            // Get the target MSBT based on this name
            string targetMsbt;
            switch (mapType)
            {
                case "Vss":
                    targetMsbt = "VSStageName";
                    break;
                case "Cop":
                    targetMsbt = "CoopStageName";
                    break;
                default:
                    // Return a generic string
                    return $"{mapType} ID {id}";
            }

            // Get the localizable (the map name minus "Fld" and the suffix)
            string localizable = mapInfoEntry.MapFileName.Substring(4, idx - 4);

            // Load the localized string from the file name
            return MsbtHolders[language].Localize(targetMsbt, localizable);
        }

    }
}