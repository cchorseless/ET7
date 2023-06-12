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
            var SeasonConfigId = self.CharacterActivity.Character.GetMyServerZone().SeasonComp.CurSeasonConfigId;
            if (self.SeasonConfigId != SeasonConfigId)
            {
                self.SeasonConfigId = SeasonConfigId;
                self.TotalChargeMoney = 0;
                self.ItemHadGet.Clear();
            }
        }

        public static void AddTotalGainMetaStone(this TActivityTotalGainMetaStoneData self, int chargeMoney)
        {
            if (chargeMoney < 0) { return; }
            self.TotalChargeMoney += chargeMoney;
            var character = self.CharacterActivity.Character;
            character.SyncHttpEntity(self);
        }
    }
}
