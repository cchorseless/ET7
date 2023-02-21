using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson.Serialization.Attributes;

namespace ET
{
    public class ServerZoneDataReportComponent : Entity, IAwake
    {
        public long CurDataReportItemId { get; set; }

        [BsonIgnore]
        public TServerZone ServerZone { get => GetParent<TServerZone>(); }
    }
}
