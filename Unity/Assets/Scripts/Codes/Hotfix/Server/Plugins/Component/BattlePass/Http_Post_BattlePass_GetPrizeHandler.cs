﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace ET.Server
{
    [HttpHandler(SceneType.Gate, "/BattlePass_GetPrize")]
    public class Http_Post_BattlePass_GetPrizeHandler : HttpPostHandler<C2H_BattlePass_GetPrize, H2C_CommonResponse>
    {
        protected override async ETTask Run(Entity domain, C2H_BattlePass_GetPrize request, H2C_CommonResponse response, long playerid)
        {
            Scene scene = domain.DomainScene();
            PlayerComponent playerComponent = scene.GetComponent<PlayerComponent>();
            Player player = playerComponent.Get(playerid);
            var character = player.GetMyCharacter();
            var sceneZone = character.GetMyServerZone();
            if (sceneZone != null && character.BattlePassComp != null)
            {
                (response.Error, response.Message) = character.BattlePassComp.GetBattlePassPrize(request.PrizeLevel ,request.IsPlusPrize);
            }
            await ETTask.CompletedTask;
        }
    }
}
