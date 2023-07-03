using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson.Serialization.Options;

namespace ET
{
    public class TGameRecordItem : Entity, IAwake
    {
        public List<long> Players = new List<long>();

        public int SuggestCount;
        
        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
        public Dictionary<string, string> RecordInfo = new Dictionary<string, string>();


        [BsonIgnore]
        public HashSet<long> LogOutPlayers = new HashSet<long>();

        [BsonIgnore]
        public ServerZoneGameRecordComponent GameRecordComp { get => this.GetParent<ServerZoneGameRecordComponent>(); }
    }
}
