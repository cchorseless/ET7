﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET.Server
{
    public static class TActivityTotalSpendMetaStoneFunc
    {
        public static void LoadAllChild(this TActivityTotalSpendMetaStone self)
        {
            self.Items.Clear();
            var curTime = TimeHelper.ServerNow() / 1000;
            var data = LuBanConfigComponent.Instance.Config().TActivityTotalSpendMetaStone.DataList.Find(
                record =>
                {
                    return record.ActivityStartTime <= curTime && curTime <= (record.ActivityEndTime);
                });

            if (data != null)
            {
                self.ConfigId = data.Id;
                self.StartTime = data.ActivityStartTime;
                self.EndTime = data.ActivityEndTime;
                data.PrizeItemGroup.ForEach(item =>
                {
                    if (!self.Items.ContainsKey(item.Index))
                    {
                        var itemList = new List<FItemInfo>();
                        item.ItemGroup.ForEach(_info =>
                        {
                            itemList.Add(new FItemInfo(_info.ItemConfigId, _info.ItemCount));
                        });
                        self.Items.Add(item.Index, itemList);
                    }
                });
            }
        }


        public static (int, string) GetPrize(this TActivityTotalSpendMetaStone self, TCharacter character, int metaStoneCount)
        {
            if (!self.IsValid())
            {
                return (ErrorCode.ERR_Error, "active not valid");
            }
            if (!self.Items.TryGetValue(metaStoneCount, out var prizeItem))
            {
                return (ErrorCode.ERR_Error, "dayIndex not valid");
            }
            var activityData = character.ActivityComp.GetActivityData<TActivityTotalSpendMetaStoneData>(EActivityType.TActivityTotalSpendMetaStone);
            if (activityData == null || !activityData.IsValid())
            {
                return (ErrorCode.ERR_Error, "activityData not valid");
            }
            if (activityData.ItemState.TryGetValue(metaStoneCount, out var itemPrizeState))
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
                        activityData.ItemState[metaStoneCount] = (int)EItemPrizeState.HadGet;
                    }
                    return addResult;
                }
            }
            return (ErrorCode.ERR_Error, "no activity data");
        }
    }
}
