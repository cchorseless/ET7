using System;
using System.Collections.Generic;
using System.Net;
using ET.Pay.WeChatPay.V3;
using ET.Pay.WeChatPay.V3.Notify;

namespace ET.Server
{

    [HttpHandler(SceneType.Http, "/WeChatPayNotifyUrl", false)]
    public class Http_ListenWeChatPayNotifyUrlHandler : HttpBasePostHandler
    {
        protected override async ETTask Run(Entity domain, HttpListenerContext context)
        {
            var request = context.Request;
            var notify = await WeChatPayComponent.Instance.NotifyV3.ExecuteAsync<WeChatPayTransactionsNotify>(request, WeChatPayComponent.Instance.PayOptions);
            if (long.TryParse(notify.OutTradeNo, out long orderid))
            {
                var order = await WeChatPayComponent.Instance.GetOrder(orderid);
                if (order != null)
                {
                    Log.Info($"WeChatPayNotify {notify.TradeState} => OutTradeNo: " + notify.OutTradeNo);
                    await WeChatPayNotifyResult.ReplySuccess(context.Response);
                    await order.SyncOrderState(notify.TradeState);
                    order.Dispose();
                    return;
                }

            }
            await WeChatPayNotifyResult.ReplyFailure(context.Response);
        }
    }
}
