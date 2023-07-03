namespace ET.Server
{
    public static partial class ServerSceneManagerComponentSystem
    {
        public static async ETTask CloseAllScene(this ServerSceneManagerComponent self)
        {
            self.IsClosing = true;
            await ETTask.CompletedTask;
            var processScenes = StartSceneConfigCategory.Instance.GetByProcess(Options.Instance.Process);
            foreach (StartSceneConfig startConfig in processScenes)
            {
                var scene = self.Get(startConfig.Id);
                if (scene != null)
                {
                    switch (scene.SceneType)
                    {
                        case SceneType.Gate:
                            await self.CloseGateScene(scene);
                            break;
                        default:
                            scene.Dispose();
                            break;
                    }
                }
            }
        }

        public static async ETTask CloseGateScene(this ServerSceneManagerComponent self, Scene gate)
        {
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

            if (ServerZoneManageComponent.Instance != null)
            {
                await ServerZoneManageComponent.Instance.SaveAllChild();
            }

            gate.Dispose();
        }
    }
}