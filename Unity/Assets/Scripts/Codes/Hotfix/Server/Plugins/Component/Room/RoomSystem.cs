using System.Collections.Generic;
using System.Text;

namespace ET.Server
{
    [ObjectSystem]
    public class RoomDestroySystem : DestroySystem<Room>
    {
        protected override void Destroy(Room self)
        {
            self.AOIUnits.Clear();

            self.SubsEnterEntities.Clear();

            self.SubsLeaveEntities.Clear();
        }
    }

    public static class RoomSystem
    {
        public static void Add(this Room self, RoomEntity roomEntity)
        {
            self.AOIUnits.Add(roomEntity.Id, roomEntity);
        }

        public static void Remove(this Room self, RoomEntity roomEntity)
        {
            self.AOIUnits.Remove(roomEntity.Id);
        }
    }
}