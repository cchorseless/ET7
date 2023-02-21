using System;
using MongoDB.Bson.Serialization.Attributes;

namespace ET
{


    public class TItem : Entity, IAwake, ISerializeToEntity
    {
        /// <summary>
        /// 配置表id
        /// </summary>
        public int ConfigId;

        public long CreateTime;

        public int ItemCount;

        public int ItemQuality;

        public long CharacterId;

        /// <summary>
        /// 是否锁定
        /// </summary>
        public bool IsLock;

        public bool IsValid = true;


        [BsonIgnore]
        public BagComponent BagComp { get => GetParent<BagComponent>(); }

    }
}