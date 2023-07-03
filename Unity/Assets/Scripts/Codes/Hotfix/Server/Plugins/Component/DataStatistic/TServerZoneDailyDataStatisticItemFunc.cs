using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET.Server
{
    [ObjectSystem]
    public class TServerZoneDailyDataStatisticItemAwakeSystem: AwakeSystem<TServerZoneDailyDataStatisticItem>
    {
        protected override void Awake(TServerZoneDailyDataStatisticItem self)
        {
            var now = TimeHelper.DateTimeNow();
            self.Time = TimeHelper.ServerNow();
            self.Year = now.Year;
            self.Month = now.Month;
            self.Day = now.Day;
        }
    }

    public static class TServerZoneDailyDataStatisticItemFunc
    {
        public static bool IsTodayData(this TServerZoneDailyDataStatisticItem self)
        {
            var now = TimeHelper.DateTimeNow();
            return self.Day == now.Day && self.Month == now.Month && self.Year == now.Year;
        }

        public static async ETTask SaveAndExit(this TServerZoneDailyDataStatisticItem self, bool bexit = true)
        {
            DBComponent db = DBManagerComponent.Instance.GetZoneDB(GameConfig.AccountDBZone);
            await db.Save(self);
            if (bexit)
            {
                self.Dispose();
            }
        }

        public static int GetTodayPlayerNew(this TServerZoneDailyDataStatisticItem self)
        {
            int r = 0;
            foreach (int count in self.HoursPlayerNew)
            {
                r += count;
            }

            return r;
        }

        public static int GetTodayPlayerOnline(this TServerZoneDailyDataStatisticItem self)
        {
            int r = 0;
            foreach (int count in self.HoursPlayerOnline)
            {
                if (r < count)
                {
                    r = count;
                }
            }

            return r;
        }

        public static void MergeData(this TServerZoneDailyDataStatisticItem self, TServerZoneDailyDataStatisticItem other)
        {
            var count = self.HoursPlayerOnline.Length;
            for (var i = 0; i < count; i++)
            {
                self.HoursPlayerNew[count] += other.HoursPlayerNew[count];
                self.HoursPlayerOnline[count] += other.HoursPlayerOnline[count];
            }
        }

        public static void UpdateHoursPlayerNew(this TServerZoneDailyDataStatisticItem self)
        {
            int hour = TimeHelper.DateTimeNow().Hour;
            var d = self.HoursPlayerNew[hour];
            self.HoursPlayerNew[hour] = d + 1;
        }

        public static void UpdateHoursPlayerOnline(this TServerZoneDailyDataStatisticItem self)
        {
            int hour = TimeHelper.DateTimeNow().Hour;
            var playerComp = self.DomainScene().GetComponent<PlayerComponent>();
            if (playerComp != null)
            {
                var count = playerComp.idPlayers.Count;
                if (self.HoursPlayerOnline[hour] < count)
                {
                    self.HoursPlayerOnline[hour] = count;
                }
            }
        }
    }
}