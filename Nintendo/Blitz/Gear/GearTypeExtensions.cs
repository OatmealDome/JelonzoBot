using System;

namespace Nintendo.Blitz.Gear
{
    public class GearTypeExtensions
    {
        public static GearType FromString(string str)
        {
            switch (str)
            {
                case "cShoes":
                    return GearType.Shoes;
                case "cClothes":
                    return GearType.Clothes;
                case "cHead":
                    return GearType.Head;
                default:
                    throw new Exception("Unknown gear type");
            }
        }
        
    }
}