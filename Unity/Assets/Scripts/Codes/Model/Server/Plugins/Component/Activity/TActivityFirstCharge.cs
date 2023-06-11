using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson.Serialization.Options;
using MongoDB.Bson.Serialization.Attributes;

namespace ET
{
    public class TActivityFirstCharge : TActivity
    {
        public List<FItemInfo> ChargeFirstPrize = new List<FItemInfo>();
        
        public List<FItemInfo> SecondFirstPrize = new List<FItemInfo>();
    }
}