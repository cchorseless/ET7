using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson.Serialization.Attributes;


namespace ET
{
    public  class TActivity : Entity, IAwake, ISerializeToEntity
    {
        public int ActivityId;

        public int ConfigId;

        public bool ValidState;

        public long StartTime;

        public long EndTime;

        [BsonIgnore]
        public ServerZoneActivityComponent ServerZoneActivity { get => this.GetParent<ServerZoneActivityComponent>(); }
    }
}
