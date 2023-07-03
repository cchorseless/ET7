using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;
using System.Linq;

namespace ET
{
    public class TCharacter: Entity, IAwake<long>, IDestroy, ITransfer
    {
        public long Int64PlayerId { get; set; }
        public long CreateTime { get; set; }
        public long LastLoginTime { get; set; }

        public bool IsFirstLoginToday { get; set; }
        public bool IsFirstLoginWeek { get; set; }
        public bool IsFirstLoginSeason { get; set; }
        public int GmLevel { get; set; }

        /// <summary>
        /// 昵称 ,steam短id,account
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 大区数据库ID
        /// </summary>
        public int ZoneID { get; set; }

        /// <summary>
        /// 服务器ID
        /// </summary>
        public int ServerID { get; set; }

        /// <summary>
        /// 等级
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public int Gender { get; set; }

        /// <summary>
        /// 会员
        /// </summary>
        public int VipType;

        /// <summary>
        /// 会员到期时间
        /// </summary>
        public long VipEndTimeSpan;

        [BsonIgnore]
        public TimerEntityComponent TimerEntityComp
        {
            get => GetComponent<TimerEntityComponent>();
        }

        [BsonIgnore]
        public SeedRandomComponent SeedRandomComp
        {
            get => GetComponent<SeedRandomComponent>();
        }

        [BsonIgnore]
        public BagComponent BagComp
        {
            get => GetComponent<BagComponent>();
        }

        [BsonIgnore]
        public CharacterDataComponent DataComp
        {
            get => GetComponent<CharacterDataComponent>();
        }

        [BsonIgnore]
        public CharacterSteamComponent SteamComp
        {
            get => GetComponent<CharacterSteamComponent>();
        }

        [BsonIgnore]
        public CharacterShopComponent ShopComp
        {
            get => GetComponent<CharacterShopComponent>();
        }

        [BsonIgnore]
        public CharacterBattlePassComponent BattlePassComp
        {
            get => GetComponent<CharacterBattlePassComponent>();
        }

        [BsonIgnore]
        public CharacterMailComponent MailComp
        {
            get => GetComponent<CharacterMailComponent>();
        }

        [BsonIgnore]
        public CharacterActivityComponent ActivityComp
        {
            get => GetComponent<CharacterActivityComponent>();
        }

        [BsonIgnore]
        public HeroManageComponent HeroManageComp
        {
            get => GetComponent<HeroManageComponent>();
        }

        [BsonIgnore]
        public CharacterDrawTreasureComponent DrawTreasureComp
        {
            get => GetComponent<CharacterDrawTreasureComponent>();
        }

        [BsonIgnore]
        public CharacterRechargeComponent RechargeComp
        {
            get => GetComponent<CharacterRechargeComponent>();
        }

        [BsonIgnore]
        public CharacterBuffComponent BuffComp
        {
            get => GetComponent<CharacterBuffComponent>();
        }

        [BsonIgnore]
        public CharacterRankComponent RankComp
        {
            get => GetComponent<CharacterRankComponent>();
        }

        [BsonIgnore]
        public CharacterTitleComponent TitleComp
        {
            get => GetComponent<CharacterTitleComponent>();
        }

        [BsonIgnore]
        public CharacterAchievementComponent AchievementComp
        {
            get => GetComponent<CharacterAchievementComponent>();
        }

        [BsonIgnore]
        public CharacterGameRecordComponent GameRecordComp
        {
            get => GetComponent<CharacterGameRecordComponent>();
        }

        [BsonIgnore]
        public CharacterBattleTeamComponent BattleTeamComp
        {
            get => GetComponent<CharacterBattleTeamComponent>();
        }
    }
}