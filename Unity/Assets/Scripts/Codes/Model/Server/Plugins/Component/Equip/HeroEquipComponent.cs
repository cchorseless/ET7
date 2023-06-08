using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace ET
{
    public class HeroEquipComponent : Entity, IAwake ,ISerializeToEntity
    {
        //当前装备
        public Dictionary<int,long> Equips = new Dictionary<int,long>();
        [BsonIgnore]
        public THeroUnit HeroUnit { get => this.GetParent<THeroUnit>(); }
    }
}