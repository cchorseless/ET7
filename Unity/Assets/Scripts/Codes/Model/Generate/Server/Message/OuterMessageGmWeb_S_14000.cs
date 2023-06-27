using ET;
using ProtoBuf;
using System.Collections.Generic;
namespace ET
{
	[ResponseType(nameof(H2C_CommonResponse))]
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
		public List<int> Roles { get; set; }

	}

	[ResponseType(nameof(G2C_GMLogin))]
	[Message(OuterMessageGmWeb.C2G_GMLogin)]
	[ProtoContract]
	public partial class C2G_GMLogin: ProtoObject, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public string Account { get; set; }

		[ProtoMember(2)]
		public string Password { get; set; }

	}

	[Message(OuterMessageGmWeb.G2C_GMLogin)]
	[ProtoContract]
	public partial class G2C_GMLogin: ProtoObject, IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public string Token { get; set; }

		[ProtoMember(4)]
		public long UserId { get; set; }

	}

	[ResponseType(nameof(H2C_CommonResponse))]
	[Message(OuterMessageGmWeb.C2G_GMGetUserInfo)]
	[ProtoContract]
	public partial class C2G_GMGetUserInfo: ProtoObject, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

	}

	[ResponseType(nameof(H2C_CommonResponse))]
	[Message(OuterMessageGmWeb.C2G_GMGetAllUserInfo)]
	[ProtoContract]
	public partial class C2G_GMGetAllUserInfo: ProtoObject, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

	}

	[ResponseType(nameof(H2C_CommonResponse))]
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

	[ResponseType(nameof(H2C_CommonResponse))]
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
		public List<int> Roles { get; set; }

	}

	[ResponseType(nameof(H2C_CommonResponse))]
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

	[ResponseType(nameof(H2C_CommonResponse))]
	[Message(OuterMessageGmWeb.C2G_GMGetProcessState)]
	[ProtoContract]
	public partial class C2G_GMGetProcessState: ProtoObject, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

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

	[ResponseType(nameof(H2C_CommonResponse))]
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

	[ResponseType(nameof(H2C_CommonResponse))]
	[Message(OuterMessageGmWeb.C2G_GMGetServerZoneInfo)]
	[ProtoContract]
	public partial class C2G_GMGetServerZoneInfo: ProtoObject, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

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

	[ResponseType(nameof(H2C_CommonResponse))]
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

	[ResponseType(nameof(H2C_CommonResponse))]
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

	public static class OuterMessageGmWeb
	{
		 public const ushort C2G_GMRegiste = 14001;
		 public const ushort C2G_GMLogin = 14002;
		 public const ushort G2C_GMLogin = 14003;
		 public const ushort C2G_GMGetUserInfo = 14004;
		 public const ushort C2G_GMGetAllUserInfo = 14005;
		 public const ushort C2G_GMChangePassword = 14006;
		 public const ushort C2G_GMEditAccount = 14007;
		 public const ushort C2G_GMProcessEdit = 14008;
		 public const ushort C2G_GMGetProcessState = 14009;
		 public const ushort C2G_GMGetLogDBInfo = 14010;
		 public const ushort G2C_GMGetLogDBInfo = 14011;
		 public const ushort C2G_GMSearchLog = 14012;
		 public const ushort G2C_GMSearchLog = 14013;
		 public const ushort C2G_GMIgnoreErrorLog = 14014;
		 public const ushort C2G_GMAddNewServerNotice = 14015;
		 public const ushort G2C_GMAddNewServerNotice = 14016;
		 public const ushort C2G_GMGetServerZoneInfo = 14017;
		 public const ushort C2G_GMSearchServerZone = 14018;
		 public const ushort G2C_GMSearchServerZone = 14019;
		 public const ushort C2G_GMAddNewServerZone = 14020;
		 public const ushort C2G_GMEditServerZone = 14021;
	}
}
