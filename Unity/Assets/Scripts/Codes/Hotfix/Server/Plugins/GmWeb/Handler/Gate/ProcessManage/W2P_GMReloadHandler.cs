using System;

namespace ET.Server

{
    [MessageHandler(SceneType.Gate)]
    public class W2P_GMReloadHandler: AMRpcHandler<M2A_GMReload, A2M_GMReload>
    {
        protected override async ETTask Run(Session session, M2A_GMReload request, A2M_GMReload response)
        {
            await ReloadDllConsoleHandler.Handle();
        }
    }
}