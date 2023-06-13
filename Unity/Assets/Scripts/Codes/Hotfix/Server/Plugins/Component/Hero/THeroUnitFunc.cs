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
            // self.HeroEquipComp.LoadAllChild();
            // self.HeroTalentComp.LoadAllChild();
        }

        public static cfg.Dota.BuildingLevelUpConfigRecord HeroConfig(this THeroUnit self)
        {
            return LuBanConfigComponent.Instance.Config().BuildingLevelUpConfig.GetOrDefault(self.ConfigId);
        }

        public static bool IsCanLevelUp(this THeroUnit self)
        {
            return self.Level < THeroUnitConfig.MaxLevel;
        }

        public static void AddBattleScore(this THeroUnit self, int score)
        {
            self.BattleScore += score;
            self.Character.RankComp.UpdataRankData((int)ERankType.HeroSumBattleSorceRank, self.HeroManageComp.GetHeroSumBattleScore());
        }

        public static void AddExp(this THeroUnit self, int exp)
        {
            self.TotalExp += exp;
            self.Exp += exp;
            while (self.Exp >= self.GetLevelUpExp() && self.GetLevelUpExp() > 0)
            {
                self.Exp -= self.GetLevelUpExp();
                self.Level += 1;
                self.HeroManageComp.SumHeroLevel += 1;
            }

            self.HeroManageComp.Character.SyncHttpEntity(self);
        }

        public static int GetLevelUpExp(this THeroUnit self)
        {
            var needExp = LuBanConfigComponent.Instance.Config().BuildingLevelUpExpConfig.GetOrDefault(self.Level);
            if (needExp != null)
            {
                return needExp.Exp;
            }

            return 0;
        }

        public static (int, string) DressSkin(this THeroUnit self, string skinConfigid)
        {
            if (!self.Skins.TryGetValue(skinConfigid, out var time) || (time != -1 && time < TimeHelper.ServerNow()))
            {
                return (ErrorCode.ERR_Error, "skin lock");
            }

            self.SkinConfigId = skinConfigid;
            return (ErrorCode.ERR_Success, "");
        }

        public static (int, string) DressEquip(this THeroUnit self, long entityid, int slot)
        {
            if (!self.IsValidSlot(slot))
            {
                return (ErrorCode.ERR_Error, "slot is not valid");
            }

            var equip = self.Character.BagComp.GetChild<TItem>(entityid);
            if (equip == null)
            {
                return (ErrorCode.ERR_Error, "equip not in bag");
            }

            self.Equips[slot] = entityid;
            return (ErrorCode.ERR_Success, "");
        }

        public static (int, string) UnDressEquip(this THeroUnit self, int slot)
        {
            if (!self.IsValidSlot(slot))
            {
                return (ErrorCode.ERR_Error, "slot is not valid");
            }

            self.Equips[slot] = 0;
            return (ErrorCode.ERR_Success, "");
        }

        public static bool IsValidSlot(this THeroUnit self, int slot)
        {
            return slot > (int)cfg.EEnum.EEquipSolt.None && slot < (int)cfg.EEnum.EEquipSolt.SlotMax;
        }
    }

    [ObjectSystem]
    public class THeroUnitAwakeSystem: AwakeSystem<THeroUnit, string>
    {
        protected override void Awake(THeroUnit self, string configid)
        {
            self.ConfigId = configid;
            self.BattleScore = self.HeroConfig().BattleScore;
            // self.AddComponent<HeroEquipComponent>();
            // self.AddComponent<HeroTalentComponent>();
        }
    }
}