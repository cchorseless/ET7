using System;

namespace ET.Server

{
    [ActorMessageHandler(SceneType.Manager)]
    public class G2W_CloseAllProcessHandler: AMActorRpcHandler<Scene, G2W_GMCloseAllProcess, W2G_GMCloseAllProcess>
    {
        protected override async ETTask Run(Scene scene, G2W_GMCloseAllProcess request, W2G_GMCloseAllProcess response)
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

                WatcherHelper.SendManage(process.Key, new W2P_GMShutDown());
            }

            await TimerComponent.Instance.WaitAsync(1000);
            W2P_GMShutDownHandler.CloseScene().Coroutine();
        }
    }
}