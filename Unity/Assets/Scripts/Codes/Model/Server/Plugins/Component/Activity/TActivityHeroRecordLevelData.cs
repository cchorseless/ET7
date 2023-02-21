using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Options;
using MongoDB.Bson.Serialization.Attributes;

namespace ET
{
    public class TActivityHeroRecordLevelData : TActivityData
    {

        public List<int> ItemGetRecord = new List<int>();
        public int HeroSumLevel;
    }
}
