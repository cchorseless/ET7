using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET.Server
{
    public static class TActivityTotalGainMetaStoneDataFunc
    {
        public static void LoadAllChild(this TActivityTotalGainMetaStoneData self)
        {
            self.AddTotalGainMetaStone(0);
        }

        public static void AddTotalGainMetaStone(this TActivityTotalGainMetaStoneData self, int gainMetaStone)
        {
            if (gainMetaStone < 0) { return; }
            self.TotalGainMetaStone += gainMetaStone;
            var character = self.CharacterActivity.Character;
            var activity = character.GetMyServerZone().ActivityComp.GetActivity<TActivityTotalGainMetaStone>(EActivityType.TActivityTotalGainMetaStone);
            foreach (var metastone in activity.Items.Keys)
            {
                if (metastone < self.TotalGainMetaStone)
                {
                    if (!self.ItemState.ContainsKey(metastone))
                    {
                        self.ItemState[metastone] = (int)EItemPrizeState.CanGet;
                    }
                }
            }
        }
    }
}
