using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace ET.Server
{
    [HttpHandler(SceneType.Gate, "/GetPrize_TaskPrize")]
    public class Http_Post_GetPrize_TaskPrizeHandler : HttpPostHandler<C2H_GetPrize_TaskPrize, H2C_CommonResponse>
    {
        protected override async ETTask Run(Entity domain, C2H_GetPrize_TaskPrize request, H2C_CommonResponse response, long playerid)
        {
            Scene scene = domain.DomainScene();
            PlayerComponent playerComponent = scene.GetComponent<PlayerComponent>();
            Player player = playerComponent.Get(playerid);
            var character = player.GetMyCharacter();
            var sceneZone = character.GetMyServerZone();
            if (sceneZone != null && character.BattlePassComp != null)
            {
                (response.Error, response.Message) = character.BattlePassComp.GetTaskPrize(request.TaskId);
            }
            await ETTask.CompletedTask;
        }
    }
}
