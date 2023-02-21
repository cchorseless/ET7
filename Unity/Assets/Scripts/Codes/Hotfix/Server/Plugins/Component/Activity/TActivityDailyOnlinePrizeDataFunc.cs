using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET.Server
{
    [Invoke(TimerInvokeType.TActivityDailyOnlinePrize)]
    public class TActivityDailyOnlinePrizeCheckTimer : ATimer<TActivityDailyOnlinePrizeData>
    {
        protected override void Run(TActivityDailyOnlinePrizeData self)
        {
            try
            {
                var character = self.CharacterActivity.Character;
                var activity = character.GetMyServerZone().ActivityComp.GetActivity<TActivityDailyOnlinePrize>(EActivityType.TActivityDailyOnlinePrize);
                var onlineTime = character.TodayTotalOnlineTime();
                foreach (var time in activity.Items.Keys)
                {
                    if (time < onlineTime)
                    {
                        if (!self.ItemState.ContainsKey(time))
                        {
                            self.ItemState[time] = (int)EItemPrizeState.CanGet;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Log.Error($"TActivityDailyOnlinePrizeCheckTimer error: {self.Id}\n{e}");
            }
        }
    }
    public static class TActivityDailyOnlinePrizeDataFunc
    {
        public static void LoadAllChild(this TActivityDailyOnlinePrizeData self)
        {
            var character = self.CharacterActivity.Character;

            if (character.IsFirstLoginToday)
            {
                self.ItemState.Clear();
            }
            var activity = character.GetMyServerZone().ActivityComp.GetActivity<TActivityDailyOnlinePrize>(EActivityType.TActivityDailyOnlinePrize);
            var onlineTime = character.TodayTotalOnlineTime();
            foreach (var time in activity.Items.Keys)
            {
                if (time < onlineTime)
                {
                    if (!self.ItemState.ContainsKey(time))
                    {
                        self.ItemState[time] = (int)EItemPrizeState.CanGet;
                    }
                }
                else
                {
                    character.TimerEntityComp.AddOnceTimer(time - onlineTime + 100, TimerInvokeType.TActivityDailyOnlinePrize, self);
                }
            }
        }
    }
}
