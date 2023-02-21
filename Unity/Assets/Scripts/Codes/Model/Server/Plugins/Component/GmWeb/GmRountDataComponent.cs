using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson.Serialization.Attributes;


namespace ET.Server
{
    public class GmRountDataComponent : Entity, IAwake, ISerializeToEntity
    {
        public string Name;

        public string Avatar;

        public string Introduction;

        public string Email;

        public string Phone;

        public List<string> Roles;

        [BsonIgnore]
        public TCharacter Character { get => this.GetParent<TCharacter>(); }
    }
}
