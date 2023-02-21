
using System.Net;

namespace ET.Pay.Alipay
{
    /// <summary>
    /// Alipay 通知响应
    /// </summary>
    public static class AlipayNotifyResult
    {
        private static readonly TContentResult success = new TContentResult() { Content = "success", ContentType = "text/plain", StatusCode = 200 };
        private static readonly TContentResult failure = new TContentResult() { Content = "failure", ContentType = "text/plain", StatusCode = 200 };

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

