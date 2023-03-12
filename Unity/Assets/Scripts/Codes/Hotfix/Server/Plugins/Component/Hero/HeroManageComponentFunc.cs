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
            self.GetAllHeroUnit().ForEach(heroUnit => { heroUnit.LoadAllChild(); });
            self.RefreshRankBattleScore();
        }

        public static void RefreshRankBattleScore(this HeroManageComponent self)
        {
            var serverzone = self.Character.GetMyServerZone();
            var seasonRank = serverzone.RankComp.CurSeasonRank;
            var SumBattleSorceRank = seasonRank.GetRank<TRankSumBattleSorce>((int)ERankType.SumBattleSorceRank);
            SumBattleSorceRank.UpdateRankData(self.Character.Id, self.GetHeroSumBattleScore());
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
            var config = LuBanConfigComponent.Instance.Config().BuildingLevelUpConfig.GetOrDefault(heroname);
            if (config == null)
            {
                return null;
            }
            return self.GetHeroUnit(config.BindHeroId);
        }
      
        public static THeroUnit GetHeroUnit(this HeroManageComponent self, int configid)
        {
            if (self.HeroUnits.TryGetValue(configid, out var entityid))
            {
                return self.GetChild<THeroUnit>(entityid);
            }

            var heroConfig = LuBanConfigComponent.Instance.Config().BuildingLevelUpConfig.GetHeroName(configid);
            if (string.IsNullOrEmpty(heroConfig))
            {
                return self.AddChild<THeroUnit, int>(configid);
            }

            return null;
        }

        public struct FHeroExpResult
        {
            public int HeroConfigId;
            public int ExpCount;
            public bool IsFull;
        }

        public static (int, string) AddHeroExp(this HeroManageComponent self, int configid, int exp)
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

        public static (int, string) AddHeroLevelByComHeroExp(this HeroManageComponent self, int configid)
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
            if (hero == null || hero.HeroEquipComp == null)
            {
                return (ErrorCode.ERR_Error, "hero cant find");
            }

            if (request.IsDressUp)
            {
                if (!long.TryParse(request.EquipId, out var EquipId))
                {
                    return (ErrorCode.ERR_Error, "EquipId error");
                }

                return hero.HeroEquipComp.DressEquip(EquipId, request.Slot);
            }
            else
            {
                return hero.HeroEquipComp.UnDressEquip(request.Slot);
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

        public static (int, string) AddHeroBanDesign(this HeroManageComponent self, int slot, List<int> banConfig)
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
    }
}