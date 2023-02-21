using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET.Server
{
    public static class TRankSeasonTeamCharpterFunc
    {
        public static void LoadAllChild(this TRankSeasonTeamCharpter self)
        {

        }

        public static (int, string) GetDailyPrize(this TRankSeasonTeamCharpter self, TCharacter character)
        {
            //if (!self.IsValid())
            //{
            //    return (ErrorCode.ERR_Error, "active not valid");
            //}
            //if (!self.Items.TryGetValue(dayIndex, out var prizeItem))
            //{
            //    return (ErrorCode.ERR_Error, "dayIndex not valid");
            //}
            //var activityData = character.ActivityComp.GetActivityData<TActivityMonthLoginData>(EActivityType.TActivityMonthLogin);
            //if (activityData == null || !activityData.IsValid())
            //{
            //    return (ErrorCode.ERR_Error, "activityData not valid");
            //}
            //if (activityData.ItemState.TryGetValue(dayIndex, out var itemPrizeState))
            //{
            //    if (itemPrizeState == (int)EItemPrizeState.HadGet)
            //    {
            //        return (ErrorCode.ERR_Error, "activityData had Get");
            //    }
            //    else if (itemPrizeState == (int)EItemPrizeState.CanNotGet)
            //    {
            //        return (ErrorCode.ERR_Error, "activityData CanNotGet");
            //    }
            //    else if (itemPrizeState == (int)EItemPrizeState.OutOfDate)
            //    {
            //        return (ErrorCode.ERR_Error, "activityData OutOfDate");
            //    }
            //    else if (itemPrizeState == (int)EItemPrizeState.CanGet)
            //    {
            //        var addResult = character.BagComp.AddTItemOrMoney(prizeItem);
            //        if (addResult.Item1 == ErrorCode.ERR_Success)
            //        {
            //            activityData.ItemState[dayIndex] = (int)EItemPrizeState.HadGet;
            //        }
            //        return addResult;
            //    }
            //}
            return (ErrorCode.ERR_Error, "no activity data");
        }
    }
}
