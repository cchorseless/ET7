using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET.Server

{
    [MessageHandler(SceneType.Gate)]
    public class W2P_GMEditServerZoneHandler : AMRpcHandler<M2A_GMEditServerZone, A2M_GMEditServerZone>
    {
        protected override async ETTask Run(Session session, M2A_GMEditServerZone request, A2M_GMEditServerZone response)
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
