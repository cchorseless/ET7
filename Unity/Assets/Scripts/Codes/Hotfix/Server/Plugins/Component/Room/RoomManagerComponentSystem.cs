using System;
using System.Collections.Generic;

namespace ET.Server
{

    [Invoke(TimerInvokeType.RoomCheckEmptyTimeout)]
    public class RoomCheckEmptyTimeout : ATimer<Room>
    {
        protected override void Run(Room self)
        {
            try
            {
                if (self.IsDisposed) { return; }
                if (self.AOIUnits.Count == 0)
                {
                    self.Dispose();
                }
            }
            catch (Exception e)
            {
                Log.Error($"RoomCheckEmptyTimeout timer error: {self.Id}\n{e}");
            }
        }
    }

    [ObjectSystem]
    public class RoomManagerComponentAwakeSystem : AwakeSystem<RoomManagerComponent>
    {
        protected override void Awake(RoomManagerComponent self)
        {
            RoomManagerComponent.Instance = self;
        }
    }
    [FriendOf(typeof(RoomManagerComponent))]
    public static class RoomManagerComponentSystem
    {
        public static void Add(this RoomManagerComponent self, RoomEntity roomEntity, long RoomId)
        {
            // 自己加入的Cell
            Room selfRoom = self.GetRoom(RoomId);
            if (roomEntity.Unit.Type == UnitType.Player)
            {
                // 遍历EnterCell
                roomEntity.SubEnter(selfRoom);
                // 遍历LeaveCell
                roomEntity.SubLeave(selfRoom);
            }

            // 自己加入的Cell
            roomEntity.Room = selfRoom;
            selfRoom.Add(roomEntity);
            // 通知订阅该Room Enter的Unit
            foreach (KeyValuePair<long, RoomEntity> kv in selfRoom.SubsEnterEntities)
            {
                kv.Value.EnterSight(roomEntity);
            }
        }

        public static void Remove(this RoomManagerComponent self, RoomEntity roomEntity)
        {
            Room room = roomEntity.Room;
            if (room == null)
            {
                return;
            }
            // 通知订阅该Room Leave的Unit
            room.Remove(roomEntity);
            foreach (KeyValuePair<long, RoomEntity> kv in room.SubsLeaveEntities)
            {
                kv.Value.LeaveSight(roomEntity);
            }
            // 通知自己订阅的Enter Cell，清理自己
            roomEntity.UnSubEnter(room);
            roomEntity.UnSubLeave(room);

            // 检查
            if (roomEntity.SeeUnits.Count > 1)
            {
                Log.Error($"roomEntity has see units: {roomEntity.SeeUnits.Count}");
            }

            if (roomEntity.BeSeeUnits.Count > 1)
            {
                Log.Error($"roomEntity has beSee units: {roomEntity.BeSeeUnits.Count}");
            }
            roomEntity.Room = null;
            /// 延时删除空房间
            if (room.AOIUnits.Count == 0)
            {
                TimerComponent.Instance.NewOnceTimer(TimeHelper.ServerNow() + 1000, TimerInvokeType.RoomCheckEmptyTimeout, room);
            }
        }

        public static Room GetRoom(this RoomManagerComponent self, long roomId)
        {
            Room room = self.GetChild<Room>(roomId);
            if (room == null)
            {
                room = self.AddChildWithId<Room>(roomId);
                Log.Error("cant find room");
            }
            return room;
        }

        public static Room CreateRoom(this RoomManagerComponent self)
        {
            Room room = self.AddChild<Room>();
            return room;
        }


        public static void Move(this RoomManagerComponent self, RoomEntity roomEntity, long roomID)
        {
            if (roomEntity.Room != null && roomEntity.Room.Id == roomID) // room没有变化
            {
                return;
            }
            self.Remove(roomEntity);
            self.Add(roomEntity, roomID);
        }

        public static void Move(this RoomManagerComponent self, RoomEntity roomEntity, Room newRoom)
        {
            if (roomEntity.Room != null && roomEntity.Room.Id == newRoom.Id) // room没有变化
            {
                return;
            }
            // 自己加入新的Cell
            self.Remove(roomEntity);
            self.Add(roomEntity, newRoom.Id);
        }
    }
}