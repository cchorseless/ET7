namespace ET.Server
{
	namespace EventType
	{

		public struct UnitEnterRoom
		{
			public RoomEntity A;
			public RoomEntity B;
		}

		public struct UnitLeaveRoom
		{
			public RoomEntity A;
			public RoomEntity B;
		}
	}
}