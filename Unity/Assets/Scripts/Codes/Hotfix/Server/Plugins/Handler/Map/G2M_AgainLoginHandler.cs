using System;

namespace ET.Server
{
    [ActorMessageHandler(SceneType.Map)]
    public class G2M_AgainLoginHandler : AMActorLocationRpcHandler<Unit, G2M_AgainLogin, M2G_AgainLogin>
    {
        protected override async ETTask Run(Unit unit, G2M_AgainLogin request, M2G_AgainLogin response)
        {
            try
            {
                if (unit != null)
                {
                    if (request.IsKnockOut)
                    {
                        var db = DBManagerComponent.Instance.GetZoneDB(unit.DomainZone());
                        await db.Save(unit);
                        unit.Dispose();
                    }
                    else
                    {
                        // 更新session
                        unit.GetComponent<UnitGateComponent>().GateSessionActorId = request.ActorId;
                    }
                }
                await ETTask.CompletedTask;
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }
    }
}