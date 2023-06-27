using System;


namespace ET.Server

{
    [MessageHandler(SceneType.Gate)]
    public class W2P_GMAddNewServerNoticeHandler : AMRpcHandler<M2A_GMAddNewNotice, A2M_GMAddNewNotice>
    {
        protected override async ETTask Run(Session session, M2A_GMAddNewNotice request, A2M_GMAddNewNotice response)
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
