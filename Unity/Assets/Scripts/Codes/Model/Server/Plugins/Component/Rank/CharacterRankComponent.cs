using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson.Serialization.Options;

namespace ET
{
    public class CharacterRankComponent: Entity, IAwake
    {
        
        public int SeasonConfigId { get; set; }
        
        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
        public Dictionary<int, long> SeasonRankData = new Dictionary<int, long>();
        
        
        [BsonIgnore]
        public TCharacter Character { get => this.GetParent<TCharacter>(); }
    }
}