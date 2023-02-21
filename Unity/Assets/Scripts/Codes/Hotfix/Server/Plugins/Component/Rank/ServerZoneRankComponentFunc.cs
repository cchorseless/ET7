using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ET.Server
{
    public enum ERankType
    {
        SumBattleSorceRank = 1,
        HeroBattleSorceRankGroup = 2,
        SeasonSingleCharpterRank = 4,
        SeasonTeamCharpterRank = 8,
    }

    public static class ServerZoneRankComponentFunc
    {
        public static void LoadAllChild(this ServerZoneRankComponent self)
        {
            var seasonConfigId = self.ServerZone.SeasonComp.CurSeasonConfigId;
            if (self.SeasonConfigId == 0 || self.SeasonConfigId != seasonConfigId)
            {
                var entity = self.AddChild<TSeasonRankData, int>(seasonConfigId);
                self.SeasonConfigId = seasonConfigId;
                self.SeasonRankData.Add(seasonConfigId, entity.Id);
                entity.LoadAllChild();
            }
            else
            {
                self.CurSeasonRank.LoadAllChild();
            }
        }

        public static TSeasonRankData GetRankData(this ServerZoneRankComponent self, int seasonConfigId)
        {
            if (self.SeasonRankData.TryGetValue(seasonConfigId, out var entityid))
            {
                return self.GetChild<TSeasonRankData>(entityid);
            }
            return null;
        }

        public static (int, string) GetCurSeasonCharacterRankDataInfo(this ServerZoneRankComponent self, int rankconfigid, long CharacterId)
        {
            switch (rankconfigid)
            {
                case (int)ERankType.SumBattleSorceRank:
                    return self.CurSeasonRank.GetRank<TRankSumBattleSorce>(rankconfigid).GetCharacterRankPageData<TRankSingleData>(CharacterId);
                case (int)ERankType.SeasonSingleCharpterRank:
                    return self.CurSeasonRank.GetRank<TRankSeasonSingleCharpter>(rankconfigid).GetCharacterRankPageData<TRankSingleData>(CharacterId);
                case (int)ERankType.SeasonTeamCharpterRank:
                    return self.CurSeasonRank.GetRank<TRankSeasonTeamCharpter>(rankconfigid).GetCharacterRankPageData<TRankSingleData>(CharacterId);
            }
            return (ErrorCode.ERR_Error, "cant find rank data");
        }

        public static (int, string) GetCurSeasonCharacterRankDataInfo(this ServerZoneRankComponent self, int rankconfigid, int heroConfigid, long CharacterId)
        {
            switch (rankconfigid)
            {
                case (int)ERankType.HeroBattleSorceRankGroup:
                    var rankData = self.CurSeasonRank.GetRank<TRankHeroBattleScoreGroup>(rankconfigid);
                    var heroRank = rankData.GetHeroRank(heroConfigid);
                    if (heroRank != null)
                    {
                        return heroRank.GetCharacterRankPageData<TRankSingleData>(CharacterId);
                    }
                    break;
            }
            return (ErrorCode.ERR_Error, "cant find rank data");
        }




        public static (int, string) GetCurSeasonRankDataInfo(this ServerZoneRankComponent self, int rankconfigid, int page, int pageCount)
        {
            if (page < 1)
            {
                return (ErrorCode.ERR_Error, "page < 1 error ");
            }
            switch (rankconfigid)
            {
                case (int)ERankType.SumBattleSorceRank:
                    return self.CurSeasonRank.GetRank<TRankSumBattleSorce>(rankconfigid).GetRankPageData<TRankSingleData>(page, pageCount);
                case (int)ERankType.SeasonSingleCharpterRank:
                    return self.CurSeasonRank.GetRank<TRankSeasonSingleCharpter>(rankconfigid).GetRankPageData<TRankSingleData>(page, pageCount);
                case (int)ERankType.SeasonTeamCharpterRank:
                    return self.CurSeasonRank.GetRank<TRankSeasonTeamCharpter>(rankconfigid).GetRankPageData<TRankSingleData>(page, pageCount);
            }
            return (ErrorCode.ERR_Error, "cant find rank data");
        }

        public static (int, string) GetCurSeasonRankDataInfo(this ServerZoneRankComponent self, int rankconfigid, int heroConfigid, int page, int pageCount )
        {
            if (page < 1)
            {
                return (ErrorCode.ERR_Error, "page < 1 error ");
            }
            switch (rankconfigid)
            {
                case (int)ERankType.HeroBattleSorceRankGroup:
                    var rankData = self.CurSeasonRank.GetRank<TRankHeroBattleScoreGroup>(rankconfigid);
                    var heroRank = rankData.GetHeroRank(heroConfigid);
                    if (heroRank != null)
                    {
                        return heroRank.GetRankPageData<TRankSingleData>(page, pageCount);
                    }
                    break;
            }
            return (ErrorCode.ERR_Error, "cant find rank data");
        }
    }
}
