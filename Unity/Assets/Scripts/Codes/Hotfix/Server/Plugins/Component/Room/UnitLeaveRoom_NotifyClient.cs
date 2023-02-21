namespace ET.Server
{
    // 离开房间通知
    [Event(SceneType.Gate)]
    public class UnitLeaveRoom_NotifyClient : AEvent<EventType.UnitLeaveRoom>
    {
        protected override async ETTask Run(Scene scene, EventType.UnitLeaveRoom args)
        {
            await ETTask.CompletedTask;
            RoomEntity a = args.A;
            RoomEntity b = args.B;
            if (a.Unit.Type != UnitType.Player)
            {
                return;
            }

            MessageHelper.NoticeUnitRemove(a.GetParent<Unit>(), b.GetParent<Unit>());
        }
    }
}