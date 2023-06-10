using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson.Serialization.Options;
using MongoDB.Bson.Serialization.Attributes;
namespace ET
{
    public class TActivityMonthLoginData : TActivityData
    {
        public List<int> ItemHadGet = new List<int>();
        public List<int> TotalLoginItemHadGet = new List<int>();
        
        public int LoginDayCount;
        
        public int MonthIndex;
    }
}
