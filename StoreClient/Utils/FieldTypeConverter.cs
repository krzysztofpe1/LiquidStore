using System;
using System.Reflection;

namespace StoreClient.Utils
{
    internal static class FieldTypeConverter
    {
        public static object Convert(PropertyInfo propInfo, string value)
        {
            try
            {
                var propType = propInfo.PropertyType;
                if (propType == typeof(string))
                    return value;
                else if (propType == typeof(int))
                    return int.Parse(value);
                else if (propType == typeof(float))
                    return float.Parse(value.Replace(',', '.'));
                else if (propType == typeof(double))
                    return double.Parse(value.Replace(',', '.'));
                else if (propType == typeof(Enum))
                    return int.Parse(value);
                return null;
            }catch(Exception ex) { return null; }
        }
    }
}
