using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET.Server
{
    [ObjectSystem]
    public class TActivityTotalOnlineTimeDataDestroySystem : DestroySystem<TActivityTotalOnlineTimeData>
    {
        protected override void Destroy(TActivityTotalOnlineTimeData self)
        {
            var character = self.CharacterActivity.Character;
            self.LastLoginTotalOnlineTime += TimeHelper.ServerNow() - character.LastLoginTime;
        }
    }

    public static class TActivityTotalOnlineTimeDataFunc
    {
        public static void LoadAllChild(this TActivityTotalOnlineTimeData self)
        {
            self.UpdateTotalOnlineTime();
        }

        public static long GetTotalOnlineTime(this TActivityTotalOnlineTimeData self)
        {
            var character = self.CharacterActivity.Character;
            return self.LastLoginTotalOnlineTime + TimeHelper.ServerNow() - character.LastLoginTime;
        }

        public static void UpdateTotalOnlineTime(this TActivityTotalOnlineTimeData self)
        {
            var character = self.CharacterActivity.Character;
            var activity = character.GetMyServerZone().ActivityComp.GetActivity<TActivityTotalOnlineTime>(EActivityType.TActivityTotalOnlineTime);
            var totaltime = self.GetTotalOnlineTime();
            foreach (var onlinetime in activity.Items.Keys)
            {
                if (onlinetime < totaltime)
                {
                    if (!self.ItemState.ContainsKey(onlinetime))
                    {
                        self.ItemState[onlinetime] = (int)EItemPrizeState.CanGet;
                    }
                }
            }
        }

    }
}
