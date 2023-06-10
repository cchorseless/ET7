using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson.Serialization.Options;
using MongoDB.Bson.Serialization.Attributes;
namespace ET
{
    public class TActivityMonthLogin : TActivity
    {
        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
        public Dictionary<int, FItemInfo> Items = new Dictionary<int, FItemInfo>();
        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
        public Dictionary<int, FItemInfo> TotalLoginItems = new Dictionary<int, FItemInfo>();
    }
}
