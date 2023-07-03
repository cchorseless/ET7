using System.Net;

namespace ET.Server
{
    [Event(SceneType.Process)]
    public class EntryEvent2_InitServer: AEvent<ET.EventType.EntryEvent2>
    {
        protected override async ETTask Run(Scene scene, ET.EventType.EntryEvent2 args)
        {
            Log.Console($"server start........................ {Options.Instance.Process}");

            Root.Instance.Scene.AddComponent<LuBanConfigComponent>();
            await LuBanConfigComponent.Instance.LoadAsync();
            // 发送普通actor消息
            Root.Instance.Scene.AddComponent<ActorMessageSenderComponent>();
            // 发送location actor消息
            Root.Instance.Scene.AddComponent<ActorLocationSenderComponent>();
            // 访问location server的组件
            Root.Instance.Scene.AddComponent<LocationProxyComponent>();
            Root.Instance.Scene.AddComponent<ActorMessageDispatcherComponent>();
            Root.Instance.Scene.AddComponent<ServerSceneManagerComponent>();
            Root.Instance.Scene.AddComponent<RobotCaseComponent>();
            // Root.Instance.Scene.AddComponent<NavmeshComponent>();
            // 数据库管理
            Root.Instance.Scene.AddComponent<DBManagerComponent>();
            // 日志输出到服务器
            Root.Instance.Scene.AddComponent<DBLogManagerComponent>();
            // redis
            Root.Instance.Scene.AddComponent<RedisManagerComponent>();
            // 监视文件改变
            Root.Instance.Scene.AddComponent<WatcherFileComponent, string>("../Watcher/");
            // 有状态计时器
            Root.Instance.Scene.AddComponent<StatefulTimerComponent>();
            // 支付
            Root.Instance.Scene.AddComponent<AliPayComponent>();
            Root.Instance.Scene.AddComponent<WeChatPayComponent>();
            // // 直接發ActorMessage給進程
            // Root.Instance.Scene.AddComponent<MailBoxComponent, MailboxType>(MailboxType.UnOrderMessageDispatcher);
            var processConfig = StartProcessConfigCategory.Instance.Get(Options.Instance.Process);
            var processScenes = StartSceneConfigCategory.Instance.GetByProcess(Options.Instance.Process);
            switch (Options.Instance.AppType)
            {
                case AppType.Server:
                {
                    // 进程同步实体组件
                    Root.Instance.Scene.AddComponent<ProcessGhostSyncComponent>();
                    // 数据库实体临时父节点组件
                    Root.Instance.Scene.AddComponent<DBTempSceneComponent>();
                    // 区服管理
                    await Root.Instance.Scene.AddComponent<ServerZoneManageComponent>().LoadAllChild();
                    Root.Instance.Scene.AddComponent<NetInnerComponent, IPEndPoint>(processConfig.InnerIPPort);
                    foreach (StartSceneConfig startConfig in processScenes)
                    {
                        await SceneFactory.CreateServerScene(ServerSceneManagerComponent.Instance, startConfig.Id, startConfig.InstanceId,
                            startConfig.Zone, startConfig.Name,
                            startConfig.Type, startConfig);
                    }

                    break;
                }
                case AppType.Watcher:
                {
                    Root.Instance.Scene.AddComponent<NetInnerComponent, IPEndPoint>(processConfig.InnerIPPort);
                    foreach (StartSceneConfig startConfig in processScenes)
                    {
                        await SceneFactory.CreateServerScene(ServerSceneManagerComponent.Instance, startConfig.Id, startConfig.InstanceId,
                            startConfig.Zone, startConfig.Name,
                            startConfig.Type, startConfig);
                    }
                    WatcherComponent watcherComponent = Root.Instance.Scene.AddComponent<WatcherComponent>();
                    watcherComponent.Start(Options.Instance.CreateScenes);
                    break;
                }
                case AppType.GameTool:
                    break;
            }

            if (Options.Instance.Console == 1)
            {
                Root.Instance.Scene.AddComponent<ConsoleComponent>();
            }
        }
    }
}