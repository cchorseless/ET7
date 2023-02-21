using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;

namespace ET
{

    public static class EShopType
    {
        public const int ServerZoneShopMax = 1000;
    }

    public class TShopUnit : Entity, IAwake, IEnable, ISerializeToEntity
    {
        public int ConfigId;

        public bool IsValid = true;

        public long CharacterId;

        /// <summary>
        /// 配置json,同步给客户端确保最新
        /// </summary>
        //public string ConfigJson;

        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
        public Dictionary<int, long> ShopSellItem = new Dictionary<int, long>();


    }
}
