using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ET.Server
{
    [ObjectSystem]
    public class TShopSellItemLoadSystem: LoadSystem<TShopSellItem>
    {
        /// <summary>
        /// 重载配置刷新
        /// </summary>
        /// <param name="self"></param>
        protected override void Load(TShopSellItem self)
        {
            self.RefreshConfigJson();
        }
    }

    public static class TShopSellItemFunc
    {
        public static void RefreshConfigJson(this TShopSellItem self)
        {
            var config = self.Config();
            if (config != null)
            {
                //self.ConfigJson = JsonHelper.ToLitJson(config);
            }
        }

        public static async ETTask<(int, string)> BuyItem(this TShopSellItem self, TCharacter character, int pricetype, int count, int paytype)
        {
            var config = self.Config();
            if (config == null)
            {
                return (ErrorCode.ERR_Error, "config cant find");
            }

            if (config.SellCount > 0 && self.BuyCount + count > config.SellCount)
            {
                return (ErrorCode.ERR_Error, "count no full");
            }

            if (config.SellValidTime > 0 && config.SellStartTime + config.SellValidTime <= TimeHelper.ServerNow())
            {
                return (ErrorCode.ERR_Error, "time no valid");
            }

            if (config.CostType == Conf.EEnum.EMoneyType.Money)
            {
                return await self.CreatePayOrder(character, pricetype, count, paytype);
            }

            var cost = config.RealPrice;
            if (pricetype == (int)Conf.EEnum.EShopPriceType.OverSeaPrice)
            {
                cost = config.OverSeaRealPrice;
            }

            cost *= count;
            if (cost < 0)
            {
                return (ErrorCode.ERR_Error, "cost cant smaller than 0");
            }

            var hadMoney = character.DataComp.NumericComp.GetAsInt((int)config.CostType);
            if (cost > hadMoney)
            {
                return (ErrorCode.ERR_Error, "cost no full");
            }

            var addstate = character.BagComp.AddTItemOrMoney(config.ItemConfigId, config.ItemCount * count);
            if (addstate.Item1 == ErrorCode.ERR_Error)
            {
                return (ErrorCode.ERR_Error, "add item fail");
            }

            if (cost > 0)
            {
                character.DataComp.ChangeNumeric((int)config.CostType, -cost);
            }

            self.BuyCount += count;
            character.SyncHttpEntity(self);
            // 统计订单物品
            character.GetMyServerZone().DataStatisticComp.GetCurDataItem().UpdateShopSellItem(config.ItemConfigId, config.ItemCount * count);
            return (ErrorCode.ERR_Success, "");
        }

        public static async ETTask<(int, string)> CreatePayOrder(this TShopSellItem self, TCharacter character, int pricetype, int count, int paytype)
        {
            string message = "";

            long orderid = 0;
            var config = self.Config();
            var money = config.RealPrice;
            var title = "商品购买";
            if (pricetype == (int)Conf.EEnum.EShopPriceType.OverSeaPrice)
            {
                money = config.OverSeaRealPrice;
                title = "Buy Item";
            }

            money = money * count;
            switch (paytype)
            {
                case (int)EPayOrderSourceType.AliPay_QrCode:
                    (orderid, message) = await AliPayComponent.Instance.GetQrCodePay(character, title, money,
                        new FItemInfo(config.ItemConfigId, config.ItemCount));
                    break;
                case (int)EPayOrderSourceType.WeChat_QrCodeV3:
                    (orderid, message) = await WeChatPayComponent.Instance.GetQrCodePayV3(character, title, money,
                        new FItemInfo(config.ItemConfigId, config.ItemCount));
                    break;
                case (int)EPayOrderSourceType.WeChat_H5PayV3:
                    (orderid, message) = await WeChatPayComponent.Instance.GetH5PayV3(character, title, money,
                        new FItemInfo(config.ItemConfigId, config.ItemCount));
                    break;
                default:
                    message = "qrcode fail, not support this paytype";
                    break;
            }

            var json = JsonHelper.GetLitObject();
            json["message"] = message;
            json["orderid"] = orderid.ToString();
            int code = orderid != 0? ErrorCode.ERR_Success : ErrorCode.ERR_Error;
            return (code, JsonHelper.ToLitJson(json));
        }

        public static Conf.Shop.ShopSellItemBean Config(this TShopSellItem self)
        {
            var shopinfo = LuBanConfigComponent.Instance.Config().ShopConfig.GetOrDefault(self.ShopId);
            if (shopinfo != null)
            {
                return shopinfo.Sellinfo.Find(info => info.SellConfigid == self.ConfigId);
            }

            return null;
        }
    }
}