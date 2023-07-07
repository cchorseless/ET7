using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET.Server
{
    [ObjectSystem]
    public class TSeasonServerZoneRankDataAwakeSystem: AwakeSystem<TSeasonServerZoneRankData, int>
    {
        protected override void Awake(TSeasonServerZoneRankData self, int seasonConfigId)
        {
            self.SeasonConfigId = seasonConfigId;
        }
    }

    public static class TSeasonServerZoneRankDataFunc
    {
        public static void LoadAllChild(this TSeasonServerZoneRankData self)
        {
            self.UpdateRank((int)ERankType.SeasonBattleSorceRank);
            self.UpdateRank((int)ERankType.HeroSumBattleSorceRank);
            self.UpdateRank((int)ERankType.SeasonSingleCharpterRank);
            // self.UpdateRank((int)ERankType.SeasonTeamCharpterRank);
            self.MergeOtherProcessRankData().Coroutine();
        }

        public static async ETTask MergeOtherProcessRankData(this TSeasonServerZoneRankData self)
        {
            int delay = RandomGenerator.RandomNumber(5, 15);
            await TimerComponent.Instance.WaitAsync(delay * 60 * 1000);
            if (self.IsDisposed)
            {
                return;
            }

            var accountDB = DBManagerComponent.Instance.GetAccountDB();
            var RankDatas = await accountDB.Query<TSeasonServerZoneRankData>(v => v.SeasonConfigId == self.SeasonConfigId
                    && v.Id != self.Id);
            var SeasonBattleSorce = self.GetRank<TRankSeasonBattleSorce>((int)ERankType.SeasonBattleSorceRank);
            var HeroSumBattleScore = self.GetRank<TRankHeroSumBattleScore>((int)ERankType.HeroSumBattleSorceRank);
            var SeasonSingleCharpter = self.GetRank<TRankSeasonSingleCharpter>((int)ERankType.SeasonSingleCharpterRank);
            foreach (var rankdata in RankDatas)
            {
                var ranks = rankdata.GetUnActiveChilds<TRankCommon>();
                foreach (var _rank in ranks)
                {
                    if (_rank.ConfigId == (int)ERankType.SeasonBattleSorceRank && SeasonBattleSorce != null)
                    {
                        SeasonBattleSorce.MergeData(_rank);
                    }
                    else if (_rank.ConfigId == (int)ERankType.HeroSumBattleSorceRank && HeroSumBattleScore != null)
                    {
                        HeroSumBattleScore.MergeData(_rank);
                    }
                    else if (_rank.ConfigId == (int)ERankType.SeasonSingleCharpterRank && SeasonSingleCharpter != null)
                    {
                        SeasonSingleCharpter.MergeData(_rank);
                    }
                }
            }

            // 存一下
            await accountDB.Save(self);
        }

        public static T AddRank<T>(this TSeasonServerZoneRankData self, int rankconfigid) where T : Entity, new()
        {
            if (!self.Ranks.ContainsKey(rankconfigid))
            {
                switch (rankconfigid)
                {
                    case (int)ERankType.SeasonBattleSorceRank:
                        var SeasonBattleSorce = self.AddChild<TRankSeasonBattleSorce>();
                        self.Ranks.Add(rankconfigid, SeasonBattleSorce.Id);
                        SeasonBattleSorce.ConfigId = rankconfigid;
                        SeasonBattleSorce.SeasonConfigId = self.SeasonConfigId;
                        SeasonBattleSorce.LoadAllChild();
                        return SeasonBattleSorce as T;
                    case (int)ERankType.HeroSumBattleSorceRank:
                        var heroBattleScore = self.AddChild<TRankHeroSumBattleScore>();
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

        public static T GetRank<T>(this TSeasonServerZoneRankData self, int rankconfigid) where T : Entity, new()
        {
            if (self.Ranks.TryGetValue(rankconfigid, out var entityid))
            {
                switch (rankconfigid)
                {
                    case (int)ERankType.SeasonBattleSorceRank:
                        return self.GetChild<TRankSeasonBattleSorce>(entityid) as T;
                    case (int)ERankType.HeroSumBattleSorceRank:
                        return self.GetChild<TRankHeroSumBattleScore>(entityid) as T;
                    case (int)ERankType.SeasonSingleCharpterRank:
                        return self.GetChild<TRankSeasonSingleCharpter>(entityid) as T;
                    case (int)ERankType.SeasonTeamCharpterRank:
                        return self.GetChild<TRankSeasonTeamCharpter>(entityid) as T;
                }
            }

            return null;
        }

        public static void UpdateRank(this TSeasonServerZoneRankData self, int rankconfigid)
        {
            if (!self.ServerZoneRankComp.SeasonRankType.Contains(rankconfigid))
            {
                self.ServerZoneRankComp.SeasonRankType.Add(rankconfigid);
            }

            if (self.Ranks.ContainsKey(rankconfigid))
            {
                switch (rankconfigid)
                {
                    case (int)ERankType.SeasonBattleSorceRank:
                        self.GetRank<TRankSeasonBattleSorce>(rankconfigid)?.LoadAllChild();
                        break;
                    case (int)ERankType.HeroSumBattleSorceRank:
                        self.GetRank<TRankHeroSumBattleScore>(rankconfigid)?.LoadAllChild();
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
                    case (int)ERankType.SeasonBattleSorceRank:
                        self.AddRank<TRankSeasonBattleSorce>(rankconfigid);
                        break;
                    case (int)ERankType.HeroSumBattleSorceRank:
                        self.AddRank<TRankHeroSumBattleScore>(rankconfigid);
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