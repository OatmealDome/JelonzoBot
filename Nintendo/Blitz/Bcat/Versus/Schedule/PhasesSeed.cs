using System;
using System.Collections.Generic;
using Syroot.NintenTools.Byaml.Serialization;

namespace Nintendo.Blitz.Bcat.Versus.Schedule
{
    [ByamlObject]
    public class PhasesSeed : IByamlSerializable
    {
        public DateTime StartDateTime
        {
            get;
            set;
        }

        [ByamlMember]
        public int MaxNonAppearanceRate
        {
            get;
            set;
        }

        [ByamlMember(Optional = true)]
        public int? UnappearRuleWeight
        {
            get;
            set;
        }

        [ByamlMember("RegularStage")]
        public RandomizerSeed RegularStagesSeed
        {
            get;
            set;
        }

        public Dictionary<VersusRule, RandomizerSeed> GachiStagesSeed
        {
            get;
            set;
        }

        public Dictionary<VersusRule, int> GachiRuleWeight
        {
            get;
            set;
        }

        public Dictionary<VersusRule, RandomizerSeed> LeagueStagesSeed
        {
            get;
            set;
        }

        public Dictionary<VersusRule, int> LeagueRuleWeight
        {
            get;
            set;
        }

        [ByamlMember]
        public List<int> HeavyRotationStages
        {
            get;
            set;
        }

        public void DeserializeByaml(IDictionary<string, object> dictionary)
        {
            StartDateTime = BlitzBcatDeserializationUtil.DeserializeDateTime(dictionary["DateTime"]);

            void PopulateStagesSeedDict(string sourceDictKey, Dictionary<VersusRule, RandomizerSeed> targetDict)
            {
                // Get the source dictionary
                Dictionary<string, object> sourceDict = (Dictionary<string, object>)dictionary[sourceDictKey];

                // Get the raw seed dictionary for every mode
                foreach (KeyValuePair<string, object> pair in sourceDict)
                {
                    // Get the VersusRule enum
                    VersusRule VersusRule = (VersusRule)BlitzBcatDeserializationUtil.GetEnumValueFromString(typeof(VersusRule), pair.Key);

                    // Create a new RandomizerSeed instance and deserialize
                    RandomizerSeed randomizerSeed = new RandomizerSeed();
                    randomizerSeed.DeserializeByaml((Dictionary<string, object>)pair.Value);

                    // Add this to the target dictionary
                    targetDict.Add(VersusRule, randomizerSeed);
                }
            }

            // Create the seed dictionaries
            GachiStagesSeed = new Dictionary<VersusRule, RandomizerSeed>();
            LeagueStagesSeed = new Dictionary<VersusRule, RandomizerSeed>();

            // Populate them
            PopulateStagesSeedDict("GachiRuleStage", GachiStagesSeed);
            PopulateStagesSeedDict("LeagueRuleStage", LeagueStagesSeed);

            void PopulateRuleWeightDict(string sourceDictKey, Dictionary<VersusRule, int> targetDict)
            {
                // Get the source dictionary
                Dictionary<string, object> sourceDict = (Dictionary<string, object>)dictionary[sourceDictKey];

                // Get the raw seed dictionary for every mode
                foreach (KeyValuePair<string, object> pair in sourceDict)
                {
                    // Get the VersusRule enum
                    VersusRule VersusRule = (VersusRule)BlitzBcatDeserializationUtil.GetEnumValueFromString(typeof(VersusRule), pair.Key);

                    // Add this to the target dictionary
                    targetDict.Add(VersusRule, (int)pair.Value);
                }
            }

            // Create the weighting dictionaries
            GachiRuleWeight = new Dictionary<VersusRule, int>();
            LeagueRuleWeight = new Dictionary<VersusRule, int>();

            // Populate them
            PopulateRuleWeightDict("GachiRuleWeight", GachiRuleWeight);
            PopulateRuleWeightDict("LeagueRuleWeight", LeagueRuleWeight);
        }

        public void SerializeByaml(IDictionary<string, object> dictionary)
        {
            throw new NotImplementedException();
        }
        
    }
}