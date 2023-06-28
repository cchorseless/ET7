using ET;
using ProtoBuf;
using System.Collections.Generic;
namespace ET
{
	[ResponseType(nameof(P2W_GMReload))]
	[Message(InnerMessageGmWeb.W2P_GMReload)]
	[ProtoContract]
	public partial class W2P_GMReload: ProtoObject, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

	}

	[Message(InnerMessageGmWeb.P2W_GMReload)]
	[ProtoContract]
	public partial class P2W_GMReload: ProtoObject, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(P2W_GMShutDown))]
	[Message(InnerMessageGmWeb.W2P_GMShutDown)]
	[ProtoContract]
	public partial class W2P_GMShutDown: ProtoObject, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

	}

	[Message(InnerMessageGmWeb.P2W_GMShutDown)]
	[ProtoContract]
	public partial class P2W_GMShutDown: ProtoObject, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(W2G_GMGetProcessInfo))]
	[Message(InnerMessageGmWeb.G2W_GMGetProcessInfo)]
	[ProtoContract]
	public partial class G2W_GMGetProcessInfo: ProtoObject, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

	}

	[Message(InnerMessageGmWeb.W2G_GMGetProcessInfo)]
	[ProtoContract]
	public partial class W2G_GMGetProcessInfo: ProtoObject, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(P2W_GMGetProcessEntityInfo))]
	[Message(InnerMessageGmWeb.W2P_GMGetProcessEntityInfo)]
	[ProtoContract]
	public partial class W2P_GMGetProcessEntityInfo: ProtoObject, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

	}

	[Message(InnerMessageGmWeb.P2W_GMGetProcessEntityInfo)]
	[ProtoContract]
	public partial class P2W_GMGetProcessEntityInfo: ProtoObject, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(W2G_GMProcessEdit))]
	[Message(InnerMessageGmWeb.G2W_GMProcessEdit)]
	[ProtoContract]
	public partial class G2W_GMProcessEdit: ProtoObject, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int ProcessId { get; set; }

// 0 reload 1 ReStart 2 Start 3 ShutDown
		[ProtoMember(92)]
		public int OperateType { get; set; }

	}

	[Message(InnerMessageGmWeb.W2G_GMProcessEdit)]
	[ProtoContract]
	public partial class W2G_GMProcessEdit: ProtoObject, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(P2G_GMAddNewNotice))]
	[Message(InnerMessageGmWeb.G2P_GMAddNewNotice)]
	[ProtoContract]
	public partial class G2P_GMAddNewNotice: ProtoObject, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(92)]
		public string NOTICE { get; set; }

		[ProtoMember(94)]
		public int OperateTime { get; set; }

	}

	[Message(InnerMessageGmWeb.P2G_GMAddNewNotice)]
	[ProtoContract]
	public partial class P2G_GMAddNewNotice: ProtoObject, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(P2G_GMAddNewServerZone))]
	[Message(InnerMessageGmWeb.G2P_GMAddNewServerZone)]
	[ProtoContract]
	public partial class G2P_GMAddNewServerZone: ProtoObject, IActorRequest
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

	[Message(InnerMessageGmWeb.P2G_GMAddNewServerZone)]
	[ProtoContract]
	public partial class P2G_GMAddNewServerZone: ProtoObject, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(P2G_GMEditServerZone))]
	[Message(InnerMessageGmWeb.G2P_GMEditServerZone)]
	[ProtoContract]
	public partial class G2P_GMEditServerZone: ProtoObject, IActorRequest
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

	[Message(InnerMessageGmWeb.P2G_GMEditServerZone)]
	[ProtoContract]
	public partial class P2G_GMEditServerZone: ProtoObject, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	public static class InnerMessageGmWeb
	{
		 public const ushort W2P_GMReload = 24001;
		 public const ushort P2W_GMReload = 24002;
		 public const ushort W2P_GMShutDown = 24003;
		 public const ushort P2W_GMShutDown = 24004;
		 public const ushort G2W_GMGetProcessInfo = 24005;
		 public const ushort W2G_GMGetProcessInfo = 24006;
		 public const ushort W2P_GMGetProcessEntityInfo = 24007;
		 public const ushort P2W_GMGetProcessEntityInfo = 24008;
		 public const ushort G2W_GMProcessEdit = 24009;
		 public const ushort W2G_GMProcessEdit = 24010;
		 public const ushort G2P_GMAddNewNotice = 24011;
		 public const ushort P2G_GMAddNewNotice = 24012;
		 public const ushort G2P_GMAddNewServerZone = 24013;
		 public const ushort P2G_GMAddNewServerZone = 24014;
		 public const ushort G2P_GMEditServerZone = 24015;
		 public const ushort P2G_GMEditServerZone = 24016;
	}
}
