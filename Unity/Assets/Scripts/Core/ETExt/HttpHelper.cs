using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace ET
{
    public static partial class HttpHelper
    {
        public static async ETTask<string> ReadStringAsync(this HttpListenerRequest request)
        {
            string msg = "";
            using (Stream inputStream = request.InputStream)
            {
                using (StreamReader reader = new StreamReader(inputStream))
                {
                    msg = await reader.ReadToEndAsync();
                }
            }
            return msg;
        }

        public static async ETTask<Dictionary<string, string>> ReadAsFormAsync(this HttpListenerRequest request)
        {
            var msg = await request.ReadStringAsync();
            var obj = new Dictionary<string, string>();
            try
            {
                obj = JsonHelper.FromLitJson<Dictionary<string, string>>(msg);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }
            return obj;
        }



    }
}