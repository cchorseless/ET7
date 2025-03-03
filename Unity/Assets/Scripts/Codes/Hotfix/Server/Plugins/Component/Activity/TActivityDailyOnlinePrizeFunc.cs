﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET.Server
{
    public static class TActivityDailyOnlinePrizeFunc
    {
        public static void LoadAllChild(this TActivityDailyOnlinePrize self)
        {
            self.Items.Clear();
            var dailyOnlinePrize = LuBanConfigComponent.Instance.Config().TActivityDailyOnlinePrize.DataList;
            self.SetNeverOutOfDate();
            dailyOnlinePrize.ForEach(item =>
            {
                if (!self.Items.ContainsKey(item.Id))
                {
                    var itemList = new List<FItemInfo>();
                    item.ItemGroup.ForEach(_info => { itemList.Add(new FItemInfo(_info.ItemConfigId, _info.ItemCount)); });
                    self.Items.Add(item.Id, itemList);
                }
            });
        }

        public static (int, string) GetPrize(this TActivityDailyOnlinePrize self, TCharacter character, int onlineTime)
        {
            if (!self.IsValid())
            {
                return (ErrorCode.ERR_Error, "active not valid");
            }

            if (!self.Items.TryGetValue(onlineTime, out var prizeItem))
            {
                return (ErrorCode.ERR_Error, "dayIndex not valid");
            }

            var activityData = character.ActivityComp.GetActivityData<TActivityDailyOnlinePrizeData>(EActivityType.TActivityDailyOnlinePrize);
            if (activityData == null || !activityData.IsValid())
            {
                return (ErrorCode.ERR_Error, "activityData not valid");
            }

            if (activityData.ItemHadGet.Contains(onlineTime))
            {
                return (ErrorCode.ERR_Error, "activityData had Get");
            }
            
            if (activityData.GetSecTodayOnlineTime() < onlineTime)
            {
                return (ErrorCode.ERR_Error, "time not enough");
            }
            
            var addResult = character.BagComp.AddTItemOrMoney(prizeItem);
            if (addResult.Item1 == ErrorCode.ERR_Success)
            {
                activityData.ItemHadGet.Add(onlineTime);
                character.SyncHttpEntity(activityData);
            }

            return addResult;
        }
    }
}