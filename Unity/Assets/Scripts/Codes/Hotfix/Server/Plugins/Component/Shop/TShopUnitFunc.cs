using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET.Server
{
    public static class TShopUnitFunc
    {
        public static cfg.Shop.ShopConfigRecord Config(this TShopUnit self)
        {
            return LuBanConfigComponent.Instance.Config().ShopConfig.GetOrDefault(self.ConfigId);
        }
        public static void RefreshConfigJson(this TShopUnit self)
        {
            var config = self.Config();
            if (config != null)
            {
                //self.ConfigJson = JsonHelper.ToLitJson(config);
            }
        }

        public static void LoadAllChild(this TShopUnit self)
        {
            if (self.Config() == null) { return; }

            foreach (var config in self.Config().Sellinfo)
            {
                var entity = self.GetShopSellItem(config.SellConfigid);
                if (entity != null)
                {
                    self.UpdateShopSellItem(config.SellConfigid);
                }
                else
                {
                    if (config.IsVaild)
                    {
                        self.AddShopSellItem(config.SellConfigid);
                    }
                }
            }
            foreach (var k in self.ShopSellItem.Keys)
            {
                if (self.Config().Sellinfo.Find(s => s.SellConfigid == k) == null)
                {
                    self.RemoveShopSellItem(k);
                }
            }

        }

        public static TShopSellItem GetShopSellItem(this TShopUnit self, int sellconfigid)
        {
            if (self.ShopSellItem.TryGetValue(sellconfigid, out var sellitemid))
            {
                return self.GetChild<TShopSellItem>(sellitemid);
            }
            return null;
        }

        public static TShopSellItem AddShopSellItem(this TShopUnit self, int sellitemid)
        {
            if (!self.ShopSellItem.ContainsKey(sellitemid))
            {
                var shopSellItem = self.AddChild<TShopSellItem>();

                shopSellItem.ConfigId = sellitemid;
                shopSellItem.ShopId = self.ConfigId;
                shopSellItem.CharacterId = self.CharacterId;
                //shopSellItem.RefreshConfigJson();
                self.ShopSellItem.Add(sellitemid, shopSellItem.Id);
                return shopSellItem;
            }
            return null;
        }

        public static void UpdateShopSellItem(this TShopUnit self, int sellconfigid)
        {
            var entity = self.GetShopSellItem(sellconfigid);
            if (entity != null)
            {
                if (!entity.Config().IsVaild)
                {
                    self.RemoveShopSellItem(sellconfigid);
                }
                else
                {
                }
            }
        }

        public static void RemoveShopSellItem(this TShopUnit self, int sellconfigid)
        {
            if (self.ShopSellItem.TryGetValue(sellconfigid, out var sellitemid))
            {
                self.ShopSellItem.Remove(sellconfigid);
                var entity = self.GetChild<TShopSellItem>(sellitemid);
                if (entity != null)
                {
                    entity.Dispose();
                }
            }
        }


    }
}
