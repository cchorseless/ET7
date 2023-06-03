using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace ET
{
    public class BagComponent: Entity, IAwake
    {
        public List<long> Items = new List<long>();

        public int MaxSize;

        /// <summary>
        /// 关卡难度
        /// </summary>
        public int DifficultyChapter = 1;
        /// <summary>
        /// 无尽难度等级
        /// </summary>
        public int DifficultyLevel = 0;

        [BsonIgnore]
        public TCharacter Character
        {
            get => this.GetParent<TCharacter>();
        }
    }
}