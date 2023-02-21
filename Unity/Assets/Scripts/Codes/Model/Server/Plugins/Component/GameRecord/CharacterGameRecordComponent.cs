using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson.Serialization.Options;


namespace ET
{
    public class CharacterGameRecordComponent : Entity, IAwake, IDestroy
    {
        public List<long> Records = new List<long>();

        public long CurRecordID;

        [BsonIgnore]
        public TCharacter Character { get => this.GetParent<TCharacter>(); }
    }
}
