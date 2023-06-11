using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson.Serialization.Options;
using MongoDB.Bson.Serialization.Attributes;
namespace ET
{
    public class TActivityFirstChargeData : TActivityData
    {
        public int ChargeTime;
    }
}