using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ET.Server
{


    [ObjectSystem]
    public class TShopSellItemLoadSystem : LoadSystem<TShopSellItem>
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


        public static (int, string) BuyItem(this TShopSellItem self, TCharacter character, int pricetype, int count)
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
            var cost = config.RealPrice;
            if (pricetype == (int)cfg.EEnum.EShopPriceType.OverSeaPrice)
            {
                cost = config.OverSeaRealPrice;
            }
            cost *= count;
            if (cost < 0)
            {
                return (ErrorCode.ERR_Error, "cost cant smaller than 0");
            }
            if (config.CostType == cfg.EEnum.EMoneyType.Money)
            {
                return (ErrorCode.ERR_Error, "Money not ready");

            }
            var hadMoney = character.DataComp.NumericComp.GetAsInt((int)config.CostType);
            if (cost > hadMoney)
            {
                return (ErrorCode.ERR_Error, "cost no full");
            }
            var addstate = character.BagComp.AddTItemOrMoney(config.ItemConfigId, config.ItemCount * count);
            if (addstate.Item1 == false)
            {
                return (ErrorCode.ERR_Error, "add item fail");
            }
            if (cost > 0)
            {
                character.DataComp.ChangeNumeric((int)config.CostType, -cost);
            }
            self.BuyCount += count;
            character.SyncHttpEntity(self);

            return (ErrorCode.ERR_Success, "");
        }


        public static cfg.Shop.ShopSellItemBean Config(this TShopSellItem self)
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
