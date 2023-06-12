using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson.Serialization.Options;
using MongoDB.Bson.Serialization.Attributes;

namespace ET
{
    public class TActivityDailyOnlinePrizeData: TActivityData, IDestroy
    {
        public List<int> ItemHadGet = new List<int>();

        public long LoginTimeSpan;
        public long TodayOnlineTime;

        [BsonIgnore]
        public CharacterActivityComponent ActivityComp
        {
            get => GetParent<CharacterActivityComponent>();
        }
    }
}