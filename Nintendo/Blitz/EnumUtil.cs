using System;

namespace Nintendo.Blitz
{
    public static class EnumUtil
    {
        public static object GetEnumValueFromString(Type enumType, string str)
        {
            object value;
            if (Enum.TryParse(enumType, str, out value))
            {
                return value;
            }
            else if (str.StartsWith('c') && Enum.TryParse(enumType, str.Substring(1), out value))
            {
                return value;
            }
            else if (Enum.TryParse(enumType, $"c{str}", out value))
            {
                return value;
            }

            throw new Exception("Unknown enum value " + str + " for " + enumType.Name);
        }
        
    }
}