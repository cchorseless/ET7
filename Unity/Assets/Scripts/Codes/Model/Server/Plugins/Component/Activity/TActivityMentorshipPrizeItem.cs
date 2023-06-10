using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson.Serialization.Options;
using MongoDB.Bson.Serialization.Attributes;

namespace ET
{
    public class TActivityMentorshipPrizeItem : Entity, IAwake<int>, ISerializeToEntity
    {
        public int ConfigId;

        [BsonIgnoreIfDefault]
        public int Progress;

        public bool IsAchieve = false;

        public bool IsPrizeGet = false;

        public List<long> PrizeGet = new List<long>();


        [BsonIgnore]
        public TActivityMentorshipTreeData MentorshipTreeData { get => this.GetParent<TActivityMentorshipTreeData>(); }
    }
}
