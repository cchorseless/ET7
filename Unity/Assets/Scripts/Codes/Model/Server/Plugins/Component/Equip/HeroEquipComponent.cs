using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace ET
{
    public enum EEquipSolt
    {
        
        Weapon = 0,
       
        Helm = 1,
        
        Necklace = 2,
       
        Medal = 3,
       
        Armor = 4,
       
        BraceletL = 5,
       
        BraceletR = 6,
       
        RingL = 7,
       
        RingR = 8,
       
        Belt = 9,
        
        Amulet = 10,
        
        Shoes = 11,
        
        SlotMax = 12
    }
    public class HeroEquipComponent : Entity, IAwake ,ISerializeToEntity
    {
        //当前装备
        public long[] Equips = new long[(int)EEquipSolt.SlotMax];
        [BsonIgnore]
        public THeroUnit HeroUnit { get => this.GetParent<THeroUnit>(); }
    }
}