using System;


namespace ET.Server

{
    [ActorMessageHandler(SceneType.Manager)]
    public class G2P_GMAddNewServerNoticeHandler : AMActorRpcHandler<Scene, G2P_GMAddNewNotice, P2G_GMAddNewNotice>
    {
        protected override async ETTask Run(Scene scene, G2P_GMAddNewNotice request, P2G_GMAddNewNotice response)
        {
            if (ServerZoneManageComponent.Instance == null)
            {
                response.Error = ErrorCode.ERR_Error;
                response.Message = "this process no perssion";
            }
            else if (string.IsNullOrEmpty(request.NOTICE))
            {
                response.Error = ErrorCode.ERR_Error;
                response.Message = "NOTICE error";
            }
            else
            {
                await ServerZoneManageComponent.Instance.AddNewNotice(request.NOTICE,
                    (long)request.OperateTime * 1000);
            }
            await ETTask.CompletedTask;

        }
    }
}
