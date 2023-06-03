using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ET.Server
{
    public enum ERankType
    {
        /// <summary>
        /// 赛季天梯积分排行榜
        /// </summary>
        SeasonBattleSorceRank = 1,
        HeroSumBattleSorceRank = 2,
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
                var entity = self.AddChild<TSeasonServerZoneRankData, int>(seasonConfigId);
                self.SeasonConfigId = seasonConfigId;
                self.SeasonRankData.Add(seasonConfigId, entity.Id);
                entity.LoadAllChild();
            }
            else
            {
                self.LoadCurSeasonRank().Coroutine();
            }
        }

        public static async ETTask LoadCurSeasonRank(this ServerZoneRankComponent self)
        {
            if (self.SeasonRankData.TryGetValue(self.SeasonConfigId, out var seasonRankDataId))
            {
                DBComponent db = DBManagerComponent.Instance.GetAccountDB();
                var CurSeasonRank = await db.Query<TSeasonServerZoneRankData>(seasonRankDataId);
                if (CurSeasonRank != null)
                {
                    self.AddChild(CurSeasonRank);
                    CurSeasonRank.LoadAllChild();
                }
            }
        }

        public static void UpdataRankData(this ServerZoneRankComponent self, int rankconfigid, long CharacterId, string SteamAccountId, int score)
        {
            switch (rankconfigid)
            {
                case (int)ERankType.SeasonBattleSorceRank:
                    self.CurSeasonRank.GetRank<TRankSeasonBattleSorce>(rankconfigid).UpdateRankData(CharacterId, SteamAccountId, score);
                    return;
                case (int)ERankType.HeroSumBattleSorceRank:
                    self.CurSeasonRank.GetRank<TRankHeroSumBattleScore>(rankconfigid).UpdateRankData(CharacterId, SteamAccountId, score);
                    return;

                case (int)ERankType.SeasonSingleCharpterRank:
                    self.CurSeasonRank.GetRank<TRankSeasonSingleCharpter>(rankconfigid).UpdateRankData(CharacterId, SteamAccountId, score);
                    return;

                case (int)ERankType.SeasonTeamCharpterRank:
                    self.CurSeasonRank.GetRank<TRankSeasonTeamCharpter>(rankconfigid).UpdateRankData(CharacterId, SteamAccountId, score);
                    return;
            }
        }

        public static TSeasonServerZoneRankData GetRankData(this ServerZoneRankComponent self, int seasonConfigId)
        {
            if (self.SeasonRankData.TryGetValue(seasonConfigId, out var entityid))
            {
                return self.GetChild<TSeasonServerZoneRankData>(entityid);
            }

            return null;
        }

        public static TRankSingleData GetCurSeasonCharacterRankDataInfo(this ServerZoneRankComponent self, int rankconfigid, long CharacterId)
        {
            TRankSingleData entity = null;
            switch (rankconfigid)
            {
                case (int)ERankType.SeasonBattleSorceRank:
                    entity = self.CurSeasonRank.GetRank<TRankSeasonBattleSorce>(rankconfigid).GetRankData<TRankSingleData>(CharacterId);
                    break;
                case (int)ERankType.HeroSumBattleSorceRank:
                    entity = self.CurSeasonRank.GetRank<TRankHeroSumBattleScore>(rankconfigid).GetRankData<TRankSingleData>(CharacterId);
                    break;
                case (int)ERankType.SeasonSingleCharpterRank:
                    entity = self.CurSeasonRank.GetRank<TRankSeasonSingleCharpter>(rankconfigid).GetRankData<TRankSingleData>(CharacterId);
                    break;
                case (int)ERankType.SeasonTeamCharpterRank:
                    entity = self.CurSeasonRank.GetRank<TRankSeasonTeamCharpter>(rankconfigid).GetRankData<TRankSingleData>(CharacterId);
                    break;
            }
            return entity;
          
        }

        public static (int, string) GetCurSeasonRankDataInfo(this ServerZoneRankComponent self, int rankconfigid, int page, int pageCount)
        {
            if (page < 1)
            {
                return (ErrorCode.ERR_Error, "page < 1 error ");
            }

            switch (rankconfigid)
            {
                case (int)ERankType.SeasonBattleSorceRank:
                    return self.CurSeasonRank.GetRank<TRankSeasonBattleSorce>(rankconfigid).GetRankPageData<TRankSingleData>(page, pageCount);
                case (int)ERankType.HeroSumBattleSorceRank:
                    return self.CurSeasonRank.GetRank<TRankHeroSumBattleScore>(rankconfigid).GetRankPageData<TRankSingleData>(page, pageCount);
                case (int)ERankType.SeasonSingleCharpterRank:
                    return self.CurSeasonRank.GetRank<TRankSeasonSingleCharpter>(rankconfigid).GetRankPageData<TRankSingleData>(page, pageCount);
                case (int)ERankType.SeasonTeamCharpterRank:
                    return self.CurSeasonRank.GetRank<TRankSeasonTeamCharpter>(rankconfigid).GetRankPageData<TRankSingleData>(page, pageCount);
            }

            return (ErrorCode.ERR_Error, "cant find rank data");
        }
    }
}