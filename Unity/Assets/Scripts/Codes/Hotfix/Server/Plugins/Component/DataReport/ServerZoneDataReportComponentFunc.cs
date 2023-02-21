using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET.Server
{
    public static class ServerZoneDataReportComponentFunc
    {
        public static void LoadAllChild(this ServerZoneDataReportComponent self)
        {
            self.GetCurDataItem();
            self.ServerZone.TimerPlanComp.AddDailyPlan(ETimePlanLabel.ServerZoneDataReportUpdate, 23, 59, 59);
        }

        public static void ServerZoneDataReportUpdate(this ServerZoneDataReportComponent self)
        {

        }


        public static TServerZoneDailyDataReportItem GetCurDataItem(this ServerZoneDataReportComponent self)
        {
            if (self.CurDataReportItemId != 0)
            {
                var entity = self.GetChild<TServerZoneDailyDataReportItem>(self.CurDataReportItemId);
                if (entity != null)
                {
                    if (entity.IsTodayData())
                    {
                        return entity;
                    }
                    else
                    {
                        entity.SaveAndDispose().Coroutine();
                        self.CurDataReportItemId = 0;
                    }
                }
            }
            var datareport = self.AddChild<TServerZoneDailyDataReportItem>();
            datareport.ServerID = self.ServerZone.ServerID;
            self.CurDataReportItemId = datareport.Id;
            return datareport;
        }
    }
}
