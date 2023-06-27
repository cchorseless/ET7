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

    [HttpHandler(SceneType.Http, "/GMProcessEdit")]
    public class Http_PostGMProcessEditHandler: HttpPostHandler<C2G_GMProcessEdit, H2C_CommonResponse>
    {
        protected override async ETTask Run(Entity domain, C2G_GMProcessEdit request, H2C_CommonResponse response, long playerid)
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
                        await ReloadDllConsoleHandler.Handle();
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
                        new M2A_GMProcessEdit() { ProcessId = request.ProcessId, OperateType = request.OperateType, });
                    response.Error = cbmsg.Error;
                    response.Message = cbmsg.Message;
                    break;
            }
        }
    }
}