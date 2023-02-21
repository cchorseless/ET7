using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson.Serialization.Options;
using MongoDB.Bson.Serialization.Attributes;

namespace ET
{
    public class THeroTalentItem : Entity, IAwake<int>, ISerializeToEntity
    {
        public int ConfigId;

        public int CostTalentPoint;

        public List<int> TalentBuff = new List<int>();

        [BsonIgnore]
        public HeroTalentComponent HeroTalentComp { get => this.GetParent<HeroTalentComponent>(); }

    }
}
