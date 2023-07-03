using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET
{
    public class TServerZone: Entity, IAwake<int, int, string>
    {
        public string ServerName;
        public int ZoneID;
        public int ServerID;
        public int ProcessID;
        public long CreateTime;
        public HashSet<int> State;
        public HashSet<int> ServerLabel;
     
        [BsonIgnore]
        public TimerEntityComponent TimerEntityComp
        {
            get => GetComponent<TimerEntityComponent>();
        }

        [BsonIgnore]
        public ServerZoneTimePlanComponent TimerPlanComp
        {
            get => GetComponent<ServerZoneTimePlanComponent>();
        }

        [BsonIgnore]
        public ServerZoneSeasonComponent SeasonComp
        {
            get => GetComponent<ServerZoneSeasonComponent>();
        }

        [BsonIgnore]
        public ServerZoneShopComponent ShopComp
        {
            get => GetComponent<ServerZoneShopComponent>();
        }

        [BsonIgnore]
        public ServerZoneActivityComponent ActivityComp
        {
            get => GetComponent<ServerZoneActivityComponent>();
        }
        [BsonIgnore]
        public ServerZoneBattleTeamComponent BattleTeamComp
        {
            get => GetComponent<ServerZoneBattleTeamComponent>();
        }
        [BsonIgnore]
        public ServerZoneMailComponent MailComp
        {
            get => GetComponent<ServerZoneMailComponent>();
        }

        [BsonIgnore]
        public ServerZoneRankComponent RankComp
        {
            get => GetComponent<ServerZoneRankComponent>();
        }

        [BsonIgnore]
        public ServerZoneCharacterComponent CharacterComp
        {
            get => GetComponent<ServerZoneCharacterComponent>();
        }

        [BsonIgnore]
        public ServerZoneRechargeComponent RechargeComp
        {
            get => GetComponent<ServerZoneRechargeComponent>();
        }


        [BsonIgnore]
        public ServerZoneBuffComponent BuffComp
        {
            get => GetComponent<ServerZoneBuffComponent>();
        }

        [BsonIgnore]
        public ServerZoneDataStatisticComponent DataStatisticComp
        {
            get => GetComponent<ServerZoneDataStatisticComponent>();
        }

        [BsonIgnore]
        public ServerZoneGameRecordComponent GameRecordComp
        {
            get => GetComponent<ServerZoneGameRecordComponent>();
        }
    }
}