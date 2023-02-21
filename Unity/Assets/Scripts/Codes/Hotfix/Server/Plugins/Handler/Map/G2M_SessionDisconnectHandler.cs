

namespace ET.Server
{
    [ActorMessageHandler(SceneType.Map)]
    public class G2M_SessionDisconnectHandler : AMActorLocationHandler<Unit, G2M_SessionDisconnect>
    {
        protected override async ETTask Run(Unit unit, G2M_SessionDisconnect message)
        {
            Scene scene = unit.DomainScene();
            var db = DBManagerComponent.Instance.GetZoneDB(scene.Zone);
            await db.Save(unit);
            UnitComponent unitComponent = scene.GetComponent<UnitComponent>();
            Log.Debug("unit dispose " + unit.Id.ToString());
            unitComponent.Remove(unit.Id);
            await ETTask.CompletedTask;

        }
    }
}