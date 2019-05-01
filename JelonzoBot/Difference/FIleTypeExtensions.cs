using System;

namespace JelonzoBot.Difference
{
    public static class FileTypeExtensions
    {
        public static FileType GetTypeFromFilePath(string path)
        {
            if (path.StartsWith("vsdata/VSSetting"))
            {
                return FileType.VersusSetting;
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
                    return FileType.CoopSetting;
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

        public static string GetPath(this FileType fileType)
        {
            switch (fileType)
            {
                case FileType.VersusSetting:
                    return "vsdata/VSSetting_{0}.byaml";
                case FileType.CoopSetting:
                    return "coopdata/CoopSetting.byaml";
                case FileType.FestivalDeliGrafitti:
                    return "fesdata/Grf_Deli_Fes.Nin_NX_NVN.szs";
                case FileType.FestivalHapTexture:
                    return "fesdata/HapTexture.bfres";
                case FileType.FestivalIconTexture:
                    return "fesdata/IconTexture.bfres";
                case FileType.FestivalPanelTexture:
                    return "fesdata/PanelTexture.bfres";
                case FileType.DummyFile:
                    return "dummy/dummy.txt";
                case FileType.FestivalDeliModel:
                    return "fesdata/Fld_Deli_Octa{0}.Nin_NX_NVN.szs";
                case FileType.FestivalDeliGraffitiSettings:
                    return "fesdata/Fld_Deli_Octa{0}_Vss.bprm";
                case FileType.FestivalDeliLayout:
                    return "fesdata/Fld_Deli_Octa{0}_Vss.szs";
                default:
                    throw new Exception("Unknown file type");
            }
        }
        
    }
}