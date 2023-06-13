using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ET.Server
{
    public static class CharacterRankComponentFunc
    {
        public static void LoadAllChild(this CharacterRankComponent self)
        {
            var seasonConfigId = self.Character.GetMyServerZone().SeasonComp.CurSeasonConfigId;
            self.SeasonConfigId = seasonConfigId;
            if (self.SeasonRankData.TryGetValue(seasonConfigId, out var entityid))
            {
                var entity = self.GetChild<TSeasonCharacterRankData>(entityid);
                entity.LoadAllChild();
            }
            else
            {
                var entity = self.AddChild<TSeasonCharacterRankData, int>(seasonConfigId);
                entity.SeasonConfigId = seasonConfigId;
                self.SeasonRankData.Add(seasonConfigId, entity.Id);
                entity.LoadAllChild();
            }
        }

        public static void UpdataRankData(this CharacterRankComponent self, int ranktype, int score, bool issync = true)
        {
            self.Character.GetMyServerZone().RankComp.UpdataRankData(ranktype, self.Character.Id, self.Character.Name, score);
            if (self.SeasonRankData.TryGetValue(self.SeasonConfigId, out var entityid))
            {
                var entity = self.GetChild<TSeasonCharacterRankData>(entityid);
                entity.UpdateRankSingleData(ranktype);
                if (issync)
                {
                    self.Character.SyncHttpEntity(entity);
                }
            }
        }
    }
}