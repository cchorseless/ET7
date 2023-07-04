using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET.Server
{
    public static class ServerZoneDataStatisticComponentFunc
    {
        public static void LoadAllChild(this ServerZoneDataStatisticComponent self)
        {
            self.GetCurDataItem().SaveAndExit(false).Coroutine();
            self.UpdateStatisticData().Coroutine();
        }

        public static async ETTask UpdateStatisticData(this ServerZoneDataStatisticComponent self)
        {
            await TimerComponent.Instance.WaitAsync(10 * 60 * 1000);
            if (self.IsDisposed)
            {
                return;
            }

            self.GetCurDataItem().UpdateHoursPlayerOnline();
            await self.GetCurDataItem().UpdateTotalPlayerCount();
            await self.GetCurDataItem().SaveAndExit(false);
            self.UpdateStatisticData().Coroutine();
        }

        public static TServerZoneDailyDataStatisticItem GetCurDataItem(this ServerZoneDataStatisticComponent self)
        {
            if (self.CurDataItemId != 0)
            {
                var entity = self.GetChild<TServerZoneDailyDataStatisticItem>(self.CurDataItemId);
                if (entity != null)
                {
                    if (entity.IsTodayData())
                    {
                        return entity;
                    }
                    else
                    {
                        entity.SaveAndExit().Coroutine();
                        self.CurDataItemId = 0;
                    }
                }
            }

            var datareport = self.AddChild<TServerZoneDailyDataStatisticItem>();
            datareport.ServerID = self.ServerZone.ServerID;
            self.CurDataItemId = datareport.Id;
            return datareport;
        }
    }
}