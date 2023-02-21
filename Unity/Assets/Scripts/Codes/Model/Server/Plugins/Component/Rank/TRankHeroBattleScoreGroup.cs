using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson.Serialization.Options;
using MongoDB.Bson.Serialization.Attributes;

namespace ET
{
    public class TRankHeroBattleScoreGroup : Entity, IAwake, ISerializeToEntity
    {
        public int ConfigId { get; set; }
        public int SeasonConfigId { get; set; }
        public string Name { get; set; }

        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
        public Dictionary<int, long> HeroBattllelScoreRank = new Dictionary<int, long>();
    }
}
