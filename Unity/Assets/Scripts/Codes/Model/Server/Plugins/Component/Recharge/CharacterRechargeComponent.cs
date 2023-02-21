using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ET
{
    public class CharacterRechargeComponent : Entity, IAwake
    {

        public int TotalCharge;

        [BsonIgnore]
        public TCharacter Character { get => this.GetParent<TCharacter>(); }
    }
}
