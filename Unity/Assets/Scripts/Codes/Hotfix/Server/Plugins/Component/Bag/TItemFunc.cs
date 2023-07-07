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
        public static Conf.Item.ItemConfigRecord Config(this TItem self)
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

            if (self.IsDisposed)
            {
                self.IsValid = false;
            }

            if (self.BagComp?.Character != null)
            {
                self.BagComp.Character.SyncHttpEntity(self);
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
                    case Conf.EEnum.EItemAwakeScript.BindEquip:
                        ((TEquipItem)self).BindEquip();
                        break;
                    case Conf.EEnum.EItemAwakeScript.AddTitle:
                        // self.AddTitle(awakescript.ScriptValue);
                        break;
                    case Conf.EEnum.EItemAwakeScript.AddHeroExp:
                        self.AddHeroExp(awakescript.ScriptValue);
                        break;
                    case Conf.EEnum.EItemAwakeScript.ActiveCourier:
                        self.ActiveCourier(awakescript.ScriptValue);
                        break;
                    case Conf.EEnum.EItemAwakeScript.ActiveSkin:
                        self.ActiveSkin(awakescript.ScriptValue);
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
            }
            else
            {
                int exp_sum = exp * self.ItemCount;
                self.BagComp.Character.HeroManageComp.AddHeroExp(hero.ConfigId, exp_sum);
            }
            self.Dispose();
        }

        public static void ActiveCourier(this TItem self, List<int> argsList)
        {
            int exp = argsList[0];
            string couriername = self.Config().BindHeroName;
            var character = self.BagComp.Character;
            if (character.DataComp.AllCouriers.Contains(couriername))
            {
                int StarStone = self.Config().DecomposeStarStone;
                if (StarStone > 0)
                {
                    self.BagComp.AddTItemOrMoney((int)Conf.EEnum.EMoneyType.StarStone, StarStone);
                }
            }
            else
            {
                character.DataComp.AllCouriers.Add(couriername);
                character.SyncHttpEntity(character.DataComp);
            }

            self.Dispose();
        }

        public static void ActiveSkin(this TItem self, List<int> argsList)
        {
            int validtime = argsList[0];
            string wearconfigid = self.Config().BindHeroName;
            var config = LuBanConfigComponent.Instance.Config().WearableConfig.GetOrDefault(wearconfigid);
            if (config != null)
            {
                var heroname = config.UsedByHeroes.Replace("npc_", "building_");
                var hero = self.BagComp.Character.HeroManageComp.GetHeroUnit(heroname);
                if (hero == null)
                {
                    Log.Warning($"AddHeroExp,hero is null,configid:{self.ConfigId} {heroname}");
                }
                else
                {
                    if (hero.Skins.TryGetValue(wearconfigid, out var time))
                    {
                        // 永久皮肤
                        if (time < 0)
                        {
                            int StarStone = self.Config().DecomposeStarStone;
                            if (StarStone > 0)
                            {
                                self.BagComp.AddTItemOrMoney((int)Conf.EEnum.EMoneyType.StarStone, StarStone);
                            }
                        }
                        else
                        {
                            // 替换为永久皮肤
                            if (validtime < 0)
                            {
                                hero.Skins[wearconfigid] = validtime;
                            }
                            else
                            {
                                hero.Skins[wearconfigid] = Math.Max(time, TimeHelper.ServerNow()) + validtime;
                            }
                        }
                    }
                    else
                    {
                        // 替换为永久皮肤
                        if (validtime < 0)
                        {
                            hero.Skins.Add(wearconfigid, validtime);
                        }
                        else
                        {
                            hero.Skins.Add(wearconfigid, TimeHelper.ServerNow() + validtime);
                        }
                    }

                    // 自动穿戴
                    if (string.IsNullOrEmpty(hero.SkinConfigId))
                    {
                        hero.DressSkin(wearconfigid);
                    }
                }
            }

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

            if (self.ItemCount < count || self.Config().UseScript == Conf.EEnum.EItemUseScript.None)
            {
                return (ErrorCode.ERR_Error, "count is not enough");
            }

            (int, string) r = (ErrorCode.ERR_Error, "cant use item");
            bool needCostItem = false;
            switch (self.Config().UseScript)
            {
                case Conf.EEnum.EItemUseScript.GetPrize:
                    r = self.GetPrize(self.Config().UseArgs, count);
                    needCostItem = (r.Item1 == ErrorCode.ERR_Success);
                    break;
                case Conf.EEnum.EItemUseScript.AddBuff:
                    r = self.AddBuff(self.Config().UseArgs, count);
                    needCostItem = (r.Item1 == ErrorCode.ERR_Success);
                    break;
                case Conf.EEnum.EItemUseScript.DressUp:
                    r = self.DressUp();
                    break;
                case Conf.EEnum.EItemUseScript.CostCount:
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