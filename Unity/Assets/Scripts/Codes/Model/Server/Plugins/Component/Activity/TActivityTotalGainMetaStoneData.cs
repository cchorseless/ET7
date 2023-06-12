using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson.Serialization.Options;
using MongoDB.Bson.Serialization.Attributes;

namespace ET
{
    public class TActivityTotalGainMetaStoneData : TActivityData
    {
        public List<int> ItemHadGet = new List<int>();

        public int TotalChargeMoney;
        public int SeasonConfigId;
        [BsonIgnore]
        public CharacterActivityComponent ActivityComp { get=>GetParent<CharacterActivityComponent>();}
    }
}
