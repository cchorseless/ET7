using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson.Serialization.Attributes;

namespace ET.Server
{
    public class GmCharacterDataComponent : Entity, IAwake, ISerializeToEntity
    {
        public string Name;

        public string Avatar;

        public string Description;

        public string Email;

        public string Phone;

        public List<string> Routes;

        [BsonIgnore]
        public TCharacter Character { get => this.GetParent<TCharacter>(); }
    }
}
