using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET.Server
{
    public static class CharacterBattleTeamComponentFunc
    {
        public static void LoadAllChild(this CharacterBattleTeamComponent self)
        {
            self.Character.RankComp.UpdateRankData((int)ERankType.SeasonBattleSorceRank, self.BattleScore, false);
        }

        public static async ETTask UploadBattleTeamRecord(this CharacterBattleTeamComponent self, FBattleTeamRecord recordinfo)
        {
            TBattleTeamRecord record = null;
            DBComponent db = DBManagerComponent.Instance.GetZoneDB(self.DomainZone());
            var BattleTeamComp = self.Character.GetMyServerZone().BattleTeamComp;
            // 先注释掉
            int RoundKey = recordinfo.RoundIndex + recordinfo.RoundCharpter * 1000;
            if (BattleTeamComp.GetBattleTeamCount() > 4000 && self.BattleTeams.TryGetValue(RoundKey, out var lastrecordid))
            {
                record = BattleTeamComp.GetChild<TBattleTeamRecord>(lastrecordid);
                if (record == null)
                {
                    record = await db.Query<TBattleTeamRecord>(lastrecordid);
                }

                if (record != null && record.Score >= recordinfo.Score)
                {
                    return;
                }
            }

            if (record == null)
            {
                record = BattleTeamComp.AddChild<TBattleTeamRecord>();
                record.RoundIndex = recordinfo.RoundIndex;
                record.RoundCharpter = recordinfo.RoundCharpter;
                record.BattleWinCount = 0;
                record.BattleLoseCount = 0;
                record.BattleDrawCount = 0;
                self.BattleTeams[record.GetRoundKey()] = record.Id;
            }

            record.SteamAccountId = recordinfo.SteamAccountId;
            record.SteamAccountName = recordinfo.SteamAccountName;
            record.Score = recordinfo.Score;
            record.SectInfo = recordinfo.SectInfo;
            record.UnitInfo = recordinfo.UnitInfo;
            record.IsRecord = true;
            await db.Save(record);
            BattleTeamComp.RegBattleTeamRecord(record);
        }

        public static (int, string) SearchBattleTeamRecord(this CharacterBattleTeamComponent self,
        int roundIndex, int score, int count)
        {
            if (roundIndex <= 0 || score <= 0 || count <= 0)
            {
                return (ErrorCode.ERR_Error, "参数错误");
            }

            var sceneZone = self.Character.GetMyServerZone();

            var r = sceneZone.BattleTeamComp.SearchBattleTeamRecord(roundIndex, score, count);
            if (r.Count == 0)
            {
                return (ErrorCode.ERR_Error, "search null");
            }

            self.SearchBattleTeams[roundIndex] = new List<long>();
            foreach (var entity in r)
            {
                self.SearchBattleTeams[roundIndex].Add(entity.Id);
            }

            return (ErrorCode.ERR_Success, MongoHelper.ToArrayClientJson(r.ToArray()));
        }

        public static (int, string) UploadBattleResult(this CharacterBattleTeamComponent self,
        int roundIndex, int score, long entityid)
        {
            if (roundIndex <= 0 || !self.SearchBattleTeams.TryGetValue(roundIndex, out var enemylist))
            {
                return (ErrorCode.ERR_Error, "参数错误");
            }

            if (!enemylist.Contains(entityid))
            {
                return (ErrorCode.ERR_Error, "参数错误");
            }

            var BattleTeamComp = self.Character.GetMyServerZone().BattleTeamComp;
            var record = BattleTeamComp.GetChild<TBattleTeamRecord>(entityid);
            if (record == null)
            {
                return (ErrorCode.ERR_Error, "找不到对手数据");
            }

            if (score > 0)
            {
                record.BattleLoseCount += 1;
            }
            else if (score < 0)
            {
                record.BattleWinCount += 1;
            }
            else
            {
                record.BattleDrawCount += 1;
            }

            self.BattleScore += score;
            self.Character.RankComp.UpdateRankData((int)ERankType.SeasonBattleSorceRank, self.BattleScore);
            self.Character.SyncHttpEntity(self);
            return (ErrorCode.ERR_Success, "" + self.BattleScore);
        }
    }
}