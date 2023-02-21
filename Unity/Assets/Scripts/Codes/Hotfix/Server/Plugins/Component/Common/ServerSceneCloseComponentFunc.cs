using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET.Server
{
    [ObjectSystem]
    public class ServerSceneCloseComponentAwakeSystem : AwakeSystem<ServerSceneCloseComponent, int>
    {
        protected override void Awake(ServerSceneCloseComponent self, int sceneType)
        {
            self.SceneType = sceneType;
        }
    }

    [ObjectSystem]
    public class ServerSceneCloseComponentDestroySystem : DestroySystem<ServerSceneCloseComponent>
    {
        protected override void Destroy(ServerSceneCloseComponent self)
        {
        }
    }


    public static class ServerSceneCloseComponentFunc
    {
        public static async ETTask CloseDomainScene(this ServerSceneCloseComponent self)
        {
            self.IsClosing = true;
            await ETTask.CompletedTask;
            switch (self.SceneType)
            {
                case (int)SceneType.Process:
                    await self.CloseProcessScene();
                    return;
                case (int)SceneType.Gate:
                    await self.CloseGateScene();
                    return;
            }
        }
        public static async ETTask CloseProcessScene(this ServerSceneCloseComponent self)
        {
            await ETTask.CompletedTask;
            var processScenes = StartSceneConfigCategory.Instance.GetByProcess(Options.Instance.Process);
            foreach (var processScene in processScenes)
            {
                if (processScene.SceneType == SceneType.Process.ToString())
                {
                    continue;
                }
                var scene = ServerSceneManagerComponent.Instance.Get(processScene.Id);
                if (scene != null)
                {
                    var SceneCloseComp = scene.GetComponent<ServerSceneCloseComponent>();
                    if (SceneCloseComp != null)
                    {
                        await SceneCloseComp.CloseDomainScene();
                    }
                }
            }
        }

        public static async ETTask CloseGateScene(this ServerSceneCloseComponent self)
        {
            await ETTask.CompletedTask;
            Scene gate = self.DomainScene();
            var playerComp = gate.GetComponent<PlayerComponent>();
            var allPlayer = playerComp.GetAll();
            foreach (var player in allPlayer)
            {
                var session = player.GetMySession();
                await player.GetComponent<PlayerLoginOutComponent>().KnockOutGate();
                if (session != null)
                {
                    session.Dispose();
                }
            }
        }
    }
}
