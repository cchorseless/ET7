using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET.Server
{

    public static class EActivityType
    {
        public const short TActivitySevenDayLogin = 1;
        public const short TActivityMonthLogin = 2;
        public const short TActivityDailyOnlinePrize = 3;
        public const short TActivityInvestMetaStone = 4;
        public const short TActivityTotalGainMetaStone = 5;
        public const short TActivityGiftCommond = 6;
        public const short TActivityFirstCharge = 7;
        public const short TActivityTotalOnlineTime = 8;
        public const short TActivityTotalSpendMetaStone = 9;
        public const short TActivityMentorshipTree = 10;
   
        public const short ServerZoneActivityMax = 1000;

    }


    public static class TActivityFunc
    {
        public static void LoadAllChild(this TActivity self)
        {
            throw new NotSupportedException("Type " + self.GetType() + " is not supported by LoadAllChild extension method");
        }

        public static void SetNeverOutOfDate(this TActivity self)
        {
            self.StartTime = 0;
            self.EndTime = TimeHelper.ServerNow() / 500;
        }

        public static bool IsValid(this TActivity self)
        {
            var config = LuBanConfigComponent.Instance.Config().ActivityConfig.GetOrDefault(self.ActivityId);
            if (config == null || !config.IsValid )
            {
                return false;
            }
            var curTime = TimeHelper.ServerNow() / 1000;
            return self.ValidState && self.EndTime > curTime && curTime >= self.StartTime;
        }

        public static bool IsOutOfDate(this TActivity self)
        {
            var config = LuBanConfigComponent.Instance.Config().ActivityConfig.GetOrDefault(self.ActivityId);
            if (config == null || !config.IsValid )
            {
                return false;
            }
            var curTime = TimeHelper.ServerNow() / 1000;
            return self.ValidState && self.EndTime <= curTime;
        }
    }
}
