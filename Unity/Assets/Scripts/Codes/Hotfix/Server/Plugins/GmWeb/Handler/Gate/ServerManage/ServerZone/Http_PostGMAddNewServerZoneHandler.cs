using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET.Server

{
    [HttpHandler(SceneType.Http, "/GMAddNewServerZone")]
    public class Http_PostGMAddNewServerZoneHandler: HttpPostHandler<C2G_GMAddNewServerZone, H2C_CommonResponse>
    {
        protected override async ETTask Run(Entity domain, C2G_GMAddNewServerZone request, H2C_CommonResponse response, long playerid)
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

            if (request.ServerLabel != 0 && !Enum.IsDefined(typeof (EServerZoneLabel), request.ServerLabel))
            {
                response.Error = ErrorCode.ERR_Error;
                response.Message = "error ServerLabel";
                return;
            }

            if (request.ServerName == null || request.ServerName.Length < 5)
            {
                response.Error = ErrorCode.ERR_Error;
                response.Message = "error ServerName";
                return;
            }

            if (request.ZoneId != 0 && !StartZoneConfigCategory.Instance.Contain(request.ZoneId))
            {
                response.Error = ErrorCode.ERR_Error;
                response.Message = "error ZoneId";
                return;
            }

            if (GameConfig.IsAccountProcess())
            {
                await ServerZoneManageComponent.Instance.AddNewServer(request.ZoneId,
                    request.ServerName,
                    request.ServerLabel,
                    (long)request.OperateTime * 1000);
            }
            else
            {
                Session serverSession = NetInnerComponent.Instance.Get(GameConfig.AccountProcessID);
                var cbmsg = (A2M_GMAddNewServerZone)await serverSession.Call(new M2A_GMAddNewServerZone()
                {
                    ZoneId = request.ZoneId,
                    ServerName = request.ServerName,
                    ServerLabel = request.ServerLabel,
                    OperateTime = request.OperateTime
                });
                response.Error = cbmsg.Error;
                response.Message = cbmsg.Message;
            }
        }
    }
}