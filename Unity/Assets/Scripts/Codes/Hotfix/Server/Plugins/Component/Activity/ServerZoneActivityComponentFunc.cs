using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ET.Server
{
    public static class ServerZoneActivityComponentFunc
    {
        public static void LoadAllChild(this ServerZoneActivityComponent self)
        {
            var allActivity = LuBanConfigComponent.Instance.Config().ActivityConfig.DataList;
            foreach (var config in allActivity)
            {
                if (config.IsValid  && config.Id < EActivityType.ServerZoneActivityMax)
                {
                    self.UpdateActivity(config.Id);
                }
            }
            foreach (var k in self.Activity.Keys)
            {
                var config = LuBanConfigComponent.Instance.Config().ActivityConfig.GetOrDefault(k);
                if (config == null || !config.IsValid  || config.Id >= EActivityType.ServerZoneActivityMax)
                {
                    self.RemoveActivity(k);
                }
            }
        }

        public static List<int> GetAllValidActivityConfigId(this ServerZoneActivityComponent self)
        {
            var r = new List<int>();
            foreach (var kv in self.Activity)
            {
                var entity = self.GetChild<TActivity>(kv.Value);
                if (entity.IsValid())
                {
                    r.Add(kv.Key);
                }
            }
            return r;
        }


        public static TActivity GetActivity(this ServerZoneActivityComponent self, int activityconfigid)
        {
            if (self.Activity.TryGetValue(activityconfigid, out var activityid))
            {
                return self.GetChild<TActivity>(activityid);
            }
            return null;
        }
        public static T GetActivity<T>(this ServerZoneActivityComponent self, int activityconfigid) where T : TActivity
        {
            if (self.Activity.TryGetValue(activityconfigid, out var activityid))
            {
                return self.GetChild<T>(activityid);
            }
            return null;
        }


        public static void UpdateActivity(this ServerZoneActivityComponent self, int activityconfigid)
        {
            if (LuBanConfigComponent.Instance.Config().ActivityConfig.GetOrDefault(activityconfigid) == null)
            {
                return;
            }
            switch (activityconfigid)
            {
                case EActivityType.TActivitySevenDayLogin:
                    self.UpdateActivity<TActivitySevenDayLogin>(activityconfigid);
                    break;
                case EActivityType.TActivityMonthLogin:
                    self.UpdateActivity<TActivityMonthLogin>(activityconfigid);
                    break;
                case EActivityType.TActivityBattlePass:
                    self.UpdateActivity<TActivityBattlePass>(activityconfigid);
                    break;
                case EActivityType.TActivityMemberShip:
                    self.UpdateActivity<TActivityMemberShip>(activityconfigid);
                    break;
                case EActivityType.TActivityHeroRecordLevel:
                    self.UpdateActivity<TActivityHeroRecordLevel>(activityconfigid);
                    break;
                case EActivityType.TActivityDailyOnlinePrize:
                    self.UpdateActivity<TActivityDailyOnlinePrize>(activityconfigid);
                    break;
                case EActivityType.TActivityInvestMetaStone:
                    self.UpdateActivity<TActivityInvestMetaStone>(activityconfigid);
                    break;
                case EActivityType.TActivityTotalGainMetaStone:
                    self.UpdateActivity<TActivityTotalGainMetaStone>(activityconfigid);
                    break;
                case EActivityType.TActivityTotalOnlineTime:
                    self.UpdateActivity<TActivityTotalOnlineTime>(activityconfigid);
                    break;
                case EActivityType.TActivityTotalSpendMetaStone:
                    self.UpdateActivity<TActivityTotalSpendMetaStone>(activityconfigid);
                    break;
                case EActivityType.TActivityGiftCommond:
                    self.UpdateActivity<TActivityGiftCommond>(activityconfigid);
                    break;
                case EActivityType.TActivityMentorshipTree:
                    self.UpdateActivity<TActivityMentorshipTree>(activityconfigid);
                    break;
            }
        }


        public static void UpdateActivity<T>(this ServerZoneActivityComponent self, int activityconfigid) where T : TActivity
        {
            bool NoCreateNew = self.Activity.ContainsKey(activityconfigid);
            if (NoCreateNew)
            {
                var activity = self.GetActivity<T>(activityconfigid);
                if (activity != null)
                {
                    if (activity.IsValid())
                    {
                        /// 坑 扩展方法无法覆盖
                        self.LoadAllChildActivity(activity);
                    }
                    else if (activity.IsOutOfDate())
                    {
                        self.Activity.Remove(activityconfigid);
                        self.SaveOutOfDateActivity(activity).Coroutine();
                        NoCreateNew = false;
                    }
                }
                else
                {
                    NoCreateNew = false;
                }
            }
            if (!NoCreateNew)
            {
                T activity = self.AddChild<T>();
                activity.ActivityId = activityconfigid;
                activity.ValidState = true;
                self.Activity.Add(activityconfigid, activity.Id);
                self.LoadAllChildActivity(activity);
            }
        }


        /// <summary>
        /// C# 不允许覆盖扩展方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="self"></param>
        /// <param name="activity"></param>
        public static void LoadAllChildActivity<T>(this ServerZoneActivityComponent self, T activity) where T : TActivity
        {
            if (activity is TActivitySevenDayLogin)
            {
                (activity as TActivitySevenDayLogin).LoadAllChild();
            }
            else if (activity is TActivityMonthLogin)
            {
                (activity as TActivityMonthLogin).LoadAllChild();
            }
            else if (activity is TActivityBattlePass)
            {
                (activity as TActivityBattlePass).LoadAllChild();
            }
            else if (activity is TActivityMemberShip)
            {
                (activity as TActivityMemberShip).LoadAllChild();
            }
            else if (activity is TActivityHeroRecordLevel)
            {
                (activity as TActivityHeroRecordLevel).LoadAllChild();
            }
            else if (activity is TActivityDailyOnlinePrize)
            {
                (activity as TActivityDailyOnlinePrize).LoadAllChild();
            }
            else if (activity is TActivityInvestMetaStone)
            {
                (activity as TActivityInvestMetaStone).LoadAllChild();
            }
            else if (activity is TActivityTotalGainMetaStone)
            {
                (activity as TActivityTotalGainMetaStone).LoadAllChild();
            }
            else if (activity is TActivityTotalOnlineTime)
            {
                (activity as TActivityTotalOnlineTime).LoadAllChild();
            }
            else if (activity is TActivityTotalSpendMetaStone)
            {
                (activity as TActivityTotalSpendMetaStone).LoadAllChild();
            }
            else if (activity is TActivityGiftCommond)
            {
                (activity as TActivityGiftCommond).LoadAllChild();
            }
            else if (activity is TActivityMentorshipTree)
            {
                (activity as TActivityMentorshipTree).LoadAllChild();
            }
        }


        public static void RemoveActivity(this ServerZoneActivityComponent self, int activityconfigid)
        {
            if (self.Activity.TryGetValue(activityconfigid, out var activityid))
            {
                self.Activity.Remove(activityconfigid);
                var entity = self.GetChild<TActivity>(activityid);
                if (entity != null)
                {
                    entity.Dispose();
                }
            }
        }

        public static async ETTask SaveOutOfDateActivity<T>(this ServerZoneActivityComponent self, T activity) where T : TActivity
        {
            var db = DBManagerComponent.Instance.GetAccountDB();
            await db.Save(activity);
            activity.Dispose();
        }

        public static bool IsValidServerZoneActivity(this ServerZoneActivityComponent self, int activityconfigid)
        {
            var config = LuBanConfigComponent.Instance.Config().ActivityConfig.GetOrDefault(activityconfigid);
            if (config == null || !config.IsValid || config.Id >= EActivityType.ServerZoneActivityMax)
            {
                return false;
            }
            var activity = self.GetActivity(config.Id);
            if (activity == null)
            {
                return false;
            }
            return activity.IsValid();
        }



    }
}
