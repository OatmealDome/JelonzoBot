using Nintendo.Blitz.Bcat.Versus.Parameter.Udemae;
using Syroot.NintenTools.Byaml.Serialization;

namespace Nintendo.Blitz.Bcat.Versus.Parameter
{
    [ByamlObject]
    public class VersusParameters
    {
        [ByamlMember("LeagueMatch")]
        public LeagueParameters League
        {
            get;
            set;
        }

        [ByamlMember("MatchMake")]
        public MatchmakingParameters Matchmaking
        {
            get;
            set;
        }

        [ByamlMember("Udemae")]
        public UdemaeParameters Udemae
        {
            get;
            set;
        }

        [ByamlMember("UdemaeX")]
        public UdemaeXParameters UdemaeX
        {
            get;
            set;
        }

    }
}