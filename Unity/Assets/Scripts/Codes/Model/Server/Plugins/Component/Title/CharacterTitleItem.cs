using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson.Serialization.Options;
using MongoDB.Bson.Serialization.Attributes;

namespace ET
{
    public class CharacterTitleItem : Entity, IAwake<int>, ISerializeToEntity
    {
        public int ConfigId;

        public bool IsValid = true;

        public long DisabledTime;

        public List<int> TitleBuff = new List<int>();

        [BsonIgnore]
        public CharacterTitleComponent CharacterTitleComp { get => this.GetParent<CharacterTitleComponent>(); }

    }
}
