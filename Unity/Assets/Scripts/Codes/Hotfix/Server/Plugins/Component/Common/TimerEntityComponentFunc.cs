using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET.Server
{
    [ObjectSystem]
    public class TimerEntityComponentDestroySystem : DestroySystem<TimerEntityComponent>
    {
        protected override void Destroy(TimerEntityComponent self)
        {
            self.TimerIdList.ForEach(timerid =>
            {
                TimerComponent.Instance.Remove(ref timerid);
            });
            self.TimerIdList.Clear();
        }
    }
    public static class TimerEntityComponentFunc
    {
        public static long AddOnceTimer(this TimerEntityComponent self, long tillTime, int type, Entity args)
        {
            var timerId = TimerComponent.Instance.NewOnceTimer(tillTime, type, args);
            self.TimerIdList.Add(timerId);
            return timerId;
        }
        public static long AddFrameTimer(this TimerEntityComponent self, int type, Entity args)
        {
            var timerId = TimerComponent.Instance.NewFrameTimer(type, args);
            self.TimerIdList.Add(timerId);
            return timerId;
        }
        public static long AddRepeatedTimer(this TimerEntityComponent self, long time, int type, Entity args)
        {
            var timerId = TimerComponent.Instance.NewRepeatedTimer(time, type, args);
            self.TimerIdList.Add(timerId);
            return timerId;
        }

        public static void RemoveTimer(this TimerEntityComponent self, long timerId)
        {
            self.TimerIdList.Remove(timerId);
            TimerComponent.Instance.Remove(ref timerId);
        }
    }
}
