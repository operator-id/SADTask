using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SearchAPI.Models;
using SearchAPI.Models.Schema;

namespace SearchAPI.Util
{
    public static class JsonParsingHelper
    {
        public static async Task<List<RealEstateBase>> ParseFromTextAsync(IndexParams indexParams)
        {
            var serializer = new JsonSerializer();
            using (var stringReader = new StringReader(indexParams.JsonString))
            using (var jsonReader = new JsonTextReader(stringReader))
            {
                var items = new List<RealEstateBase>();
                
                for (; await jsonReader.ReadAsync();)
                {
                    if (jsonReader.TokenType != JsonToken.StartObject)
                    {
                        continue;
                    }

                    var type = ReflectionHelper.GetTypeFromName(indexParams.ModelType);
                    if (type == null)
                    {
                        continue;
                    }
                    var item = serializer.Deserialize(jsonReader, type);
                    items.Add(item as RealEstateBase);
                }
                return items;
            }
        }
    }
}