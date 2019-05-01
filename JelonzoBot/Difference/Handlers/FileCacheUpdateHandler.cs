using System.Collections.Generic;
using System.Threading.Tasks;
using BcatBotFramework.Difference;
using JelonzoBot.Core;
using Nintendo.Blitz;
using Nintendo.Blitz.Bcat.Coop;
using Nintendo.Blitz.Bcat.Festival;
using Nintendo.Blitz.Bcat.Versus;

namespace JelonzoBot.Difference.Handlers
{
    public class FileCacheUpdateHandler
    {
        [JelonzoBotDifferenceHandler(FileType.VersusSetting, DifferenceType.Changed, 100)]
        public static Task HandleVersusSetting(RomType romType, Dictionary<string, byte[]> data, VersusSetting previousSetting, VersusSetting newSetting, byte[] rawFile)
        {
            FileCache.SetVersusSettingForRomType(romType, newSetting, rawFile);

            return Task.FromResult(0);
        }

        [JelonzoBotDifferenceHandler(FileType.FestivalByaml, DifferenceType.Changed, 100)]
        public static Task HandleFestival(RomType romType, Dictionary<string, byte[]> data, FestivalSetting previousSetting, FestivalSetting newSetting, byte[] rawFile)
        {
            FileCache.AddOrUpdateFestivalSetting(romType, newSetting, rawFile);

            return Task.FromResult(0);
        }

        [JelonzoBotDifferenceHandler(FileType.CoopSetting, DifferenceType.Changed, 100)]
        public static Task HandleCoop(RomType romType, Dictionary<string, byte[]> data, CoopSetting previousSetting, CoopSetting newSetting, byte[] rawFile)
        {
            // Only do this once
            if (romType != RomType.NorthAmerica)
            {
                return Task.FromResult(0);
            }

            // Update CoopSetting
            FileCache.SetCoopSetting(newSetting, rawFile);

            return Task.FromResult(0);
        }

    }
}