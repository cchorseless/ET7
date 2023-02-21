namespace ET.Server
{
    // 进入房间通知
    [Event(SceneType.Gate)]
    public class UnitEnterRoom_NotifyClient : AEvent<EventType.UnitEnterRoom>
    {
        protected override async ETTask Run(Scene scene, EventType.UnitEnterRoom args)
        {
            await ETTask.CompletedTask;
            RoomEntity a = args.A;
            RoomEntity b = args.B;
            if (a.Id == b.Id)
            {
                return;
            }

            Unit ua = a.GetParent<Unit>();
            if (ua.Type != UnitType.Player)
            {
                return;
            }

            Unit ub = b.GetParent<Unit>();

            MessageHelper.NoticeUnitAdd(ua, ub);
        }
    }
}