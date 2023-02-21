

using System.Net;

namespace ET.Pay.WeChatPay.V3
{
    /// <summary>
    /// WeChatPay V3 通知应答
    /// </summary>
    public static class WeChatPayNotifyResult
    {


        private static readonly TContentResult success = new TContentResult() { Content = "{\"code\":\"SUCCESS\",\"message\":\"SUCCESS\"}", ContentType = "application/json", StatusCode = 200 };
        private static readonly TContentResult failure = new TContentResult() { Content = "{\"code\":\"FAIL\",\"message\":\"FAIL\"}", ContentType = "application/json", StatusCode = 500 };

        ///// <summary>
        ///// 成功
        ///// </summary>
        public static async ETTask ReplySuccess(HttpListenerResponse response)
        {
            response.StatusCode = success.StatusCode;
            response.ContentType = success.ContentType;
            await response.OutputStream.WriteAsync(success.Content.ToByteArray());
        }

        ///// <summary>
        ///// 失败
        ///// </summary>
        public static async ETTask ReplyFailure(HttpListenerResponse response)
        {
            response.StatusCode = failure.StatusCode;
            response.ContentType = failure.ContentType;
            await response.OutputStream.WriteAsync(failure.Content.ToByteArray());
        }
    }
}

