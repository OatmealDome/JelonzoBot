using System;
using System.Collections.Generic;
using System.Drawing;
using Nintendo.Bcat;
using Nintendo.Blitz.Bcat;
using Syroot.NintenTools.Byaml.Serialization;

namespace Nintendo.Blitz.Bcat.Festival
{
    [ByamlObject]
    public class Team : IByamlSerializable
    {
        [ByamlMember("Color")]
        public List<object> Color4f;

        public Dictionary<Language, string> Name;

        public Dictionary<Language, string> ShortName;

        public Color GetColor4fAsColor()
        {
            return Color.FromArgb
            (
                Convert.ToInt32(BlitzBcatDeserializationUtil.GetValueAsFloat(Color4f[3]) * 255.0f),
                Convert.ToInt32(BlitzBcatDeserializationUtil.GetValueAsFloat(Color4f[0]) * 255.0f),
                Convert.ToInt32(BlitzBcatDeserializationUtil.GetValueAsFloat(Color4f[1]) * 255.0f),
                Convert.ToInt32(BlitzBcatDeserializationUtil.GetValueAsFloat(Color4f[2]) * 255.0f)
            );
        }

        public void DeserializeByaml(IDictionary<string, object> dictionary)
        {
            // Create new dictionaries
            Name = new Dictionary<Language, string>();
            ShortName = new Dictionary<Language, string>();

            void PopulateDictionary(string sourceDictKey, Dictionary<Language, string> targetDict)
            {
                // Check if the source dictionary even exists
                if (dictionary.TryGetValue(sourceDictKey, out object value))
                {
                    // Get the dictionary
                    Dictionary<string, object> sourceDict = (Dictionary<string, object>)value;

                    // Loop over every short name
                    foreach (KeyValuePair<string, object> pair in sourceDict)
                    {
                        targetDict.Add(LanguageExtensions.FromSeadCode(pair.Key), (string)pair.Value);
                    }
                }
            }

            PopulateDictionary("Name", Name);
            PopulateDictionary("ShortName", ShortName);
        }

        public void SerializeByaml(IDictionary<string, object> dictionary)
        {
            throw new NotImplementedException();
        }

    }
}