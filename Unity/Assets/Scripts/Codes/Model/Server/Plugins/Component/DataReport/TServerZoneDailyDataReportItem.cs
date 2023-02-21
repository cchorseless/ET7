using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson.Serialization.Attributes;


namespace ET
{
    public class TServerZoneDailyDataReportItem : Entity, IAwake, ISerializeToEntity
    {
        public long Time { get; set; }

        public int ServerID { get; set; }


        [BsonIgnore]
        public ServerZoneDataReportComponent DataReportComp { get => GetParent<ServerZoneDataReportComponent>(); }
    }
}
