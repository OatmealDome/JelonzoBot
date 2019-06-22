using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using BcatBotFramework.Core;
using BcatBotFramework.Core.Config;
using BcatBotFramework.Difference;
using BnTxx;
using BnTxx.Formats;
using JelonzoBot.Blitz;
using JelonzoBot.Core;
using JelonzoBot.Core.Config;
using JelonzoBot.Json;
using Newtonsoft.Json;
using Nintendo;
using Nintendo.Blitz;
using Nintendo.Blitz.Bcat.Festival;
using S3;
using Syroot.BinaryData;
using Syroot.NintenTools.Byaml.Dynamic;
using Syroot.NintenTools.NSW.Bfres;

namespace JelonzoBot.Difference.Handlers.Web
{
    public class FestivalWebHandler
    {
        [JelonzoBotDifferenceHandler(FileType.FestivalByaml, DifferenceType.Changed, 1)]
        public static Task HandleFestival(RomType romType, Dictionary<string, byte[]> data, FestivalSetting previousFestival, FestivalSetting newFestival, byte[] rawFile)
        {
            // Check if this is the same
            if (previousFestival.FestivalId == newFestival.FestivalId)
            {
                //return Task.FromResult(0);
            }

            // Construct the path
            string s3Path = $"/splatoon/festival/{romType.ToString()}/{newFestival.FestivalId}";

            // Deserialize the FestivalSetting dynamically
            dynamic settingDynamic = ByamlLoader.GetByamlDynamic(rawFile);

            // Serialize the FestivalSetting to JSON
            string json = JsonConvert.SerializeObject(settingDynamic);

            // Upload to S3
            S3Api.TransferFile(Encoding.UTF8.GetBytes(json), s3Path, "setting.json", "application/json");

            // Load the panel texture file
            using (MemoryStream panelStream = new MemoryStream(data[FileType.FestivalPanelTexture.GetPath()]))
            {
                // Parse the BFRES
                ResFile panelRes = new ResFile(panelStream);

                // Load the BNTX
                using (MemoryStream bntxStream = new MemoryStream(panelRes.ExternalFiles[0].Data))
                {
                    // Parse the BNTX
                    BinaryTexture bt = new BinaryTexture(bntxStream);

                    // Decode the first texture
                    if (PixelDecoder.TryDecode(bt.Textures[0], out Bitmap Img))
                    {
                        // Open a destination MemoryStream
                        using (MemoryStream bitmapStream = new MemoryStream())
                        {
                            // Write the bitmap as a PNG to the MemoryStream
                            Img.Save(bitmapStream, ImageFormat.Png);

                            // Get the data
                            byte[] imageData = bitmapStream.ToArray();

                            // Upload to S3
                            S3Api.TransferFile(imageData, s3Path, "panel.png");

                            // Write to local
                            File.WriteAllBytes(string.Format(FileCache.FESTIVAL_PANEL_PATH, romType.ToString(), newFestival.FestivalId), imageData);
                        }
                    }
                }
            }

            lock (WebFileHandler.Lock)
            {
                // Get the WebConfig
                JelonzoBotWebConfig webConfig = ((JelonzoBotConfiguration)Configuration.LoadedConfiguration).WebConfig;

                // Connect to the remote server if needed
                WebFileHandler.Connect(webConfig);

                // Format the container list path
                string manifestPath = webConfig.LatestFestivalManifestPath;

                // Check if the file exists
                LatestFestivalManifest manifest;
                if (WebFileHandler.Exists(manifestPath))
                {
                    // Deserialize the manifest
                    manifest = WebFileHandler.ReadAllText<LatestFestivalManifest>(manifestPath);
                }
                else
                {
                    // Create a new manifest
                    manifest = new LatestFestivalManifest();
                    manifest.NorthAmerica = FileCache.GetLatestFestivalSettingForRomType(RomType.NorthAmerica).FestivalId;
                    manifest.Europe = FileCache.GetLatestFestivalSettingForRomType(RomType.Europe).FestivalId;
                    manifest.Japan = FileCache.GetLatestFestivalSettingForRomType(RomType.Japan).FestivalId;
                }
                
                // Update the manifest
                switch (romType)
                {
                    case RomType.NorthAmerica:
                        manifest.NorthAmerica = newFestival.FestivalId;
                        break;
                    case RomType.Europe:
                        manifest.Europe = newFestival.FestivalId;
                        break;
                    case RomType.Japan:
                        manifest.Japan = newFestival.FestivalId;
                        break;
                    default:
                        throw new Exception("Invalid RomType");
                }

                // Upload the manifest
                WebFileHandler.WriteSerializedJson(manifestPath, manifest);

                // Get the FestivalSetting JSON path
                string path = webConfig.FestivalSettingPath;
                switch (romType)
                {
                    case RomType.NorthAmerica:
                        path = string.Format(path, "na");
                        break;
                    case RomType.Europe:
                        path = string.Format(path, "eu");
                        break;
                    case RomType.Japan:
                        path = string.Format(path, "jp");
                        break;
                    default:
                        throw new Exception("Invalid RomType");
                }

                // Upload to the server
                WebFileHandler.WriteAllText(path, json);

                // Disconnect
                WebFileHandler.Disconnect();
            }

            return Task.FromResult(0);
        }

    }
}