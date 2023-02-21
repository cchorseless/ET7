using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;

namespace ET
{
    public class ServerZoneShopComponent : Entity, IAwake
    {
        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
        public Dictionary<int, long> ShopUnit = new Dictionary<int, long>();


        [BsonIgnore]
        public TServerZone ServerZone { get => GetParent<TServerZone>(); }
    }
}
