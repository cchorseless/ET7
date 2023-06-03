using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET.Server
{
    public enum EItemPrizeState
    {
        CanGet = 1,
        CanNotGet = 2,
        HadGet = 4,
        OutOfDate = 8,
    }

    public static class TItemFunc
    {
        public static cfg.Item.ItemConfigRecord Config(this TItem self)
        {
            return LuBanConfigComponent.Instance.Config().ItemConfig.GetOrDefault(self.ConfigId);
        }

        public static void ChangeItemCount(this TItem self, int count)
        {
            self.ItemCount += count;
            if (self.ItemCount <= 0)
            {
                if (self.IsInBag())
                {
                    self.BagComp.RemoveTItem(self);
                }
            }
        }

        public static bool IsInBag(this TItem self)
        {
            return self.BagComp != null && self.BagComp.Items.Contains(self.Id);
        }

        public static void ActiveItem(this TItem self)
        {
            if (self.ItemQuality == 0)
            {
                self.ItemQuality = 1;
            }

            foreach (var awakescript in self.Config().AwakeScript)
            {
                switch (awakescript.ScriptName)
                {
                    case cfg.EEnum.EItemAwakeScript.BindEquip:
                        ((TEquipItem)self).BindEquip();
                        break;
                    case cfg.EEnum.EItemAwakeScript.AddTitle:
                        self.AddTitle(awakescript.ScriptValue);
                        break;
                    case cfg.EEnum.EItemAwakeScript.AddHeroExp:
                        self.AddHeroExp(awakescript.ScriptValue);
                        break;
                }
            }
        }

        public static void AddTitle(this TItem self, List<int> titleGroup)
        {
            titleGroup.ForEach(titleConfigId => { self.BagComp.Character.TitleComp.AddTitle(titleConfigId); });
            self.Dispose();
        }

        public static void AddHeroExp(this TItem self, List<int> argsList)
        {
            int exp = argsList[0];
            string heroname = self.Config().BindHeroName;
            var hero = self.BagComp.Character.HeroManageComp.GetHeroUnit(heroname);
            if (hero == null)
            {
                Log.Warning($"AddHeroExp,hero is null,configid:{self.ConfigId} {heroname}");
                return;
            }
            int exp_sum = exp * self.ItemCount;
            self.BagComp.Character.HeroManageComp.AddHeroExp(hero.ConfigId, exp_sum);
            self.Dispose();
        }

        public static (int, string) ApplyUse(this TItem self, int count)
        {
            if (self.IsDisposed)
            {
                return (ErrorCode.ERR_Error, "item is destroy");
            }

            if (count <= 0)
            {
                return (ErrorCode.ERR_Error, "count is 0");
            }

            if (!self.IsInBag())
            {
                return (ErrorCode.ERR_Error, "not in bag");
            }

            if (self.ItemCount < count || self.Config().UseScript == cfg.EEnum.EItemUseScript.None)
            {
                return (ErrorCode.ERR_Error, "count is not enough");
            }

            (int, string) r = (ErrorCode.ERR_Error, "cant use item");
            bool needCostItem = false;
            switch (self.Config().UseScript)
            {
                case cfg.EEnum.EItemUseScript.GetPrize:
                    r = self.GetPrize(self.Config().UseArgs, count);
                    needCostItem = (r.Item1 == ErrorCode.ERR_Success);
                    break;
                case cfg.EEnum.EItemUseScript.AddBuff:
                    r = self.AddBuff(self.Config().UseArgs, count);
                    needCostItem = (r.Item1 == ErrorCode.ERR_Success);
                    break;
                case cfg.EEnum.EItemUseScript.DressUp:
                    r = self.DressUp();
                    break;
                case cfg.EEnum.EItemUseScript.CostCount:
                    needCostItem = true;
                    r = (ErrorCode.ERR_Success, "use success");
                    break;
            }

            if (needCostItem)
            {
                self.ChangeItemCount(-count);
            }
            return r;
        }

        public static (int, string) GetPrize(this TItem self, List<int> prizeidList, int count)
        {
            if (prizeidList.Count == 0)
            {
                return (ErrorCode.ERR_Error, "prizeidList IsEmpty");
            }

            if (!self.IsInBag())
            {
                return (ErrorCode.ERR_Error, "not in bag");
            }

            var prizeItems = new List<FItemInfo>();
            foreach (var prizeid in prizeidList)
            {
                var pool = LuBanConfigComponent.Instance.Config().ItemPrizePoolConfig.GetOrDefault(prizeid);
                if (pool != null)
                {
                    pool.GetRandomItemId(count).ForEach(iteminfo =>
                    {
                        if (iteminfo != null)
                        {
                            prizeItems.Add(new FItemInfo(iteminfo.ItemConfigId, iteminfo.ItemCount));
                        }
                    });
                }
            }

            return self.BagComp.AddTItemOrMoney(prizeItems);
        }

        public static (int, string) DressUp(this TItem self)
        {
            if (self is TEquipItem)
            {
                TEquipItem item = (TEquipItem)self;
                var heroManageComp = self.BagComp.Character.HeroManageComp;
                var hero = heroManageComp.GetHeroUnit(item.Config().BindHeroName);
                if (hero == null || hero.HeroEquipComp == null)
                {
                    return (ErrorCode.ERR_Error, "hero cant find");
                }

                return hero.HeroEquipComp.DressEquip(self.Id, (int)item.EquipConfig().EquipSlot);
            }

            return (ErrorCode.ERR_Error, "item cant dress up");
        }

        public static (int, string) AddBuff(this TItem self, List<int> argsList, int count)
        {
            if (argsList.Count == 0 || count <= 0)
            {
                return (ErrorCode.ERR_Error, "argsList error");
            }

            for (int i = 0; i < argsList.Count; i++)
            {
                var buffConfigId = argsList[i];
                for (int j = 0; j < count; j++)
                {
                    self.BagComp.Character.BuffComp.AddBuff(buffConfigId);
                }
            }

            return (ErrorCode.ERR_Success, JsonHelper.ToLitJson(argsList));
        }
    }
}