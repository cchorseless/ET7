using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;


namespace ET
{
    public class CharacterMailComponent : Entity, IAwake
    {
        public int MaxSize { get; set; }
        public long LastMailId;
        public List<long> Mails = new List<long>();

        [BsonIgnore]
        public TCharacter Character { get => this.GetParent<TCharacter>(); }
    }
}
