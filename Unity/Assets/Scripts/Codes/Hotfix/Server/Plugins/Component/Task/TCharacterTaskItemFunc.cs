using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET.Server
{
    [ObjectSystem]
    public class TCharacterTaskItemAwakeSystem : AwakeSystem<TCharacterTaskItem, int>
    {
        protected override void Awake(TCharacterTaskItem self, int configId)
        {
            self.ConfigId = configId;
        }
    }

    public static class ETaskFinishType
    {
        public const string KillEnemyCount = "KillEnemyCount";

    }


    public static class TCharacterTaskItemFunc
    {
        public static void LoadAllChild(this TCharacterTaskItem self)
        {
            self.UpdateTaskState();
        }

        public static cfg.Task.TaskConfigRecord TaskConfig(this TCharacterTaskItem self)
        {
            return LuBanConfigComponent.Instance.Config().TaskConfig.GetOrDefault(self.ConfigId);
        }

        public static bool TaskFinishGreaterThan(this TCharacterTaskItem self, string taskKey, int value)
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
                return self.CharacterTaskComp.Character.DataComp.GreaterThan(numericType, value);
            }
            return false;
        }


        public static void UpdateTaskState(this TCharacterTaskItem self)
        {
            if (!self.IsAchieve)
            {
                var finish = self.TaskConfig().TaskFinishCondition;
                foreach (var condition in finish)
                {
                    if (!self.TaskFinishGreaterThan(condition.KeyString, condition.ValueInt))
                    {
                        return;
                    }
                }
                self.IsAchieve = true;
            }
        }
    }
}
