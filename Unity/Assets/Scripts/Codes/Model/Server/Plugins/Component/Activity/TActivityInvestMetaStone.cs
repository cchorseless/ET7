using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson.Serialization.Options;
using MongoDB.Bson.Serialization.Attributes;

namespace ET
{
    public class TActivityInvestMetaStone : TActivity
    {
        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
        public Dictionary<int, ValueTupleStruct<int, int>> Items = new Dictionary<int,ValueTupleStruct<int, int>>();
    }
}
