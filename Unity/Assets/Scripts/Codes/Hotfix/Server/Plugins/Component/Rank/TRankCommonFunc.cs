using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET.Server
{
    public static class TRankCommonFunc
    {
        public static void LoadAllChild(this TRankCommon self)
        {
            self.RankSort();
        }

        public static void RankSort(this TRankCommon self)
        {
            self.RankData.Sort((a, b) =>
            {
                return self.GetChild<TRankSingleData>(a).Score - self.GetChild<TRankSingleData>(b).Score;
            });
            for (int i = 0; i < self.RankData.Count; i++)
            {
                self.GetChild<TRankSingleData>(self.RankData[i]).RankIndex = i;
            }
        }
        public static (int, string) GetCharacterRankPageData<T>(this TRankCommon self, long characterId) where T : Entity
        {
            var entity = self.GetRankData<T>(characterId);
            if (entity == null)
            {
                return (ErrorCode.ERR_Error, "cant find data");
            }
            else
            {
                return (ErrorCode.ERR_Success, MongoHelper.ToClientJson(entity));

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
            var r = JsonHelper.GetLitArray();
            for (int i = startIndex; i < endIndex; i++)
            {
                var entity = self.GetChild<T>(self.RankData[i]);
                if (entity != null)
                {
                    r.Add(MongoHelper.ToClientJson(entity));
                }
            }
            return (ErrorCode.ERR_Success, JsonHelper.ToLitJson(r));
        }
        public static T GetRankData<T>(this TRankCommon self, long characterid) where T : Entity
        {
            if (self.CharacterRankData.TryGetValue(characterid, out long rankdataId))
            {
                return self.GetChild<T>(rankdataId);
            }
            return null;
        }

        public static TRankSingleData UpdateRankData(this TRankCommon self, long characterid, int score)
        {
            TRankSingleData rankdata;
            if (self.CharacterRankData.TryGetValue(characterid, out long rankdataId))
            {
                rankdata = self.GetChild<TRankSingleData>(rankdataId);
                rankdata.Score = score;
            }
            else
            {
                rankdata = self.AddChild<TRankSingleData>();
                rankdata.CharacterId = characterid;
                rankdata.Score = score;
                rankdata.RankIndex = self.RankData.Count;
                self.CharacterRankData.Add(characterid, rankdata.Id);
                self.RankData.Add(rankdata.Id);
            }
            return rankdata;
        }
    }
}
