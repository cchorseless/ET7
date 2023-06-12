using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET.Server
{
    [ObjectSystem]
    public class TActivityDailyOnlinePrizeDataSystem: DestroySystem<TActivityDailyOnlinePrizeData>
    {
        protected override void Destroy(TActivityDailyOnlinePrizeData self)
        {
            self.TodayOnlineTime += TimeHelper.ServerNow() - self.LoginTimeSpan;
        }
    }
    public static class TActivityDailyOnlinePrizeDataFunc
    {
        public static void LoadAllChild(this TActivityDailyOnlinePrizeData self)
        {
            var character = self.CharacterActivity.Character;

            if (character.IsFirstLoginToday)
            {
                self.ItemHadGet.Clear();
                self.TodayOnlineTime = 0;
            }

            self.LoginTimeSpan = TimeHelper.ServerNow();
        }

        public static long GetSecTodayOnlineTime(this TActivityDailyOnlinePrizeData self)
        {
            var character = self.CharacterActivity.Character;
            return (TimeHelper.ServerNow() - self.LoginTimeSpan + self.TodayOnlineTime) / 1000;
        }
    }
}