using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson.Serialization.Options;
using MongoDB.Bson.Serialization.Attributes;

namespace ET
{
    public class CharacterBattlePassComponent: Entity, IAwake
    {
        public List<long> DailyTasks = new List<long>();
        public List<long> WeekTasks = new List<long>();
        public List<long> SeasonTasks = new List<long>();

        public bool IsReplaceDailyTask;

        public int BattlePassLevel;

        public int BattlePassExp;

        public List<int> BattlePassPrizeGet = new List<int>();
        public List<int> BattlePassChargeGet = new List<int>();
        public int SeasonConfigId;

        public long SeasonEndTimeSpan;
        public long DailyEndTimeSpan;
        public long WeekEndTimeSpan;

        /// <summary>
        /// 赛季通行证开启
        /// </summary>
        public bool IsBattlePass;
        /// <summary>
        /// 兑换消耗的道具Id
        /// </summary>
        public int ChargeItemConfigId = 20001;

        [BsonIgnore]
        public TCharacter Character
        {
            get => this.GetParent<TCharacter>();
        }
    }
}