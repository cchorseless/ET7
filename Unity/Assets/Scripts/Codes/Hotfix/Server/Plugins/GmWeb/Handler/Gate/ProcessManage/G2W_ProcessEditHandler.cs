using System;

namespace ET.Server

{
    [ActorMessageHandler(SceneType.Manager)]
    public class G2W_ProcessEditHandler: AMActorRpcHandler<Scene, G2W_GMProcessEdit, W2G_GMProcessEdit>
    {
        protected override async ETTask Run(Scene scene, G2W_GMProcessEdit request, W2G_GMProcessEdit response)
        {
            await ETTask.CompletedTask;
            var watchComp = WatcherComponent.Instance;
            if (watchComp == null)
            {
                response.Error = ErrorCode.ERR_Error;
                response.Message = "this is no watcher";
                return;
            }

            if (!StartProcessConfigCategory.Instance.Contain(request.ProcessId) ||
                StartProcessConfigCategory.Instance.Get(request.ProcessId).MachineId != WatcherHelper.GetThisMachineConfig().Id)

            {
                response.Error = ErrorCode.ERR_Error;
                response.Message = $"cant find Process {request.ProcessId}";
                return;
            }

            await TimerComponent.Instance.WaitAsync(100);
            try
            {
                watchComp.Processes.TryGetValue(request.ProcessId, out var process);
                if (process != null)
                {
                    switch (request.OperateType)
                    {
                        case (int)EProcessEdit.Reload:
                            await WatcherHelper.CallManage(request.ProcessId, new W2P_GMReload());
                            break;
                        case (int)EProcessEdit.ReStart:
                        case (int)EProcessEdit.ShutDown:
                            // todo 保存玩家进度
                            await WatcherHelper.CallManage(request.ProcessId, new W2P_GMShutDown());
                            await TimerComponent.Instance.WaitAsync(5000);
                            watchComp.Processes.Remove(request.ProcessId);
                            Log.Debug($"SHUTDOWN SUCCESS -{request.ProcessId}");
                            break;
                    }

                    if (request.OperateType == (int)EProcessEdit.ReStart)
                    {
                        request.OperateType = (int)EProcessEdit.Start;
                    }
                }

                if (request.OperateType == (int)EProcessEdit.Start && !watchComp.Processes.ContainsKey(request.ProcessId))
                {
                    var newProcess = WatcherHelper.StartProcess(request.ProcessId);
                    watchComp.Processes.Add(request.ProcessId, newProcess);
                    Log.Debug($"START SUCCESS -{request.ProcessId}");
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }
    }
}