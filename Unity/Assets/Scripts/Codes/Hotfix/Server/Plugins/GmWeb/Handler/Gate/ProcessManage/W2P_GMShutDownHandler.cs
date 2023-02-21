using System;


namespace ET.Server

{
    [MessageHandler(SceneType.Gate)]
    public class W2P_GMShutDownHandler : AMRpcHandler<W2G_GMShutDown, G2W_GMShutDown>
    {
        protected override async ETTask Run(Session session, W2G_GMShutDown request, G2W_GMShutDown response)
        {
            await Root.Instance.Scene.GetComponent<ServerSceneCloseComponent>().CloseDomainScene();
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
