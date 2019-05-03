using System.Collections.Generic;
using Syroot.NintenTools.Byaml.Serialization;

namespace Nintendo.Blitz.Mush.WeaponInfo
{
    [ByamlObject]
    public class WeaponInfoEntry : IByamlSerializable
    {
        [ByamlMember]
        public int Addition
        {
            get;
            set;
        }

        [ByamlMember]
        public string ArcName
        {
            get;
            set;
        }

        [ByamlMember]
        public int CL
        {
            get;
            set;
        }

        [ByamlMember]
        public int CM
        {
            get;
            set;
        }

        [ByamlMember]
        public int CS
        {
            get;
            set;
        }

        [ByamlMember]
        public int CT
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

        public DoubleType Double
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
        
        public ParamLevel InkSaverLv
        {
            get;
            set;
        }

        [ByamlMember(Optional = true)]
        public string InkSaverType // not used?? only in code
        {
            get;
            set;
        }

        [ByamlMember]
        public int IsPressRelease
        {
            get;
            set;
        }
        
        public LockType Lock
        {
            get;
            set;
        }

        [ByamlMember]
        public string MainUpGearPowerType
        {
            get;
            set;
        }

        [ByamlMember]
        public string Material
        {
            get;
            set;
        }

        [ByamlMember]
        public string ModelName
        {
            get;
            set;
        }

        public ParamLevel MoveVelLv
        {
            get;
            set;
        }

        [ByamlMember]
        public string Name
        {
            get;
            set;
        }

        [ByamlMember("Param0")]
        public string ParamZero
        {
            get;
            set;
        }

        [ByamlMember("Param1")]
        public string ParamOne
        {
            get;
            set;
        }

        [ByamlMember("Param2")]
        public string ParamTwo
        {
            get;
            set;
        }

        [ByamlMember("ParamValue0")]
        public int ParamZeroValue
        {
            get;
            set;
        }

        [ByamlMember("ParamValue1")]
        public int ParamOneValue
        {
            get;
            set;
        }

        [ByamlMember("ParamValue2")]
        public int ParamTwoValue
        {
            get;
            set;
        }

        [ByamlMember]
        public int Price
        {
            get;
            set;
        }

        [ByamlMember(Optional = true)] // why does Nintendo do this
        public int? Range
        {
            get;
            set;
        }

        [ByamlMember]
        public int Rank
        {
            get;
            set;
        }

        [ByamlMember]
        public string ShotMoveVelType
        {
            get;
            set;
        }

        [ByamlMember]
        public string Special
        {
            get;
            set;
        }

        [ByamlMember]
        public int SpecialCost
        {
            get;
            set;
        }
        
        public ParamLevel StealthMoveAccLv
        {
            get;
            set;
        }

        [ByamlMember]
        public string Sub
        {
            get;
            set;
        }

        [ByamlMember]
        public int UIRefId
        {
            get;
            set;
        }

        [ByamlMember("備考")]
        public string Notes
        {
            get;
            set;
        }

        public void DeserializeByaml(IDictionary<string, object> dictionary)
        {
            // Deserialize enums
            Double = (DoubleType)EnumUtil.GetEnumValueFromString(typeof(DoubleType), (string)dictionary["Double"]);
            InkSaverLv = (ParamLevel)EnumUtil.GetEnumValueFromString(typeof(ParamLevel), (string)dictionary["InkSaverLv"]);
            Lock = (LockType)EnumUtil.GetEnumValueFromString(typeof(LockType), (string)dictionary["Lock"]);
            MoveVelLv = (ParamLevel)EnumUtil.GetEnumValueFromString(typeof(ParamLevel), (string)dictionary["MoveVelLv"]);
            StealthMoveAccLv = (ParamLevel)EnumUtil.GetEnumValueFromString(typeof(ParamLevel), (string)dictionary["StealthMoveAccLv"]);
        }

        public void SerializeByaml(IDictionary<string, object> dictionary)
        {
            throw new System.NotImplementedException();
        }

    }
}