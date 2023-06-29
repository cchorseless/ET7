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

    [HttpHandler(SceneType.GmWeb, "/GMProcessEdit")]
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

            var config = StartProcessConfigCategory.Instance.Get(request.ProcessId);
            if (config == null)
            {
                response.Error = ErrorCode.ERR_Error;
                response.Message = $"cant find Process {request.ProcessId}";
                return;
            }
            if (config.IsWatcher())
            {
                response.Error = ErrorCode.ERR_Error;
                response.Message = $"cant handle watcher Process {request.ProcessId}";
                return;
            }
            switch (request.OperateType)
            {
                case (int)EProcessEdit.Reload:
                case (int)EProcessEdit.ReStart:
                case (int)EProcessEdit.Start:
                case (int)EProcessEdit.ShutDown:
                    var cbmsg = await WatcherHelper.CallWatcher(new G2W_GMProcessEdit()
                    {
                        ProcessId = request.ProcessId, OperateType = request.OperateType,
                    });
                    response.Error = cbmsg.Error;
                    response.Message = cbmsg.Message;
                    break;
            }
        }
    }
}