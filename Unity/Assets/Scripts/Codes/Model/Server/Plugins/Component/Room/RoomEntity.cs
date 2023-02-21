using System.Collections.Generic;

namespace ET
{
    public class RoomEntity : Entity, IAwake<long>, IDestroy
    {
        public Unit Unit => this.GetParent<Unit>();

        public Room Room { get; set; }

        // 我看的见的Unit
        public Dictionary<long, RoomEntity> SeeUnits { get; set; } = new Dictionary<long, RoomEntity>();

        // 看见我的Unit
        public Dictionary<long, RoomEntity> BeSeeUnits { get; set; } = new Dictionary<long, RoomEntity>();

        // 我看的见的Player
        public Dictionary<long, RoomEntity> SeePlayers { get; set; } = new Dictionary<long, RoomEntity>();

        // 看见我的Player单独放一个Dict，用于广播
        public Dictionary<long, RoomEntity> BeSeePlayers { get; set; } = new Dictionary<long, RoomEntity>();
    }
}