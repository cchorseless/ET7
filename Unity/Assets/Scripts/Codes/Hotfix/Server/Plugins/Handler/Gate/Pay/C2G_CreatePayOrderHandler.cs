using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET.Server
{
    [MessageHandler(SceneType.Gate)]
    public class C2G_CreatePayOrderHandler : AMRpcHandler<C2G_CreatePayOrder, G2C_CreatePayOrder>
    {
        protected override async ETTask Run(Session session, C2G_CreatePayOrder request, G2C_CreatePayOrder response)
        {
            await ETTask.CompletedTask;
            try
            {
                Player player = session.GetComponent<SessionPlayerComponent>().GetMyPlayer();
                TCharacter character = player.GetMyCharacter();
                if (character == null || request.Money <= 0 || request.Title == null)
                {
                    await ETTask.CompletedTask;
                    return;
                }
                int payType = request.PayType;
                switch (payType)
                {
                    case (int)EPayOrderSourceType.AliPay_QrCode:
                        response.Message = await AliPayComponent.Instance.GetQrCodePay(player.GetMyCharacter(), request.Title, request.Money);
                        break;
                    case (int)EPayOrderSourceType.WeChat_QrCodeV3:
                        response.Message = await WeChatPayComponent.Instance.GetQrCodePayV3(player.GetMyCharacter(), request.Title, request.Money);
                        break;
                    case (int)EPayOrderSourceType.WeChat_H5PayV3:
                        response.Message = await WeChatPayComponent.Instance.GetH5PayV3(player.GetMyCharacter(), request.Title, request.Money);
                        break;

                    default:
                        break;
                }
            }
            catch (Exception e)
            {
                ReplyError(response, e);
            }
        }
    }
}
