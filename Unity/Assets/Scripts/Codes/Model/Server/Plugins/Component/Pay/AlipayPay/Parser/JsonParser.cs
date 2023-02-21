using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using ET.Pay.Alipay.Parser.JsonConverters;

namespace ET.Pay.Alipay.Parser
{
    public static class JsonParser
    {
        public static readonly JsonSerializerOptions JsonSerializerOptions = new JsonSerializerOptions() { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping, AllowTrailingCommas = true };

        static JsonParser()
        {
            JsonSerializerOptions.Converters.Add(new NumberToStringConverter());
        }
    }
}
