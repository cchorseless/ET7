using ET;
using ProtoBuf;
using System.Collections.Generic;
namespace ET
{
	[ResponseType(nameof(G2C_GMRegiste))]
	[Message(OuterMessageGmWeb.C2G_GMRegiste)]
	[ProtoContract]
	public partial class C2G_GMRegiste: ProtoObject, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public string Account { get; set; }

		[ProtoMember(2)]
		public string Password { get; set; }

		[ProtoMember(3)]
		public int GmLevel { get; set; }

		[ProtoMember(4)]
		public string Des { get; set; }

		[ProtoMember(5)]
		public List<string> Routes { get; set; }

	}

	[Message(OuterMessageGmWeb.G2C_GMRegiste)]
	[ProtoContract]
	public partial class G2C_GMRegiste: ProtoObject, IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(R2C_GMLogin))]
	[Message(OuterMessageGmWeb.C2R_GMLogin)]
	[ProtoContract]
	public partial class C2R_GMLogin: ProtoObject, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public string Account { get; set; }

		[ProtoMember(2)]
		public string Password { get; set; }

	}

	[Message(OuterMessageGmWeb.R2C_GMLogin)]
	[ProtoContract]
	public partial class R2C_GMLogin: ProtoObject, IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public string Address { get; set; }

		[ProtoMember(2)]
		public long Key { get; set; }

		[ProtoMember(3)]
		public long GateId { get; set; }

		[ProtoMember(4)]
		public long UserId { get; set; }

	}

	[ResponseType(nameof(G2C_GMLoginGate))]
	[Message(OuterMessageGmWeb.C2G_GMLoginGate)]
	[ProtoContract]
	public partial class C2G_GMLoginGate: ProtoObject, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public long Key { get; set; }

		[ProtoMember(2)]
		public long GateId { get; set; }

		[ProtoMember(3)]
		public long UserId { get; set; }

	}

	[Message(OuterMessageGmWeb.G2C_GMLoginGate)]
	[ProtoContract]
	public partial class G2C_GMLoginGate: ProtoObject, IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public long PlayerId { get; set; }

	}

	[ResponseType(nameof(G2C_GMEnterGame))]
	[Message(OuterMessageGmWeb.C2G_GMEnterGame)]
	[ProtoContract]
	public partial class C2G_GMEnterGame: ProtoObject, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public int Index { get; set; }

	}

	[Message(OuterMessageGmWeb.G2C_GMEnterGame)]
	[ProtoContract]
	public partial class G2C_GMEnterGame: ProtoObject, IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(3)]
		public string Message { get; set; }

// 自己unitId
		[ProtoMember(4)]
		public long MyId { get; set; }

	}

	[ResponseType(nameof(G2C_GMProcessEdit))]
	[Message(OuterMessageGmWeb.C2G_GMProcessEdit)]
	[ProtoContract]
	public partial class C2G_GMProcessEdit: ProtoObject, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int ProcessId { get; set; }

// 0 reload 1 ReStart 2 Start 3 ShutDown
		[ProtoMember(1)]
		public int OperateType { get; set; }

	}

	[Message(OuterMessageGmWeb.G2C_GMProcessEdit)]
	[ProtoContract]
	public partial class G2C_GMProcessEdit: ProtoObject, IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(G2C_GMGetProcessState))]
	[Message(OuterMessageGmWeb.C2G_GMGetProcessState)]
	[ProtoContract]
	public partial class C2G_GMGetProcessState: ProtoObject, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

	}

	[Message(OuterMessageGmWeb.G2C_GMGetProcessState)]
	[ProtoContract]
	public partial class G2C_GMGetProcessState: ProtoObject, IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(G2C_GMGetLogDBInfo))]
	[Message(OuterMessageGmWeb.C2G_GMGetLogDBInfo)]
	[ProtoContract]
	public partial class C2G_GMGetLogDBInfo: ProtoObject, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

	}

	[Message(OuterMessageGmWeb.G2C_GMGetLogDBInfo)]
	[ProtoContract]
	public partial class G2C_GMGetLogDBInfo: ProtoObject, IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public string Ip { get; set; }

		[ProtoMember(2)]
		public string Port { get; set; }

		[ProtoMember(3)]
		public string DbName { get; set; }

	}

	[ResponseType(nameof(G2C_GMSearchLog))]
	[Message(OuterMessageGmWeb.C2G_GMSearchLog)]
	[ProtoContract]
	public partial class C2G_GMSearchLog: ProtoObject, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public int StartTime { get; set; }

		[ProtoMember(2)]
		public int EndTime { get; set; }

		[ProtoMember(3)]
		public string Title { get; set; }

		[ProtoMember(4)]
		public int LogLevel { get; set; }

		[ProtoMember(5)]
		public string Label { get; set; }

		[ProtoMember(6)]
		public string ProcessId { get; set; }

		[ProtoMember(7)]
		public int PageCount { get; set; }

		[ProtoMember(8)]
		public int PageIndex { get; set; }

	}

	[Message(OuterMessageGmWeb.G2C_GMSearchLog)]
	[ProtoContract]
	public partial class G2C_GMSearchLog: ProtoObject, IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public List<string> SearchResult { get; set; }

		[ProtoMember(2)]
		public int SearchCount { get; set; }

	}

	[ResponseType(nameof(G2C_GMIgnoreErrorLog))]
	[Message(OuterMessageGmWeb.C2G_GMIgnoreErrorLog)]
	[ProtoContract]
	public partial class C2G_GMIgnoreErrorLog: ProtoObject, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public string LogId { get; set; }

		[ProtoMember(92)]
		public int LogTime { get; set; }

		[ProtoMember(93)]
		public string LogProcess { get; set; }

	}

	[Message(OuterMessageGmWeb.G2C_GMIgnoreErrorLog)]
	[ProtoContract]
	public partial class G2C_GMIgnoreErrorLog: ProtoObject, IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(G2C_GMChangePassword))]
	[Message(OuterMessageGmWeb.C2G_GMChangePassword)]
	[ProtoContract]
	public partial class C2G_GMChangePassword: ProtoObject, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public string OldPassWord { get; set; }

		[ProtoMember(2)]
		public string NewPassWord { get; set; }

	}

	[Message(OuterMessageGmWeb.G2C_GMChangePassword)]
	[ProtoContract]
	public partial class G2C_GMChangePassword: ProtoObject, IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(G2C_GMGetAccountRoleInfo))]
	[Message(OuterMessageGmWeb.C2G_GMGetAccountRoleInfo)]
	[ProtoContract]
	public partial class C2G_GMGetAccountRoleInfo: ProtoObject, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

	}

	[Message(OuterMessageGmWeb.G2C_GMGetAccountRoleInfo)]
	[ProtoContract]
	public partial class G2C_GMGetAccountRoleInfo: ProtoObject, IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(G2C_GMEditAccount))]
	[Message(OuterMessageGmWeb.C2G_GMEditAccount)]
	[ProtoContract]
	public partial class C2G_GMEditAccount: ProtoObject, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public string Account { get; set; }

		[ProtoMember(2)]
		public int Operate { get; set; }

		[ProtoMember(3)]
		public int GmLevel { get; set; }

		[ProtoMember(4)]
		public string Des { get; set; }

		[ProtoMember(5)]
		public List<string> Routes { get; set; }

	}

	[Message(OuterMessageGmWeb.G2C_GMEditAccount)]
	[ProtoContract]
	public partial class G2C_GMEditAccount: ProtoObject, IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(G2C_GMAddNewServerNotice))]
	[Message(OuterMessageGmWeb.C2G_GMAddNewServerNotice)]
	[ProtoContract]
	public partial class C2G_GMAddNewServerNotice: ProtoObject, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(92)]
		public string NOTICE { get; set; }

		[ProtoMember(94)]
		public int OperateTime { get; set; }

	}

	[Message(OuterMessageGmWeb.G2C_GMAddNewServerNotice)]
	[ProtoContract]
	public partial class G2C_GMAddNewServerNotice: ProtoObject, IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(G2C_GMGetServerZoneInfo))]
	[Message(OuterMessageGmWeb.C2G_GMGetServerZoneInfo)]
	[ProtoContract]
	public partial class C2G_GMGetServerZoneInfo: ProtoObject, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

	}

	[Message(OuterMessageGmWeb.G2C_GMGetServerZoneInfo)]
	[ProtoContract]
	public partial class G2C_GMGetServerZoneInfo: ProtoObject, IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(G2C_GMSearchServerZone))]
	[Message(OuterMessageGmWeb.C2G_GMSearchServerZone)]
	[ProtoContract]
	public partial class C2G_GMSearchServerZone: ProtoObject, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int ZoneId { get; set; }

		[ProtoMember(92)]
		public int ServerLabel { get; set; }

		[ProtoMember(93)]
		public int State { get; set; }

		[ProtoMember(7)]
		public int PageCount { get; set; }

		[ProtoMember(8)]
		public int PageIndex { get; set; }

	}

	[Message(OuterMessageGmWeb.G2C_GMSearchServerZone)]
	[ProtoContract]
	public partial class G2C_GMSearchServerZone: ProtoObject, IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public List<string> SearchResult { get; set; }

		[ProtoMember(2)]
		public int SearchCount { get; set; }

	}

	[ResponseType(nameof(G2C_GMAddNewServerZone))]
	[Message(OuterMessageGmWeb.C2G_GMAddNewServerZone)]
	[ProtoContract]
	public partial class C2G_GMAddNewServerZone: ProtoObject, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int ZoneId { get; set; }

		[ProtoMember(92)]
		public string ServerName { get; set; }

		[ProtoMember(93)]
		public int ServerLabel { get; set; }

		[ProtoMember(94)]
		public int OperateTime { get; set; }

	}

	[Message(OuterMessageGmWeb.G2C_GMAddNewServerZone)]
	[ProtoContract]
	public partial class G2C_GMAddNewServerZone: ProtoObject, IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(G2C_GMEditServerZone))]
	[Message(OuterMessageGmWeb.C2G_GMEditServerZone)]
	[ProtoContract]
	public partial class C2G_GMEditServerZone: ProtoObject, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int State { get; set; }

		[ProtoMember(92)]
		public long ServerId { get; set; }

		[ProtoMember(93)]
		public int OperateType { get; set; }

		[ProtoMember(94)]
		public int OperateTime { get; set; }

		[ProtoMember(95)]
		public int ServerLabel { get; set; }

		[ProtoMember(96)]
		public string ServerName { get; set; }

	}

	[Message(OuterMessageGmWeb.G2C_GMEditServerZone)]
	[ProtoContract]
	public partial class G2C_GMEditServerZone: ProtoObject, IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	public static class OuterMessageGmWeb
	{
		 public const ushort C2G_GMRegiste = 14001;
		 public const ushort G2C_GMRegiste = 14002;
		 public const ushort C2R_GMLogin = 14003;
		 public const ushort R2C_GMLogin = 14004;
		 public const ushort C2G_GMLoginGate = 14005;
		 public const ushort G2C_GMLoginGate = 14006;
		 public const ushort C2G_GMEnterGame = 14007;
		 public const ushort G2C_GMEnterGame = 14008;
		 public const ushort C2G_GMProcessEdit = 14009;
		 public const ushort G2C_GMProcessEdit = 14010;
		 public const ushort C2G_GMGetProcessState = 14011;
		 public const ushort G2C_GMGetProcessState = 14012;
		 public const ushort C2G_GMGetLogDBInfo = 14013;
		 public const ushort G2C_GMGetLogDBInfo = 14014;
		 public const ushort C2G_GMSearchLog = 14015;
		 public const ushort G2C_GMSearchLog = 14016;
		 public const ushort C2G_GMIgnoreErrorLog = 14017;
		 public const ushort G2C_GMIgnoreErrorLog = 14018;
		 public const ushort C2G_GMChangePassword = 14019;
		 public const ushort G2C_GMChangePassword = 14020;
		 public const ushort C2G_GMGetAccountRoleInfo = 14021;
		 public const ushort G2C_GMGetAccountRoleInfo = 14022;
		 public const ushort C2G_GMEditAccount = 14023;
		 public const ushort G2C_GMEditAccount = 14024;
		 public const ushort C2G_GMAddNewServerNotice = 14025;
		 public const ushort G2C_GMAddNewServerNotice = 14026;
		 public const ushort C2G_GMGetServerZoneInfo = 14027;
		 public const ushort G2C_GMGetServerZoneInfo = 14028;
		 public const ushort C2G_GMSearchServerZone = 14029;
		 public const ushort G2C_GMSearchServerZone = 14030;
		 public const ushort C2G_GMAddNewServerZone = 14031;
		 public const ushort G2C_GMAddNewServerZone = 14032;
		 public const ushort C2G_GMEditServerZone = 14033;
		 public const ushort G2C_GMEditServerZone = 14034;
	}
}
