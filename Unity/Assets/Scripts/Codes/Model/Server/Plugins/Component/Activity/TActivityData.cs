using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET
{
    public abstract class TActivityData : Entity, IAwake, ISerializeToEntity
    {
        public int ConfigId;

        public long StartTime;

        public long EndTime;

        [BsonIgnore]
        public CharacterActivityComponent CharacterActivity { get => this.GetParent<CharacterActivityComponent>(); }
    }
}
