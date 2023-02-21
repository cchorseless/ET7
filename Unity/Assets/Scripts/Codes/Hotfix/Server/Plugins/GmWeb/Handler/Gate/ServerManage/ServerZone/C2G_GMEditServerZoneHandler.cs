using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET.Server

{

    [MessageHandler(SceneType.Gate)]
    public class C2G_GMEditServerZoneHandler : AMRpcHandler<C2G_GMEditServerZone, G2C_GMEditServerZone>
    {
        protected override async ETTask Run(Session session, C2G_GMEditServerZone request, G2C_GMEditServerZone response)
        {
            await ETTask.CompletedTask;
            try
            {
                Player player = session.GetComponent<SessionPlayerComponent>().GetMyPlayer();
                if (!player.HasGmRolePermission(EGmPlayerRole.GmRole_WebServerManager))
                {
                    response.Error = ErrorCode.ERR_Error;
                    response.Message = "no Permission";
                    return;
                }
                if (request.ServerId == 0)
                {
                    response.Error = ErrorCode.ERR_Error;
                    response.Message = "error ServerId";
                    return;
                }
                var accontDB = DBManagerComponent.Instance.GetAccountDB();
                var server = await accontDB.Query<TServerZone>(request.ServerId);
                if (server == null)
                {
                    response.Error = ErrorCode.ERR_Error;
                    response.Message = "error server zone";
                    return;
                }
                // 修改
                if (request.OperateType == 0)
                {
                    if (request.ServerName != null && request.ServerName.Length < 5)
                    {
                        response.Error = ErrorCode.ERR_Error;
                        response.Message = "error ServerName";
                        return;
                    }
                }
                // 开启 
                else if (request.OperateType == 1)
                {
                    if (!server.State.Contains((int)EServerZoneState.Closing))
                    {
                        response.Error = ErrorCode.ERR_Error;
                        response.Message = "error OperateType";
                        return;
                    }
                }
                // 关闭
                else if (request.OperateType == 2)
                {
                    if (!server.State.Contains((int)EServerZoneState.Working))
                    {
                        response.Error = ErrorCode.ERR_Error;
                        response.Message = "error OperateType";
                        return;
                    }

                }
                if (GameConfig.IsAccountProcess())
                {

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
                }
                else
                {
                    Session serverSession = NetInnerComponent.Instance.Get(GameConfig.AccountProcessID);
                    var cbmsg = (A2M_GMEditServerZone)await serverSession.Call(new M2A_GMEditServerZone()
                    {
                        State = request.State,
                        ServerName = request.ServerName,
                        ServerLabel = request.ServerLabel,
                        OperateTime = request.OperateTime,
                        OperateType = request.OperateType,
                        ServerId = request.ServerId,
                    });
                }

            }
            catch (Exception e)
            {
                ReplyError(response, e);
            }

        }
    }
}
