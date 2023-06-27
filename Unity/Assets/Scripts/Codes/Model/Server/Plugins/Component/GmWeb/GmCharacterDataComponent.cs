using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson.Serialization.Attributes;

namespace ET.Server
{
    public class GmCharacterDataComponent : Entity, IAwake<long>
    {
        public long Int64PlayerId;
        
        public string Name;

        public string Avatar;

        public string Description;

        public string Email;

        public string Phone;

        public List<int> Roles;

        [BsonIgnore]
        public Player Player { get => this.GetParent<Player>(); }
    }
}
