using MongoDB.Bson.Serialization.Attributes;

namespace ET.Server
{
    [ChildOf(typeof(PlayerComponent))]
    public sealed class Player : Entity, IAwake<string>, IDestroy
    {
        public string Account { get; set; }
		
        public long UnitId { get; set; }

        [BsonIgnore]
        public long CharacterId { get; set; }
        /// <summary>
        /// 客户端unit
        /// </summary>
        [BsonIgnore]
        public long GateSessionActorId { get; set; }

        [BsonIgnore]
        public bool IsOnline { get; set; }

        [BsonIgnore]
        public int GmLevel { get; set; }
    }
}