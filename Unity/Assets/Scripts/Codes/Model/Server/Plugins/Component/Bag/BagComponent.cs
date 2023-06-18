using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace ET
{
    public class BagComponent: Entity, IAwake
    {
        public List<long> Items = new List<long>();

        public int MaxSize;

        [BsonIgnore]
        public TCharacter Character
        {
            get => this.GetParent<TCharacter>();
        }
    }
}