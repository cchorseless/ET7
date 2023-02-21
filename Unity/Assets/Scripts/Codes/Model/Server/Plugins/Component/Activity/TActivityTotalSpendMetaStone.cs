using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson.Serialization.Options;
using MongoDB.Bson.Serialization.Attributes;

namespace ET
{
    public class TActivityTotalSpendMetaStone : TActivity
    {
        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
        public Dictionary<int, List<ValueTupleStruct<int, int>>> Items = new Dictionary<int, List<ValueTupleStruct<int, int>>>();
    }
}
