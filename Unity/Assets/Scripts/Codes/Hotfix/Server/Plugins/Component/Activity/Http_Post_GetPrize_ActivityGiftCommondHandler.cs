using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace ET.Server
{
    [HttpHandler(SceneType.Gate, "/GetPrize_ActivityGiftCommond")]
    public class Http_Post_GetPrize_ActivityGiftCommondHandler : HttpPostHandler<C2H_GetPrize_ActivityGiftCommond, H2C_CommonResponse>
    {
        protected override async ETTask Run(Entity domain, C2H_GetPrize_ActivityGiftCommond request, H2C_CommonResponse response, long playerid)
        {
            Scene scene = domain.DomainScene();
            PlayerComponent playerComponent = scene.GetComponent<PlayerComponent>();
            Player player = playerComponent.Get(playerid);
            var character = player.GetMyCharacter();
            var sceneZone = character.GetMyServerZone();
            if (sceneZone != null && sceneZone.ActivityComp != null && character.ActivityComp != null)
            {
                var activity = sceneZone.ActivityComp.GetActivity<TActivityGiftCommond>(EActivityType.TActivityGiftCommond);
                if (activity != null && activity.IsValid())
                {
                    (response.Error, response.Message) = activity.GetPrize(character, request.GiftConfigId);
                }
            }
            await ETTask.CompletedTask;
        }
    }
}
