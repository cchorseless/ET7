using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson.Serialization.Options;
using MongoDB.Bson.Serialization.Attributes;

namespace ET
{
    public class TRankCommon : Entity, IAwake, ISerializeToEntity
    {
        public int ConfigId { get; set; }
        public int SeasonConfigId { get; set; }
        // 最大记录排行数据
        public int MaxRandDataCount = 1000;
 
        public string Name { get; set; }

        public List<long> RankData = new List<long>();

        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
        public Dictionary<long, long> CharacterRankData = new Dictionary<long, long>();
    }
}
