using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ET.Server
{
    public static class TActivitySevenDayLoginDataFunc
    {
        public static void LoadAllChild(this TActivitySevenDayLoginData self)
        {
            var SeasonConfigId = self.CharacterActivity.Character.GetMyServerZone().SeasonComp.CurSeasonConfigId;
            if (self.SeasonConfigId != SeasonConfigId)
            {
                self.SeasonConfigId = SeasonConfigId;
                self.LoginDayCount = 0;
                self.ItemHadGet.Clear();
            }
            
            if (self.CharacterActivity.Character.IsFirstLoginToday && self.LoginDayCount < 7)
            {
                self.LoginDayCount++;
            }
        }
    }
}
