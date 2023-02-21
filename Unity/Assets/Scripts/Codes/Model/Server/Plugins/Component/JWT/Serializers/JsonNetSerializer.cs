using System;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ET.JWT.Serializers
{
    /// <summary>
    /// JSON serializer using Newtonsoft.Json implementation.
    /// </summary>
    public sealed class JsonNetSerializer : IJsonSerializer
    {
        private static JsonNetSerializer Instance;
        private static readonly JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions() { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };

        /// <summary>
        /// Creates a new instance of <see cref="JsonNetSerializer" />
        /// </summary>
        public JsonNetSerializer()
        {
        }
        public static JsonNetSerializer GetInstance()
        {
            if (Instance == null)
            {
                Instance = new JsonNetSerializer();
            }
            return Instance;
        }


        /// <inheritdoc />
        /// <exception cref="ArgumentNullException" />
        public string Serialize(object obj)
        {
            if (obj is null)
                throw new ArgumentNullException(nameof(obj));
            return JsonSerializer.Serialize(obj, jsonSerializerOptions);
            //var sb = new StringBuilder();
            //using var stringWriter = new StringWriter(sb);
            //using var jsonWriter = new JsonTextWriter(stringWriter);
            //_serializer.Serialize(jsonWriter, obj);
            //return sb.ToString();
        }

        /// <inheritdoc />
        /// <exception cref="ArgumentNullException" />
        /// <exception cref="ArgumentException" />
        public object Deserialize(Type type, string json)
        {
            if (type is null)
                throw new ArgumentNullException(nameof(type));
            if (String.IsNullOrEmpty(json))
                throw new ArgumentException(nameof(json));
            return JsonSerializer.Deserialize(json, type, jsonSerializerOptions);

            //using var stringReader = new StringReader(json);
            //using var jsonReader = new JsonTextReader(stringReader);
            //return _serializer.Deserialize(jsonReader, type);
        }
    }
}