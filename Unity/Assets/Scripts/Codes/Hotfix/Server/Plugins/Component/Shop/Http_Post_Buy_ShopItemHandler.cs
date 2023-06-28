using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace ET.Server
{
    [HttpHandler(SceneType.Gate, "/Buy_ShopItem")]
    public class Http_Post_Buy_ShopItemHandler: HttpPostHandler<C2H_Buy_ShopItem, H2C_CommonResponse>
    {
        protected override async ETTask Run(Entity domain, C2H_Buy_ShopItem request, H2C_CommonResponse response, long playerid)
        {
            Scene scene = domain.DomainScene();
            PlayerComponent playerComponent = scene.GetComponent<PlayerComponent>();
            Player player = playerComponent.Get(playerid);
            var character = player.GetMyCharacter();
            if (character != null && character.ShopComp != null)
            {
                (response.Error, response.Message) = await character.ShopComp.BuyShopUnit(request);
            }

            await ETTask.CompletedTask;
        }
    }
}