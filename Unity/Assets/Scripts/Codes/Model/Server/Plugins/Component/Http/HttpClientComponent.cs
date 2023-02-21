using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace ET
{
    public class HttpClientComponent : Entity, IAwake, IDestroy
    {
        public static HttpClientComponent Instance;
        [BsonIgnore]
        public HttpClient Client;
        [BsonIgnore]
        public readonly HttpClientHandler ClientHandler = new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
        };
    }
}
