using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET.Server
{
    public static class EBattlePassTaskType
    {
        public const string Daily = "Daily";
        public const string Week = "Week";
        public const string Season = "Season";
    }

    public static class BattlePassConfig
    {
        public const int DailyTaskMaxCount = 3;
        public const int WeekTaskMaxCount = 5;
        public const int SeasonTaskMaxCount = 5;
    }

    public static class CharacterBattlePassComponentFunc
    {
        public static void LoadAllChild(this CharacterBattlePassComponent self)
        {
            var SeasonComp = self.Character.GetMyServerZone().SeasonComp;
            if (self.SeasonConfigId != SeasonComp.CurSeasonConfigId)
            {
                self.StartNewSeason();
            }

            self.SeasonConfigId = SeasonComp.CurSeasonConfigId;
            self.SeasonEndTimeSpan = SeasonComp.CurSeason.EndTime;
            var now = TimeHelper.DateTimeNow();
            self.DailyEndTimeSpan = TimeHelper.DailyEndTime(now);
            self.WeekEndTimeSpan = TimeHelper.WeekEndTime(now);
            self.LoadDailyTask();
            self.LoadWeekTask();
            // self.LoadSeasonTask();
        }

        public static void StartNewSeason(this CharacterBattlePassComponent self)
        {
            var alltasks = new List<long>();
            alltasks.AddRange(self.DailyTasks);
            alltasks.AddRange(self.WeekTasks);
            alltasks.AddRange(self.SeasonTasks);
            foreach (var entityId in alltasks)
            {
                var entity = self.GetChild<TBattlePassTaskItem>(entityId);
                if (entity != null)
                {
                    entity.Dispose();
                }
            }

            self.DailyTasks.Clear();
            self.WeekTasks.Clear();
            self.SeasonTasks.Clear();
            self.BattlePassLevel = 0;
            self.BattlePassExp = 0;
            self.BattlePassPrizeGet.Clear();
            self.BattlePassChargeGet.Clear();
            self.DailyEndTimeSpan = 0;
            self.WeekEndTimeSpan = 0;
            self.SeasonEndTimeSpan = 0;
            self.IsReplaceDailyTask = false;
            self.IsBattlePass = false;
        }

        public static void LoadDailyTask(this CharacterBattlePassComponent self)
        {
            if (self.Character.IsFirstLoginToday)
            {
                self.IsReplaceDailyTask = true;
                if (self.DailyTasks.Count < BattlePassConfig.DailyTaskMaxCount)
                {
                    var taskid = self.RandomTask(EBattlePassTaskType.Daily);
                    var taskEntity = self.AddChild<TBattlePassTaskItem, int>(taskid);
                    taskEntity.LoadAllChild();
                    self.DailyTasks.Add(taskEntity.Id);
                }
            }
        }

        public static void LoadWeekTask(this CharacterBattlePassComponent self)
        {
            if (self.Character.IsFirstLoginWeek)
            {
                self.WeekTasks.ForEach(taskId => { self.GetChild<TBattlePassTaskItem>(taskId)?.Dispose(); });
                self.WeekTasks.Clear();
                for (int i = 0; i < BattlePassConfig.WeekTaskMaxCount; i++)
                {
                    var taskid = self.RandomTask(EBattlePassTaskType.Week);
                    var taskEntity = self.AddChild<TBattlePassTaskItem, int>(taskid);
                    taskEntity.LoadAllChild();
                    self.WeekTasks.Add(taskEntity.Id);
                }
            }
        }

        public static void LoadSeasonTask(this CharacterBattlePassComponent self)
        {
            if (self.Character.IsFirstLoginSeason)
            {
                self.SeasonTasks.ForEach(taskId => { self.GetChild<TBattlePassTaskItem>(taskId)?.Dispose(); });
                self.SeasonTasks.Clear();
                for (int i = 0; i < BattlePassConfig.SeasonTaskMaxCount; i++)
                {
                    var taskid = self.RandomTask(EBattlePassTaskType.Season);
                    var taskEntity = self.AddChild<TBattlePassTaskItem, int>(taskid);
                    taskEntity.LoadAllChild();
                    self.SeasonTasks.Add(taskEntity.Id);
                }
            }
        }

        public static int RandomTask(this CharacterBattlePassComponent self, string taskType)
        {
            var tasks = LuBanConfigComponent.Instance.Config().BattlePassTaskConfig.DataList
                    .FindAll(config => { return config.TaskType == taskType; });
            return RandomGenerator.RandomArray(tasks).Id;
        }

        public static (int, string) GetBattlePassPrize(this CharacterBattlePassComponent self, int prizeLevel, bool isPlusPrize)
        {
            if (self.BattlePassLevel < prizeLevel)
            {
                return (ErrorCode.ERR_Error, "BattlePassLevel not enough");
            }

            if (!self.IsBattlePass && isPlusPrize)
            {
                return (ErrorCode.ERR_Error, "not plus");
            }

            int prizeKey = prizeLevel;
            if (isPlusPrize)
            {
                prizeKey += 10000;
            }

            if (self.BattlePassPrizeGet.Contains(prizeKey))
            {
                return (ErrorCode.ERR_Error, "prize had get");
            }

            var config = LuBanConfigComponent.Instance.Config().BattlePassLevelUpConfig.DataList.Find(v =>
            {
                return v.SeasonId == self.SeasonConfigId && v.BattlePassLevel == prizeLevel;
            });
            if (config == null)
            {
                return (ErrorCode.ERR_Error, "cant find config");
            }

            var itemInfo = new FItemInfo();
            if (isPlusPrize)
            {
                itemInfo.ItemConfigId = config.TaskSpePrize.ItemConfigId;
                itemInfo.ItemCount = config.TaskSpePrize.ItemCount;
            }
            else
            {
                itemInfo.ItemConfigId = config.TaskComPrize.ItemConfigId;
                itemInfo.ItemCount = config.TaskComPrize.ItemCount;
            }

            var r = self.Character.BagComp.AddTItemOrMoney(itemInfo.ItemConfigId, itemInfo.ItemCount);
            if (r.Item1)
            {
                self.BattlePassPrizeGet.Add(prizeKey);
                self.Character.SyncHttpEntity(self);
                return (ErrorCode.ERR_Success, itemInfo.ToString());
            }

            return (ErrorCode.ERR_Error, "item add fail");
        }

        public static (int, string) GetTaskPrize(this CharacterBattlePassComponent self, string taskId_str)
        {
            if (!long.TryParse(taskId_str, out var entityId))
            {
                return (ErrorCode.ERR_Error, "taskId_str not valid");
            }

            var entity = self.GetChild<TBattlePassTaskItem>(entityId);
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

            // int prizeMultiple = 1;
            // if (self.Character.ActivityComp.IsMemberShipVip())
            // {
            //     prizeMultiple = 2;
            // }
            var prize = entity.TaskConfig().TaskPrize;
            var itemsPrize = new FItemInfo(prize.ItemConfigId, prize.ItemCount);
            var r = self.Character.BagComp.AddTItemOrMoney(itemsPrize.ItemConfigId, itemsPrize.ItemCount);
            if (r.Item1)
            {
                entity.IsPrizeGet = true;
                if (self.DailyTasks.Contains(entityId))
                {
                    entity.Dispose();
                    self.DailyTasks.Remove(entityId);
                }

                return (ErrorCode.ERR_Success, itemsPrize.ToString());
            }

            return (ErrorCode.ERR_Error, "add item fail");
        }

        public static (int, string) ChangeDailyTaskState(this CharacterBattlePassComponent self, string taskId_str, bool isDropTask)
        {
            if (!long.TryParse(taskId_str, out var entityId))
            {
                return (ErrorCode.ERR_Error, "taskId_str not valid");
            }

            if (!self.DailyTasks.Contains(entityId))
            {
                return (ErrorCode.ERR_Error, "entityId not valid");
            }

            var entity = self.GetChild<TBattlePassTaskItem>(entityId);
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
                var taskid = self.RandomTask(EBattlePassTaskType.Daily);
                var taskEntity = self.AddChild<TBattlePassTaskItem, int>(taskid);
                taskEntity.LoadAllChild();
                self.DailyTasks.Add(taskEntity.Id);
            }

            return (ErrorCode.ERR_Success, "");
        }

        public static (int, string) ChargeBattlePassPrize(this CharacterBattlePassComponent self, int chargeConfigId)
        {
            if (self.BattlePassChargeGet.Contains(chargeConfigId))
            {
                return (ErrorCode.ERR_Error, "charge had get");
            }

            var config = LuBanConfigComponent.Instance.Config().BattlePassChargeConfig.GetOrDefault(chargeConfigId);
            if (config == null)
            {
                return (ErrorCode.ERR_Error, "cant find config");
            }

            var needcount = config.ChargeBy.ItemCount;
            var costItem = self.Character.BagComp.GetOneTItem<TItem>(self.ChargeItemConfigId);
            if (costItem == null || needcount > costItem.ItemCount)
            {
                return (ErrorCode.ERR_Error, "item not enough");
            }

            var itemInfo = new FItemInfo();
            itemInfo.ItemConfigId = config.ChargeTo.ItemConfigId;
            itemInfo.ItemCount = config.ChargeTo.ItemCount;

            var r = self.Character.BagComp.AddTItemOrMoney(itemInfo.ItemConfigId, itemInfo.ItemCount);
            if (r.Item1)
            {
                costItem.ChangeItemCount(-needcount);
                self.BattlePassChargeGet.Add(chargeConfigId);
                self.Character.SyncHttpEntity(costItem);
                self.Character.SyncHttpEntity(self);
                return (ErrorCode.ERR_Success, itemInfo.ToString());
            }

            return (ErrorCode.ERR_Error, "item add fail");
        }
    }
}