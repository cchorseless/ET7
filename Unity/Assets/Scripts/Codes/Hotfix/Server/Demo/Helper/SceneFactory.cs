using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace ET.Server
{
    public static class SceneFactory
    {
        public static async ETTask<Scene> CreateServerScene(Entity parent, long id, long instanceId, int zone, string name, SceneType sceneType, StartSceneConfig startSceneConfig = null)
        {
            await ETTask.CompletedTask;
            Scene scene = EntitySceneFactory.CreateScene(id, instanceId, zone, sceneType, name, parent);

            scene.AddComponent<MailBoxComponent, MailboxType>(MailboxType.UnOrderMessageDispatcher);

            switch (scene.SceneType)
            {
                case SceneType.Router:
                    scene.AddComponent<RouterComponent, IPEndPoint, string>(startSceneConfig.OuterIPPort,
                        startSceneConfig.StartProcessConfig.InnerIP
                    );
                    break;
                case SceneType.RouterManager: // 正式发布请用CDN代替RouterManager
                    // 云服务器在防火墙那里做端口映射
                    scene.AddComponent<HttpComponent, string>($"http://*:{startSceneConfig.OuterPort}/");
                    break;
                case SceneType.Realm:
                    //scene.AddComponent<NetServerComponent, IPEndPoint>(startSceneConfig.InnerIPOutPort);
                    // if (startSceneConfig.Process == GameConfig.GmWebProcessID)
                    // {
                    //     scene.AddComponent<NetServerComponent, IEnumerable<string>>(new List<string>() { startSceneConfig.WebSocketUrl });
                    // }
                    scene.AddComponent<HttpComponent, string>(startSceneConfig.HttpUrl);
                    scene.AddComponent<ServerSceneCloseComponent, int>((int)SceneType.Realm);
                    break;
                case SceneType.Gate:
                    //scene.AddComponent<NetServerComponent, IPEndPoint>(startSceneConfig.InnerIPOutPort);
                    // if (startSceneConfig.Process == GameConfig.GmWebProcessID)
                    // {
                    //     scene.AddComponent<NetServerComponent, IEnumerable<string>>(new List<string>() { startSceneConfig.WebSocketUrl });
                    // }
                    scene.AddComponent<HttpComponent, string>(startSceneConfig.HttpUrl);
                    scene.AddComponent<PlayerComponent>();
                    scene.AddComponent<GateSessionKeyComponent>();
                    scene.AddComponent<ServerSceneCloseComponent, int>((int)SceneType.Gate);
                    break;
                case SceneType.Map:
                    scene.AddComponent<UnitComponent>();
                    scene.AddComponent<AOIManagerComponent>();
                    break;
                case SceneType.Location:
                    scene.AddComponent<LocationComponent>();
                    break;
                // GM
                case SceneType.Http:
                    scene.AddComponent<HttpComponent, string>(startSceneConfig.HttpUrl);
                    scene.AddComponent<PlayerComponent>();
                    break;
                case SceneType.Robot:
                    scene.AddComponent<RobotManagerComponent>();
                    break;
                case SceneType.BenchmarkServer:
                    scene.AddComponent<BenchmarkServerComponent>();
                    scene.AddComponent<NetServerComponent, IPEndPoint>(startSceneConfig.OuterIPPort);
                    break;
                case SceneType.BenchmarkClient:
                    scene.AddComponent<BenchmarkClientComponent>();
                    break;
            }

            return scene;
        }
    }
}