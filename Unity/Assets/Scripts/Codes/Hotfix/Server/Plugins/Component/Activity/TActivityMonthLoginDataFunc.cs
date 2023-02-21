using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET.Server
{
    [ObjectSystem]
    public class TActivityMonthLoginDataAwakeSystem : AwakeSystem<TActivityMonthLoginData>
    {
        protected override void Awake(TActivityMonthLoginData self)
        {
            var serverZone = self.CharacterActivity.Character.GetMyServerZone();
            var activity = serverZone.ActivityComp.GetActivity<TActivityMonthLogin>(EActivityType.TActivityMonthLogin);
            foreach (var index in activity.Items.Keys)
            {
                self.ItemState.Add(index, (int)EItemPrizeState.CanNotGet);
            }
            foreach(var index in activity.TotalLoginItems.Keys)
            {
                self.TotalLoginItemState.Add(index, (int)EItemPrizeState.CanNotGet);
            }
        }
    }


    public static class TActivityMonthLoginDataFunc
    {
        public static void LoadAllChild(this TActivityMonthLoginData self)
        {
            if (self.CharacterActivity.Character.IsFirstLoginToday)
            {
                var dayindex = DateTime.Today.Day;
                if (self.ItemState.ContainsKey(dayindex) && self.ItemState[dayindex] == (int)EItemPrizeState.CanNotGet)
                {
                    self.ItemState[dayindex] = (int)EItemPrizeState.CanGet;
                }
                int totalLoginDay = 0;
                foreach (var item in self.ItemState)
                {
                    if (item.Value == (int)EItemPrizeState.CanGet || item.Value == (int)EItemPrizeState.HadGet)
                    {
                        totalLoginDay++;
                    }
                }
                if (totalLoginDay > 0 && self.TotalLoginItemState.ContainsKey(totalLoginDay)
                    && self.TotalLoginItemState[totalLoginDay] == (int)EItemPrizeState.CanNotGet)
                {
                    self.TotalLoginItemState[totalLoginDay] = (int)EItemPrizeState.CanGet;
                }
            }
        }
    }
}
