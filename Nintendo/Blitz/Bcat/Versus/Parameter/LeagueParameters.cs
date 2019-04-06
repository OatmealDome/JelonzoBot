using System;
using System.Collections.Generic;
using Syroot.NintenTools.Byaml.Serialization;

namespace Nintendo.Blitz.Bcat.Versus.Parameter
{
    [ByamlObject]
    public class LeagueParameters : IByamlSerializable
    {
        [ByamlMember]
        public int RequiredBattleTimes
        {
            get;
            set;
        }

        [ByamlMember]
        public int ResultDelayTimeMax
        {
            get;
            set;
        }

        [ByamlMember]
        public int ResultWaitingTime
        {
            get;
            set;
        }

        public Dictionary<VersusRule, GlickoConstants> PairGlickoConstants
        {
            get;
            set;
        }

        public Dictionary<VersusRule, GlickoConstants> TeamGlickoConstants
        {
            get;
            set;
        }

        public void DeserializeByaml(IDictionary<string, object> dictionary)
        {
            void PopulateGlickoConstants(string sourceDictKey, Dictionary<VersusRule, GlickoConstants> constantsDict)
            {
                // Get the source dictionary
                Dictionary<string, object> sourceDict = (Dictionary<string, object>)dictionary[sourceDictKey];

                // Get the raw constants dictionary for every mode
                foreach (KeyValuePair<string, object> pair in sourceDict)
                {
                    // Create a new GlickoConstants instance and populate it
                    GlickoConstants glickoConstants = new GlickoConstants();
                    glickoConstants.DeserializeByaml((Dictionary<string, object>)pair.Value);

                    // Get the VersusRule enum
                    VersusRule VersusRule = (VersusRule)EnumUtil.GetEnumValueFromString(typeof(VersusRule), pair.Key);

                    // Add this to the target dictionary
                    constantsDict.Add(VersusRule, glickoConstants);
                }
            }

            // Create the dictionaries
            PairGlickoConstants = new Dictionary<VersusRule, GlickoConstants>();
            TeamGlickoConstants = new Dictionary<VersusRule, GlickoConstants>();

            // Populate them
            PopulateGlickoConstants("Pair", PairGlickoConstants);
            PopulateGlickoConstants("Team", TeamGlickoConstants);
        }

        public void SerializeByaml(IDictionary<string, object> dictionary)
        {
            throw new NotImplementedException();
        }

    }
}