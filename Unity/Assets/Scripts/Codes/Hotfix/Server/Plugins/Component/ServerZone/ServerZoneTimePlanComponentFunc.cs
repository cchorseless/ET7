using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ET.Server
{
    [ObjectSystem]
    public class ServerZoneTimePlanComponentAwakeSystem : AwakeSystem<ServerZoneTimePlanComponent>
    {
        protected override void Awake(ServerZoneTimePlanComponent self)
        {
            self.LoadAllChild();
        }
    }

    public static class ServerZoneTimePlanComponentFunc
    {
        public static void LoadAllChild(this ServerZoneTimePlanComponent self)
        {
        }

        public static void AddHourPlan(this ServerZoneTimePlanComponent self, ETimePlanLabel label, int min)
        {
            var now = TimeHelper.ServerNow();
            var today = DateTime.Today.ToUniversalTime().AddMinutes(min);
            while (TimeInfo.Instance.Transition(today) - now <= 1000)
            {
                today = today.AddMinutes(min);
            }
            var delay = TimeInfo.Instance.Transition(today) - now;
            var plan = self.AddChild<TServerZoneTimePlan>();
            plan.TimePlanType = TimerInvokeType.ServerZoneHourPlan;
            plan.TimePlanLabel = (int)label;
            plan.Min = min;
            self.ServerZone.TimerEntityComp.AddOnceTimer(now + delay, TimerInvokeType.ServerZoneHourPlan, plan);
        }


        public static void AddDailyPlan(this ServerZoneTimePlanComponent self, ETimePlanLabel label, int hour, int min, int sec)
        {
            var now = TimeHelper.ServerNow();
            var today = DateTime.Today.ToUniversalTime().AddHours(hour).AddMinutes(min).AddSeconds(sec);
            var delay = TimeInfo.Instance.Transition(today) - now;
            if (delay <= 1000)
            {
                today = today.AddDays(1);
                delay = TimeInfo.Instance.Transition(today) - now;
            }
            var plan = self.AddChild<TServerZoneTimePlan>();
            plan.TimePlanType = TimerInvokeType.ServerZoneDailyPlan;
            plan.TimePlanLabel = (int)label;
            plan.Hour = hour;
            plan.Min = min;
            plan.Sec = sec;
            self.ServerZone.TimerEntityComp.AddOnceTimer(now + delay, TimerInvokeType.ServerZoneDailyPlan, plan);
        }
        public static void AddWeekPlan(this ServerZoneTimePlanComponent self, ETimePlanLabel label, int dayIndex, int hour, int min, int sec)
        {
            var now = TimeHelper.ServerNow();
            var today = DateTime.Today.ToUniversalTime().AddHours(hour).AddMinutes(min).AddSeconds(sec);
            int dalayDay = dayIndex - (int)today.DayOfWeek;
            if (dalayDay >= 0)
            {
                today = today.AddDays(dalayDay);
            }
            else
            {
                today = today.AddDays(dalayDay + 7);
            }
            var delay = TimeInfo.Instance.Transition(today) - now;
            if (delay <= 1000)
            {
                today = today.AddDays(7);
                delay = TimeInfo.Instance.Transition(today) - now;
            }
            var plan = self.AddChild<TServerZoneTimePlan>();
            plan.TimePlanType = TimerInvokeType.ServerZoneWeekPlan;
            plan.TimePlanLabel = (int)label;
            plan.DayIndex = dayIndex;
            plan.Hour = hour;
            plan.Min = min;
            plan.Sec = sec;
            self.ServerZone.TimerEntityComp.AddOnceTimer(now + delay, TimerInvokeType.ServerZoneWeekPlan, plan);
        }
        public static void AddMonthPlan(this ServerZoneTimePlanComponent self, ETimePlanLabel label, int dayIndex, int hour, int min, int sec)
        {
            var now = TimeHelper.ServerNow();
            var today = DateTime.Today.ToUniversalTime().AddHours(hour).AddMinutes(min).AddSeconds(sec);
            int dalayDay = dayIndex - today.Day;
            if (dalayDay >= 0)
            {
                today = today.AddDays(dalayDay);
            }
            else
            {
                today = today.AddMonths(1);
            }
            var delay = TimeInfo.Instance.Transition(today) - now;
            if (delay <= 1000)
            {
                today = today.AddMonths(1);
                delay = TimeInfo.Instance.Transition(today) - now;
            }
            var plan = self.AddChild<TServerZoneTimePlan>();
            plan.TimePlanType = TimerInvokeType.ServerZoneMonthPlan;
            plan.TimePlanLabel = (int)label;
            plan.DayIndex = dayIndex;
            plan.Hour = hour;
            plan.Min = min;
            plan.Sec = sec;
            self.ServerZone.TimerEntityComp.AddOnceTimer(now + delay, TimerInvokeType.ServerZoneMonthPlan, plan);
        }
    }
}
