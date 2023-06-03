using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET.Server
{
    [ObjectSystem]
    public class TSeasonCharacterRankDataAwakeSystem: AwakeSystem<TSeasonCharacterRankData, int>
    {
        protected override void Awake(TSeasonCharacterRankData self, int seasonConfigId)
        {
            self.SeasonConfigId = seasonConfigId;
        }
    }

    public static class TSeasonCharacterRankDataFunc
    {
        public static void LoadAllChild(this TSeasonCharacterRankData self)
        {
            var RankComp = self.CharacterRankComp.Character.GetMyServerZone().RankComp;

            if (self.SeasonConfigId == RankComp.SeasonConfigId)
            {
                foreach (var ranktype in RankComp.SeasonRankType)
                {
                    TRankSingleData entity = null;
                    if (self.RankDatas.TryGetValue(ranktype, out var entityid))
                    {
                        entity = self.GetChild<TRankSingleData>(entityid);
                    }
                    else
                    {
                        entity = self.AddChild<TRankSingleData>();
                        self.RankDatas.Add(ranktype, entity.Id);
                        entity.CharacterId = self.CharacterRankComp.Character.Id;
                        entity.SteamAccountId = self.CharacterRankComp.Character.Name;
                        entity.RankType = ranktype;
                    }

                    if (entity != null)
                    {
                        var rankdata = RankComp.GetCurSeasonCharacterRankDataInfo(ranktype, entity.CharacterId);
                        if (rankdata != null)
                        {
                            entity.Score = rankdata.Score;
                            entity.RankIndex = rankdata.RankIndex;
                        }
                    }
                   
                }
            }
        }
    }
}