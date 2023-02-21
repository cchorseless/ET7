﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ET.Server
{
    public static class TActivitySevenDayLoginFunc
    {
        public static void LoadAllChild(this TActivitySevenDayLogin self)
        {
            self.Items.Clear();
            var curTime = TimeHelper.ServerNow() / 1000;
            var sevenDayData = LuBanConfigComponent.Instance.Config().TActivitySevenDayLogin.DataList.Find(
                record =>
                {
                    return record.ActivityStartTime <= curTime && curTime <= (record.ActivityEndTime);
                });

            if (sevenDayData != null )
            {
                self.ConfigId = sevenDayData.Id;
                self.StartTime = sevenDayData.ActivityStartTime;
                self.EndTime = sevenDayData.ActivityEndTime;
                sevenDayData.PrizeItemGroup.ForEach(item =>
                {
                    if (!self.Items.ContainsKey(item.Index))
                    {
                        var itemList = new List<ValueTupleStruct<int, int>>();
                        item.ItemGroup.ForEach(_info =>
                        {
                            itemList.Add(new ValueTupleStruct<int, int>(_info.ItemConfigId, _info.ItemCount));
                        });
                        self.Items.Add(item.Index, itemList);
                    }
                });
            }
        }

        public static (int, string) GetPrize(this TActivitySevenDayLogin self, TCharacter character, int dayIndex)
        {
            if (!self.IsValid())
            {
                return (ErrorCode.ERR_Error, "active not valid");
            }
            if (!self.Items.TryGetValue(dayIndex, out var prizeItem))
            {
                return (ErrorCode.ERR_Error, "dayIndex not valid");
            }
            var activityData = character.ActivityComp.GetActivityData<TActivitySevenDayLoginData>(EActivityType.TActivitySevenDayLogin);
            if (activityData == null || !activityData.IsValid())
            {
                return (ErrorCode.ERR_Error, "activityData not valid");
            }
            if (activityData.ItemState.TryGetValue(dayIndex, out var itemPrizeState))
            {
                if (itemPrizeState == (int)EItemPrizeState.HadGet)
                {
                    return (ErrorCode.ERR_Error, "activityData had Get");
                }
                else if (itemPrizeState == (int)EItemPrizeState.CanNotGet)
                {
                    return (ErrorCode.ERR_Error, "activityData CanNotGet");
                }
                else if (itemPrizeState == (int)EItemPrizeState.OutOfDate)
                {
                    return (ErrorCode.ERR_Error, "activityData OutOfDate");
                }
                else if (itemPrizeState == (int)EItemPrizeState.CanGet)
                {
                    var addResult = character.BagComp.AddTItemOrMoney(prizeItem);
                    if (addResult.Item1 == ErrorCode.ERR_Success)
                    {
                        activityData.ItemState[dayIndex] = (int)EItemPrizeState.HadGet;
                    }
                    return addResult;
                }
            }
            return (ErrorCode.ERR_Error, "no activity data");
        }
    }
}
