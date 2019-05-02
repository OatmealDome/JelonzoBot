using System;
using System.Collections.Generic;
using Nintendo.Bcat;
using Nintendo.Blitz.Bcat;
using Nintendo.Blitz.News;
using Nintendo.Blitz.News.Command;
using Syroot.NintenTools.Byaml.Serialization;

namespace Nintendo.Blitz.Bcat.Festival
{
    [ByamlObject]
    public class FestivalSetting : IByamlSerializable
    {
        [ByamlMember]
        public int DayChangeRetryLimitSec
        {
            get;
            set;
        }

        [ByamlMember]
        public int DayChangeRetryWaitAddSec
        {
            get;
            set;
        }

        [ByamlMember]
        public int FesSameTeamMatchWaitTime
        {
            get;
            set;
        }

        [ByamlMember]
        public int FestivalId
        {
            get;
            set;
        }

        [ByamlMember]
        public int RequiredBattleTimes
        {
            get;
            set;
        }

        public VersusRule VersusRule
        {
            get;
            set;
        }

        [ByamlMember]
        public int SpecialStage
        {
            get;
            set;
        }

        [ByamlMember(Optional = true)]
        public string SpecialType
        {
            get;
            set;
        }

        [ByamlMember]
        public int StartDelayTimeMax
        {
            get;
            set;
        }

        [ByamlMember]
        public int Version
        {
            get;
            set;
        }

        [ByamlMember]
        public int VoteAnimType
        {
            get;
            set;
        }

        [ByamlMember]
        public int VoteTextType
        {
            get;
            set;
        }

        [ByamlMember]
        public float WeightTenMatchRate
        {
            get;
            set;
        }

        [ByamlMember]
        public int WinBonusPoint
        {
            get;
            set;
        }

        [ByamlMember]
        public int WinnerCacheMinutes
        {
            get;
            set;
        }

        [ByamlMember]
        public int WinnerMyselfCacheMinutes
        {
            get;
            set;
        }

        public Dictionary<string, Dictionary<Language, List<ScriptCommand>>> News
        {
            get;
            set;
        }

        [ByamlMember]
        public List<Team> Teams
        {
            get;
            set;
        }

        [ByamlMember("Time")]
        public Times Times
        {
            get;
            set;
        }

        [ByamlMember(Optional = true)]
        public WeightMatchRequirements WeightMatchRequirements
        {
            get;
            set;
        }

        public GlickoConstants GlickoConstants
        {
            get;
            set;
        }

        public void DeserializeByaml(IDictionary<string, object> dictionary)
        {
            // Deserialize the VersusRule
            VersusRule = (VersusRule)EnumUtil.GetEnumValueFromString(typeof(VersusRule), (string)dictionary["Rule"]);
            
            // Create the News dictionary
            News = new Dictionary<string, Dictionary<Language, List<ScriptCommand>>>();

            // Get the news list
            List<object> newsList = (List<object>)dictionary["News"];

            // Loop over every news
            foreach (object obj in newsList)
            {
                // Create a new inner dictionary
                Dictionary<Language, List<ScriptCommand>> innerDict = new Dictionary<Language, List<ScriptCommand>>();

                // Get the news dictionary
                Dictionary<string, object> news = (Dictionary<string, object>)obj;

                // Loop over every language key
                foreach (string code in news.Keys)
                {
                    // Skip NewsType
                    if (code == "NewsType")
                    {
                        continue;
                    }

                    // Create the script list
                    List<ScriptCommand> commandList = ScriptParser.ParseCommandList((List<object>)news[code]);

                    // Get the language code
                    Language language = LanguageExtensions.FromSeadCode(code);

                    // Add this to the inner dictionary
                    innerDict.Add(language, commandList);
                }

                // Add the inner dictionary
                News.Add((string)news["NewsType"], innerDict);
            }

            // Load the Glicko constants
            GlickoConstants = new GlickoConstants();
            GlickoConstants.DeserializeByaml((Dictionary<string, object>)dictionary[VersusRule]);
        }

        public void SerializeByaml(IDictionary<string, object> dictionary)
        {
            throw new NotImplementedException();
        }

    }
}