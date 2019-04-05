using Syroot.NintenTools.Byaml.Serialization;

namespace Nintendo.Blitz.Bcat.Versus.Parameter.Udemae
{
    [ByamlObject]
    public class UdemaeXParameters
    {
        [ByamlMember]
        public int CrownEarnRank
        {
            get;
            set;
        }

        [ByamlMember]
        public int DemotedLine
        {
            get;
            set;
        }

        [ByamlMember]
        public int DemotedLineInSeason
        {
            get;
            set;
        }

        [ByamlMember]
        public int OutsideRangeRank
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

        [ByamlMember]
        public int StartXPowerMax
        {
            get;
            set;
        }

        [ByamlMember]
        public int StartXPowerMin
        {
            get;
            set;
        }

        [ByamlMember]
        public int StartXRD
        {
            get;
            set;
        }

        [ByamlMember]
        public string UnifiedMonth
        {
            get;
            set;
        }
        
    }
}