using System.Net;

namespace ET.Pay.WeChatPay.V2
{
    /// <summary>
    /// WeChatPay V2 通知应答
    /// </summary>
    public static class WeChatPayNotifyResult
    {

        private static readonly TContentResult success = new TContentResult() { Content = "<xml><return_code><![CDATA[SUCCESS]]></return_code><return_msg><![CDATA[SUCCESS]]></return_msg></xml>", ContentType = "text/xml", StatusCode = 200 };
        private static readonly TContentResult failure = new TContentResult() { Content = "<xml><return_code><![CDATA[FAIL]]></return_code><return_msg><![CDATA[FAIL]]></return_msg></xml>", ContentType = "text/xml", StatusCode = 200 };

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

