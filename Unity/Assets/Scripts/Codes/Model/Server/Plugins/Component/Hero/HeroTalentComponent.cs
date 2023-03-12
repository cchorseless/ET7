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

        public List<int> TalentLearn = new List<int>();

        [BsonIgnore]
        public THeroUnit HeroUnit { get => this.GetParent<THeroUnit>(); }

    }
}
