using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET.Server
{
    [ObjectSystem]
    public class TSeasonRankDataAwakeSystem : AwakeSystem<TSeasonRankData, int>
    {
        protected override void Awake(TSeasonRankData self, int seasonConfigId)
        {
            self.SeasonConfigId = seasonConfigId;
        }
    }
    public static class TSeasonRankDataFunc
    {
        public static void LoadAllChild(this TSeasonRankData self)
        {
            self.UpdateRank((int)ERankType.SumBattleSorceRank);
            self.UpdateRank((int)ERankType.HeroBattleSorceRankGroup);
            self.UpdateRank((int)ERankType.SeasonSingleCharpterRank);
            self.UpdateRank((int)ERankType.SeasonTeamCharpterRank);
        }

        public static T AddRank<T>(this TSeasonRankData self, int rankconfigid) where T : Entity, new()
        {
            if (!self.Ranks.ContainsKey(rankconfigid))
            {
                switch (rankconfigid)
                {
                    case (int)ERankType.SumBattleSorceRank:
                        var sumBattleSorce = self.AddChild<TRankSumBattleSorce>();
                        self.Ranks.Add(rankconfigid, sumBattleSorce.Id);
                        sumBattleSorce.ConfigId = rankconfigid;
                        sumBattleSorce.SeasonConfigId = self.SeasonConfigId;
                        sumBattleSorce.LoadAllChild();
                        return sumBattleSorce as T;
                    case (int)ERankType.HeroBattleSorceRankGroup:
                        var heroBattleScore = self.AddChild<TRankHeroBattleScoreGroup>();
                        self.Ranks.Add(rankconfigid, heroBattleScore.Id);
                        heroBattleScore.ConfigId = rankconfigid;
                        heroBattleScore.SeasonConfigId = self.SeasonConfigId;
                        heroBattleScore.LoadAllChild();
                        return heroBattleScore as T;
                    case (int)ERankType.SeasonSingleCharpterRank:
                        var seasonCharpter = self.AddChild<TRankSeasonSingleCharpter>();
                        self.Ranks.Add(rankconfigid, seasonCharpter.Id);
                        seasonCharpter.ConfigId = rankconfigid;
                        seasonCharpter.SeasonConfigId = self.SeasonConfigId;
                        seasonCharpter.LoadAllChild();
                        return seasonCharpter as T;
                    case (int)ERankType.SeasonTeamCharpterRank:
                        var seasonTeamCharpter = self.AddChild<TRankSeasonTeamCharpter>();
                        self.Ranks.Add(rankconfigid, seasonTeamCharpter.Id);
                        seasonTeamCharpter.ConfigId = rankconfigid;
                        seasonTeamCharpter.SeasonConfigId = self.SeasonConfigId;
                        seasonTeamCharpter.LoadAllChild();
                        return seasonTeamCharpter as T;
                }
            }
            return null;
        }
        public static T GetRank<T>(this TSeasonRankData self, int rankconfigid) where T : Entity, new()
        {
            if (self.Ranks.TryGetValue(rankconfigid, out var entityid))
            {
                switch (rankconfigid)
                {
                    case (int)ERankType.SumBattleSorceRank:
                        return self.GetChild<TRankSumBattleSorce>(entityid) as T;
                    case (int)ERankType.HeroBattleSorceRankGroup:
                        return self.GetChild<TRankHeroBattleScoreGroup>(entityid) as T;
                    case (int)ERankType.SeasonSingleCharpterRank:
                        return self.GetChild<TRankSeasonSingleCharpter>(entityid) as T;
                    case (int)ERankType.SeasonTeamCharpterRank:
                        return self.GetChild<TRankSeasonTeamCharpter>(entityid) as T;
                }
            }
            return null;
        }
        public static void UpdateRank(this TSeasonRankData self, int rankconfigid)
        {
            if (self.Ranks.ContainsKey(rankconfigid))
            {
                switch (rankconfigid)
                {
                    case (int)ERankType.SumBattleSorceRank:
                        self.GetRank<TRankSumBattleSorce>(rankconfigid)?.LoadAllChild();
                        break;
                    case (int)ERankType.HeroBattleSorceRankGroup:
                        self.GetRank<TRankHeroBattleScoreGroup>(rankconfigid)?.LoadAllChild();
                        break;
                    case (int)ERankType.SeasonSingleCharpterRank:
                        self.GetRank<TRankSeasonSingleCharpter>(rankconfigid)?.LoadAllChild();
                        break;
                    case (int)ERankType.SeasonTeamCharpterRank:
                        self.GetRank<TRankSeasonTeamCharpter>(rankconfigid)?.LoadAllChild();
                        break;
                }
            }
            else
            {
                switch (rankconfigid)
                {
                    case (int)ERankType.SumBattleSorceRank:
                        self.AddRank<TRankSumBattleSorce>(rankconfigid);
                        break;
                    case (int)ERankType.HeroBattleSorceRankGroup:
                        self.AddRank<TRankHeroBattleScoreGroup>(rankconfigid);
                        break;
                    case (int)ERankType.SeasonSingleCharpterRank:
                        self.AddRank<TRankSeasonSingleCharpter>(rankconfigid);
                        break;
                    case (int)ERankType.SeasonTeamCharpterRank:
                        self.AddRank<TRankSeasonTeamCharpter>(rankconfigid);
                        break;
                }
            }
        }
    }
}
