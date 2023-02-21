using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson.Serialization.Options;
using MongoDB.Bson.Serialization.Attributes;

namespace ET
{
    public class HeroTalentComponent : Entity, IAwake, ISerializeToEntity
    {
        public int TalentPoint;
        public int TotalTalentPoint;

        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
        public Dictionary<int, long> Talents = new Dictionary<int, long>();

        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
        public Dictionary<int, int[]> TalentLearn = new Dictionary<int, int[]>();

        [BsonIgnore]
        public THeroUnit HeroUnit { get => this.GetParent<THeroUnit>(); }

    }
}
