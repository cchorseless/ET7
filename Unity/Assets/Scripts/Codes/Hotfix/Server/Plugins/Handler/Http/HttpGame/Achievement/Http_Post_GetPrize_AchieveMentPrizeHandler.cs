using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace ET.Server
{
    [HttpHandler(SceneType.Gate, "/GetPrize_AchieveMentPrize")]
    public class Http_Post_GetPrize_AchieveMentPrizeHandler : HttpPostHandler<C2H_GetPrize_AchieveMentPrize, H2C_CommonResponse>
    {
        protected override async ETTask Run(Entity domain, C2H_GetPrize_AchieveMentPrize request, H2C_CommonResponse response, long playerid)
        {
            Scene scene = domain.DomainScene();
            PlayerComponent playerComponent = scene.GetComponent<PlayerComponent>();
            Player player = playerComponent.Get(playerid);
            var character = player.GetMyCharacter();
            var sceneZone = character.GetMyServerZone();
            if (sceneZone != null && character.AchievementComp != null)
            {
                (response.Error, response.Message) = character.AchievementComp.GetPrize(request.AchieveMentConfigId);
            }
            await ETTask.CompletedTask;
        }
    }
}
