using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace ET.Server
{
    [HttpHandler(SceneType.Gate, "/ChangeDailyTaskState")]
    public class Http_Post_ChangeDailyTaskStateHandler : HttpPostHandler<C2H_ChangeDailyTaskState, H2C_CommonResponse>
    {
        protected override async ETTask Run(Entity domain, C2H_ChangeDailyTaskState request, H2C_CommonResponse response, long playerid)
        {
            Scene scene = domain.DomainScene();
            PlayerComponent playerComponent = scene.GetComponent<PlayerComponent>();
            Player player = playerComponent.Get(playerid);
            var character = player.GetMyCharacter();
            var sceneZone = character.GetMyServerZone();
            if (sceneZone != null && character.TaskComp != null)
            {
                (response.Error, response.Message) = character.TaskComp.ChangeDailyTaskState(request.TaskId, request.isDropTask);

            }
            await ETTask.CompletedTask;
        }
    }
}
