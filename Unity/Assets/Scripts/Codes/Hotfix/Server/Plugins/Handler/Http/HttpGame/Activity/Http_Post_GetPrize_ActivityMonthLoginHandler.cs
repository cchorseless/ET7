﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace ET.Server
{
    [HttpHandler(SceneType.Gate, "/GetPrize_ActivityMonthLogin")]
    public class Http_Post_GetPrize_ActivityMonthLoginHandler : HttpPostHandler<C2H_GetPrize_ActivityMonthLogin, H2C_CommonResponse>
    {
        protected override async ETTask Run(Entity domain, C2H_GetPrize_ActivityMonthLogin request, H2C_CommonResponse response, long playerid)
        {
            Scene scene = domain.DomainScene();
            PlayerComponent playerComponent = scene.GetComponent<PlayerComponent>();
            Player player = playerComponent.Get(playerid);
            var character = player.GetMyCharacter();
            var sceneZone = character.GetMyServerZone();
            if (sceneZone != null && sceneZone.ActivityComp != null && character.ActivityComp != null)
            {
                var activity = sceneZone.ActivityComp.GetActivity<TActivityMonthLogin>(EActivityType.TActivityMonthLogin);
                if (activity != null && activity.IsValid())
                {
                    (response.Error, response.Message) = activity.GetPrize(character, request.Day);
                }
            }
            await ETTask.CompletedTask;
        }
    }
}
