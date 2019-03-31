using System.Collections;
using System.Collections.Generic;
using Syroot.NintenTools.Byaml.Serialization;

namespace Nintendo.Blitz.Bcat.Coop
{
    [ByamlObject]
    public class CoopSetting
    {
        [ByamlMember]
        public int Version
        {
            get;
            set;
        }

        [ByamlMember("MonthlyRewardGears")]
        public List<RewardGear> MonthlyRewards
        {
            get;
            set;
        }

        [ByamlMember]
        public List<CoopPhase> Phases
        {
            get;
            set;
        }

        [ByamlMember]
        public List<int> RateRanges
        {
            get;
            set;
        }

    }
}