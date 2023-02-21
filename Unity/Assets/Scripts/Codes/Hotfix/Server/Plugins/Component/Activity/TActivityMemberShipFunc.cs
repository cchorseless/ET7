using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET.Server
{


    public static class TActivityMemberShipFunc
    {
        public static void LoadAllChild(this TActivityMemberShip self)
        {
            self.SetNeverOutOfDate();

        }

        public static async ETTask<(int, string)> BuyMemberShip(this TActivityMemberShip self, TCharacter character, int buyType, int payType)
        {
            await ETTask.CompletedTask;
            if (!self.IsValid())
            {
                return (ErrorCode.ERR_Error, "active not valid");
            }
            var activityData = character.ActivityComp.GetActivityData<TActivityMemberShipData>(EActivityType.TActivityMemberShip);
            if (activityData == null || !activityData.IsValid())
            {
                return (ErrorCode.ERR_Error, "activityData not valid");
            }
            int money_fen;
            switch (buyType)
            {
                case 1:
                    money_fen = 30 * 100;
                    break;
                case 2:
                    money_fen = 60 * 100;
                    break;
                default:
                    return (ErrorCode.ERR_Error, "buyType not valid");
            }
            switch (payType)
            {
                case (int)EPayOrderSourceType.AliPay_QrCode:
                    var aliQrcode = await AliPayComponent.Instance.GetQrCodePay(character, "MemberShip", money_fen, EPayOrderLabel.PayForMemberShip);
                    return (ErrorCode.ERR_Success, aliQrcode);

                case (int)EPayOrderSourceType.WeChat_QrCodeV3:
                    var weChatQrcode = await WeChatPayComponent.Instance.GetQrCodePayV3(character, "MemberShip", money_fen, EPayOrderLabel.PayForMemberShip);
                    return (ErrorCode.ERR_Success, weChatQrcode);
                default:
                    return (ErrorCode.ERR_Error, "payType not valid");
            }
        }

        public static void OnBuyMemberShip(this TActivityMemberShip self, TCharacter character, TPayOrderItem payOrder)
        {
            var activityData = character.ActivityComp.GetActivityData<TActivityMemberShipData>(EActivityType.TActivityMemberShip);
            if (activityData == null || !activityData.IsValid())
            {
                return;
            }
            if (payOrder.IsPaySuccess())
            {
                var time = 0;
                switch (payOrder.TotalAmount)
                {
                    case 1:
                        time = 30 * 24 * 3600;
                        break;
                    case 2:
                        time = 300 * 24 * 3600;
                        break;
                }
                if (activityData.IsVip())
                {
                    activityData.VipEndTime += time;
                }
                else
                {
                    activityData.VipStartTime = TimeHelper.ServerNow() / 1000;
                    activityData.VipEndTime = activityData.VipStartTime + time;
                }
                character.SyncHttpEntity(activityData);
            }
            character.SyncHttpEntity(payOrder);
        }

    }
}
