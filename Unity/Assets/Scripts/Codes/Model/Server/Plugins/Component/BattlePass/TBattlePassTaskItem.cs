using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson.Serialization.Options;
using MongoDB.Bson.Serialization.Attributes;

namespace ET
{
    public class TBattlePassTaskItem : Entity, IAwake<int>, ISerializeToEntity
    {
        public int ConfigId;

        public int Progress;

        public bool IsAchieve = false;

        public bool IsPrizeGet = false;

        [BsonIgnore]
        public CharacterBattlePassComponent BattlePassComp { get => this.GetParent<CharacterBattlePassComponent>(); }
    }
}
