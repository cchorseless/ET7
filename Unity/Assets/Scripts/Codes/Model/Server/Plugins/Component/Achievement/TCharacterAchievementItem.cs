using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson.Serialization.Attributes;


namespace ET
{
    public class TCharacterAchievementItem : Entity, IAwake<int>, ISerializeToEntity
    {
        public int ConfigId;

        public bool IsAchieve = false;

        public bool IsPrizeGet = false;

        [BsonIgnore]
        public CharacterAchievementComponent AchievementComp { get => this.GetParent<CharacterAchievementComponent>(); }

    }
}
