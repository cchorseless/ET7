using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET.Server
{
    public static class TActivityDataFunc
    {
        public static void LoadAllChild(this TActivityData self)
        {
            throw new NotSupportedException("Type " + self.GetType() + " is not supported by LoadAllChild extension method");
        }


        public static bool IsValid(this TActivityData self)
        {
            var config = LuBanConfigComponent.Instance.Config().ActivityConfig.GetOrDefault(self.ConfigId);
            if (config == null || !config.IsValid )
            {
                return false;
            }
            var curTime = TimeHelper.ServerNow() / 1000;
            return self.EndTime > curTime && curTime >= self.StartTime;
        }
    }
}
