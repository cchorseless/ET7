using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET.Server
{
    [ObjectSystem]
    public class TServerZoneDailyDataReportItemAwakeSystem : AwakeSystem<TServerZoneDailyDataReportItem>
    {
        protected override void Awake(TServerZoneDailyDataReportItem self)
        {
            self.Time = TimeInfo.Instance.Transition(DateTime.Today.ToUniversalTime());
        }
    }

    public static class TServerZoneDailyDataReportItemFunc
    {
        public static bool IsTodayData(this TServerZoneDailyDataReportItem self)
        {
            var date = TimeInfo.Instance.ToDateTime(self.Time);
            var now = DateTime.Today.ToUniversalTime();
            return date.DayOfYear == now.DayOfYear && date.Year == now.Year;
        }
        public static async ETTask SaveAndDispose(this TServerZoneDailyDataReportItem self)
        {
            DBComponent db = DBManagerComponent.Instance.GetZoneDB(GameConfig.AccountDBZone);
            await db.Save(self);
            self.Dispose();
        }
    }
}
