using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET.Server
{
    public static class TActivityDailyOnlinePrizeDataFunc
    {
        public static void LoadAllChild(this TActivityDailyOnlinePrizeData self)
        {
            var character = self.CharacterActivity.Character;

            if (character.IsFirstLoginToday)
            {
                self.ItemHadGet.Clear();
            }

            self.LoginTimeSpan = TimeHelper.ServerNow();
        }

        public static long GetSecTodayOnlineTime(this TActivityDailyOnlinePrizeData self)
        {
            var character = self.CharacterActivity.Character;
            return (TimeHelper.ServerNow() - self.LoginTimeSpan + character.TodayOnlineTime) / 1000;
        }
    }
}