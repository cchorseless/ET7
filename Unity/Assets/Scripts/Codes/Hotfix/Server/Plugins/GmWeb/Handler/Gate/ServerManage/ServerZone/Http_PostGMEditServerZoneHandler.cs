using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET.Server

{
    [HttpHandler(SceneType.Http, "/GMEditServerZone")]
    public class Http_PostGMEditServerZoneHandler: HttpPostHandler<C2G_GMEditServerZone, H2C_CommonResponse>
    {
        protected override async ETTask Run(Entity domain, C2G_GMEditServerZone request, H2C_CommonResponse response, long playerid)

        {
            Scene scene = domain.DomainScene();
            PlayerComponent playerComponent = scene.GetComponent<PlayerComponent>();
            Player player = playerComponent.Get(playerid);

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
    }
}