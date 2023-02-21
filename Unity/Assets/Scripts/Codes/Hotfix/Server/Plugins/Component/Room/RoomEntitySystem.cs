using System.Collections.Generic;

namespace ET.Server
{
    public static class RoomEntitySystem
    {
        [ObjectSystem]
        public class AwakeSystem : AwakeSystem<RoomEntity, long>
        {
            protected override void Awake(RoomEntity self, long roomId)
            {
                self.Domain.GetComponent<RoomManagerComponent>().Add(self, roomId);
            }
        }

        [ObjectSystem]
        public class DestroySystem : DestroySystem<RoomEntity>
        {
            protected override void Destroy(RoomEntity self)
            {
                self.Domain.GetComponent<RoomManagerComponent>()?.Remove(self);
                self.SeeUnits.Clear();
                self.SeePlayers.Clear();
                self.BeSeePlayers.Clear();
                self.BeSeeUnits.Clear();
                self.Room = null;
            }
        }

        // 获取在自己视野中的对象
        public static Dictionary<long, RoomEntity> GetSeeUnits(this RoomEntity self)
        {
            return self.SeeUnits;
        }

        public static Dictionary<long, RoomEntity> GetBeSeePlayers(this RoomEntity self)
        {
            return self.BeSeePlayers;
        }

        public static Dictionary<long, RoomEntity> GetSeePlayers(this RoomEntity self)
        {
            return self.SeePlayers;
        }

        // cell中的unit进入self的视野
        public static void SubEnter(this RoomEntity self, Room room)
        {
            room.SubsEnterEntities.Add(self.Id, self);
            foreach (KeyValuePair<long, RoomEntity> kv in room.AOIUnits)
            {
                if (kv.Key == self.Id)
                {
                    continue;
                }

                self.EnterSight(kv.Value);
            }
        }

        public static void UnSubEnter(this RoomEntity self, Room room)
        {
            room.SubsEnterEntities.Remove(self.Id);
        }

        public static void SubLeave(this RoomEntity self, Room room)
        {
            room.SubsLeaveEntities.Add(self.Id, self);
        }

        // cell中的unit离开self的视野
        public static void UnSubLeave(this RoomEntity self, Room room)
        {
            foreach (KeyValuePair<long, RoomEntity> kv in room.AOIUnits)
            {
                if (kv.Key == self.Id)
                {
                    continue;
                }

                self.LeaveSight(kv.Value);
            }

            room.SubsLeaveEntities.Remove(self.Id);
        }

        // enter进入self视野
        public static void EnterSight(this RoomEntity self, RoomEntity enter)
        {
            // 有可能之前在Enter，后来出了Enter还在LeaveCell，这样仍然没有删除，继续进来Enter，这种情况不需要处理
            if (self.SeeUnits.ContainsKey(enter.Id))
            {
                return;
            }

            if (!RoomSeeCheckHelper.IsCanSee(self, enter))
            {
                return;
            }

            if (self.Unit.Type == UnitType.Player)
            {
                if (enter.Unit.Type == UnitType.Player)
                {
                    self.SeeUnits.Add(enter.Id, enter);
                    enter.BeSeeUnits.Add(self.Id, self);
                    self.SeePlayers.Add(enter.Id, enter);
                    enter.BeSeePlayers.Add(self.Id, self);

                }
                else
                {
                    self.SeeUnits.Add(enter.Id, enter);
                    enter.BeSeeUnits.Add(self.Id, self);
                    enter.BeSeePlayers.Add(self.Id, self);
                }
            }
            else
            {
                if (enter.Unit.Type == UnitType.Player)
                {
                    self.SeeUnits.Add(enter.Id, enter);
                    enter.BeSeeUnits.Add(self.Id, self);
                    self.SeePlayers.Add(enter.Id, enter);
                }
                else
                {
                    self.SeeUnits.Add(enter.Id, enter);
                    enter.BeSeeUnits.Add(self.Id, self);
                }
            }
            EventSystem.Instance.Publish(self.DomainScene(), new EventType.UnitEnterRoom() { A = self, B = enter });
        }

        // leave离开self视野
        public static void LeaveSight(this RoomEntity self, RoomEntity leave)
        {
            if (self.Id == leave.Id)
            {
                return;
            }

            if (!self.SeeUnits.ContainsKey(leave.Id))
            {
                return;
            }

            self.SeeUnits.Remove(leave.Id);
            if (leave.Unit.Type == UnitType.Player)
            {
                self.SeePlayers.Remove(leave.Id);
            }

            leave.BeSeeUnits.Remove(self.Id);
            if (self.Unit.Type == UnitType.Player)
            {
                leave.BeSeePlayers.Remove(self.Id);
            }

            EventSystem.Instance.Publish(self.DomainScene(), new EventType.UnitLeaveRoom { A = self, B = leave });
        }

        /// <summary>
        /// 是否在Unit视野范围内
        /// </summary>
        /// <param name="self"></param>
        /// <param name="unitId"></param>
        /// <returns></returns>
        public static bool IsBeSee(this RoomEntity self, long unitId)
        {
            return self.BeSeePlayers.ContainsKey(unitId);
        }
    }
}