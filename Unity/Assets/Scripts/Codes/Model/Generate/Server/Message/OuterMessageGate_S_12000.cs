using ET;
using ProtoBuf;
using System.Collections.Generic;
namespace ET
{
	[Message(OuterMessageGate.G2C_Test)]
	[ProtoContract]
	public partial class G2C_Test: ProtoObject, IMessage
	{
	}

	[Message(OuterMessageGate.G2C_TestHotfixMessage)]
	[ProtoContract]
	public partial class G2C_TestHotfixMessage: ProtoObject, IMessage
	{
		[ProtoMember(1)]
		public string Info { get; set; }

	}

	[ResponseType(nameof(G2C_LoginGate))]
	[Message(OuterMessageGate.C2G_LoginGate)]
	[ProtoContract]
	public partial class C2G_LoginGate: ProtoObject, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public long Key { get; set; }

		[ProtoMember(2)]
		public long GateId { get; set; }

		[ProtoMember(3)]
		public long UserId { get; set; }

		[ProtoMember(4)]
		public int ServerId { get; set; }

	}

	[Message(OuterMessageGate.G2C_LoginGate)]
	[ProtoContract]
	public partial class G2C_LoginGate: ProtoObject, IResponse
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

	[ResponseType(nameof(G2C_GetCharacter))]
	[Message(OuterMessageGate.C2G_GetCharacter)]
	[ProtoContract]
	public partial class C2G_GetCharacter: ProtoObject, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

	}

	[Message(OuterMessageGate.G2C_GetCharacter)]
	[ProtoContract]
	public partial class G2C_GetCharacter: ProtoObject, IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public List<NCharacterInfo> Characters { get; set; }

		[ProtoMember(2)]
		public int Index { get; set; }

	}

	[Message(OuterMessageGate.NCharacterInfo)]
	[ProtoContract]
	public partial class NCharacterInfo: ProtoObject, IMessage
	{
		[ProtoMember(1)]
		public string Name { get; set; }

		[ProtoMember(2)]
		public int Gender { get; set; }

		[ProtoMember(3)]
		public int CharacterClass { get; set; }

		[ProtoMember(4)]
		public long Gold { get; set; }

		[ProtoMember(5)]
		public int Gem { get; set; }

		[ProtoMember(6)]
		public int Level { get; set; }

	}

	[ResponseType(nameof(G2C_CharacterCreate))]
	[Message(OuterMessageGate.C2G_CharacterCreate)]
	[ProtoContract]
	public partial class C2G_CharacterCreate: ProtoObject, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public int Index { get; set; }

		[ProtoMember(2)]
		public string CharacterName { get; set; }

		[ProtoMember(3)]
		public int CharacterClass { get; set; }

		[ProtoMember(4)]
		public int Gender { get; set; }

	}

	[Message(OuterMessageGate.G2C_CharacterCreate)]
	[ProtoContract]
	public partial class G2C_CharacterCreate: ProtoObject, IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public int Index { get; set; }

		[ProtoMember(2)]
		public NCharacterInfo Characters { get; set; }

	}

	[ResponseType(nameof(G2C_EnterMap))]
	[Message(OuterMessageGate.C2G_EnterMap)]
	[ProtoContract]
	public partial class C2G_EnterMap: ProtoObject, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public int Index { get; set; }

	}

	[Message(OuterMessageGate.G2C_EnterMap)]
	[ProtoContract]
	public partial class G2C_EnterMap: ProtoObject, IResponse
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

	[ResponseType(nameof(G2C_EnterGame))]
	[Message(OuterMessageGate.C2G_EnterGame)]
	[ProtoContract]
	public partial class C2G_EnterGame: ProtoObject, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public int Index { get; set; }

	}

	[Message(OuterMessageGate.G2C_EnterGame)]
	[ProtoContract]
	public partial class G2C_EnterGame: ProtoObject, IResponse
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

	[ResponseType(nameof(G2C_CreateRoom))]
	[Message(OuterMessageGate.C2G_CreateRoom)]
	[ProtoContract]
	public partial class C2G_CreateRoom: ProtoObject, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public int RoomType { get; set; }

	}

	[Message(OuterMessageGate.G2C_CreateRoom)]
	[ProtoContract]
	public partial class G2C_CreateRoom: ProtoObject, IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public long RoomId { get; set; }

	}

	[ResponseType(nameof(G2C_EnterRoom))]
	[Message(OuterMessageGate.C2G_EnterRoom)]
	[ProtoContract]
	public partial class C2G_EnterRoom: ProtoObject, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public long RoomId { get; set; }

	}

	[Message(OuterMessageGate.G2C_EnterRoom)]
	[ProtoContract]
	public partial class G2C_EnterRoom: ProtoObject, IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public long RoomId { get; set; }

	}

	[ResponseType(nameof(G2C_LeaveRoom))]
	[Message(OuterMessageGate.C2G_LeaveRoom)]
	[ProtoContract]
	public partial class C2G_LeaveRoom: ProtoObject, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public long RoomId { get; set; }

	}

	[Message(OuterMessageGate.G2C_LeaveRoom)]
	[ProtoContract]
	public partial class G2C_LeaveRoom: ProtoObject, IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[Message(OuterMessageGate.G2C_KnockOutClient)]
	[ProtoContract]
	public partial class G2C_KnockOutClient: ProtoObject, IMessage
	{
		[ProtoMember(1)]
		public string Info { get; set; }

	}

	[ResponseType(nameof(G2C_ReLoginGate))]
	[Message(OuterMessageGate.C2G_ReLoginGate)]
	[ProtoContract]
	public partial class C2G_ReLoginGate: ProtoObject, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public long Key { get; set; }

		[ProtoMember(2)]
		public long GateId { get; set; }

		[ProtoMember(3)]
		public long UserId { get; set; }

		[ProtoMember(4)]
		public int ServerId { get; set; }

		[ProtoMember(5)]
		public string Account { get; set; }

		[ProtoMember(6)]
		public string Password { get; set; }

	}

	[Message(OuterMessageGate.G2C_ReLoginGate)]
	[ProtoContract]
	public partial class G2C_ReLoginGate: ProtoObject, IResponse
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

	[ResponseType(nameof(G2C_EasyLoginGate))]
	[Message(OuterMessageGate.C2G_EasyLoginGate)]
	[ProtoContract]
	public partial class C2G_EasyLoginGate: ProtoObject, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public string Account { get; set; }

		[ProtoMember(2)]
		public string Password { get; set; }

	}

	[Message(OuterMessageGate.G2C_EasyLoginGate)]
	[ProtoContract]
	public partial class G2C_EasyLoginGate: ProtoObject, IResponse
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

	public static class OuterMessageGate
	{
		 public const ushort G2C_Test = 12001;
		 public const ushort G2C_TestHotfixMessage = 12002;
		 public const ushort C2G_LoginGate = 12003;
		 public const ushort G2C_LoginGate = 12004;
		 public const ushort C2G_GetCharacter = 12005;
		 public const ushort G2C_GetCharacter = 12006;
		 public const ushort NCharacterInfo = 12007;
		 public const ushort C2G_CharacterCreate = 12008;
		 public const ushort G2C_CharacterCreate = 12009;
		 public const ushort C2G_EnterMap = 12010;
		 public const ushort G2C_EnterMap = 12011;
		 public const ushort C2G_EnterGame = 12012;
		 public const ushort G2C_EnterGame = 12013;
		 public const ushort C2G_CreateRoom = 12014;
		 public const ushort G2C_CreateRoom = 12015;
		 public const ushort C2G_EnterRoom = 12016;
		 public const ushort G2C_EnterRoom = 12017;
		 public const ushort C2G_LeaveRoom = 12018;
		 public const ushort G2C_LeaveRoom = 12019;
		 public const ushort G2C_KnockOutClient = 12020;
		 public const ushort C2G_ReLoginGate = 12021;
		 public const ushort G2C_ReLoginGate = 12022;
		 public const ushort C2G_EasyLoginGate = 12023;
		 public const ushort G2C_EasyLoginGate = 12024;
	}
}
