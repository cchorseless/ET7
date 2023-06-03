using ET;
using ProtoBuf;
using System.Collections.Generic;
namespace ET
{
	[ResponseType(nameof(M2G_AgainLogin))]
	[Message(InnerMessageGate.G2M_AgainLogin)]
	[ProtoContract]
	public partial class G2M_AgainLogin: ProtoObject, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public bool IsKnockOut { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

	}

	[Message(InnerMessageGate.M2G_AgainLogin)]
	[ProtoContract]
	public partial class M2G_AgainLogin: ProtoObject, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[Message(InnerMessageGate.G2M_SessionDisconnect)]
	[ProtoContract]
	public partial class G2M_SessionDisconnect: ProtoObject, IActorLocationMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

	}

	public static class InnerMessageGate
	{
		 public const ushort G2M_AgainLogin = 22001;
		 public const ushort M2G_AgainLogin = 22002;
		 public const ushort G2M_SessionDisconnect = 22003;
	}
}
