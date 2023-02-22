using System;
using System.Net.Http;
using System.Net.Http.Json;

namespace ET
{
    public static class HttpClientComponentFunc
    {
        [ObjectSystem]
        public class HttpClientComponentSystemAwakeSystem: AwakeSystem<HttpClientComponent>
        {
            protected override void Awake(HttpClientComponent self)
            {
                self.Client = new HttpClient();
            }
        }

        [ObjectSystem]
        public class HttpClientComponentSystemDestroySystem: DestroySystem<HttpClientComponent>
        {
            protected override void Destroy(HttpClientComponent self)
            {
                self.Client.Dispose();
                self.Client = null;
            }
        }

        public static async ETTask<Response> GetAsync<Response>(this HttpClientComponent self, string url) where Response : class, IResponse
        {
            Response requestData = null;
            try
            {
                var msg = await self.Client.GetStringAsync(url);
                requestData = JsonHelper.FromJson<Response>(msg);
                if (requestData == null)
                {
                    throw new Exception($"消息类型转换错误:  {url}");
                }
            }
            catch (Exception e)
            {
                Log.Error(e);
                requestData = Activator.CreateInstance<Response>();
            }

            return requestData;
        }

        public static async ETTask<Response> PostAsync<Request, Response>(this HttpClientComponent self, string url, Request sendData)
                where Request : class, IRequest where Response : class, IResponse
        {
            Response requestData = null;
            try
            {
                var msg = await self.Client.PostAsJsonAsync(url, sendData);
                msg.EnsureSuccessStatusCode();
                requestData = await msg.Content.ReadFromJsonAsync<Response>();
                if (requestData == null)
                {
                    throw new Exception($"消息类型转换错误:  {url}");
                }
            }
            catch (Exception e)
            {
                Log.Error(e);
                requestData = Activator.CreateInstance<Response>();
            }

            return requestData;
        }
    }
}