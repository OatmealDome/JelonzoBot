using System.Collections.Generic;
using Syroot.NintenTools.Byaml.Serialization;

namespace Nintendo.Blitz.Mush
{
    [ByamlObject]
    public class MapInfoEntry : IByamlSerializable
    {
        [ByamlMember(Optional = true)]
        public float? AbnormalYPos
        {
            get;
            set;
        }

        [ByamlMember("BGMType")]
        public string BgmType
        {
            get;
            set;
        }

        public bool BakeLightForceEnable
        {
            get;
            set;
        }

        [ByamlMember]
        public int CoopAddition
        {
            get;
            set;
        }

        public bool EffectFakePointLight
        {
            get;
            set;
        }

        [ByamlMember(Optional = true)]
        public string EnvBrightness
        {
            get;
            set;
        }

        [ByamlMember]
        public string EnvHour
        {
            get;
            set;
        }

        [ByamlMember]
        public string FixTeamColor
        {
            get;
            set;
        }

        [ByamlMember]
        public int Id
        {
            get;
            set;
        }

        [ByamlMember]
        public string MapFileName
        {
            get;
            set;
        }

        [ByamlMember]
        public int MiniMapBravoInvType
        {
            get;
            set;
        }

        [ByamlMember]
        public float MiniMapPitch
        {
            get;
            set;
        }

        [ByamlMember]
        public float MiniMapScale
        {
            get;
            set;
        }

        [ByamlMember]
        public string MiniMapTrans
        {
            get;
            set;
        }

        [ByamlMember]
        public float MiniMapYaw
        {
            get;
            set;
        }

        [ByamlMember]
        public int MsnAreaNo
        {
            get;
            set;
        }

        [ByamlMember(Optional = true)]
        public int? MsnLyr_Blaster
        {
            get;
            set;
        }

        [ByamlMember(Optional = true)]
        public int? MsnLyr_Brush
        {
            get;
            set;
        }

        [ByamlMember(Optional = true)]
        public int? MsnLyr_Roller
        {
            get;
            set;
        }

        [ByamlMember(Optional = true)]
        public int? MsnLyr_Shooter
        {
            get;
            set;
        }

        [ByamlMember(Optional = true)]
        public int? MsnLyr_Slosher
        {
            get;
            set;
        }

        [ByamlMember(Optional = true)]
        public int? MsnLyr_Spinner
        {
            get;
            set;
        }

        [ByamlMember(Optional = true)]
        public int? MsnLyr_Twins
        {
            get;
            set;
        }

        [ByamlMember(Optional = true)]
        public int? MsnLyr_Umbrella
        {
            get;
            set;
        }

        [ByamlMember]
        public int MsnMainWeaponId
        {
            get;
            set;
        }

        [ByamlMember]
        public int MsnStageNo
        {
            get;
            set;
        }

        public bool OnlyObjPaint
        {
            get;
            set;
        }

        [ByamlMember]
        public int PrivateMatchOrder
        {
            get;
            set;
        }

        [ByamlMember]
        public string ReleaseVer
        {
            get;
            set;
        }

        [ByamlMember]
        public string SndSceneEnv
        {
            get;
            set;
        }

        public void DeserializeByaml(IDictionary<string, object> dictionary)
        {
            // WHy is this even a thing in the first place? Can't they just use a bool type?
            bool DeserializeFakeBool(object target)
            {
                if (target == null)
                {
                    return false;
                }

                string str = ((string)target).ToLower();

                return str == "on";
            }

            BakeLightForceEnable = DeserializeFakeBool(dictionary["BakeLightForceEnable"]);
            EffectFakePointLight = DeserializeFakeBool(dictionary["EffectFakePointLight"]);
            OnlyObjPaint = DeserializeFakeBool(dictionary["OnlyObjPaint"]);
        }

        public void SerializeByaml(IDictionary<string, object> dictionary)
        {
            throw new System.NotImplementedException();
        }

    }
}