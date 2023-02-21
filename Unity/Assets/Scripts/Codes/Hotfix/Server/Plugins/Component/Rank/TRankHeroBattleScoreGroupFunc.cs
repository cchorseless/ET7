using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ET.Server
{
    public static class TRankHeroBattleScoreGroupFunc
    {
        public static void LoadAllChild(this TRankHeroBattleScoreGroup self)
        {
            self.RankSort();
        }

        public static void RankSort(this TRankHeroBattleScoreGroup self)
        {
            foreach (var heroInfo in self.HeroBattllelScoreRank)
            {
                var herorank = self.GetHeroRank(heroInfo.Key);
                if (herorank != null)
                {
                    herorank.RankSort();
                }
            }
        }

        public static TRankHeroBattleScore GetHeroRank(this TRankHeroBattleScoreGroup self, int heroid)
        {
            if (self.HeroBattllelScoreRank.TryGetValue(heroid, out long herorankId))
            {
                return self.GetChild<TRankHeroBattleScore>(herorankId);
            }
            return null;
        }

        public static TRankSingleData UpdateHeroRankData(this TRankHeroBattleScoreGroup self, int heroid, long characterid, int score)
        {
            TRankHeroBattleScore rank = self.GetHeroRank(heroid);
            if (rank == null)
            {
                rank = self.AddChild<TRankHeroBattleScore>();
                rank.SeasonConfigId = self.SeasonConfigId;
                rank.HeroConfigId = heroid;
            }
            return rank.UpdateRankData(characterid, score);
        }





    }
}
