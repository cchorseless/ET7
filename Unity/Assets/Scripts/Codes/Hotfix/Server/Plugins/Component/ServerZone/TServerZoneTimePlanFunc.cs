using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET.Server
{
    [Invoke(TimerInvokeType.ServerZoneHourPlan)]
    public class TServerZoneTimePlan_HourCheckTimer : ATimer<TServerZoneTimePlan>
    {
        protected override void Run(TServerZoneTimePlan self)
        {
            self.TimePlanComp.ServerZone.TimerEntityComp.AddOnceTimer(TimeHelper.ServerNow() + self.Min * 60 * 1000, TimerInvokeType.ServerZoneHourPlan, self);
        }
    }


    [Invoke(TimerInvokeType.ServerZoneDailyPlan)]
    public class TServerZoneTimePlan_DailyCheckTimer : ATimer<TServerZoneTimePlan>
    {
        protected override void Run(TServerZoneTimePlan self)
        {
            switch (self.TimePlanLabel)
            {
                case (int)ETimePlanLabel.ServerZoneDataReportUpdate:
                    self.TimePlanComp.ServerZone.DataReportComp?.ServerZoneDataReportUpdate();
                    break;
            }
            self.TimePlanComp.AddDailyPlan((ETimePlanLabel)self.TimePlanLabel, self.Hour, self.Min, self.Sec);
            self.Dispose();
        }
    }

    [Invoke(TimerInvokeType.ServerZoneWeekPlan)]
    public class TServerZoneTimePlan_WeekCheckTimer : ATimer<TServerZoneTimePlan>
    {
        protected override void Run(TServerZoneTimePlan self)
        {
            self.TimePlanComp.AddWeekPlan((ETimePlanLabel)self.TimePlanLabel, self.DayIndex, self.Hour, self.Min, self.Sec);
            self.Dispose();
        }
    }

    [Invoke(TimerInvokeType.ServerZoneMonthPlan)]
    public class TServerZoneTimePlan_MonthCheckTimer : ATimer<TServerZoneTimePlan>
    {
        protected override void Run(TServerZoneTimePlan self)
        {
            self.TimePlanComp.AddMonthPlan((ETimePlanLabel)self.TimePlanLabel, self.DayIndex, self.Hour, self.Min, self.Sec);
            self.Dispose();
        }
    }

    public enum ETimePlanLabel
    {
        Common = 0,
        ServerZoneDataReportUpdate = 1,
    }


    public static class TServerZoneTimePlanFunc
    {
        public static bool IsHourTime(this TServerZoneTimePlan self, int min)
        {
            return self.TimePlanType == TimerInvokeType.ServerZoneHourPlan &&
            self.Min == min;
        }
        public static bool IsDailyTime(this TServerZoneTimePlan self, int hour, int min, int sec)
        {
            return self.TimePlanType == TimerInvokeType.ServerZoneDailyPlan &&
            self.Hour == hour &&
            self.Min == min &&
            self.Sec == sec;
        }
        public static bool IsWeekTime(this TServerZoneTimePlan self, int dayIndex, int hour, int min, int sec)
        {
            return self.TimePlanType == TimerInvokeType.ServerZoneWeekPlan &&
            self.DayIndex == dayIndex &&
            self.Hour == hour &&
            self.Min == min &&
            self.Sec == sec;
        }
        public static bool IsMonthTime(this TServerZoneTimePlan self, int dayIndex, int hour, int min, int sec)
        {
            return self.TimePlanType == TimerInvokeType.ServerZoneMonthPlan &&
            self.DayIndex == dayIndex &&
            self.Hour == hour &&
            self.Min == min &&
            self.Sec == sec;
        }
    }
}
