using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson.Serialization.Options;

namespace ET
{
    public class ServerZoneRankComponent : Entity, IAwake
    {
        public int SeasonConfigId { get; set; }


        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
        public Dictionary<int, long> SeasonRankData = new Dictionary<int, long>();

        [BsonIgnore]
        public TSeasonRankData CurSeasonRank
        {
            get
            {
                if (SeasonRankData.TryGetValue(SeasonConfigId, out var seasonRankDataId))
                {
                    return GetChild<TSeasonRankData>(seasonRankDataId);
                }
                return null;
            }
        }

        [BsonIgnore]
        public TServerZone ServerZone { get => GetParent<TServerZone>(); }
    }
}
