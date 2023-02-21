using System.Collections.Generic;

namespace ET
{
    public class Room: Entity, IAwake, IDestroy
    {
        // 处在这个Room的单位
        public Dictionary<long, RoomEntity> AOIUnits { get; set; } = new Dictionary<long, RoomEntity>();

        // 订阅了这个Room的进入事件
        public Dictionary<long, RoomEntity> SubsEnterEntities { get; set; } = new Dictionary<long, RoomEntity>();

        // 订阅了这个Room的退出事件
        public Dictionary<long, RoomEntity> SubsLeaveEntities { get; set; } = new Dictionary<long, RoomEntity>();
    }
}