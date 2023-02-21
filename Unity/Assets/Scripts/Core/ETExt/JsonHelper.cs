using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace ET
{
    public static partial class JsonHelper
    {
        [StaticField]
        private static readonly JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions()
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };

        public static string ToLitJson(JsonNode obj)
        {
            return obj.ToJsonString(jsonSerializerOptions);
        }
        public static string ToLitJson(object message)
        {
            return JsonSerializer.Serialize(message, jsonSerializerOptions);
            //return   LitJson.JsonMapper.ToJson(message);
        }
        //public static LitJson.JsonData FromLitJson(string json)
        public static JsonNode FromLitJson(string json)
        {
            return JsonNode.Parse(json);
            //return LitJson.JsonMapper.ToObject(json);
        }
        public static T FromLitJson<T>(string json)
        {
            return JsonSerializer.Deserialize<T>(json, jsonSerializerOptions);
            //return LitJson.JsonMapper.ToObject<T>(json);
        }
        public static List<string> Keys(JsonObject obj)
        {
            var keys = new List<string>();
            foreach (var kv in obj)
            {
                keys.Add(kv.Key);
            }
            return keys;
            //return LitJson.JsonMapper.ToObject<T>(json);
        }
        ////public static LitJson.JsonData GetLitJson()
        //public static JsonDocument GetLitJson()
        //{
        //    //return MonoPool.Instance.Fetch(typeof(LitJson.JsonData)) as LitJson.JsonData;
        //    //return new LitJson.JsonData();
        //    JsonNode jsonNode = new JsonArray();
        //    JsonDocument jNode = JsonDocument.Parse("{\"Value\":\"Text\",\"Array\":[1,5,13,17,2]}");
        //    return jNode["Value"];
        //}
        public static JsonObject GetLitObject()
        {
            return new JsonObject();
        }

        public static JsonArray GetLitArray()
        {
            return new JsonArray();
        }

        public static JsonArray GetLitArray(Array array)
        {
            var json = GetLitArray();
            foreach (var item in array)
            {
                json.Add(item);
            }
            return json;
        }
    }
}