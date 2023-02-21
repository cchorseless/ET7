using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson.Serialization.Options;
using MongoDB.Bson.Serialization.Attributes;

namespace ET
{
    public class TActivityTotalOnlineTimeData : TActivityData, IDestroy
    {
        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
        public Dictionary<int, int> ItemState = new Dictionary<int, int>();

        public long LastLoginTotalOnlineTime;

        [BsonIgnore]
        public CharacterActivityComponent ActivityComp { get => GetParent<CharacterActivityComponent>(); }
    }
}
