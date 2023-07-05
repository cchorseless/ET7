using ET.Pay.Alipay;
using ET.Pay.Alipay.Notify;
using System;
using System.Collections.Generic;
using System.Net;

namespace ET.Server
{
    [HttpHandler(SceneType.Http, "/AliPayNotifyUrl", false)]
    public class Http_ListenAliPayNotifyUrlHandler: HttpBasePostHandler
    {
        protected override async ETTask Run(Entity domain, HttpListenerContext context)
        {
            var request = context.Request;
            var notify = await AliPayComponent.Instance.Notify.ExecuteAsync<AlipayTradePrecreateNotify>(request, AliPayComponent.Instance.PayOptions);
            if (long.TryParse(notify.OutTradeNo, out long orderid))
            {
                var order = await AliPayComponent.Instance.GetOrder(orderid);
                if (order != null)
                {
                    DBLogger.Instance.Important("PayNotify", $"WeChatPayNotify {notify.TradeStatus} => OutTradeNo: " + notify.OutTradeNo);
                    await AlipayNotifyResult.ReplySuccess(context.Response);
                    order.SyncOrderState(notify.TradeStatus);
                    order.Dispose();
                    return;
                }
            }

            await AlipayNotifyResult.ReplyFailure(context.Response);
        }
    }
}