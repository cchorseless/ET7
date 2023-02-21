using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;


namespace ET
{
    public class HttpPlayerSessionComponent : Entity, IAwake, IDestroy
    {
        public long LastRecvTime
        {
            get;
            set;
        }
        [BsonIgnore]
        public List<string> SyncString = new List<string>();
        [BsonIgnore]
        public G2C_Ping Response;
        [BsonIgnore]
        public ETCancellationToken CancelTimer;
    }
}
