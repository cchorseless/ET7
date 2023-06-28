using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET.Server

{

    [ActorMessageHandler(SceneType.Manager)]
    public class G2P_GMAddNewServerZoneHandler : AMActorRpcHandler<Scene,G2P_GMAddNewServerZone, P2G_GMAddNewServerZone>
    {
        protected override async ETTask Run(Scene scene, G2P_GMAddNewServerZone request, P2G_GMAddNewServerZone response)
        {
            if (ServerZoneManageComponent.Instance == null)
            {
                response.Error = ErrorCode.ERR_Error;
                response.Message = "this process no perssion";
            }
            else if (request.ServerName == null || request.ServerName.Length < 5)
            {
                response.Error = ErrorCode.ERR_Error;
                response.Message = "ServerName error";
            }
            else if (request.ServerLabel != 0 && !Enum.IsDefined(typeof(EServerZoneLabel), request.ServerLabel))
            {
                response.Error = ErrorCode.ERR_Error;
                response.Message = "error ServerLabel";
            }

            else if (request.ZoneId != 0 && !StartZoneConfigCategory.Instance.Contain(request.ZoneId))
            {
                response.Error = ErrorCode.ERR_Error;
                response.Message = "error ZoneId";
            }
            else
            {
                await ServerZoneManageComponent.Instance.AddNewServer(request.ZoneId,
                    request.ServerName, request.ServerLabel, (long)request.OperateTime * 1000);
            }
            await ETTask.CompletedTask;

        }
    }
}
