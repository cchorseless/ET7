using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET.Server

{
    [MessageHandler(SceneType.Gate)]
    public class C2G_GMAddNewServerZoneHandler : AMRpcHandler<C2G_GMAddNewServerZone, G2C_GMAddNewServerZone>
    {
        protected override async ETTask Run(Session session, C2G_GMAddNewServerZone request, G2C_GMAddNewServerZone response)
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
                if (request.ServerLabel != 0 && !Enum.IsDefined(typeof(EServerZoneLabel), request.ServerLabel))
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
            catch (Exception e)
            {
                ReplyError(response, e);
            }

        }
    }
}
