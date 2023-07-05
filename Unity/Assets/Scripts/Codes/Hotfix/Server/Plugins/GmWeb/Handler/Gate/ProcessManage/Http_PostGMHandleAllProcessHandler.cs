using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ET.Server

{

    [HttpHandler(SceneType.GmWeb, "/GMHandleAllProcess")]
    public class Http_PostGMHandleAllProcessHandler: HttpPostHandler<C2G_GMHandleAllProcess, H2C_CommonResponse>
    {
        protected override async ETTask Run(Entity domain, C2G_GMHandleAllProcess request, H2C_CommonResponse response, long playerid)
        {
            await ETTask.CompletedTask;
            Scene scene = domain.DomainScene();
            PlayerComponent playerComponent = scene.GetComponent<PlayerComponent>();
            Player player = playerComponent.Get(playerid);
            if (!player.HasGmRolePermission(EGmPlayerRole.GmRole_Admin))
            {
                response.Error = ErrorCode.ERR_Error;
                response.Message = "no Permission";
                return;
            }
          
            WatcherHelper.SendWatcher(new G2W_GMHandleAllProcess()
            {
                HandleType = request.HandleType
            });
            long TimeSpan = (long)request.TimeSpan * 1000;
            if (TimeHelper.ServerNow() >= TimeSpan)
            {
      
            }
            else
            {
             // todo 延时   
            }
        }
    }
}