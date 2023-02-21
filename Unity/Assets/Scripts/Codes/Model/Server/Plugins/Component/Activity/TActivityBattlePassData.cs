using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson.Serialization.Options;
using MongoDB.Bson.Serialization.Attributes;

namespace ET
{
    public class TActivityBattlePassData : TActivityData
    {
        public bool IsVip = false;
        public int Level;

        public List<int> ItemGetRecord = new List<int>();
        public List<int> VipItemGetRecord = new List<int>();
    }
}
