using System.Net.Http;
using System.Collections.Generic;

namespace ET.Pay.WeChatPay
{
    public abstract class WeChatPayObject
    {
        
    }
    public struct TContentResult
    {
        public string Content { get; set; }
        public string ContentType { get; set; }
        public int StatusCode { get; set; }
    }
    public interface IHttpClientFactory
    {
        HttpClient CreateClient(string name);
    }

    public class HttpClientFactory : IHttpClientFactory
    {
        private Dictionary<string, HttpClient> _httpClientFactories = new Dictionary<string, HttpClient>();
        public HttpClient CreateClient(string name)
        {
            if (!_httpClientFactories.TryGetValue(name, out var client))
            {
                client = new HttpClient();
                _httpClientFactories.Add(name, client);
            }
            return client;
        }
    }
}
