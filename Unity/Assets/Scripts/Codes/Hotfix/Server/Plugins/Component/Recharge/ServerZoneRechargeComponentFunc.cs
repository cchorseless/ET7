using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET.Server
{
    public enum ERechargeType
    {
        Recharge_30 = 30,
        Recharge_68 = 68,
        Recharge_128 = 128,
        Recharge_198 = 198,
        Recharge_328 = 328,
        Recharge_648 = 648,
        Recharge_9999 = 9999,
    }


    public static class ServerZoneRechargeComponentFunc
    {
        public static void LoadAllChild(this ServerZoneRechargeComponent self)
        {

        }

        public static async ETTask<(int, string)> RechargeMetaStone(this ServerZoneRechargeComponent self, TCharacter character, int buyType, int payType)
        {
            await ETTask.CompletedTask;
            int money_fen;
            switch (buyType)
            {
                case (int)ERechargeType.Recharge_30:
                case (int)ERechargeType.Recharge_68:
                case (int)ERechargeType.Recharge_128:
                case (int)ERechargeType.Recharge_198:
                case (int)ERechargeType.Recharge_328:
                case (int)ERechargeType.Recharge_648:
                case (int)ERechargeType.Recharge_9999:
                    money_fen = buyType * 100;
                    break;
                default:
                    return (ErrorCode.ERR_Error, "buyType not valid");
            }
            switch (payType)
            {
                case (int)EPayOrderSourceType.AliPay_QrCode:
                    var aliQrcode = await AliPayComponent.Instance.GetQrCodePay(character, "Recharge", money_fen, EPayOrderLabel.PayForMetaStone);
                    return (ErrorCode.ERR_Success, aliQrcode);

                case (int)EPayOrderSourceType.WeChat_QrCodeV3:
                    var weChatQrcode = await WeChatPayComponent.Instance.GetQrCodePayV3(character, "Recharge", money_fen, EPayOrderLabel.PayForMetaStone);
                    return (ErrorCode.ERR_Success, weChatQrcode);
                default:
                    return (ErrorCode.ERR_Error, "payType not valid");
            }
        }

        public static void OnRechargeMetaStone(this ServerZoneRechargeComponent self, TCharacter character, TPayOrderItem payOrder)
        {
            var rechargeComp = character.RechargeComp;
            if (rechargeComp == null)
            {
                return;
            }
            if (payOrder.IsPaySuccess())
            {
                switch (payOrder.TotalAmount / 100)
                {
                    case (int)ERechargeType.Recharge_30:
                    case (int)ERechargeType.Recharge_68:
                    case (int)ERechargeType.Recharge_128:
                    case (int)ERechargeType.Recharge_198:
                    case (int)ERechargeType.Recharge_328:
                    case (int)ERechargeType.Recharge_648:
                    case (int)ERechargeType.Recharge_9999:
                        character.RechargeComp.RechargeMetaStoneSuccess(payOrder);
                        break;
                }
            }
            character.SyncHttpEntity(payOrder);
        }

    }
}
