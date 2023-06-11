using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson.Serialization.Options;
using MongoDB.Bson.Serialization.Attributes;

namespace ET
{
    public class TActivityDailyOnlinePrizeData : TActivityData
    {
        public List<int> ItemHadGet = new List<int>();

        public long LoginTimeSpan;
        [BsonIgnore]
        public CharacterActivityComponent ActivityComp { get=>GetParent<CharacterActivityComponent>();}
    }
}
