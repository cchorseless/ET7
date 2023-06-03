using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson.Serialization.Options;

namespace ET
{
    /// <summary>
    /// 排行榜数据包，单独保存数据
    /// </summary>
    public class TSeasonServerZoneRankData : Entity, IAwake<int>
    {
        public int SeasonConfigId { get; set; }


        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
        public Dictionary<int, long> Ranks = new Dictionary<int, long>();


        [BsonIgnore]
        public ServerZoneRankComponent ServerZoneRankComp { get => GetParent<ServerZoneRankComponent>(); }
    }
}
