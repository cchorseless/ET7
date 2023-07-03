using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ET.Server
{
    public static class EBuffTargetType
    {
        public const string AllPlayer = "AllPlayer";
        public const string AllHero = "AllHero";
        public const string SingleHero = "SingleHero";
    }
    public static class TBuffItemFunc
    {
        public static Conf.Item.ItemBuffConfigRecord Config(this TBuffItem self)
        {
            return LuBanConfigComponent.Instance.Config().ItemBuffConfig.GetOrDefault(self.ConfigId);
        }

        public static bool IsOutOfDate(this TBuffItem self)
        {
            if (self.DisabledTime == -1) { return false; }
            return TimeHelper.ServerNow() > self.DisabledTime;
        }



    }
}
