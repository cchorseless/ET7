using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson.Serialization.Attributes;


namespace ET
{
    public class TBuffItem : Entity, IAwake, ISerializeToEntity
    {

        public int ConfigId;

        public int BuffLayerCount;

        public long DisabledTime;

        public bool IsValid;

        [BsonIgnore]
        public CharacterBuffComponent BuffComp { get => GetParent<CharacterBuffComponent>(); }
    }
}
