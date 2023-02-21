
namespace ET
{
    public static partial class TimeHelper
    {
        public static int WeekCount(DateTime dt)
        {
            int firstWeekend = Convert.ToInt32(DateTime.Parse(dt.Year + "-1-1").DayOfWeek);
            int weekDay = firstWeekend == 0 ? 1 : (7 - firstWeekend + 1);
            int currentDay = dt.DayOfYear;
            return Convert.ToInt32(Math.Ceiling((currentDay - weekDay) / 7.0)) + 1;
        }
    }
}