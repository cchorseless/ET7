using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET.Server
{
    [ObjectSystem]
    public class TCharacterAchievementItemAwakeSystem : AwakeSystem<TCharacterAchievementItem, int>
    {
        protected override void Awake(TCharacterAchievementItem self, int configId)
        {
            self.ConfigId = configId;
        }
    }

    public static class EAchieveFinishType
    {
        public const string KillEnemyCount = "KillEnemyCount";

    }

    [FriendOf(typeof(TCharacterAchievementItem))]
    public static class TCharacterAchievementItemFunc
    {
        public static void LoadAllChild(this TCharacterAchievementItem self)
        {
            self.UpdateAchieveState();

        }

        public static Conf.Achievement.AchievementConfigRecord AchieveConfig(this TCharacterAchievementItem self)
        {
            return LuBanConfigComponent.Instance.Config().AchievementConfig.GetOrDefault(self.ConfigId);
        }

        public static bool GreaterThan(this TCharacterAchievementItem self, string achieveKey, int value)
        {
            int numericType = 0;
            switch (achieveKey)
            {
                case EAchieveFinishType.KillEnemyCount:
                    numericType = EMoneyType.KillEnemyCount;
                    break;
            }
            if (numericType > 0)
            {
                return self.AchievementComp.Character.DataComp.GreaterThan(numericType, value);
            }
            return false;
        }


        public static void UpdateAchieveState(this TCharacterAchievementItem self)
        {
            if (!self.IsAchieve)
            {
                var finish = self.AchieveConfig().AchieveFinishCondition;
                foreach (var condition in finish)
                {
                    if (!self.GreaterThan(condition.KeyString, condition.ValueInt))
                    {
                        return;
                    }
                }
                self.IsAchieve = true;
            }
        }
    }
}
