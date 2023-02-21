using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET.Server
{
    [ObjectSystem]
    public class ServerZoneShopComponentAwakeSystem : AwakeSystem<ServerZoneShopComponent>
    {
        protected override void Awake(ServerZoneShopComponent self)
        {
            //self.AddComponent<GhostEntityComponent, int>(self.ServerZone.ServerID);
        }
    }
    public static class ServerZoneShopComponentFunc
    {
        public static void LoadAllChild(this ServerZoneShopComponent self)
        {
            var allShop = LuBanConfigComponent.Instance.Config().ShopConfig.DataList;
            foreach (var config in allShop)
            {
                if (config.IsVaild  && config.ShopType < EShopType.ServerZoneShopMax)
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
                if (config == null || !config.IsVaild || config.ShopType >= EShopType.ServerZoneShopMax)
                {
                    self.RemoveShopUnit(k);
                }
            }
        }

        public static TShopUnit GetShopUnit(this ServerZoneShopComponent self, int shopconfigid)
        {
            if (self.ShopUnit.TryGetValue(shopconfigid, out var shopid))
            {
                return self.GetChild<TShopUnit>(shopid);
            }
            return null;
        }

        public static TShopUnit AddShopUnit(this ServerZoneShopComponent self, int shopconfigid)
        {
            if (!self.ShopUnit.ContainsKey(shopconfigid) &&
                LuBanConfigComponent.Instance.Config().ShopConfig.GetOrDefault(shopconfigid) != null)
            {
                var shopUnit = self.AddChild<TShopUnit>();
                //shopUnit.AddComponent<GhostEntityComponent, int>(self.ServerZone.ServerID);
                shopUnit.ConfigId = shopconfigid;
                self.ShopUnit.Add(shopconfigid, shopUnit.Id);
                shopUnit.LoadAllChild();
                return shopUnit;

            }
            return null;
        }


        public static void RemoveShopUnit(this ServerZoneShopComponent self, int shopconfigid)
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
