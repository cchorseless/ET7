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

        public async ETTask CloseScene()
        {
            await TimerComponent.Instance.WaitAsync(200);
            var comp = Root.Instance.Scene.GetComponent<ServerSceneCloseComponent>();
            if (comp != null)
            {
                await comp.CloseDomainScene();
            }
            await TimerComponent.Instance.WaitAsync(2000);
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