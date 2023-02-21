﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace ET.Server
{
    [HttpHandler(SceneType.Gate, "/GetPrize_ActivityTotalGainMetaStone")]
    public class Http_Post_GetPrize_ActivityTotalGainMetaStoneHandler : HttpPostHandler<C2H_GetPrize_ActivityTotalGainMetaStone, H2C_CommonResponse>
    {
        protected override async ETTask Run(Entity domain, C2H_GetPrize_ActivityTotalGainMetaStone request, H2C_CommonResponse response, long playerid)
        {
            Scene scene = domain.DomainScene();
            PlayerComponent playerComponent = scene.GetComponent<PlayerComponent>();
            Player player = playerComponent.Get(playerid);
            var character = player.GetMyCharacter();
            var sceneZone = character.GetMyServerZone();
            if (sceneZone != null && sceneZone.ActivityComp != null && character.ActivityComp != null)
            {
                var activity = sceneZone.ActivityComp.GetActivity<TActivityTotalGainMetaStone>(EActivityType.TActivityTotalGainMetaStone);
                if (activity != null && activity.IsValid())
                {
                    (response.Error, response.Message) = activity.GetPrize(character, request.PrizeIndex);
                }
            }
            await ETTask.CompletedTask;
        }
    }
}
