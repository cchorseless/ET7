using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace ET
{
    public static partial class HttpHelper
    {
        public static async ETTask<string> ReadStringAsync(this HttpListenerRequest request)
        {
            string msg = "";
            using (Stream inputStream = request.InputStream)
            {
                using (StreamReader reader = new StreamReader(inputStream, Encoding.UTF8))
                {
                    msg = await reader.ReadToEndAsync();
                }
            }

            return msg;
        }

        public static async ETTask<Dictionary<string, string>> ReadAsFormAsync(this HttpListenerRequest request)
        {
            var msg = await request.ReadStringAsync();
            Log.Debug(msg);
            var obj = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(msg))
            {
                var a = msg.Split("&");
                foreach (var str in a)
                {
                    var index = str.IndexOf('=');
                    if (index > -1)
                    {
                        obj.Add(str.Substring(0, index), str.Substring(index + 1));
                    }
                }
            }
            Log.Debug(MongoHelper.ToJson(obj));
            return obj;
        }
    }
}