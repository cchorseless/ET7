using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace ET.Server
{
    [HttpHandler(SceneType.Gate, "/Recharge_MetaStone")]
    public class Http_Post_Recharge_MetaStoneHandler : HttpPostHandler<C2H_Recharge_MetaStone, H2C_CommonResponse>
    {
        protected override async ETTask Run(Entity domain, C2H_Recharge_MetaStone request, H2C_CommonResponse response, long playerid)
        {
            Scene scene = domain.DomainScene();
            PlayerComponent playerComponent = scene.GetComponent<PlayerComponent>();
            Player player = playerComponent.Get(playerid);
            var character = player.GetMyCharacter();
            var sceneZone = character.GetMyServerZone();
            if (sceneZone != null && sceneZone.RechargeComp != null && character.RechargeComp != null)
            {
                (response.Error, response.Message) = await sceneZone.RechargeComp.RechargeMetaStone(character, request.BuyType ,request.PayType);
            }
            await ETTask.CompletedTask;
        }
    }
}
