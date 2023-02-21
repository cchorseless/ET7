using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson.Serialization.Options;
using MongoDB.Bson.Serialization.Attributes;

namespace ET
{
    public class TActivityTotalSpendMetaStoneData : TActivityData
    {
        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
        public Dictionary<int, int> ItemState = new Dictionary<int, int>();

        public int TotalSpendMetaStone;

        [BsonIgnore]
        public CharacterActivityComponent ActivityComp { get=>GetParent<CharacterActivityComponent>();}
    }
}
