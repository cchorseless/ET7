using ET;
using ProtoBuf;
using System.Collections.Generic;
namespace ET
{
	[ResponseType(nameof(R2C_Registe))]
	[Message(OuterMessageRealm.C2R_Registe)]
	[ProtoContract]
	public partial class C2R_Registe: ProtoObject, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public string Account { get; set; }

		[ProtoMember(2)]
		public string Password { get; set; }

	}

	[Message(OuterMessageRealm.R2C_Registe)]
	[ProtoContract]
	public partial class R2C_Registe: ProtoObject, IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(R2C_Login))]
	[Message(OuterMessageRealm.C2R_Login)]
	[ProtoContract]
	public partial class C2R_Login: ProtoObject, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public string Account { get; set; }

		[ProtoMember(2)]
		public string Password { get; set; }

		[ProtoMember(3)]
		public int ServerId { get; set; }

		[ProtoMember(4)]
		public int GateId { get; set; }

	}

	[Message(OuterMessageRealm.R2C_Login)]
	[ProtoContract]
	public partial class R2C_Login: ProtoObject, IResponse
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

	public static class OuterMessageRealm
	{
		 public const ushort C2R_Registe = 11001;
		 public const ushort R2C_Registe = 11002;
		 public const ushort C2R_Login = 11003;
		 public const ushort R2C_Login = 11004;
	}
}
