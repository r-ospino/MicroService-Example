using System;
using System.Linq;

namespace Weelo.RafaelOspino.Api.Utils
{
    public static class TypeExtensions
    {
        /// <summary>
        /// https://stackoverflow.com/a/16466437
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetFriendlyName(this Type type)
        {
            if (type == typeof(int)) return "int";

            if (type == typeof(short)) return "short";

            if (type == typeof(byte)) return "byte";

            if (type == typeof(bool)) return "bool";
            
            if (type == typeof(long)) return "long";
            
            if (type == typeof(float)) return "float";
            
            if (type == typeof(double)) return "double";
            
            if (type == typeof(decimal)) return "decimal";
            
            if (type == typeof(string)) return "string";
            
            if (type.IsGenericType) return $"{type.Name.Split('`')[0]}<{string.Join(", ", type.GetGenericArguments().Select(x => GetFriendlyName(x)).ToArray())}>";
            
            return type.Name;
        }
    }
}
