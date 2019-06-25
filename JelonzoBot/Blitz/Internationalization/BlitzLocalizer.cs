using System.Collections.Generic;
using System.IO;
using System.Linq;
using BcatBotFramework.Internationalization;
using Nintendo.Archive;
using Nintendo.Bcat;
using Nintendo.Blitz;
using Nintendo.Blitz.Mush;
using Nintendo.Blitz.Mush.WeaponInfo;

namespace JelonzoBot.Blitz.Internationalization
{
    public static class BlitzLocalizer
    {
        public static Dictionary<Language, MsbtHolder> MsbtHolders;
        private static List<MapInfoEntry> MapInfoEntries;
        private static List<WeaponInfoEntry> WeaponInfoEntries;
        
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
            
            // Load MapInfo
            MapInfoEntries = ByamlLoader.GetByamlDeserialized<List<MapInfoEntry>>("/Mush/MapInfo.release.byml");
        
            // Load WeaponInfo_Main
            WeaponInfoEntries = ByamlLoader.GetByamlDeserialized<List<WeaponInfoEntry>>("/Mush/WeaponInfo_Main.release.byml"); 
        }

        public static void Dispose()
        {

        }

        public static string LocalizeStage(int id, Language language)
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

        public static Dictionary<Language, string> LocalizeStageToAllLanguages(int id)
        {
            // Create a new Dictionary
            Dictionary<Language, string> valueDict = new Dictionary<Language, string>();

            // Populate the Dictionary
            foreach (Language language in BlitzUtil.SupportedLanguages)
            {
                // Localize the key to this Language
                valueDict.Add(language, LocalizeStage(id, language));
            }

            // Return the Dictionary
            return valueDict;
        }

        public static string LocalizeRule(VersusRule rule, Language language)
        {
            // Load the localized string
            return MsbtHolders[language].Localize("VSRuleName", rule.ToString());
        }

        public static Dictionary<Language, string> LocalizeRuleToAllLanguages(VersusRule rule)
        {
            // Create a new Dictionary
            Dictionary<Language, string> valueDict = new Dictionary<Language, string>();

            // Populate the Dictionary
            foreach (Language language in BlitzUtil.SupportedLanguages)
            {
                // Localize the key to this Language
                valueDict.Add(language, LocalizeRule(rule, language));
            }

            // Return the Dictionary
            return valueDict;
        }

        public static string LocalizeWeapon(int id, Language language)
        {
            // Check if this is a special coop weapon
            if (id == -1)
            {
                return Localizer.Localize("weapon.coop_random", language);
            }
            else if (id == -2)
            {
                return Localizer.Localize("weapon.coop_random_grizzco", language);
            }

            // Get the entry from MapInfo
            WeaponInfoEntry weaponInfoEntry = WeaponInfoEntries.Where(x => x.Id == id).FirstOrDefault();

            // Check if the entry exists
            if (weaponInfoEntry == null)
            {
                // Return a generic string
                return $"ID ${id}";
            }

            // Load the localized string
            return MsbtHolders[language].Localize("WeaponName_Main", weaponInfoEntry.Name);
        }
        
        public static Dictionary<Language, string> LocalizeWeaponToAllLanguages(int id)
        {
            // Create a new Dictionary
            Dictionary<Language, string> valueDict = new Dictionary<Language, string>();

            // Populate the Dictionary
            foreach (Language language in BlitzUtil.SupportedLanguages)
            {
                // Localize the key to this Language
                valueDict.Add(language, LocalizeWeapon(id, language));
            }

            // Return the Dictionary
            return valueDict;
        }
        
    }
}