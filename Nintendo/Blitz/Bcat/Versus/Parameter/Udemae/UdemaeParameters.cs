using System;
using System.Collections.Generic;
using System.Linq;
using Syroot.NintenTools.Byaml.Serialization;

namespace Nintendo.Blitz.Bcat.Versus.Parameter.Udemae
{
    [ByamlObject]
    public class UdemaeParameters : IByamlSerializable
    {
        [ByamlMember]
        public int EarnPointMax
        {
            get;
            set;
        }

        [ByamlMember]
        public int EarnPointMin
        {
            get;
            set;
        }

        [ByamlMember]
        public int JumpUpMax
        {
            get;
            set;
        }

        [ByamlMember]
        public int MaintenaceLine
        {
            get;
            set;
        }

        [ByamlMember]
        public int OverSJumpUpMax
        {
            get;
            set;
        }

        [ByamlMember]
        public int OverSplusEarnPointMax
        {
            get;
            set;
        }

        [ByamlMember]
        public int OverSplusEarnPointMin
        {
            get;
            set;
        }

        [ByamlMember]
        public int OverSplusJumpDownMax
        {
            get;
            set;
        }

        [ByamlMember]
        public int OverSplusMaintenaceLine
        {
            get;
            set;
        }

        public Dictionary<VersusRule, List<int>> TargetRates
        {
            get;
            set;
        }

        public Dictionary<VersusRule, List<int>> OverSplusTargetRates
        {
            get;
            set;
        }

        public Dictionary<string, UdemaePointRetention> PointRetentionRates
        {
            get;
            set;
        }

        public void DeserializeByaml(IDictionary<string, object> dictionary)
        {
            void PopulateTargetRates(string sourceDictKey, Dictionary<VersusRule, List<int>> rateRanges)
            {
                // Get the source dictionary
                Dictionary<string, object> sourceDict = (Dictionary<string, object>)dictionary[sourceDictKey];

                // Get the raw constants dictionary for every mode
                foreach (KeyValuePair<string, object> pair in sourceDict)
                {
                    // Get the VersusRule enum
                    VersusRule VersusRule = (VersusRule)EnumUtil.GetEnumValueFromString(typeof(VersusRule), pair.Key);

                    // Get the list of rates
                    List<object> rates = (List<object>)pair.Value;

                    // Add this to the target dictionary
                    rateRanges.Add(VersusRule, rates.Cast<int>().ToList());
                }
            }

            // Create the target rates  dictionaries
            TargetRates = new Dictionary<VersusRule, List<int>>();
            OverSplusTargetRates = new Dictionary<VersusRule, List<int>>();

            // Populate them
            PopulateTargetRates("TargetRates", TargetRates);
            PopulateTargetRates("OverSplusTargetRates", OverSplusTargetRates);

            // Create the point retention dictionary
            PointRetentionRates = new Dictionary<string, UdemaePointRetention>();

            // Get the source dictionary
            Dictionary<string, object> retentionRatesSource = (Dictionary<string, object>)dictionary["PointRetationRates"];

            // Loop over every rank
            foreach (KeyValuePair<string, object> pair in retentionRatesSource)
            {
                // Create a new UdemaePointRetention instance and deserialize it
                UdemaePointRetention retention = new UdemaePointRetention();
                retention.DeserializeByaml((Dictionary<string, object>)pair.Value);

                // Add this to the target dictionary
                PointRetentionRates.Add(pair.Key, retention);
            }
        }

        public void SerializeByaml(IDictionary<string, object> dictionary)
        {
            throw new NotImplementedException();
        }

    }
}