using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson.Serialization.Attributes;


namespace ET
{
    public class TServerZoneTimePlan : Entity, IAwake
    {
        public int TimePlanType { get; set; }

        public int TimePlanLabel { get; set; }

        public int DayIndex { get; set; }
        public int Hour { get; set; }
        public int Min { get; set; }
        public int Sec { get; set; }

        [BsonIgnore]
        public ServerZoneTimePlanComponent TimePlanComp { get => GetParent<ServerZoneTimePlanComponent>(); }
    }
}
