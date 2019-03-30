namespace JelonzoBot.Difference
{
    public class BlitzBcatFileTypeExtensions
    {
        public BlitzBcatFileType GetTypeFromFilePath(string path)
        {
            if (path.StartsWith("vsdata/VSSetting"))
            {
                return BlitzBcatFileType.VSSetting;
            }

            if (path.StartsWith("fesdata/Fld_Deli"))
            {
                if (path.EndsWith("NVN.szs"))
                {
                    return BlitzBcatFileType.FestivalDeliModel;
                }
                else if (path.EndsWith("bprm"))
                {
                    return BlitzBcatFileType.FestivalDeliGraffitiSettings;
                }
                else if (path.EndsWith("Vss.szs"))
                {
                    return BlitzBcatFileType.FestivalDeliLayout;
                }
            }

            switch (path)
            {
                case "coopdata/CoopSetting.byaml":
                    return BlitzBcatFileType.SalmonRunSchedule;
                case "fesdata/Festival.byaml":
                    return BlitzBcatFileType.FestivalByaml;
                case "fesdata/Grf_Deli_Fes.Nin_NX_NVN.szs":
                    return BlitzBcatFileType.FestivalDeliGrafitti;
                case "fesdata/HapTexture.bfres":
                    return BlitzBcatFileType.FestivalHapTexture;
                case "fesdata/IconTexture.bfres":
                    return BlitzBcatFileType.FestivalIconTexture;
                case "fesdata/PanelTexture.bfres":
                    return BlitzBcatFileType.FestivalPanelTexture;
                case "dummy/dummy.txt":
                    return BlitzBcatFileType.DummyFile;
            }

            return BlitzBcatFileType.Unknown;
        }
        
    }
}