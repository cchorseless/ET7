namespace ET.Server
{
    [HttpHandler(SceneType.Gate, "/LoginOut")]
    public class Http_GetLoginOutHandler : HttpGetHandler<H2C_CommonResponse>
    {
        protected override async ETTask Run(Entity domain, H2C_CommonResponse response, long playerid)
        {
            Scene scene = domain.DomainScene();
            PlayerComponent playerComponent = scene.GetComponent<PlayerComponent>();
            Player player = playerComponent.Get(playerid);
            var LoginOutComp = player.GetComponent<PlayerLoginOutComponent>();
            response.Error = ErrorCode.ERR_Error;
            await LoginOutComp.KnockOutGate();
            response.Error = ErrorCode.ERR_Success;
        }
    }
}
