using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET.Server
{
    public static class TActivityMemberShipDataFunc
    {
        public static void LoadAllChild(this TActivityMemberShipData self)
        {
        }

        public static bool IsVip(this TActivityMemberShipData self)
        {
            var curTime = TimeHelper.ServerNow() / 1000;
            return self.VipEndTime > curTime && curTime >= self.VipStartTime;
        }

    }
}
