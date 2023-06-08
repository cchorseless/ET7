using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET
{
    public class THeroUnit: Entity, IAwake<string>, ISerializeToEntity
    {
        public string ConfigId;
        public int Level;
        public int Exp;
        public int TotalExp;
        public string SkinConfigId;
        public int BattleScore;

        // 解锁的皮肤  ID:时间
        public Dictionary<string, long> Skins = new Dictionary<string, long>();

        //当前装备
        public Dictionary<int, long> Equips = new Dictionary<int, long>();

        [BsonIgnore]
        public HeroManageComponent HeroManageComp
        {
            get => this.GetParent<HeroManageComponent>();
        }

        [BsonIgnore]
        public HeroEquipComponent HeroEquipComp
        {
            get => this.GetComponent<HeroEquipComponent>();
        }

        [BsonIgnore]
        public HeroTalentComponent HeroTalentComp
        {
            get => this.GetComponent<HeroTalentComponent>();
        }

        [BsonIgnore]
        public TCharacter Character
        {
            get => HeroManageComp.Character;
        }
    }
}