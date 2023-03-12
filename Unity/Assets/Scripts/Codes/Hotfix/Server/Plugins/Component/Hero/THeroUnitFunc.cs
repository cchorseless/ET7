using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ET.Server
{
    public static class THeroUnitFunc
    {
        public static void LoadAllChild(this THeroUnit self)
        {
            self.HeroEquipComp.LoadAllChild();
            self.HeroTalentComp.LoadAllChild();
            self.RefreshBattleScore();
            self.RefreshHeroBattleScoreRank();
        }
        public static string BindHeroName(this THeroUnit self)
        {
            return  LuBanConfigComponent.Instance.Config().BuildingLevelUpConfig.GetHeroName(self.ConfigId);
        }
        
        public static cfg.Dota.BuildingLevelUpConfigRecord HeroConfig(this THeroUnit self)
        {
            return LuBanConfigComponent.Instance.Config().BuildingLevelUpConfig.GetOrDefault(self.BindHeroName());
        }

        public static bool IsCanLevelUp(this THeroUnit self)
        {
            return self.Level < THeroUnitConfig.MaxLevel;
        }

        public static void AddExp(this THeroUnit self, int exp)
        {
            self.TotalExp += exp;
            self.Exp += exp;
            while (self.Exp >= self.GetLevelUpExp() && self.GetLevelUpExp() > 0)
            {
                self.Exp -= self.GetLevelUpExp();
                self.Level += 1;
                self.Character.ActivityComp.GetActivityData<TActivityHeroRecordLevelData>(EActivityType.TActivityHeroRecordLevel)?.AddHeroSumLevel();
            }
        }

        public static int GetLevelUpExp(this THeroUnit self)
        {
            var needExp = LuBanConfigComponent.Instance.Config().HeroLevelUpConfig.GetOrDefault(self.Level + 1);
            if (needExp != null)
            {
                return needExp.Exp;
            }
            return 0;
        }


        public static void RefreshBattleScore(this THeroUnit self)
        {
        }

        public static void RefreshHeroBattleScoreRank(this THeroUnit self)
        {
            var serverzone = self.Character.GetMyServerZone();
            var seasonRank = serverzone.RankComp.CurSeasonRank;
            var HeroBattleSorceRankGroup = seasonRank.GetRank<TRankHeroBattleScoreGroup>((int)ERankType.HeroBattleSorceRankGroup);
            HeroBattleSorceRankGroup.UpdateHeroRankData(self.ConfigId, self.Character.Id, self.BattleScore);
        }
    }

    [ObjectSystem]
    public class THeroUnitAwakeSystem : AwakeSystem<THeroUnit, int>
    {
        protected override void Awake(THeroUnit self, int configid)
        {
            self.ConfigId = configid;
            self.AddComponent<HeroEquipComponent>();
            self.AddComponent<HeroTalentComponent>();
        }
    }
}
