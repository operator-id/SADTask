using System;
using System.Collections.Generic;
using System.Linq;
using SearchAPI.Models.Schema;

namespace SearchAPI.Util
{
    public static class ReflectionHelper
    {
        private static readonly Dictionary<string, Type> ParsableTypes = MapTypesToDictionary();

        public static Type GetTypeFromName(string typeName)
        {
            var nameToLower = typeName.ToLower();
            return ParsableTypes.ContainsKey(nameToLower) ? ParsableTypes[nameToLower] : null;
        }

        private static Dictionary<string, Type> MapTypesToDictionary()
        {
            var types = AppDomain.CurrentDomain
                .GetAssemblies()
                .SelectMany(t => t.GetTypes())
                .Where(t => t.IsClass && t.Namespace == typeof(RealEstateBase).Namespace);

            return types.ToDictionary(type => type.Name.ToLower());
        }
    }
}