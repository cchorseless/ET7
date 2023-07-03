using System;
using System.Collections;
using System.Diagnostics;

namespace ET.Server
{
    public static class WatcherHelper
    {
        public static StartMachineConfig GetThisMachineConfig()
        {
            var processConfig = StartProcessConfigCategory.Instance.Get(Options.Instance.Process);
            return StartMachineConfigCategory.Instance.Get(processConfig.MachineId);
        }

        public static StartProcessConfig GetThisWatcherConfig()
        {
            var processConfig = StartProcessConfigCategory.Instance.Get(Options.Instance.Process);
            var all = StartProcessConfigCategory.Instance.GetAll();
            foreach (var kv in all.Values)
            {
                if (kv.MachineId == processConfig.MachineId && kv.IsWatcher())
                {
                    return kv;
                }
            }

            return null;
        }

        public static void SendWatcher(IActorRequest request)
        {
            var actorid = StartSceneConfigCategory.Instance.GetManageActorId(GetThisWatcherConfig().Id);
            ActorMessageSenderComponent.Instance.Send(actorid, request);
        }

        public static async ETTask<IActorResponse> CallWatcher(IActorRequest request)
        {
            var actorid = StartSceneConfigCategory.Instance.GetManageActorId(GetThisWatcherConfig().Id);
            return await ActorMessageSenderComponent.Instance.Call(actorid, request);
        }

        public static async ETTask<IActorResponse> CallManage(int processid, IActorRequest request)
        {
            var actorid = StartSceneConfigCategory.Instance.GetManageActorId(processid);
            return await ActorMessageSenderComponent.Instance.Call(actorid, request);
        }

        public static void SendManage(int processid, IActorRequest request)
        {
            var actorid = StartSceneConfigCategory.Instance.GetManageActorId(processid);
            ActorMessageSenderComponent.Instance.Send(actorid, request);
        }

        public static Session GetThisMachineWatcherSession()
        {
            return NetInnerComponent.Instance.Get(GetThisWatcherConfig().Id);
        }

        public static Process StartProcess(int processId, int createScenes = 0)
        {
            StartProcessConfig startProcessConfig = StartProcessConfigCategory.Instance.Get(processId);
            const string exe = "dotnet";
            string arguments = $"App.dll" +
                    $" --Process={startProcessConfig.Id}" +
                    $" --AppType=Server" +
                    $" --StartConfig={Options.Instance.StartConfig}" +
                    $" --Develop={Options.Instance.Develop}" +
                    $" --CreateScenes={createScenes}" +
                    $" --LogLevel={Options.Instance.LogLevel}" +
                    $" --Console={Options.Instance.Console}";
            Log.Debug($"{exe} {arguments}");
            Process process = ProcessHelper.Run(exe, arguments);
            return process;
        }
    }
}