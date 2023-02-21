using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson.Serialization.Attributes;

namespace ET
{
    public class PlayerLoginOutComponent : Entity, IAwake
    {
        [BsonIgnore]
        public ETCancellationToken CancelTimer { get; set; }

        [BsonIgnore]
        public ETTask LogOutHandler { get; set; }
        [BsonIgnore]
        public bool IsHadKnockOut { get; set; } = false;
        [BsonIgnore]
        public int LogOutType { get; set; } = 0;
        [BsonIgnore]
        public long ReConnectKey { get; set; } = 0;

    }
}
