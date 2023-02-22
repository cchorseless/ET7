namespace ET.Server
{
    [HttpHandler(SceneType.Gate, "/Ping")]
    public class Http_GetPingHandler: HttpGetHandler<G2C_Ping>
    {
        protected override async ETTask Run(Entity domain, G2C_Ping response, long playerid)
        {
            Scene scene = domain.DomainScene();
            PlayerComponent playerComponent = scene.GetComponent<PlayerComponent>();
            Player player = playerComponent.Get(playerid);
            response.Time = TimeHelper.ServerNow();
            var HttpPlayerSession = player.GetComponent<HttpPlayerSessionComponent>();
            HttpPlayerSession.LastRecvTime = response.Time;
            HttpPlayerSession.Response = response;
            await ETTask.CompletedTask;
            if (HttpPlayerSession.SyncString.Count > 0)
            {
                HttpPlayerSession.SendToClient();
                return;
            }
             if (HttpPlayerSession.IsWaiting())
            {
                HttpPlayerSession.CancelWaiting();
            }
            else
            {
                HttpPlayerSession.CancelTimer = new ETCancellationToken();
                await TimerComponent.Instance.WaitAsync(GameConfig.HttpPollingCheckInterval, HttpPlayerSession.CancelTimer);
                HttpPlayerSession.Response = null;
                HttpPlayerSession.CancelTimer = null;
            }
        }
    }
}