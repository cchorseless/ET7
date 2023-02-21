using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Options;
using MongoDB.Bson.Serialization.Attributes;

namespace ET
{
    public class TActivityBattlePass : TActivity
    {
        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
        public Dictionary<int, ValueTupleStruct<int, int>> Items = new Dictionary<int, ValueTupleStruct<int, int>>();
        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
        public Dictionary<int, ValueTupleStruct<int, int>> VipItems = new Dictionary<int, ValueTupleStruct<int, int>>();
    }
}
