using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson.Serialization.Options;

namespace ET
{
    public class TSeasonCharacterRankData: Entity, IAwake<int>, ISerializeToEntity
    {
        public int SeasonConfigId { get; set; }

        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
        public Dictionary<int, long> RankDatas = new Dictionary<int, long>();

        [BsonIgnore]
        public CharacterRankComponent CharacterRankComp
        {
            get => GetParent<CharacterRankComponent>();
        }
    }
}