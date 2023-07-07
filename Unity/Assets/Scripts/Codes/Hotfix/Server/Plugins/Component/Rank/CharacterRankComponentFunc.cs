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
            TSeasonCharacterRankData entity = null;
            if (self.SeasonRankData.TryGetValue(seasonConfigId, out var entityid))
            {
                entity = self.GetChild<TSeasonCharacterRankData>(entityid);
            }

            if (entity == null)
            {
                entity = self.AddChild<TSeasonCharacterRankData, int>(seasonConfigId);
                entity.SeasonConfigId = seasonConfigId;
                self.SeasonRankData.Add(seasonConfigId, entity.Id);
            }
        }

        public static void UpdateRankData(this CharacterRankComponent self, int ranktype, int score, bool issync = true)
        {
            var CurSeasonRank = self.Character.GetMyServerZone().RankComp.CurSeasonRank;
            var CharacterId = self.Character.Id;
            var SteamAccountId = self.Character.Name;
            switch (ranktype)
            {
                case (int)ERankType.SeasonBattleSorceRank:
                    CurSeasonRank.GetRank<TRankSeasonBattleSorce>(ranktype)?.UpdateRankData(CharacterId, SteamAccountId, score, true);
                    break;
                case (int)ERankType.HeroSumBattleSorceRank:
                    CurSeasonRank.GetRank<TRankHeroSumBattleScore>(ranktype)?.UpdateRankData(CharacterId, SteamAccountId, score, true);
                    break;

                case (int)ERankType.SeasonSingleCharpterRank:
                    CurSeasonRank.GetRank<TRankSeasonSingleCharpter>(ranktype)?.UpdateRankData(CharacterId, SteamAccountId, score, true);
                    break;

                case (int)ERankType.SeasonTeamCharpterRank:
                    CurSeasonRank.GetRank<TRankSeasonTeamCharpter>(ranktype)?.UpdateRankData(CharacterId, SteamAccountId, score, true);
                    break;
            }

            if (self.SeasonRankData.TryGetValue(self.SeasonConfigId, out var entityid))
            {
                var entity = self.GetChild<TSeasonCharacterRankData>(entityid);
                entity.UpdateCharacterRankData(ranktype, score);
                if (issync)
                {
                    self.Character.SyncHttpEntity(entity);
                }
            }
        }
    }
}