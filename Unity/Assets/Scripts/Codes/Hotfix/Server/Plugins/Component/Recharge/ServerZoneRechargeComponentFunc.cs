using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET.Server
{
    public enum ERechargeType
    {
        Recharge_30 = 30,
        Recharge_68 = 68,
        Recharge_128 = 128,
        Recharge_198 = 198,
        Recharge_328 = 328,
        Recharge_648 = 648,
        Recharge_9999 = 9999,
    }


    public static class ServerZoneRechargeComponentFunc
    {
        public static void LoadAllChild(this ServerZoneRechargeComponent self)
        {

        }

    }
}
