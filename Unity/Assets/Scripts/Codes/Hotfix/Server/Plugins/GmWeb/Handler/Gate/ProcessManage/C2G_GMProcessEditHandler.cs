using System;


namespace ET.Server

{

    enum EProcessEdit
    {
        Reload = 0,
        ReStart = 1,
        Start = 2,
        ShutDown = 3
    }


    [MessageHandler(SceneType.Gate)]
    public class C2G_GMProcessEditHandler : AMRpcHandler<C2G_GMProcessEdit, G2C_GMProcessEdit>
    {
        protected override async ETTask Run(Session session, C2G_GMProcessEdit request, G2C_GMProcessEdit response)
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

                if (!StartProcessConfigCategory.Instance.Contain(request.ProcessId))
                {
                    response.Error = ErrorCode.ERR_Error;
                    response.Message = $"cant find Process {request.ProcessId}";
                    return;
                }

                switch (request.OperateType)
                {
                    case (int)EProcessEdit.Reload:
                        if (request.ProcessId == Options.Instance.Process)
                        {
                            await LuBanConfigComponent.Instance.Reload();
                            CodeLoader.Instance.LoadHotfix();
                            EventSystem.Instance.Load();
                            Log.Debug($"RELOAD SUCCESS {Options.Instance.AppType}-{Options.Instance.Process}");
                        }
                        else
                        {
                            Session serverSession = NetInnerComponent.Instance.Get(request.ProcessId);
                            await serverSession.Call(new M2A_GMReload());
                        }
                        break;
                    case (int)EProcessEdit.ReStart:
                    case (int)EProcessEdit.Start:
                    case (int)EProcessEdit.ShutDown:
                        var cbmsg = await WatcherSessionComponent.Instance.GetThisMachineWatcherSession().Call(
                               new M2A_GMProcessEdit()
                               {
                                   ProcessId = request.ProcessId,
                                   OperateType = request.OperateType,
                               });
                        response.Error = cbmsg.Error;
                        response.Message = cbmsg.Message;
                        break;
                }
            }
            catch (Exception e)
            {
                ReplyError(response, e);
            }

        }
    }
}