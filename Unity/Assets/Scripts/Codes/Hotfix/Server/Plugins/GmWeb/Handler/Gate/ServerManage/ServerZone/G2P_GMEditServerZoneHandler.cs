using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET.Server

{
    [ActorMessageHandler(SceneType.Manager)]
    public class G2P_GMEditServerZoneHandler : AMActorRpcHandler<Scene,G2P_GMEditServerZone, P2G_GMEditServerZone>
    {
        protected override async ETTask Run(Scene scene, G2P_GMEditServerZone request, P2G_GMEditServerZone response)
        {
            if (ServerZoneManageComponent.Instance == null)
            {
                response.Error = ErrorCode.ERR_Error;
                response.Message = "this process no perssion";
            }
            switch (request.OperateType)
            {
                case 0:
                    response.Error = await ServerZoneManageComponent.Instance.EditServer(request.ServerId,
       request.ServerName, request.ServerLabel, request.State);
                    break;
                case 1:
                case 2:
                    response.Error = await ServerZoneManageComponent.Instance.ManageServer(request.ServerId,
       request.OperateType, (long)request.OperateTime * 1000);
                    break;
            }
            await ETTask.CompletedTask;

        }
    }
}
