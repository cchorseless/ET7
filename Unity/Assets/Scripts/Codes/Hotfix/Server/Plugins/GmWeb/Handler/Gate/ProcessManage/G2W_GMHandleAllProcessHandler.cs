using System;

namespace ET.Server

{
    [ActorMessageHandler(SceneType.Manager)]
    public class G2W_GMHandleAllProcessHandler: AMActorRpcHandler<Scene, G2W_GMHandleAllProcess, W2G_GMHandleAllProcess>
    {
        protected override async ETTask Run(Scene scene, G2W_GMHandleAllProcess request, W2G_GMHandleAllProcess response)
        {
            var watchComp = WatcherComponent.Instance;
            if (watchComp == null)
            {
                response.Error = ErrorCode.ERR_Error;
                response.Message = "this is no watcher";
                return;
            }

            foreach (var process in watchComp.Processes)
            {
                var config = StartProcessConfigCategory.Instance.Get(process.Key);
                if (config.IsWatcher() || config.IsRobot())
                {
                    continue;
                }

                if (request.HandleType == (int)EProcessEdit.Reload)
                {
                    WatcherHelper.SendManage(process.Key, new W2P_GMReload());
                }
                else if (request.HandleType == (int)EProcessEdit.ShutDown)
                {
                    WatcherHelper.SendManage(process.Key, new W2P_GMShutDown());
                }
            }

            await TimerComponent.Instance.WaitAsync(1000);
            if (request.HandleType == (int)EProcessEdit.Reload)
            {
                ReloadDllConsoleHandler.Handle().Coroutine();
            }
            else if (request.HandleType == (int)EProcessEdit.ShutDown)
            {
                W2P_GMShutDownHandler.CloseScene().Coroutine();
            }
        }
    }
}