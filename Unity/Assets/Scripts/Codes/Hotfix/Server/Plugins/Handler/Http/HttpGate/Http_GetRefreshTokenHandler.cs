namespace ET.Server
{
    [HttpHandler(SceneType.Gate, "/RefreshToken")]
    public class Http_GetRefreshTokenHandler : HttpGetHandler<G2C_LoginGate>
    {
        protected override async ETTask Run(Entity domain, G2C_LoginGate response, long playerid)
        {
            Scene scene = domain.DomainScene();
            PlayerComponent playerComponent = scene.GetComponent<PlayerComponent>();
            Player player = playerComponent.Get(playerid);
            player.GateSessionActorId = RandomGenerator.RandInt64();
            // 可以直接发协议给客户端
            response.PlayerId = playerid;
            response.Message = scene.GetComponent<HttpComponent>().AuthorizeToken(playerid, player.GateSessionActorId, 24);
            await ETTask.CompletedTask;
        }
    }
}
