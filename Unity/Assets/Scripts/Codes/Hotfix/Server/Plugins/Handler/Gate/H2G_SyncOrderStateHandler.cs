using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET.Server
{
    [ActorMessageHandler(SceneType.Gate)]
    public class H2G_SyncOrderStateHandler: AMActorRpcHandler<Scene, Actor_SyncOrderStateRequest, Actor_SyncOrderStateResponse>
    {
        protected override async ETTask Run(Scene scene, Actor_SyncOrderStateRequest request, Actor_SyncOrderStateResponse response)
        {
            TPayOrderItem order = null;
            if (request.OrderPaySource == (int)EPayOrderSourceType.AliPay_QrCode)
            {
                order = await AliPayComponent.Instance.GetOrder(request.OrderId);
            }
            else if (request.OrderPaySource == (int)EPayOrderSourceType.WeChat_QrCodeV3)
            {
                order = await WeChatPayComponent.Instance.GetOrder(request.OrderId);
            }

            if (order != null)
            {
                var isfinish = order.UpdateOrderState(request.OrderState);
                if (isfinish)
                {
                    order.PayFinishAddItem();
                    await order.SaveAndExit();
                }
                else
                {
                    await order.SaveAndExit(false);
                }
              
            }

            await ETTask.CompletedTask;
        }
    }
}