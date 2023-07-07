using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson.Serialization.Options;

namespace ET
{
    public class ServerZoneGameRecordComponent : Entity, IAwake ,IDestroy
    {
        [BsonIgnore]
        public List<long> Records = new List<long>();

        [BsonIgnore]
        public TServerZone ServerZone { get => GetParent<TServerZone>(); }
    }
}
