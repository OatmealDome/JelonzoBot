using System.Collections.Generic;
using System.IO;
using Nintendo.Archive;
using Nintendo.Text;

namespace JelonzoBot.Blitz.Internationalization
{
    public class MsbtHolder
    {
        public Dictionary<string, Dictionary<string, string>> Msbts = new Dictionary<string, Dictionary<string, string>>();

        public MsbtHolder(Sarc commonSarc)
        {
            foreach (KeyValuePair<string, byte[]> pair in commonSarc)
            {
                // Load and add this MSBT
                Msbts.Add(Path.GetFileNameWithoutExtension(pair.Key), LoadMsbt(pair.Value));
            }
        }

        public string Localize(string sourceMsbt, string localizable)
        {
            return Msbts[sourceMsbt][localizable].Trim('\0');
        }

        private static Dictionary<string, string> LoadMsbt(byte[] rawMsbt)
        {
            // Create a new dictionary to hold the text
            Dictionary<string, string> textMappings = new Dictionary<string, string>();

            // Parse the MSBT
            MSBT msbt = new MSBT(rawMsbt);

            // Loop over every TXT2 entry
            for (int i = 0; i < msbt.TXT2.NumberOfStrings; i++)
            {
                IEntry entry = msbt.HasLabels ? msbt.LBL1.Labels[i] : msbt.TXT2.Strings[i];
                textMappings.Add(entry.ToString(), msbt.FileEncoding.GetString(entry.Value));
            }

            return textMappings;
        }
        
    }
}