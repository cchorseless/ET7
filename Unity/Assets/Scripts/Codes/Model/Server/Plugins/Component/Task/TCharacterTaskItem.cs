using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson.Serialization.Options;
using MongoDB.Bson.Serialization.Attributes;

namespace ET
{
    public class TCharacterTaskItem : Entity, IAwake<int>, ISerializeToEntity
    {
        public int ConfigId;

        [BsonIgnoreIfDefault]
        public int Progress;

        public bool IsAchieve = false;

        public bool IsPrizeGet = false;

        [BsonIgnore]
        public CharacterTaskComponent CharacterTaskComp { get => this.GetParent<CharacterTaskComponent>(); }
    }
}
