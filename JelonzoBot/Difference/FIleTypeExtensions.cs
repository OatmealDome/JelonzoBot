namespace JelonzoBot.Difference
{
    public class FileTypeExtensions
    {
        public FileType GetTypeFromFilePath(string path)
        {
            if (path.StartsWith("vsdata/VSSetting"))
            {
                return FileType.VSSetting;
            }

            if (path.StartsWith("fesdata/Fld_Deli"))
            {
                if (path.EndsWith("NVN.szs"))
                {
                    return FileType.FestivalDeliModel;
                }
                else if (path.EndsWith("bprm"))
                {
                    return FileType.FestivalDeliGraffitiSettings;
                }
                else if (path.EndsWith("Vss.szs"))
                {
                    return FileType.FestivalDeliLayout;
                }
            }

            switch (path)
            {
                case "coopdata/CoopSetting.byaml":
                    return FileType.SalmonRunSchedule;
                case "fesdata/Festival.byaml":
                    return FileType.FestivalByaml;
                case "fesdata/Grf_Deli_Fes.Nin_NX_NVN.szs":
                    return FileType.FestivalDeliGrafitti;
                case "fesdata/HapTexture.bfres":
                    return FileType.FestivalHapTexture;
                case "fesdata/IconTexture.bfres":
                    return FileType.FestivalIconTexture;
                case "fesdata/PanelTexture.bfres":
                    return FileType.FestivalPanelTexture;
                case "dummy/dummy.txt":
                    return FileType.DummyFile;
            }

            return FileType.Unknown;
        }
        
    }
}