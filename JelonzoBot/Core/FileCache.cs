using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using BcatBotFramework.Core.Config;
using Nintendo.Blitz;
using Nintendo.Blitz.Bcat.Coop;
using Nintendo.Blitz.Bcat.Versus;
using Nintendo.Blitz.Bcat.Festival;
using Syroot.NintenTools.Byaml.Serialization;

namespace JelonzoBot.Core
{
    public static class FileCache
    {
        // Paths
        private static string LOCAL_DIRECTORY = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "FileCache");
        public static string FESTIVAL_SETTING_PATH = Path.Combine(LOCAL_DIRECTORY, "Festival", "{0}", "{1}.byaml");
        public static string VERSUS_SETTING_PATH = Path.Combine(LOCAL_DIRECTORY, "VSSetting", "{0}.byaml");
        public static string COOP_SETTING_PATH = Path.Combine(LOCAL_DIRECTORY, "CoopSetting.byaml");

        // Loaded files
        private static Dictionary<RomType, List<FestivalSetting>> FestivalSettings;
        private static Dictionary<RomType, VersusSetting> VersusSettings;
        private static CoopSetting CoopSetting;
        
        public static void Initialize()
        {
            if (!Configuration.LoadedConfiguration.FirstRunCompleted)
            {
                throw new Exception("Cannot initialize FileCache if the first run is not complete");
            }

            // Create a ByamlSerializer
            ByamlSerializer byamlSerializer = new ByamlSerializer(new ByamlSerializerSettings()
            {
                ByteOrder = Syroot.BinaryData.ByteOrder.LittleEndian,
                SupportPaths = false,
                Version = ByamlVersion.Version1
            });

            // Create the FestivalSettings dictionary
            FestivalSettings = new Dictionary<RomType, List<FestivalSetting>>();

            // Populate the FestivalSettings dictionary with RomTypes and lists
            foreach (RomType romType in Program.BcatPairs.Keys)
            {
                // Create a list for this RomType
                FestivalSettings[romType] = new List<FestivalSetting>();

                // Get all FestivalSetting byamls for this RomType
                string[] festPaths = Directory.GetFiles(Path.Combine(LOCAL_DIRECTORY, "Festival", romType.ToString()));

                // Loop over every path
                foreach (string festPath in festPaths)
                {
                    // Deserialize the byaml
                    FestivalSetting festivalSetting = byamlSerializer.Deserialize<FestivalSetting>(festPath);

                    // Add this to the list
                    FestivalSettings[romType].Add(festivalSetting);
                }

                // Sort in ascending order
                FestivalSettings[romType].Sort((x, y) => x.Times.Start.CompareTo(y.Times.Start));
            }

            // Create the VersusSettings dictionary
            VersusSettings = new Dictionary<RomType, VersusSetting>();

            // Get all VersusSetting byamls
            string[] versusPaths = Directory.GetFiles(Path.Combine(LOCAL_DIRECTORY, "VSSetting"));

            // Loop over every path
            foreach (string versusPath in versusPaths)
            {
                // Get the RomType
                RomType romType = (RomType)EnumUtil.GetEnumValueFromString(typeof(RomType), Path.GetFileNameWithoutExtension(versusPath));

                // Deserialize the byaml
                VersusSettings[romType] = byamlSerializer.Deserialize<VersusSetting>(versusPath);
            }

            // Deserialize the CoopSetting byaml
            CoopSetting = byamlSerializer.Deserialize<CoopSetting>(COOP_SETTING_PATH);
        }

        public static void Dispose()
        {

        }

        public static IEnumerable<FestivalSetting> GetAllFestivalSettingsForRomType(RomType romType)
        {
            return FestivalSettings[romType].AsReadOnly();
        }

        public static FestivalSetting GetLatestFestivalSettingForRomType(RomType romType)
        {
            return FestivalSettings[romType].Last();
        }

        public static FestivalSetting GetFestivalSettingForId(RomType romType, int id)
        {
            return FestivalSettings[romType].Where(x => x.FestivalId == id).FirstOrDefault();
        }

        public static void AddOrUpdateFestivalSetting(RomType romType, FestivalSetting festivalSetting, byte[] rawSetting)
        {
            // Check if this festival is already in the cache
            int idx = FestivalSettings[romType].FindIndex(x => x.FestivalId == festivalSetting.FestivalId);

            // Check if it hasn't been added
            if (idx == -1)
            {
                // Add it
                FestivalSettings[romType].Add(festivalSetting);
            }
            else
            {
                // Update the existing entry
                FestivalSettings[romType][idx] = festivalSetting;
            }

            // Construct the local path
            string localPath = string.Format(FESTIVAL_SETTING_PATH, romType.ToString(), festivalSetting.FestivalId);

            // Write out the file
            File.WriteAllBytes(localPath, rawSetting);
        }

        public static VersusSetting GetVersusSettingForRomType(RomType romType)
        {
            return VersusSettings[romType];
        }

        public static void SetVersusSettingForRomType(RomType romType, VersusSetting versusSetting, byte[] rawSetting)
        {
            VersusSettings[romType] = versusSetting;

            // Construct the local path
            string localPath = string.Format(VERSUS_SETTING_PATH, romType.ToString());

            // Write out the file
            File.WriteAllBytes(localPath, rawSetting);
        }

        public static CoopSetting GetCoopSetting()
        {
            return CoopSetting;
        }

        public static void SetCoopSetting(CoopSetting coopSetting, byte[] rawSetting)
        {
            CoopSetting = coopSetting;

            // Write out the local file
            File.WriteAllBytes(COOP_SETTING_PATH, rawSetting);
        }

    }
}