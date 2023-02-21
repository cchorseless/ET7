using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace ET.Server
{
    [HttpHandler(SceneType.Gate, "/GetPrize_ActivityBattlePass")]
    public class Http_Post_GetPrize_ActivityBattlePassHandler : HttpPostHandler<C2H_GetPrize_ActivityBattlePass, H2C_CommonResponse>
    {
        protected override async ETTask Run(Entity domain, C2H_GetPrize_ActivityBattlePass request, H2C_CommonResponse response, long playerid)
        {
            Scene scene = domain.DomainScene();
            PlayerComponent playerComponent = scene.GetComponent<PlayerComponent>();
            Player player = playerComponent.Get(playerid);
            var character = player.GetMyCharacter();
            var sceneZone = character.GetMyServerZone();
            if (sceneZone != null && sceneZone.ActivityComp != null && character.ActivityComp != null)
            {
                var activity = sceneZone.ActivityComp.GetActivity<TActivityBattlePass>(EActivityType.TActivityBattlePass);
                if (activity != null && activity.IsValid())
                {
                    if (request.IsOneKeyGetPrize)
                    {
                        (response.Error, response.Message) = activity.OneKeyGetPrize(character);
                    }
                    else
                    {
                        (response.Error, response.Message) = activity.GetPrize(character, request.Level, request.IsVip);
                    }
                }
            }
            await ETTask.CompletedTask;
        }
    }
}
