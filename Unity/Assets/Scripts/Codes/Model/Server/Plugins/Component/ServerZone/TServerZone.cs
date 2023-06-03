using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET
{
    // 要求确保 同一个不能出现在两个进程中，进程:ServerZone= 一对多 滚服可以
    // 单服 大服 ，需要 ServerZone:进程 = 一对多
    public class TServerZone: Entity, IAwake<int, int, string>
    {
        public string ServerName;
        public int ZoneID;
        public int ServerID;
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
        public ServerZoneTaskComponent TaskComp
        {
            get => GetComponent<ServerZoneTaskComponent>();
        }

        [BsonIgnore]
        public ServerZoneRankComponent RankComp
        {
            get => GetComponent<ServerZoneRankComponent>();
        }

        [BsonIgnore]
        public ServerZoneDrawTreasureComponent DrawTreasureComp
        {
            get => GetComponent<ServerZoneDrawTreasureComponent>();
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
        public ServerZoneTItemManageComponent TItemManageComp
        {
            get => GetComponent<ServerZoneTItemManageComponent>();
        }

        [BsonIgnore]
        public ServerZoneBuffComponent BuffComp
        {
            get => GetComponent<ServerZoneBuffComponent>();
        }

        [BsonIgnore]
        public ServerZoneDataReportComponent DataReportComp
        {
            get => GetComponent<ServerZoneDataReportComponent>();
        }

        [BsonIgnore]
        public ServerZoneGameRecordComponent GameRecordComp
        {
            get => GetComponent<ServerZoneGameRecordComponent>();
        }
    }
}