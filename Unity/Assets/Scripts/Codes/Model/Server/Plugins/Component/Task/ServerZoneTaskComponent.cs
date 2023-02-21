using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET
{
    public class ServerZoneTaskComponent : Entity, IAwake
    {

        [BsonIgnore]
        public TServerZone ServerZone { get => GetParent<TServerZone>(); }
    }
}
