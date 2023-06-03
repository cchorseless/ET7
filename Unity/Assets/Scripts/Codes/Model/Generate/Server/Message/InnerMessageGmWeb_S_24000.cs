using ET;
using ProtoBuf;
using System.Collections.Generic;
namespace ET
{
	[ResponseType(nameof(A2M_GMReload))]
	[Message(InnerMessageGmWeb.M2A_GMReload)]
	[ProtoContract]
	public partial class M2A_GMReload: ProtoObject, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

	}

	[Message(InnerMessageGmWeb.A2M_GMReload)]
	[ProtoContract]
	public partial class A2M_GMReload: ProtoObject, IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(G2W_GMShutDown))]
	[Message(InnerMessageGmWeb.W2G_GMShutDown)]
	[ProtoContract]
	public partial class W2G_GMShutDown: ProtoObject, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

	}

	[Message(InnerMessageGmWeb.G2W_GMShutDown)]
	[ProtoContract]
	public partial class G2W_GMShutDown: ProtoObject, IResponse
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
	public partial class G2W_GMGetProcessInfo: ProtoObject, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

	}

	[Message(InnerMessageGmWeb.W2G_GMGetProcessInfo)]
	[ProtoContract]
	public partial class W2G_GMGetProcessInfo: ProtoObject, IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(G2W_GMGetProcessEntityInfo))]
	[Message(InnerMessageGmWeb.W2G_GMGetProcessEntityInfo)]
	[ProtoContract]
	public partial class W2G_GMGetProcessEntityInfo: ProtoObject, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

	}

	[Message(InnerMessageGmWeb.G2W_GMGetProcessEntityInfo)]
	[ProtoContract]
	public partial class G2W_GMGetProcessEntityInfo: ProtoObject, IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(A2M_GMProcessEdit))]
	[Message(InnerMessageGmWeb.M2A_GMProcessEdit)]
	[ProtoContract]
	public partial class M2A_GMProcessEdit: ProtoObject, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int ProcessId { get; set; }

// 0 reload 1 ReStart 2 Start 3 ShutDown
		[ProtoMember(92)]
		public int OperateType { get; set; }

	}

	[Message(InnerMessageGmWeb.A2M_GMProcessEdit)]
	[ProtoContract]
	public partial class A2M_GMProcessEdit: ProtoObject, IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(A2M_GMAddNewNotice))]
	[Message(InnerMessageGmWeb.M2A_GMAddNewNotice)]
	[ProtoContract]
	public partial class M2A_GMAddNewNotice: ProtoObject, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(92)]
		public string NOTICE { get; set; }

		[ProtoMember(94)]
		public int OperateTime { get; set; }

	}

	[Message(InnerMessageGmWeb.A2M_GMAddNewNotice)]
	[ProtoContract]
	public partial class A2M_GMAddNewNotice: ProtoObject, IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(A2M_GMAddNewServerZone))]
	[Message(InnerMessageGmWeb.M2A_GMAddNewServerZone)]
	[ProtoContract]
	public partial class M2A_GMAddNewServerZone: ProtoObject, IRequest
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

	[Message(InnerMessageGmWeb.A2M_GMAddNewServerZone)]
	[ProtoContract]
	public partial class A2M_GMAddNewServerZone: ProtoObject, IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(A2M_GMEditServerZone))]
	[Message(InnerMessageGmWeb.M2A_GMEditServerZone)]
	[ProtoContract]
	public partial class M2A_GMEditServerZone: ProtoObject, IRequest
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

	[Message(InnerMessageGmWeb.A2M_GMEditServerZone)]
	[ProtoContract]
	public partial class A2M_GMEditServerZone: ProtoObject, IResponse
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
		 public const ushort M2A_GMReload = 24001;
		 public const ushort A2M_GMReload = 24002;
		 public const ushort W2G_GMShutDown = 24003;
		 public const ushort G2W_GMShutDown = 24004;
		 public const ushort G2W_GMGetProcessInfo = 24005;
		 public const ushort W2G_GMGetProcessInfo = 24006;
		 public const ushort W2G_GMGetProcessEntityInfo = 24007;
		 public const ushort G2W_GMGetProcessEntityInfo = 24008;
		 public const ushort M2A_GMProcessEdit = 24009;
		 public const ushort A2M_GMProcessEdit = 24010;
		 public const ushort M2A_GMAddNewNotice = 24011;
		 public const ushort A2M_GMAddNewNotice = 24012;
		 public const ushort M2A_GMAddNewServerZone = 24013;
		 public const ushort A2M_GMAddNewServerZone = 24014;
		 public const ushort M2A_GMEditServerZone = 24015;
		 public const ushort A2M_GMEditServerZone = 24016;
	}
}
