using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET.Server
{
    public static class TActivityTotalSpendMetaStoneDataFunc
    {
        public static void LoadAllChild(this TActivityTotalSpendMetaStoneData self)
        {
            self.AddTotalGainMetaStone(0);
        }

        public static void AddTotalGainMetaStone(this TActivityTotalSpendMetaStoneData self, int spendMetaStone)
        {
            if (spendMetaStone < 0) { return; }
            self.TotalSpendMetaStone += spendMetaStone;
            var character = self.CharacterActivity.Character;
            var activity = character.GetMyServerZone().ActivityComp.GetActivity<TActivityTotalSpendMetaStone>(EActivityType.TActivityTotalSpendMetaStone);
            foreach (var metastone in activity.Items.Keys)
            {
                if (metastone < self.TotalSpendMetaStone)
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
