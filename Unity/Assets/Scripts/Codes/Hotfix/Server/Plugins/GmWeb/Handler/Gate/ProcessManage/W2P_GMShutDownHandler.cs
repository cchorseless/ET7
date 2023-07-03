using System;

namespace ET.Server

{
    [ActorMessageHandler(SceneType.Manager)]
    public class W2P_GMShutDownHandler: AMActorRpcHandler<Scene, W2P_GMShutDown, P2W_GMShutDown>
    {
        protected override async ETTask Run(Scene scene, W2P_GMShutDown request, P2W_GMShutDown response)
        {
            await ETTask.CompletedTask;
            CloseScene().Coroutine();
        }

        public static async ETTask CloseScene()
        {
            await TimerComponent.Instance.WaitAsync(200);
            // 关闭支付
            if (WeChatPayComponent.Instance != null)
            {
                WeChatPayComponent.Instance.IsWorking = false;
            }

            if (AliPayComponent.Instance != null)
            {
                AliPayComponent.Instance.IsWorking = false;
            }

            var comp = ServerSceneManagerComponent.Instance;
            if (comp != null)
            {
                await comp.CloseAllScene();
            }

            await TimerComponent.Instance.WaitAsync(1000);
            try
            {
                Game.Close();
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
            finally
            {
                Environment.Exit(0);
            }
        }
    }
}