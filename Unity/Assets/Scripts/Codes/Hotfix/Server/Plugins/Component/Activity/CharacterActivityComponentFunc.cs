using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET.Server
{
    [FriendOf(typeof(CharacterActivityComponent))]
    public static class CharacterActivityComponentFunc
    {
        public static void LoadAllChild(this CharacterActivityComponent self)
        {
            var serverzone = self.Character.GetMyServerZone();
            var activityConfig = serverzone.ActivityComp.GetAllValidActivityConfigId();
            foreach (var configId in activityConfig)
            {
                self.UpdateActivityData(configId);
            }
            foreach (var k in self.ActivityData.Keys)
            {
                if (k < EActivityType.ServerZoneActivityMax &&
                    !serverzone.ActivityComp.IsValidServerZoneActivity(k))
                {
                    self.RemoveActivityData(k);
                }
            }
        }

        public static TActivityData GetActivityData(this CharacterActivityComponent self, int activityconfigid)
        {
            if (self.ActivityData.TryGetValue(activityconfigid, out var activityid))
            {
                return self.GetChild<TActivityData>(activityid);
            }
            return null;
        }


        public static T GetActivityData<T>(this CharacterActivityComponent self, int activityconfigid) where T : TActivityData, new()
        {
            if (self.ActivityData.TryGetValue(activityconfigid, out var activityid))
            {
                return self.GetChild<T>(activityid);
            }
            return null;
        }


        public static void UpdateActivityData(this CharacterActivityComponent self, int activityconfigid)
        {
            if (LuBanConfigComponent.Instance.Config().ActivityConfig.GetOrDefault(activityconfigid) == null)
            {
                return;
            }
            switch (activityconfigid)
            {
                case EActivityType.TActivitySevenDayLogin:
                    self.UpdateActivityData<TActivitySevenDayLoginData>(activityconfigid);
                    break;
                case EActivityType.TActivityMonthLogin:
                    self.UpdateActivityData<TActivityMonthLoginData>(activityconfigid);
                    break;
                case EActivityType.TActivityHeroRecordLevel:
                    self.UpdateActivityData<TActivityHeroRecordLevelData>(activityconfigid);
                    break;
                case EActivityType.TActivityDailyOnlinePrize:
                    self.UpdateActivityData<TActivityDailyOnlinePrizeData>(activityconfigid);
                    break;
                case EActivityType.TActivityInvestMetaStone:
                    self.UpdateActivityData<TActivityInvestMetaStoneData>(activityconfigid);
                    break;
                case EActivityType.TActivityTotalGainMetaStone:
                    self.UpdateActivityData<TActivityTotalGainMetaStoneData>(activityconfigid);
                    break;
                case EActivityType.TActivityTotalOnlineTime:
                    self.UpdateActivityData<TActivityTotalOnlineTimeData>(activityconfigid);
                    break;
                case EActivityType.TActivityTotalSpendMetaStone:
                    self.UpdateActivityData<TActivityTotalSpendMetaStoneData>(activityconfigid);
                    break;
                case EActivityType.TActivityGiftCommond:
                    self.UpdateActivityData<TActivityGiftCommondData>(activityconfigid);
                    break;
                case EActivityType.TActivityMentorshipTree:
                    self.UpdateActivityData<TActivityMentorshipTreeData>(activityconfigid);
                    break;
            }
        }


        public static void UpdateActivityData<T>(this CharacterActivityComponent self, int activityconfigid) where T : TActivityData, new()
        {
            bool NoCreateNew = self.ActivityData.ContainsKey(activityconfigid);
            if (NoCreateNew)
            {
                var activityData = self.GetActivityData<T>(activityconfigid);
                if (activityData.IsValid())
                {
                  self.LoadAllChildActivityData(activityData);
                }
                else
                {
                    self.ActivityData.Remove(activityconfigid);
                    activityData.Dispose();
                    NoCreateNew = false;
                }
            }
            if (!NoCreateNew)
            {
                var activityInfo = self.Character.GetMyServerZone().ActivityComp.GetActivity(activityconfigid);
                var activityData = self.AddChild<T>();
                activityData.ConfigId = activityconfigid;
                activityData.StartTime = activityInfo.StartTime;
                activityData.EndTime = activityInfo.EndTime;
                self.ActivityData.Add(activityconfigid, activityData.Id);
                self.LoadAllChildActivityData(activityData);
            }

        }

        /// <summary>
        /// C# 不允许覆盖扩展方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="self"></param>
        /// <param name="activitydata"></param>
        public static void LoadAllChildActivityData<T>(this CharacterActivityComponent self, T activitydata) where T : TActivityData
        {
            if (activitydata is TActivitySevenDayLoginData)
            {
                (activitydata as TActivitySevenDayLoginData).LoadAllChild();
            }
            else if (activitydata is TActivityMonthLoginData)
            {
                (activitydata as TActivityMonthLoginData).LoadAllChild();
            }
            else if (activitydata is TActivityHeroRecordLevelData)
            {
                (activitydata as TActivityHeroRecordLevelData).LoadAllChild();
            }
            else if (activitydata is TActivityDailyOnlinePrizeData)
            {
                (activitydata as TActivityDailyOnlinePrizeData).LoadAllChild();
            }
            else if (activitydata is TActivityInvestMetaStoneData)
            {
                (activitydata as TActivityInvestMetaStoneData).LoadAllChild();
            }
            else if (activitydata is TActivityTotalGainMetaStoneData)
            {
                (activitydata as TActivityTotalGainMetaStoneData).LoadAllChild();
            }
            else if (activitydata is TActivityTotalOnlineTimeData)
            {
                (activitydata as TActivityTotalOnlineTimeData).LoadAllChild();
            }
            else if (activitydata is TActivityTotalSpendMetaStoneData)
            {
                (activitydata as TActivityTotalSpendMetaStoneData).LoadAllChild();
            }
            else if (activitydata is TActivityGiftCommondData)
            {
                (activitydata as TActivityGiftCommondData).LoadAllChild();
            }
            else if (activitydata is TActivityMentorshipTreeData)
            {
                (activitydata as TActivityMentorshipTreeData).LoadAllChild();
            }
        }


        public static void RemoveActivityData(this CharacterActivityComponent self, int activityconfigid)
        {
            if (self.ActivityData.TryGetValue(activityconfigid, out var activityid))
            {
                self.ActivityData.Remove(activityconfigid);
                var entity = self.GetChild<TActivityData>(activityid);
                if (entity != null)
                {
                    entity.Dispose();
                }
            }
        }
    }
}
