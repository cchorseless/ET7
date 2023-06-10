using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET.Server
{
    public static class TActivityMonthLoginDataFunc
    {
        public static void LoadAllChild(this TActivityMonthLoginData self)
        {
            var monthindex = TimeHelper.DateTimeNow().Month;
            if (self.MonthIndex != monthindex)
            {
                self.MonthIndex = monthindex;
                self.LoginDayCount = 0;
                self.ItemHadGet.Clear();
                self.TotalLoginItemHadGet.Clear();
            }
            if (self.CharacterActivity.Character.IsFirstLoginToday)
            {
                self.LoginDayCount++;
            }
        }
    }
}