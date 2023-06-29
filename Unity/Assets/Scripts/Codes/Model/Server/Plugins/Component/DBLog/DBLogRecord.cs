using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET.Server
{
    public class DBLogRecord : Entity, IAwake, IDestroy
    {
        [BsonIgnoreIfDefault]
        public string Process { get; set; }
        [BsonIgnoreIfDefault]
        public long Time { get; set; }
        [BsonIgnoreIfDefault]
        public int Level { get; set;
        }
        [BsonIgnoreIfDefault]
        public string Msg { get; set; }
        [BsonIgnoreIfDefault]
        public bool IsIgnore { get; set; }

        [BsonIgnoreIfDefault]
        public string Label { get; set; }

        [BsonIgnoreIfDefault]
        public int Count { get; set; }
    }
}
