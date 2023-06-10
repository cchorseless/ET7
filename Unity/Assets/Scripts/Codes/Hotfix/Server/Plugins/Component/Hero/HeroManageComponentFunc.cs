using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET.Server
{
    public static class HeroManageComponentFunc
    {
        public static void LoadAllChild(this HeroManageComponent self)
        {
            var allhero = LuBanConfigComponent.Instance.Config().BuildingLevelUpConfig.DataList;
            foreach (var heroconfig in allhero)
            {
                if (heroconfig.IsValid && self.GetHeroUnit(heroconfig.Id) == null)
                {
                    var entity = self.AddChild<THeroUnit, string>(heroconfig.Id);
                    self.HeroUnits.Add(heroconfig.Id, entity.Id);
                }
            }

            self.SumHeroLevel = 0;
            self.GetAllHeroUnit().ForEach(heroUnit =>
            {
                self.SumHeroLevel += heroUnit.Level;
                heroUnit.LoadAllChild();
            });
            self.RefreshRankBattleScore();
        }

        public static void RefreshRankBattleScore(this HeroManageComponent self)
        {
            var serverzone = self.Character.GetMyServerZone();
            var seasonRank = serverzone.RankComp.CurSeasonRank;
            var SeasonBattleSorceRank = seasonRank.GetRank<TRankHeroSumBattleScore>((int)ERankType.HeroSumBattleSorceRank);
            SeasonBattleSorceRank.UpdateRankData(self.Character.Id, self.Character.Name, self.GetHeroSumBattleScore());
        }

        public static List<THeroUnit> GetAllHeroUnit(this HeroManageComponent self)
        {
            var r = new List<THeroUnit>();
            foreach (var entityid in self.HeroUnits.Values)
            {
                var entity = self.GetChild<THeroUnit>(entityid);
                if (entity != null && entity.HeroConfig().IsValid)
                {
                    r.Add(entity);
                }
            }

            return r;
        }

        public static int GetHeroSumBattleScore(this HeroManageComponent self)
        {
            int sum = 0;
            self.GetAllHeroUnit().ForEach(h => { sum += h.BattleScore; });
            return sum;
        }

        public static THeroUnit GetHeroUnit(this HeroManageComponent self, string heroname)
        {
            var heroConfig = LuBanConfigComponent.Instance.Config().BuildingLevelUpConfig.GetOrDefault(heroname);
            if (heroConfig == null || heroConfig.IsValid == false)
            {
                return null;
            }

            if (self.HeroUnits.TryGetValue(heroname, out var entityid))
            {
                return self.GetChild<THeroUnit>(entityid);
            }

            return null;
        }

        public struct FHeroExpResult
        {
            public string HeroConfigId;
            public int ExpCount;
            public bool IsFull;
        }

        public static (int, string) AddHeroExp(this HeroManageComponent self, string configid, int exp)
        {
            var hero = self.GetHeroUnit(configid);
            if (hero == null)
            {
                return (ErrorCode.ERR_Error, "hero cant find");
            }

            var r = new FHeroExpResult() { ExpCount = exp, HeroConfigId = configid };
            if (hero.IsCanLevelUp())
            {
                hero.AddExp(exp);
                r.IsFull = false;
            }
            else
            {
                self.Character.BagComp.AddTItemOrMoney((int)EMoneyType.ComHeroExp, exp / 2);
                r.IsFull = true;
            }

            return (ErrorCode.ERR_Success, JsonHelper.ToLitJson(r));
        }

        public static (int, string) AddHeroLevelByComHeroExp(this HeroManageComponent self, string configid)
        {
            var hero = self.GetHeroUnit(configid);
            if (hero == null)
            {
                return (ErrorCode.ERR_Error, "hero cant find");
            }

            if (!hero.IsCanLevelUp())
            {
                return (ErrorCode.ERR_Error, "hero cant level up");
            }

            var needExp = hero.GetLevelUpExp() - hero.Exp;
            var bagComp = self.Character.BagComp;
            if (bagComp.GetTItemCount((int)EMoneyType.ComHeroExp) < needExp)
            {
                return (ErrorCode.ERR_Error, "ComHeroExp not enough");
            }

            bagComp.AddTItemOrMoney((int)EMoneyType.ComHeroExp, -needExp);
            return self.AddHeroExp(configid, needExp);
        }

        public static (int, string) ChangeHeroDressEquipState(this HeroManageComponent self, C2H_ChangeHeroDressEquipState request)
        {
            var hero = self.Character.HeroManageComp.GetHeroUnit(request.HeroId);
            if (hero == null)
            {
                return (ErrorCode.ERR_Error, "hero cant find");
            }

            if (request.IsDressUp)
            {
                if (!long.TryParse(request.EquipId, out var EquipId))
                {
                    return (ErrorCode.ERR_Error, "EquipId error");
                }

                return hero.DressEquip(EquipId, request.Slot);
            }
            else
            {
                return hero.UnDressEquip(request.Slot);
            }
        }

        public static THeroBanDesign GetHeroBanDesign(this HeroManageComponent self, int slot)
        {
            if (self.HeroBanDesign.Count <= slot || slot < 0)
            {
                return null;
            }

            return self.GetChild<THeroBanDesign>(self.HeroBanDesign[slot]);
        }

        public static (int, string) AddHeroBanDesign(this HeroManageComponent self, int slot, List<string> banConfig)
        {
            if (slot >= 5)
            {
                return (ErrorCode.ERR_Error, "slot out of range");
            }

            var banDesign = self.GetHeroBanDesign(slot);
            if (banDesign == null)
            {
                banDesign = self.AddChild<THeroBanDesign>();
                self.HeroBanDesign.Add(banDesign.Id);
            }

            banDesign.BanConfigInfo.Clear();
            if (banConfig != null)
            {
                banDesign.BanConfigInfo.AddRange(banConfig);
            }

            return (ErrorCode.ERR_Success, "");
        }

        public static (int, string) GetInfoPassPrize(this HeroManageComponent self, int prizeLevel, bool IsOnluKey)
        {
            var prizelevels = new List<int>();
            if (IsOnluKey)
            {
                var levels = LuBanConfigComponent.Instance.Config().InfoPassLevelUpConfig.DataMap.Keys;
                foreach (var level in levels)
                {
                    if (level <= self.SumHeroLevel && !self.HeroLevelPrizeGet.Contains(level))
                    {
                        prizelevels.Add(level);
                    }
                }

                if (prizelevels.Count == 0)
                {
                    return (ErrorCode.ERR_Error, "no prize can get");
                }
            }
            else
            {
                if (self.SumHeroLevel < prizeLevel)
                {
                    return (ErrorCode.ERR_Error, "SumHeroLevel not enough");
                }

                if (self.HeroLevelPrizeGet.Contains(prizeLevel))
                {
                    return (ErrorCode.ERR_Error, "prize_had_get");
                }

                var config = LuBanConfigComponent.Instance.Config().InfoPassLevelUpConfig.GetOrDefault(prizeLevel);
                if (config == null)
                {
                    return (ErrorCode.ERR_Error, "cant find config");
                }

                prizelevels.Add(prizeLevel);
            }

            var itemInfo = new List<FItemInfo>();
            foreach (var configID in prizelevels)
            {
                var config = LuBanConfigComponent.Instance.Config().InfoPassLevelUpConfig.GetOrDefault(configID);
                if (config != null)
                {
                    itemInfo.Add(new FItemInfo(config.TaskComPrize.ItemConfigId, config.TaskComPrize.ItemCount));
                }
            }

            var r = self.Character.BagComp.AddTItemOrMoney(itemInfo);
            if (r.Item1 == ErrorCode.ERR_Success)
            {
                self.HeroLevelPrizeGet.AddRange(prizelevels);
                self.Character.SyncHttpEntity(self);
                return (ErrorCode.ERR_Success, itemInfo.ToListString());
            }

            return (ErrorCode.ERR_Error, "item add fail");
        }
    }
}