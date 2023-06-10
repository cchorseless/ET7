using ET;
using ProtoBuf;
using System.Collections.Generic;
namespace ET
{
	[Message(OuterMessageHttp.H2C_CommonResponse)]
	[ProtoContract]
	public partial class H2C_CommonResponse: ProtoObject, IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(H2C_CommonResponse))]
	[Message(OuterMessageHttp.C2H_GetServerZoneInfo)]
	[ProtoContract]
	public partial class C2H_GetServerZoneInfo: ProtoObject, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public int ServerId { get; set; }

	}

	[Message(OuterMessageHttp.FServerInfo)]
	[ProtoContract]
	public partial class FServerInfo: ProtoObject, IMessage
	{
		[ProtoMember(1)]
		public string ServerName { get; set; }

		[ProtoMember(2)]
		public int ServerID { get; set; }

		[ProtoMember(3)]
		public int ZoneID { get; set; }

		[ProtoMember(4)]
		public List<int> State { get; set; }

	}

	[Message(OuterMessageHttp.H2C_GetServerList)]
	[ProtoContract]
	public partial class H2C_GetServerList: ProtoObject, IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(93)]
		public List<FServerInfo> ServerList { get; set; }

	}

	[Message(OuterMessageHttp.H2C_GetServerNotice)]
	[ProtoContract]
	public partial class H2C_GetServerNotice: ProtoObject, IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(H2C_GetServerUrl))]
	[Message(OuterMessageHttp.C2H_GetServerUrl)]
	[ProtoContract]
	public partial class C2H_GetServerUrl: ProtoObject, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public int ZoneId { get; set; }

		[ProtoMember(2)]
		public int ServerId { get; set; }

	}

	[Message(OuterMessageHttp.H2C_GetServerUrl)]
	[ProtoContract]
	public partial class H2C_GetServerUrl: ProtoObject, IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(93)]
		public string Ip { get; set; }

		[ProtoMember(94)]
		public int Port { get; set; }

	}

	[ResponseType(nameof(H2C_GetAccountLoginKey))]
	[Message(OuterMessageHttp.C2H_GetAccountLoginKey)]
	[ProtoContract]
	public partial class C2H_GetAccountLoginKey: ProtoObject, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public string Account { get; set; }

	}

	[Message(OuterMessageHttp.H2C_GetAccountLoginKey)]
	[ProtoContract]
	public partial class H2C_GetAccountLoginKey: ProtoObject, IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(93)]
		public string Key { get; set; }

	}

	[ResponseType(nameof(H2C_CommonResponse))]
	[Message(OuterMessageHttp.C2H_SetServerKey)]
	[ProtoContract]
	public partial class C2H_SetServerKey: ProtoObject, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public string ServerKey { get; set; }

		[ProtoMember(2)]
		public string Name { get; set; }

		[ProtoMember(3)]
		public string Label { get; set; }

	}

	[ResponseType(nameof(H2C_CommonResponse))]
	[Message(OuterMessageHttp.C2H_CreateGameRecord)]
	[ProtoContract]
	public partial class C2H_CreateGameRecord: ProtoObject, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public List<string> Players { get; set; }

	}

	[ResponseType(nameof(H2C_CommonResponse))]
	[Message(OuterMessageHttp.C2H_UploadGameRecord)]
	[ProtoContract]
	public partial class C2H_UploadGameRecord: ProtoObject, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public List<string> Keys { get; set; }

		[ProtoMember(2)]
		public List<string> Values { get; set; }

	}

	[ResponseType(nameof(H2C_CommonResponse))]
	[Message(OuterMessageHttp.C2H_UploadCharacterGameRecord)]
	[ProtoContract]
	public partial class C2H_UploadCharacterGameRecord: ProtoObject, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public List<string> Keys { get; set; }

		[ProtoMember(2)]
		public List<string> Values { get; set; }

	}

	[ResponseType(nameof(H2C_CommonResponse))]
	[Message(OuterMessageHttp.C2H_GetPrize_ActivitySevenDayLogin)]
	[ProtoContract]
	public partial class C2H_GetPrize_ActivitySevenDayLogin: ProtoObject, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public int Day { get; set; }

	}

	[ResponseType(nameof(H2C_CommonResponse))]
	[Message(OuterMessageHttp.C2H_GetPrize_ActivityMonthLogin)]
	[ProtoContract]
	public partial class C2H_GetPrize_ActivityMonthLogin: ProtoObject, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public int Day { get; set; }

	}

	[ResponseType(nameof(H2C_CommonResponse))]
	[Message(OuterMessageHttp.C2H_GetPrize_ActivityMonthTotalLogin)]
	[ProtoContract]
	public partial class C2H_GetPrize_ActivityMonthTotalLogin: ProtoObject, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public int Day { get; set; }

	}

	[ResponseType(nameof(H2C_CommonResponse))]
	[Message(OuterMessageHttp.C2H_GetPrize_ActivityDailyOnlinePrize)]
	[ProtoContract]
	public partial class C2H_GetPrize_ActivityDailyOnlinePrize: ProtoObject, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public int PrizeIndex { get; set; }

	}

	[ResponseType(nameof(H2C_CommonResponse))]
	[Message(OuterMessageHttp.C2H_GetPrize_ActivityTotalGainMetaStone)]
	[ProtoContract]
	public partial class C2H_GetPrize_ActivityTotalGainMetaStone: ProtoObject, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public int PrizeIndex { get; set; }

	}

	[ResponseType(nameof(H2C_CommonResponse))]
	[Message(OuterMessageHttp.C2H_GetPrize_ActivityTotalOnlineTime)]
	[ProtoContract]
	public partial class C2H_GetPrize_ActivityTotalOnlineTime: ProtoObject, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public int PrizeIndex { get; set; }

	}

	[ResponseType(nameof(H2C_CommonResponse))]
	[Message(OuterMessageHttp.C2H_GetPrize_ActivityTotalSpendMetaStone)]
	[ProtoContract]
	public partial class C2H_GetPrize_ActivityTotalSpendMetaStone: ProtoObject, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public int PrizeIndex { get; set; }

	}

	[ResponseType(nameof(H2C_CommonResponse))]
	[Message(OuterMessageHttp.C2H_GetPrize_ActivityInvestMetaStone)]
	[ProtoContract]
	public partial class C2H_GetPrize_ActivityInvestMetaStone: ProtoObject, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public int MetaStone { get; set; }

	}

	[ResponseType(nameof(H2C_CommonResponse))]
	[Message(OuterMessageHttp.C2H_GetPrize_ActivityGiftCommond)]
	[ProtoContract]
	public partial class C2H_GetPrize_ActivityGiftCommond: ProtoObject, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public int GiftConfigId { get; set; }

	}

	[ResponseType(nameof(H2C_CommonResponse))]
	[Message(OuterMessageHttp.C2H_Recharge_MetaStone)]
	[ProtoContract]
	public partial class C2H_Recharge_MetaStone: ProtoObject, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public int BuyType { get; set; }

		[ProtoMember(2)]
		public int PayType { get; set; }

	}

	[ResponseType(nameof(H2C_CommonResponse))]
	[Message(OuterMessageHttp.C2H_Handle_CharacterMail)]
	[ProtoContract]
	public partial class C2H_Handle_CharacterMail: ProtoObject, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public int HandleType { get; set; }

		[ProtoMember(2)]
		public bool IsOneKey { get; set; }

		[ProtoMember(3)]
		public string MailId { get; set; }

	}

	[ResponseType(nameof(H2C_CommonResponse))]
	[Message(OuterMessageHttp.C2H_DrawTreasure)]
	[ProtoContract]
	public partial class C2H_DrawTreasure: ProtoObject, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public int TreasureConfigId { get; set; }

		[ProtoMember(2)]
		public int TreasureCount { get; set; }

	}

	[ResponseType(nameof(H2C_CommonResponse))]
	[Message(OuterMessageHttp.C2H_CurRankDataInfo)]
	[ProtoContract]
	public partial class C2H_CurRankDataInfo: ProtoObject, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public int RankType { get; set; }

		[ProtoMember(3)]
		public int Page { get; set; }

		[ProtoMember(4)]
		public int PerPageCount { get; set; }

	}

	[ResponseType(nameof(H2C_CommonResponse))]
	[Message(OuterMessageHttp.C2H_CharacterRankDataInfo)]
	[ProtoContract]
	public partial class C2H_CharacterRankDataInfo: ProtoObject, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public int RankType { get; set; }

		[ProtoMember(4)]
		public string CharacterId { get; set; }

	}

	[ResponseType(nameof(H2C_CommonResponse))]
	[Message(OuterMessageHttp.C2H_Buy_ShopItem)]
	[ProtoContract]
	public partial class C2H_Buy_ShopItem: ProtoObject, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public int ShopConfigId { get; set; }

		[ProtoMember(2)]
		public int SellConfigId { get; set; }

		[ProtoMember(3)]
		public int PriceType { get; set; }

		[ProtoMember(4)]
		public int ItemCount { get; set; }

	}

	[ResponseType(nameof(H2C_CommonResponse))]
	[Message(OuterMessageHttp.C2H_Use_BagItem)]
	[ProtoContract]
	public partial class C2H_Use_BagItem: ProtoObject, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public string ItemId { get; set; }

		[ProtoMember(4)]
		public int ItemCount { get; set; }

	}

	[ResponseType(nameof(H2C_CommonResponse))]
	[Message(OuterMessageHttp.C2H_MergeBagEquip)]
	[ProtoContract]
	public partial class C2H_MergeBagEquip: ProtoObject, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public List<string> ItemId { get; set; }

	}

	[ResponseType(nameof(H2C_CommonResponse))]
	[Message(OuterMessageHttp.C2H_ReplaceEquipProps)]
	[ProtoContract]
	public partial class C2H_ReplaceEquipProps: ProtoObject, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public string ItemId { get; set; }

		[ProtoMember(2)]
		public string ItemPropId { get; set; }

		[ProtoMember(3)]
		public string CostItemId { get; set; }

		[ProtoMember(4)]
		public string CostItemPropId { get; set; }

	}

	[ResponseType(nameof(H2C_CommonResponse))]
	[Message(OuterMessageHttp.C2H_Use_AddHeroLevelByComHeroExp)]
	[ProtoContract]
	public partial class C2H_Use_AddHeroLevelByComHeroExp: ProtoObject, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(4)]
		public string HeroId { get; set; }

	}

	[ResponseType(nameof(H2C_CommonResponse))]
	[Message(OuterMessageHttp.C2H_Save_AddHeroBanDesign)]
	[ProtoContract]
	public partial class C2H_Save_AddHeroBanDesign: ProtoObject, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(4)]
		public int Slot { get; set; }

		[ProtoMember(1)]
		public List<string> BanHeroList { get; set; }

	}

	[ResponseType(nameof(H2C_CommonResponse))]
	[Message(OuterMessageHttp.C2H_ChangeHeroDressEquipState)]
	[ProtoContract]
	public partial class C2H_ChangeHeroDressEquipState: ProtoObject, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public int Slot { get; set; }

		[ProtoMember(2)]
		public string EquipId { get; set; }

		[ProtoMember(3)]
		public bool IsDressUp { get; set; }

		[ProtoMember(4)]
		public string HeroId { get; set; }

	}

	[ResponseType(nameof(H2C_CommonResponse))]
	[Message(OuterMessageHttp.C2H_ChangeHeroTalentState)]
	[ProtoContract]
	public partial class C2H_ChangeHeroTalentState: ProtoObject, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public int TalentLevel { get; set; }

		[ProtoMember(2)]
		public int TalentIndex { get; set; }

		[ProtoMember(3)]
		public bool IsLearn { get; set; }

		[ProtoMember(4)]
		public string HeroId { get; set; }

	}

	[ResponseType(nameof(H2C_CommonResponse))]
	[Message(OuterMessageHttp.C2H_GetInfoPassPrize)]
	[ProtoContract]
	public partial class C2H_GetInfoPassPrize: ProtoObject, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public int PrizeLevel { get; set; }

		[ProtoMember(3)]
		public bool IsOnlyKey { get; set; }

	}

	[ResponseType(nameof(H2C_CommonResponse))]
	[Message(OuterMessageHttp.C2H_ChangeCharacterTitleState)]
	[ProtoContract]
	public partial class C2H_ChangeCharacterTitleState: ProtoObject, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public int TitleConfigId { get; set; }

		[ProtoMember(3)]
		public bool IsDress { get; set; }

	}

	[ResponseType(nameof(H2C_CommonResponse))]
	[Message(OuterMessageHttp.C2H_GetPrize_AchieveMentPrize)]
	[ProtoContract]
	public partial class C2H_GetPrize_AchieveMentPrize: ProtoObject, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public int AchieveMentConfigId { get; set; }

	}

	[ResponseType(nameof(H2C_CommonResponse))]
	[Message(OuterMessageHttp.C2H_ChangeDailyTaskState)]
	[ProtoContract]
	public partial class C2H_ChangeDailyTaskState: ProtoObject, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public string TaskId { get; set; }

		[ProtoMember(2)]
		public bool isDropTask { get; set; }

	}

	[ResponseType(nameof(H2C_CommonResponse))]
	[Message(OuterMessageHttp.C2H_GetPrize_TaskPrize)]
	[ProtoContract]
	public partial class C2H_GetPrize_TaskPrize: ProtoObject, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public string TaskId { get; set; }

	}

	[ResponseType(nameof(H2C_CommonResponse))]
	[Message(OuterMessageHttp.C2H_BattlePass_GetPrize)]
	[ProtoContract]
	public partial class C2H_BattlePass_GetPrize: ProtoObject, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public int PrizeLevel { get; set; }

		[ProtoMember(2)]
		public bool IsPlusPrize { get; set; }

	}

	[ResponseType(nameof(H2C_CommonResponse))]
	[Message(OuterMessageHttp.C2H_BattlePass_ChargePrize)]
	[ProtoContract]
	public partial class C2H_BattlePass_ChargePrize: ProtoObject, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public int ConfigId { get; set; }

	}

	[ResponseType(nameof(H2C_CommonResponse))]
	[Message(OuterMessageHttp.C2H_Mentorship_ApplyForMaster)]
	[ProtoContract]
	public partial class C2H_Mentorship_ApplyForMaster: ProtoObject, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public string MasterX64str { get; set; }

		[ProtoMember(2)]
		public string ApplyMessage { get; set; }

	}

	[ResponseType(nameof(H2C_CommonResponse))]
	[Message(OuterMessageHttp.C2H_Mentorship_DropTree)]
	[ProtoContract]
	public partial class C2H_Mentorship_DropTree: ProtoObject, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public string EntityId { get; set; }

		[ProtoMember(2)]
		public bool IsMaster { get; set; }

	}

	[ResponseType(nameof(H2C_CommonResponse))]
	[Message(OuterMessageHttp.C2H_Mentorship_ChangeApplyState)]
	[ProtoContract]
	public partial class C2H_Mentorship_ChangeApplyState: ProtoObject, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public string EntityId { get; set; }

		[ProtoMember(2)]
		public bool IsAgree { get; set; }

	}

	[ResponseType(nameof(H2C_CommonResponse))]
	[Message(OuterMessageHttp.C2H_DrawEnemy_GetEnemyInfo)]
	[ProtoContract]
	public partial class C2H_DrawEnemy_GetEnemyInfo: ProtoObject, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public int RoundIndex { get; set; }

		[ProtoMember(2)]
		public int RoundCharpter { get; set; }

		[ProtoMember(3)]
		public int EnemyCount { get; set; }

		[ProtoMember(4)]
		public int Score { get; set; }

	}

	[Message(OuterMessageHttp.FBattleUnitInfoItem)]
	[ProtoContract]
	public partial class FBattleUnitInfoItem: ProtoObject, IMessage
	{
		[ProtoMember(1)]
		public string UnitName { get; set; }

		[ProtoMember(2)]
		public int Level { get; set; }

		[ProtoMember(3)]
		public int Star { get; set; }

		[ProtoMember(4)]
		public int PosX { get; set; }

		[ProtoMember(5)]
		public int PosY { get; set; }

		[ProtoMember(6)]
		public string WearBundleId { get; set; }

		[ProtoMember(7)]
		public List<string> EquipInfo { get; set; }

		[ProtoMember(8)]
		public List<string> Buffs { get; set; }

	}

	[Message(OuterMessageHttp.FBattleTeamRecord)]
	[ProtoContract]
	public partial class FBattleTeamRecord: ProtoObject, IMessage
	{
		[ProtoMember(1)]
		public string SteamAccountId { get; set; }

		[ProtoMember(2)]
		public string SteamAccountName { get; set; }

		[ProtoMember(3)]
		public int RoundIndex { get; set; }

		[ProtoMember(4)]
		public int RoundCharpter { get; set; }

		[ProtoMember(5)]
		public int Score { get; set; }

		[ProtoMember(6)]
		public List<string> SectInfo { get; set; }

		[ProtoMember(7)]
		public List<FBattleUnitInfoItem> UnitInfo { get; set; }

	}

	[ResponseType(nameof(H2C_CommonResponse))]
	[Message(OuterMessageHttp.C2H_DrawEnemy_UploadEnemyInfo)]
	[ProtoContract]
	public partial class C2H_DrawEnemy_UploadEnemyInfo: ProtoObject, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public string SteamAccountId { get; set; }

		[ProtoMember(2)]
		public string SteamAccountName { get; set; }

		[ProtoMember(6)]
		public List<FBattleTeamRecord> TeamInfo { get; set; }

	}

	[ResponseType(nameof(H2C_CommonResponse))]
	[Message(OuterMessageHttp.C2H_DrawEnemy_UploadBattleResult)]
	[ProtoContract]
	public partial class C2H_DrawEnemy_UploadBattleResult: ProtoObject, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public int RoundIndex { get; set; }

		[ProtoMember(2)]
		public int RoundCharpter { get; set; }

		[ProtoMember(3)]
		public string EnemyEntityId { get; set; }

		[ProtoMember(4)]
		public int BattleScore { get; set; }

	}

	public static class OuterMessageHttp
	{
		 public const ushort H2C_CommonResponse = 15001;
		 public const ushort C2H_GetServerZoneInfo = 15002;
		 public const ushort FServerInfo = 15003;
		 public const ushort H2C_GetServerList = 15004;
		 public const ushort H2C_GetServerNotice = 15005;
		 public const ushort C2H_GetServerUrl = 15006;
		 public const ushort H2C_GetServerUrl = 15007;
		 public const ushort C2H_GetAccountLoginKey = 15008;
		 public const ushort H2C_GetAccountLoginKey = 15009;
		 public const ushort C2H_SetServerKey = 15010;
		 public const ushort C2H_CreateGameRecord = 15011;
		 public const ushort C2H_UploadGameRecord = 15012;
		 public const ushort C2H_UploadCharacterGameRecord = 15013;
		 public const ushort C2H_GetPrize_ActivitySevenDayLogin = 15014;
		 public const ushort C2H_GetPrize_ActivityMonthLogin = 15015;
		 public const ushort C2H_GetPrize_ActivityMonthTotalLogin = 15016;
		 public const ushort C2H_GetPrize_ActivityDailyOnlinePrize = 15017;
		 public const ushort C2H_GetPrize_ActivityTotalGainMetaStone = 15018;
		 public const ushort C2H_GetPrize_ActivityTotalOnlineTime = 15019;
		 public const ushort C2H_GetPrize_ActivityTotalSpendMetaStone = 15020;
		 public const ushort C2H_GetPrize_ActivityInvestMetaStone = 15021;
		 public const ushort C2H_GetPrize_ActivityGiftCommond = 15022;
		 public const ushort C2H_Recharge_MetaStone = 15023;
		 public const ushort C2H_Handle_CharacterMail = 15024;
		 public const ushort C2H_DrawTreasure = 15025;
		 public const ushort C2H_CurRankDataInfo = 15026;
		 public const ushort C2H_CharacterRankDataInfo = 15027;
		 public const ushort C2H_Buy_ShopItem = 15028;
		 public const ushort C2H_Use_BagItem = 15029;
		 public const ushort C2H_MergeBagEquip = 15030;
		 public const ushort C2H_ReplaceEquipProps = 15031;
		 public const ushort C2H_Use_AddHeroLevelByComHeroExp = 15032;
		 public const ushort C2H_Save_AddHeroBanDesign = 15033;
		 public const ushort C2H_ChangeHeroDressEquipState = 15034;
		 public const ushort C2H_ChangeHeroTalentState = 15035;
		 public const ushort C2H_GetInfoPassPrize = 15036;
		 public const ushort C2H_ChangeCharacterTitleState = 15037;
		 public const ushort C2H_GetPrize_AchieveMentPrize = 15038;
		 public const ushort C2H_ChangeDailyTaskState = 15039;
		 public const ushort C2H_GetPrize_TaskPrize = 15040;
		 public const ushort C2H_BattlePass_GetPrize = 15041;
		 public const ushort C2H_BattlePass_ChargePrize = 15042;
		 public const ushort C2H_Mentorship_ApplyForMaster = 15043;
		 public const ushort C2H_Mentorship_DropTree = 15044;
		 public const ushort C2H_Mentorship_ChangeApplyState = 15045;
		 public const ushort C2H_DrawEnemy_GetEnemyInfo = 15046;
		 public const ushort FBattleUnitInfoItem = 15047;
		 public const ushort FBattleTeamRecord = 15048;
		 public const ushort C2H_DrawEnemy_UploadEnemyInfo = 15049;
		 public const ushort C2H_DrawEnemy_UploadBattleResult = 15050;
	}
}
