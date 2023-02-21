using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET.Server

{
    [MessageHandler(SceneType.Gate)]
    public class C2G_GMAddNewServerNoticeHandler : AMRpcHandler<C2G_GMAddNewServerNotice, G2C_GMAddNewServerNotice>
    {
        protected override async ETTask Run(Session session, C2G_GMAddNewServerNotice request, G2C_GMAddNewServerNotice response)
        {
                    await ETTask.CompletedTask;
            try
            {
                Player player = session.GetComponent<SessionPlayerComponent>().GetMyPlayer();
                if (!player.HasGmRolePermission(EGmPlayerRole.GmRole_WebActivityEditer))
                {
                    response.Error = ErrorCode.ERR_Error;
                    response.Message = "no Permission";
                    return;
                }
                if (request.NOTICE == null || request.NOTICE.Length == 0)
                {
                    response.Error = ErrorCode.ERR_Error;
                    response.Message = "error NOTICE ";
                    return;
                }

                if ( GameConfig.IsAccountProcess())
                {
                    await ServerZoneManageComponent.Instance.AddNewNotice(request.NOTICE, (long)request.OperateTime * 1000);
                }
                else
                {
                    Session serverSession = NetInnerComponent.Instance.Get(GameConfig.AccountProcessID);
                    var cbmsg = (A2M_GMAddNewNotice)await serverSession.Call(new M2A_GMAddNewNotice()
                    {
                        NOTICE = request.NOTICE,
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
