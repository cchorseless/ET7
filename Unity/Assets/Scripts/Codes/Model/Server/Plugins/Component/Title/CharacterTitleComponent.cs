using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Options;
using MongoDB.Bson.Serialization.Attributes;


namespace ET
{
    public class CharacterTitleComponent : Entity, IAwake
    {

        public int DressTitleConfigId;

        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
        public Dictionary<int, long> Titles = new Dictionary<int, long>();

        [BsonIgnore]
        public TCharacter Character { get => this.GetParent<TCharacter>(); }
    }
}
