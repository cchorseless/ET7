using System;

namespace ET.Server

{
    [ActorMessageHandler(SceneType.Manager)]
    public class W2P_GMReloadHandler: AMActorRpcHandler<Scene,W2P_GMReload, P2W_GMReload>
    {
        protected override async ETTask Run(Scene scene, W2P_GMReload request, P2W_GMReload response)
        {
            await ReloadDllConsoleHandler.Handle();
        }
    }
}