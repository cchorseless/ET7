using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET.Server
{
    public static class CharacterShopComponentFunc
    {
        public static (int, string) BuyShopUnit(this CharacterShopComponent self, C2H_Buy_ShopItem data)
        {
            if (data.ItemCount <= 0)
            {
                return (ErrorCode.ERR_Error, "cant buy 0");
            }

            var shop = self.GetShopUnit(data.ShopConfigId);
            if (shop == null || !shop.IsValid)
            {
                return (ErrorCode.ERR_Error, "cant find shop");
            }

            var item = shop.GetShopSellItem(data.SellConfigId);
            if (item == null)
            {
                return (ErrorCode.ERR_Error, "cant find item");
            }

            return item.BuyItem(self.Character, data.PriceType, data.ItemCount);
        }

        public static TShopUnit GetShopUnit(this CharacterShopComponent self, long shopid)
        {
            var entity = self.GetChild<TShopUnit>(shopid);
            if (entity == null)
            {
                if (self.Character.GetMyServerZone().ShopComp != null)
                {
                    entity = self.Character.GetMyServerZone().ShopComp.GetChild<TShopUnit>(shopid);
                }
            }

            return entity;
        }

        public static TShopUnit GetShopUnit(this CharacterShopComponent self, int shopconfigid)
        {
            if (self.ShopUnit.TryGetValue(shopconfigid, out var shopid))
            {
                return self.GetShopUnit(shopid);
            }

            if (self.Character.GetMyServerZone().ShopComp != null &&
                self.Character.GetMyServerZone().ShopComp.ShopUnit.TryGetValue(shopconfigid, out shopid))
            {
                return self.GetShopUnit(shopid);
            }

            return null;
        }

        public static void LoadAllChild(this CharacterShopComponent self)
        {
            var allShop = LuBanConfigComponent.Instance.Config().ShopConfig.DataList;
            foreach (var config in allShop)
            {
                if (config.IsVaild && config.ShopType > EShopType.ServerZoneShopMax)
                {
                    var shopUnit = self.GetShopUnit(config.Id);
                    if (shopUnit == null)
                    {
                        self.AddShopUnit(config.Id);
                    }
                    else
                    {
                        shopUnit.LoadAllChild();
                    }
                }
            }

            foreach (var k in self.ShopUnit.Keys)
            {
                var config = LuBanConfigComponent.Instance.Config().ShopConfig.GetOrDefault(k);
                if (config == null || !config.IsVaild || config.ShopType <= EShopType.ServerZoneShopMax)
                {
                    self.RemoveShopUnit(k);
                }
            }
        }

        public static TShopUnit AddShopUnit(this CharacterShopComponent self, int shopconfigid)
        {
            if (!self.ShopUnit.ContainsKey(shopconfigid) &&
                LuBanConfigComponent.Instance.Config().ShopConfig.GetOrDefault(shopconfigid) != null)
            {
                var shopUnit = self.AddChild<TShopUnit>();
                shopUnit.ConfigId = shopconfigid;
                shopUnit.IsValid = true;
                shopUnit.CharacterId = self.Character.Id;
                self.ShopUnit.Add(shopconfigid, shopUnit.Id);
                shopUnit.LoadAllChild();
                return shopUnit;
            }

            return null;
        }

        public static void RemoveShopUnit(this CharacterShopComponent self, int shopconfigid)
        {
            if (self.ShopUnit.TryGetValue(shopconfigid, out var sellshopid))
            {
                self.ShopUnit.Remove(shopconfigid);
                var entity = self.GetChild<TShopUnit>(sellshopid);
                if (entity != null)
                {
                    entity.Dispose();
                }
            }
        }
    }
}