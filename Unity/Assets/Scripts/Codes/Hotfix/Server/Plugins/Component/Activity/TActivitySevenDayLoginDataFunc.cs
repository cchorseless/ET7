using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ET.Server
{
    [ObjectSystem]
    public class TActivitySevenDayLoginDataAwakeSystem : AwakeSystem<TActivitySevenDayLoginData>
    {
        protected override void Awake(TActivitySevenDayLoginData self)
        {
            for (var i = 1; i < 8; i++)
            {
                self.ItemState.Add(i, (int)EItemPrizeState.CanNotGet);
            }
        }
    }

    public static class TActivitySevenDayLoginDataFunc
    {
        public static void LoadAllChild(this TActivitySevenDayLoginData self)
        {
            if (self.CharacterActivity.Character.IsFirstLoginToday && self.LoginDayCount < 7)
            {
                self.LoginDayCount++;
                if (self.ItemState.ContainsKey(self.LoginDayCount))
                {
                    if (self.ItemState[self.LoginDayCount] == (int)EItemPrizeState.CanNotGet)
                    {
                        self.ItemState[self.LoginDayCount] = (int)EItemPrizeState.CanGet;
                    }
                }
            }
        }
    }
}
