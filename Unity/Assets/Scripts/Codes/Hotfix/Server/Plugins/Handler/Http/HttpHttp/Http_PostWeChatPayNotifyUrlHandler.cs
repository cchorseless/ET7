using System;
using System.Collections.Generic;
using System.Net;
using ET.Pay.WeChatPay.V3;
using ET.Pay.WeChatPay.V3.Notify;

namespace ET.Server
{

    [HttpHandler(SceneType.Http, "/WeChatPayNotifyUrl", false)]
    public class Http_PostWeChatPayNotifyUrlHandler : HttpBasePostHandler
    {
        protected override async ETTask Run(Entity domain, HttpListenerContext context)
        {
            var request = context.Request;
            var notify = await WeChatPayComponent.Instance.NotifyV3.ExecuteAsync<WeChatPayTransactionsNotify>(request, WeChatPayComponent.Instance.PayOptions);
            if (long.TryParse(notify.OutTradeNo, out long orderid))
            {
                var order = await WeChatPayComponent.Instance.QueryOrder(orderid);
                if (order != null)
                {
                    Log.Info($"WeChatPayNotify {notify.TradeState} => OutTradeNo: " + notify.OutTradeNo);
                    switch (notify.TradeState)
                    {
                        case WeChatPayTradeState.Success:
                            order.State.Add((int)EPayOrderState.PaySuccess);
                            break;
                        case WeChatPayTradeState.Closed:
                        case WeChatPayTradeState.Revoked:
                        case WeChatPayTradeState.PayError:
                        case WeChatPayTradeState.NotPay:
                            order.State.Add((int)EPayOrderState.PayFail);
                            break;
                    }
                    await DBManagerComponent.Instance.GetAccountDB().Save(order);
                    await WeChatPayNotifyResult.ReplySuccess(context.Response);
                    await order.SyncOrderState();
                    order.Dispose();
                    return;
                }

            }
            await WeChatPayNotifyResult.ReplyFailure(context.Response);
        }
    }
}
