using System;
using System.Collections.Generic;

namespace ET.Server
{
    [HttpHandler(SceneType.GmWeb, "/GMLoginOut")]
    public class Http_PostGMLoginOutHandler: HttpPostHandler<C2G_GMLoginOut, H2C_CommonResponse>
    {
        protected override async ETTask Run(Entity domain, C2G_GMLoginOut request, H2C_CommonResponse response, long playerid)
        {
            await ETTask.CompletedTask;
            Scene scene = domain.DomainScene();
            PlayerComponent playerComponent = scene.GetComponent<PlayerComponent>();
            Player player = playerComponent.Get(playerid);
            if (player != null)
            {
                var GmCharacterData = player.GetComponent<GmCharacterDataComponent>();
                if (GmCharacterData != null)
                {
                    var accountDB = DBManagerComponent.Instance.GetAccountDB();
                    await accountDB.Save(GmCharacterData);
                }
                LogOut(player).Coroutine();
            }
        }

        private async ETTask LogOut(Player player)
        {
            await TimerComponent.Instance.WaitAsync(1000);
            player.Dispose();
        }
    }
}