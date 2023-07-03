using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson.Serialization.Attributes;

namespace ET
{
    public class TServerZoneDailyDataStatisticItem: Entity, IAwake, ISerializeToEntity
    {
        public long Time;
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
        public int ServerID { get; set; }

        public int[] HoursPlayerNew = new int[24];
        
        public int[] HoursPlayerOnline = new int[24];
        
        
        
        [BsonIgnore]
        public ServerZoneDataStatisticComponent DataStatisticComp
        {
            get => GetParent<ServerZoneDataStatisticComponent>();
        }
    }
}