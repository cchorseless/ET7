using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET.Server
{
    [ObjectSystem]
    public class TBattlePassTaskItemAwakeSystem : AwakeSystem<TBattlePassTaskItem, int>
    {
        protected override void Awake(TBattlePassTaskItem self, int configId)
        {
            self.ConfigId = configId;
        }
    }

    public static class ETaskFinishType
    {
        public const string KillEnemyCount = "KillEnemyCount";

    }


    public static class TBattlePassTaskItemFunc
    {
        public static void LoadAllChild(this TBattlePassTaskItem self)
        {
            self.UpdateTaskState();
        }

        public static Conf.Dota.BattlePassTaskConfigRecord TaskConfig(this TBattlePassTaskItem self)
        {
            return LuBanConfigComponent.Instance.Config().BattlePassTaskConfig.GetOrDefault(self.ConfigId);
        }

        public static bool TaskFinishGreaterThan(this TBattlePassTaskItem self, string taskKey, int value)
        {
            int numericType = 0;
            switch (taskKey)
            {
                case ETaskFinishType.KillEnemyCount:
                    numericType = EMoneyType.KillEnemyCount;
                    break;
            }
            if (numericType > 0)
            {
                return self.BattlePassComp.Character.DataComp.GreaterThan(numericType, value);
            }
            return false;
        }


        public static void UpdateTaskState(this TBattlePassTaskItem self)
        {
            if (!self.IsAchieve)
            {
                var condition = self.TaskConfig().TaskFinishCondition;
                if (!self.TaskFinishGreaterThan(condition.KeyString, condition.ValueInt))
                {
                    return;
                }
                self.IsAchieve = true;
            }
        }
    }
}
