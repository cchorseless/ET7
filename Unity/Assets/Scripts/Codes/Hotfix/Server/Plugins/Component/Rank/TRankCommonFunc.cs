using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET.Server
{
    public static class TRankCommonFunc
    {
        public static async ETTask AutoRankSort(this TRankCommon self, int intervalSecond)
        {
            self.RankSort();
            await TimerComponent.Instance.WaitAsync(intervalSecond * 1000);
            if (self != null && self.IsDisposed == false)
            {
                self.AutoRankSort(intervalSecond * 1000).Coroutine();
            }
        }

        public static void MergeData(this TRankCommon self, TRankCommon other)
        {
            if (other == null)
            {
                return;
            }

            var childs = other.GetUnActiveChilds<TRankSingleData>();
            foreach (var entity in childs)
            {
                if (entity.IsRobot)
                {
                    continue;
                }

                var oldentity = self.GetRankData<TRankSingleData>(entity.CharacterId);
                if (oldentity != null && (oldentity.Score == entity.Score || oldentity.IsMasterData))
                {
                    continue;
                }

                self.UpdateRankData(entity.CharacterId, entity.SteamAccountId, entity.Score);
            }
        }

        public static void LoadFakerData(this TRankCommon self, int minscore, int maxscore)
        {
            int fakerCount = 100 - self.RankData.Count;
            if (fakerCount > 0)
            {
                for (var i = 0; i < fakerCount; i++)
                {
                    int accountid = RandomGenerator.RandomNumber(100000000, 999999999);
                    int score = RandomGenerator.RandomNumber(minscore, maxscore);
                    var entity = self.UpdateRankData(RandomGenerator.RandInt64(), accountid + "", score);
                    if (entity != null)
                    {
                        entity.IsRobot = true;
                    }
                }
            }
        }

        public static void RankSort(this TRankCommon self)
        {
            self.RankData.Sort((a, b) => { return self.GetChild<TRankSingleData>(b).Score - self.GetChild<TRankSingleData>(a).Score; });
            var count = self.RankData.Count;
            for (int i = 0; i < count; i++)
            {
                // 删除多余数据
                if (i >= self.MaxRandDataCount)
                {
                    var entity = self.GetChild<TRankSingleData>(self.RankData[i]);
                    if (entity != null)
                    {
                        self.CharacterRankData.Remove(entity.CharacterId);
                        entity.Dispose();
                    }
                }
                else
                {
                    self.GetChild<TRankSingleData>(self.RankData[i]).RankIndex = i + 1;
                }
            }

            if (count > self.MaxRandDataCount)
            {
                self.RankData = self.RankData.GetRange(0, self.MaxRandDataCount);
            }
        }

        public static (int, string) GetRankPageData<T>(this TRankCommon self, int page, int pageCount = 10) where T : Entity
        {
            int startIndex = (page - 1) * pageCount;
            int endIndex = (page) * pageCount;
            if (self.RankData.Count < startIndex + 1)
            {
                return (ErrorCode.ERR_Error, "out of index");
            }

            endIndex = Math.Min(endIndex, self.RankData.Count);
            var r = new List<T>();
            for (int i = startIndex; i < endIndex; i++)
            {
                var entity = self.GetChild<T>(self.RankData[i]);
                if (entity != null)
                {
                    r.Add(entity);
                }
            }

            return (ErrorCode.ERR_Success, MongoHelper.ToArrayClientJson(r.ToArray()));
        }

        public static T GetRankData<T>(this TRankCommon self, long characterid) where T : Entity
        {
            if (self.CharacterRankData.TryGetValue(characterid, out long rankdataId))
            {
                return self.GetChild<T>(rankdataId);
            }

            return null;
        }

        public static TRankSingleData UpdateRankData(this TRankCommon self, long characterid, string SteamAccountId, int score,
        bool ismasterdata = false)
        {
            TRankSingleData rankdata;
            if (self.CharacterRankData.TryGetValue(characterid, out long rankdataId))
            {
                rankdata = self.GetChild<TRankSingleData>(rankdataId);
                rankdata.Score = score;
            }
            else
            {
                // 清理机器人
                if (self.CharacterRankData.Count >= self.MaxRandDataCount)
                {
                    foreach (var kv in self.CharacterRankData)
                    {
                        rankdata = self.GetChild<TRankSingleData>(kv.Value);
                        // 有机器人
                        if (rankdata.IsRobot)
                        {
                            rankdata.CharacterId = characterid;
                            rankdata.SteamAccountId = SteamAccountId;
                            rankdata.RankType = self.ConfigId;
                            rankdata.Score = score;
                            rankdata.RankIndex = self.RankData.Count;
                            rankdata.IsRobot = false;
                            rankdata.IsMasterData = ismasterdata;
                            self.CharacterRankData.Add(characterid, rankdata.Id);
                            self.CharacterRankData.Remove(kv.Key);
                            return rankdata;
                        }
                    }
                }

                // 没有机器人 也可以添加，排序会删掉
                rankdata = self.AddChild<TRankSingleData>();
                rankdata.CharacterId = characterid;
                rankdata.SteamAccountId = SteamAccountId;
                rankdata.RankType = self.ConfigId;
                rankdata.Score = score;
                rankdata.RankIndex = self.RankData.Count;
                rankdata.IsMasterData = ismasterdata;
                self.CharacterRankData.Add(characterid, rankdata.Id);
                self.RankData.Add(rankdata.Id);
            }

            return rankdata;
        }
    }
}