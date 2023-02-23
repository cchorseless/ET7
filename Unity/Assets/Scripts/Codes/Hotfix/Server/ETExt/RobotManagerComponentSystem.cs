using System;
using System.Collections.Generic;
using ET.Client;

namespace ET.Server
{
    public static partial class RobotManagerComponentSystem
    {
        [ObjectSystem]
        public class RobotManagerComponentAwakeSystem : AwakeSystem<RobotManagerComponent>
        {
            protected override void Awake(RobotManagerComponent self)
            {
                self.AutoCreateRobot(700).Coroutine();
            }
        }

        public static async ETTask  AutoCreateRobot(this RobotManagerComponent self, int count)
        {
            await TimerComponent.Instance.WaitAsync(2000);
            // 获取当前进程的RobotScene
            using (ListComponent<StartSceneConfig> thisProcessRobotScenes = ListComponent<StartSceneConfig>.Create())
            {
                List<StartSceneConfig> robotSceneConfigs = StartSceneConfigCategory.Instance.Robots;
                foreach (StartSceneConfig robotSceneConfig in robotSceneConfigs)
                {
                    if (robotSceneConfig.Process != Options.Instance.Process)
                    {
                        continue;
                    }

                    thisProcessRobotScenes.Add(robotSceneConfig);
                }

                // 创建机器人
                for (int i = 0; i < count; i++)
                {
                    int index = i % thisProcessRobotScenes.Count;
                    StartSceneConfig robotSceneConfig = thisProcessRobotScenes[index];
                    Scene robotScene = ServerSceneManagerComponent.Instance.Get(robotSceneConfig.Id);
                    RobotManagerComponent robotManagerComponent = robotScene.GetComponent<RobotManagerComponent>();
                    Scene robot = await robotManagerComponent.NewHttpRobot(Options.Instance.Process * 10000 + i);
                    // robot.AddComponent<AIComponent, int>(1);
                    Log.Console($"create robot {robot.Zone}");
                    await TimerComponent.Instance.WaitAsync(200);
                }
            }
        }
        
        // 创建机器人，生命周期是RobotCase
        public static async ETTask<Scene> NewHttpRobot(this RobotManagerComponent self, int zone)
        {
            Scene clientScene = null;
            try
            {
                clientScene = await Client.SceneFactory.CreateClientScene(zone, zone.ToString());
                var httpcomp = clientScene.AddComponent<HttpDotaComponent>();
                await httpcomp.LoginHttp("Robot_" + zone);
                return clientScene;
            }
            catch (Exception e)
            {
                clientScene?.Dispose();
                throw new Exception($"RobotCase create robot fail, zone: {zone}", e);
            }
        }
    }
}