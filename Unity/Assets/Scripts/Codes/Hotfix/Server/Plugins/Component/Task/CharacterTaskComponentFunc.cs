using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET.Server
{
    public static class ECharacterTaskType
    {
        public const string Daily = "Daily";
        public const string Week = "Week";
        public const string Season = "Season";
    }

    public static class TaskConfig
    {
        public const int DailyTaskMaxCount = 3;
        public const int WeekTaskMaxCount = 5;
        public const int SeasonTaskMaxCount = 5;
    }

    public static class CharacterTaskComponentFunc
    {
        public static void LoadAllChild(this CharacterTaskComponent self)
        {
            self.LoadDailyTask();
            self.LoadWeekTask();
            self.LoadSeasonTask();
        }


        public static void LoadDailyTask(this CharacterTaskComponent self)
        {
            if (self.Character.IsFirstLoginToday)
            {
                self.IsReplaceDailyTask = true;
                if (self.DailyTasks.Count < TaskConfig.DailyTaskMaxCount)
                {
                    var taskid = self.RandomTask(ECharacterTaskType.Daily);
                    var taskEntity = self.AddChild<TCharacterTaskItem, int>(taskid);
                    taskEntity.LoadAllChild();
                    self.DailyTasks.Add(taskEntity.Id);
                }
            }
        }

        public static void LoadWeekTask(this CharacterTaskComponent self)
        {
            if (self.Character.IsFirstLoginWeek)
            {
                self.WeekTasks.ForEach(taskId =>
                {
                    self.GetChild<TCharacterTaskItem>(taskId)?.Dispose();
                });
                self.WeekTasks.Clear();
                for (int i = 0; i < TaskConfig.WeekTaskMaxCount; i++)
                {
                    var taskid = self.RandomTask(ECharacterTaskType.Week);
                    var taskEntity = self.AddChild<TCharacterTaskItem, int>(taskid);
                    taskEntity.LoadAllChild();
                    self.WeekTasks.Add(taskEntity.Id);
                }
            }
        }

        public static void LoadSeasonTask(this CharacterTaskComponent self)
        {
            if (self.Character.IsFirstLoginSeason)
            {
                self.SeasonTasks.ForEach(taskId =>
                {
                    self.GetChild<TCharacterTaskItem>(taskId)?.Dispose();
                });
                self.SeasonTasks.Clear();
                for (int i = 0; i < TaskConfig.SeasonTaskMaxCount; i++)
                {
                    var taskid = self.RandomTask(ECharacterTaskType.Season);
                    var taskEntity = self.AddChild<TCharacterTaskItem, int>(taskid);
                    taskEntity.LoadAllChild();
                    self.SeasonTasks.Add(taskEntity.Id);
                }
            }
        }

        public static int RandomTask(this CharacterTaskComponent self, string taskType)
        {
            var tasks = LuBanConfigComponent.Instance.Config().TaskConfig.DataList.FindAll(config =>
            {
                return config.TaskType == taskType;
            });
            return RandomGenerator.RandomArray(tasks).Id;
        }

        public static (int, string) GetPrize(this CharacterTaskComponent self, string taskId_str)
        {
            if (!long.TryParse(taskId_str, out var entityId))
            {
                return (ErrorCode.ERR_Error, "taskId_str not valid");
            }
            var entity = self.GetChild<TCharacterTaskItem>(entityId);
            if (entity == null)
            {
                return (ErrorCode.ERR_Error, "task Entity not valid");
            }
            entity.UpdateTaskState();
            if (!entity.IsAchieve)
            {
                return (ErrorCode.ERR_Error, "task not achieve");
            }
            if (!entity.IsPrizeGet)
            {
                return (ErrorCode.ERR_Error, "task had PrizeGet");
            }
            int prizeMultiple = 1;
            if (self.Character.ActivityComp.IsMemberShipVip())
            {
                prizeMultiple = 2;
            }
            List<FItemInfo> itemsPrize = new List<FItemInfo>();
            entity.TaskConfig().TaskPrize.ForEach(item =>
            {
                itemsPrize.Add(new FItemInfo(item.ItemConfigId, item.ItemCount * prizeMultiple));
            });
            var r = self.Character.BagComp.AddTItemOrMoney(itemsPrize);
            if (r.Item1 == ErrorCode.ERR_Success)
            {
                entity.IsPrizeGet = true;
                if (self.DailyTasks.Contains(entityId))
                {
                    entity.Dispose();
                    self.DailyTasks.Remove(entityId);
                }
            }
            return r;
        }


        public static (int, string) ChangeDailyTaskState(this CharacterTaskComponent self, string taskId_str, bool isDropTask)
        {
            if (!long.TryParse(taskId_str, out var entityId))
            {
                return (ErrorCode.ERR_Error, "taskId_str not valid");
            }
            if (!self.DailyTasks.Contains(entityId))
            {
                return (ErrorCode.ERR_Error, "entityId not valid");
            }
            var entity = self.GetChild<TCharacterTaskItem>(entityId);
            if (entity == null)
            {
                return (ErrorCode.ERR_Error, "task Entity not valid");
            }
            if (isDropTask)
            {
                entity.Dispose();
                self.DailyTasks.Remove(entityId);
            }
            else
            {
                if (!self.IsReplaceDailyTask)
                {
                    return (ErrorCode.ERR_Error, "ReplaceDailyTask only once");
                }
                entity.Dispose();
                self.DailyTasks.Remove(entityId);
                var taskid = self.RandomTask(ECharacterTaskType.Daily);
                var taskEntity = self.AddChild<TCharacterTaskItem, int>(taskid);
                taskEntity.LoadAllChild();
                self.DailyTasks.Add(taskEntity.Id);
            }
            return (ErrorCode.ERR_Success, "");
        }
    }
}
