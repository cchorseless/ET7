using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET.Server
{
    [ObjectSystem]
    public class TServerZoneSeasonAwakeSystem: AwakeSystem<TServerZoneSeason, int>
    {
        protected override void Awake(TServerZoneSeason self, int configid)
        {
            self.ConfigId = configid;
            self.StartNewSeason();
        }
    }

    public static class TServerZoneSeasonFunc
    {
        public static void LoadAllChild(this TServerZoneSeason self)
        {
            var config = LuBanConfigComponent.Instance.Config().SeasonConfig.GetOrDefault(self.ConfigId);
            if (config == null)
            {
                Log.Error("cant find season config");
            }

            self.EndTime = config.SeasonEndTime;
            self.StartTime = config.SeasonStartTime;
        }

        public static bool IsValid(this TServerZoneSeason self)
        {
            var config = LuBanConfigComponent.Instance.Config().SeasonConfig.GetOrDefault(self.ConfigId);
            if (config == null)
            {
                return false;
            }

            var curTime = TimeHelper.ServerNow() / 1000;
            return self.EndTime > curTime && curTime >= self.StartTime;
        }

        public static bool IsInSeasonDuration(this TServerZoneSeason self, long curTime)
        {
            return self.EndTime > curTime && curTime >= self.StartTime;
        }

        public static void StartNewSeason(this TServerZoneSeason self)
        {
        }
    }
}