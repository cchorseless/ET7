using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;

namespace ET
{
    public class TServerZoneDailyDataStatisticItem: Entity, IAwake, ISerializeToEntity
    {
        public long Time;
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
        public int ServerID { get; set; }

        public int TotalPlayerCount;
        
        public int[] HoursPlayerNew = new int[24];

        public int[] HoursPlayerOnline = new int[24];

        public int[] HoursBattleCount = new int[24];

        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
        public Dictionary<int, int> OrderIncome = new Dictionary<int, int>();

        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
        public Dictionary<int, int> ShopSellItem = new Dictionary<int, int>();

        [BsonIgnore]
        public ServerZoneDataStatisticComponent DataStatisticComp
        {
            get => GetParent<ServerZoneDataStatisticComponent>();
        }
    }
}