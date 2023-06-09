using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace ET.Server
{
    [HttpHandler(SceneType.Gate, "/BattlePass_ChargePrize")]
    public class Http_Post_BattlePass_ChargePrizeHandler: HttpPostHandler<C2H_BattlePass_ChargePrize, H2C_CommonResponse>
    {
        protected override async ETTask Run(Entity domain, C2H_BattlePass_ChargePrize request, H2C_CommonResponse response, long playerid)
        {
            Scene scene = domain.DomainScene();
            PlayerComponent playerComponent = scene.GetComponent<PlayerComponent>();
            Player player = playerComponent.Get(playerid);
            var character = player.GetMyCharacter();
            var sceneZone = character.GetMyServerZone();
            if (sceneZone != null && character.BattlePassComp != null)
            {
                (response.Error, response.Message) = character.BattlePassComp.ChargeBattlePassPrize(request.ConfigId);
            }

            await ETTask.CompletedTask;
        }
    }
}