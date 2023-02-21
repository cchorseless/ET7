using ET.Pay.Alipay;
using ET.Pay.Alipay.Notify;
using System;
using System.Collections.Generic;
using System.Net;


namespace ET.Server
{

    [HttpHandler(SceneType.Http, "/AliPayNotifyUrl", false)]
    public class Http_PostAliPayNotifyUrlHandler : HttpBasePostHandler
    {
        protected override async ETTask Run(Entity domain, HttpListenerContext context)
        {
            var request = context.Request;
            var notify = await AliPayComponent.Instance.Notify.ExecuteAsync<AlipayTradePrecreateNotify>(request, AliPayComponent.Instance.PayOptions);
            if (long.TryParse(notify.OutTradeNo, out long orderid))
            {
                var order = await AliPayComponent.Instance.QueryOrder(orderid);
                if (order != null)
                {
                    Log.Info($"WeChatPayNotify {notify.TradeStatus} => OutTradeNo: " + notify.OutTradeNo);
                    switch (notify.TradeStatus)
                    {
                        case AlipayTradeStatus.Success:
                            order.State.Add((int)EPayOrderState.PaySuccess);
                            break;
                        case AlipayTradeStatus.Wait:
                            order.State.Add((int)EPayOrderState.PayWait);
                            break;
                        case AlipayTradeStatus.Closed:
                        case AlipayTradeStatus.Finished:
                            order.State.Add((int)EPayOrderState.PayFail);
                            break;
                    }
                    await DBManagerComponent.Instance.GetAccountDB().Save(order);
                    await AlipayNotifyResult.ReplySuccess(context.Response);
                    await order.SyncOrderState();
                    order.Dispose();
                    return;
                }

            }
            await AlipayNotifyResult.ReplyFailure(context.Response);
        }
    }
}
