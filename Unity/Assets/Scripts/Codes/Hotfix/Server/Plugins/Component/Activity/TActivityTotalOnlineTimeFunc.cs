using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET.Server
{
    public static class TActivityTotalOnlineTimeFunc
    {
        public static void LoadAllChild(this TActivityTotalOnlineTime self)
        {
            self.Items.Clear();
            var curTime = TimeHelper.ServerNow() / 1000;
            var data = LuBanConfigComponent.Instance.Config().TActivityTotalOnlineTime.DataList.Find(
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


        public static (int, string) GetPrize(this TActivityTotalOnlineTime self, TCharacter character, int onlineTime)
        {
            if (!self.IsValid())
            {
                return (ErrorCode.ERR_Error, "active not valid");
            }
            if (!self.Items.TryGetValue(onlineTime, out var prizeItem))
            {
                return (ErrorCode.ERR_Error, "dayIndex not valid");
            }
            var activityData = character.ActivityComp.GetActivityData<TActivityTotalOnlineTimeData>(EActivityType.TActivityTotalOnlineTime);
            if (activityData == null || !activityData.IsValid())
            {
                return (ErrorCode.ERR_Error, "activityData not valid");
            }
            activityData.UpdateTotalOnlineTime();
            if (activityData.ItemState.TryGetValue(onlineTime, out var itemPrizeState))
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
                        activityData.ItemState[onlineTime] = (int)EItemPrizeState.HadGet;
                    }
                    return addResult;
                }
            }
            return (ErrorCode.ERR_Error, "no activity data");
        }
    }
}
