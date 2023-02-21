using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace ET.Pay.WeChatPay.V2.Extensions
{
    public static class HttpClientExtensions
    {
        public static async ETTask<string> PostAsync<T>(this HttpClient client, IWeChatPayRequest<T> request, IDictionary<string, string> textParams) where T : WeChatPayResponse
        {
            var url = request.GetRequestUrl();
            var content = WeChatPayUtility.BuildContent(textParams);
            using (var reqContent = new StringContent(content, Encoding.UTF8, "application/xml"))
            using (var resp = await client.PostAsync(url, reqContent))
            using (var respContent = resp.Content)
            {
                return await respContent.ReadAsStringAsync();
            }
        }

        public static async ETTask<string> PostAsync<T>(this HttpClient client, IWeChatPayCertRequest<T> request, IDictionary<string, string> textParams) where T : WeChatPayResponse
        {
            var url = request.GetRequestUrl();
            var content = WeChatPayUtility.BuildContent(textParams);
            using (var reqContent = new StringContent(content, Encoding.UTF8, "application/xml"))
            using (var resp = await client.PostAsync(url, reqContent))
            using (var respContent = resp.Content)
            {
                return await respContent.ReadAsStringAsync();
            }
        }
    }
}
