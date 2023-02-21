using ET;
using ProtoBuf;
using System.Collections.Generic;
namespace ET
{
	[ResponseType(nameof(G2R_GetLoginKey))]
	[Message(InnerMessageRealm.R2G_GetLoginKey)]
	[ProtoContract]
	public partial class R2G_GetLoginKey: ProtoObject, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public string Account { get; set; }

	}

	[Message(InnerMessageRealm.G2R_GetLoginKey)]
	[ProtoContract]
	public partial class G2R_GetLoginKey: ProtoObject, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public long Key { get; set; }

		[ProtoMember(2)]
		public long GateId { get; set; }

	}

	[ResponseType(nameof(G2R_CheckHasLogined))]
	[Message(InnerMessageRealm.R2G_CheckHasLogined)]
	[ProtoContract]
	public partial class R2G_CheckHasLogined: ProtoObject, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long PlayerId { get; set; }

		[ProtoMember(94)]
		public bool IsKnockOut { get; set; }

	}

	[Message(InnerMessageRealm.G2R_CheckHasLogined)]
	[ProtoContract]
	public partial class G2R_CheckHasLogined: ProtoObject, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(93)]
		public long GateId { get; set; }

	}

	public static class InnerMessageRealm
	{
		 public const ushort R2G_GetLoginKey = 21001;
		 public const ushort G2R_GetLoginKey = 21002;
		 public const ushort R2G_CheckHasLogined = 21003;
		 public const ushort G2R_CheckHasLogined = 21004;
	}
}
