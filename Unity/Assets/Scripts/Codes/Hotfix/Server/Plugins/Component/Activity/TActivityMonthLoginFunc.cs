using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET.Server
{
    public static class TActivityMonthLoginFunc
    {
        public static void LoadAllChild(this TActivityMonthLogin self)
        {
            self.Items.Clear();
            self.TotalLoginItems.Clear();
            var curTime = TimeHelper.ServerNow() / 1000;
            var monthData = LuBanConfigComponent.Instance.Config().TActivityMonthLogin.DataList.Find(record =>
            {
                return record.ActivityStartTime <= curTime && curTime <= record.ActivityEndTime;
            });
            if (monthData != null)
            {
                self.ConfigId = monthData.Id;
                self.StartTime = monthData.ActivityStartTime;
                self.EndTime = monthData.ActivityEndTime;
                monthData.LoginPrize.ForEach(item =>
                {
                    if (!self.Items.ContainsKey(item.Index))
                    {
                        self.Items.Add(item.Index, new FItemInfo(item.ItemConfigId, item.ItemCount));
                    }
                });
                monthData.TotalLoginPrize.ForEach(item =>
                {
                    if (!self.TotalLoginItems.ContainsKey(item.Index))
                    {
                        self.TotalLoginItems.Add(item.Index, new FItemInfo(item.ItemConfigId, item.ItemCount));
                    }
                });
            }
        }

        public static (int, string) GetPrize(this TActivityMonthLogin self, TCharacter character, int dayIndex)
        {
            if (!self.IsValid())
            {
                return (ErrorCode.ERR_Error, "active not valid");
            }

            if (!self.Items.TryGetValue(dayIndex, out var prizeItem))
            {
                return (ErrorCode.ERR_Error, "dayIndex not valid");
            }

            var activityData = character.ActivityComp.GetActivityData<TActivityMonthLoginData>(EActivityType.TActivityMonthLogin);
            if (activityData == null || !activityData.IsValid())
            {
                return (ErrorCode.ERR_Error, "activityData not valid");
            }
            if (dayIndex > activityData.LoginDayCount)
            {
                return (ErrorCode.ERR_Error, "dayIndex not enough");
            }
            if (activityData.ItemHadGet.Contains(dayIndex))
            {
                return (ErrorCode.ERR_Error, "activityData had Get");
            }

            var addResult = character.BagComp.AddTItemOrMoney(prizeItem.ItemConfigId, prizeItem.ItemCount);
            if (addResult.Item1 == ErrorCode.ERR_Success)
            {
                activityData.ItemHadGet.Add(dayIndex);
                character.SyncHttpEntity(activityData);
            }

            return addResult;
        }

        public static (int, string) GetTotalPrize(this TActivityMonthLogin self, TCharacter character, int dayIndex)
        {
            if (!self.IsValid())
            {
                return (ErrorCode.ERR_Error, "active not valid");
            }

            if (!self.TotalLoginItems.TryGetValue(dayIndex, out var prizeItem))
            {
                return (ErrorCode.ERR_Error, "dayIndex not valid");
            }

            var activityData = character.ActivityComp.GetActivityData<TActivityMonthLoginData>(EActivityType.TActivityMonthLogin);
            if (activityData == null || !activityData.IsValid())
            {
                return (ErrorCode.ERR_Error, "activityData not valid");
            }
            if (dayIndex > activityData.LoginDayCount)
            {
                return (ErrorCode.ERR_Error, "dayIndex not enough");
            }
            if (activityData.TotalLoginItemHadGet.Contains(dayIndex))
            {
                return (ErrorCode.ERR_Error, "activityData had Get");
            }

            var addResult = character.BagComp.AddTItemOrMoney(prizeItem.ItemConfigId, prizeItem.ItemCount);
            if (addResult.Item1 == ErrorCode.ERR_Success)
            {
                activityData.TotalLoginItemHadGet.Add(dayIndex);
                character.SyncHttpEntity(activityData);
            }

            return addResult;
        }
    }
}