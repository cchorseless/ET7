using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET.Server
{
    public static class ServerZoneSeasonComponentFunc
    {
        public static void LoadAllChild(this ServerZoneSeasonComponent self)
        {
            var curTime = TimeHelper.ServerNow() / 1000;
            var curSeasonConfig = LuBanConfigComponent.Instance.Config().SeasonConfig.DataList.Find(record =>
            {
                return record.SeasonStartTime <= curTime && curTime <= record.SeasonEndTime;
            });
            if(curSeasonConfig == null)
            {
                Log.Error("CANT FIND SEASON CONFIG");
                return;
            }
            var season = self.CurSeason;
            if (self.CurSeasonConfigId == 0 || season == null)
            {
                self.AddSeason(curSeasonConfig.Id);
            }
            else
            {
                if (season.IsValid())
                {
                    season.LoadAllChild();
                }
                else
                {
                    self.AddSeason(curSeasonConfig.Id);
                }
            }

        }

        public static void AddSeason(this ServerZoneSeasonComponent self, int seasonConfigId)
        {
            if (!self.Seasons.ContainsKey(seasonConfigId) &&
                LuBanConfigComponent.Instance.Config().SeasonConfig.GetOrDefault(seasonConfigId) != null)
            {
                var entity = self.AddChild<TServerZoneSeason, int>(seasonConfigId);
                self.Seasons.Add(seasonConfigId, entity.Id);
                self.CurSeasonConfigId = seasonConfigId;
                entity.LoadAllChild();
            }
        }
    }
}
