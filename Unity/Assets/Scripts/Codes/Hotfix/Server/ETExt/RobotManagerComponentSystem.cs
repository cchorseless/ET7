using System;
using System.Collections.Generic;
using ET.Client;

namespace ET.Server
{
    public static partial class RobotManagerComponentSystem
    {
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