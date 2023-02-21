using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace ET.Server
{
    [HttpHandler(SceneType.Gate, "/Buy_ActivityMemberShip")]
    public class Http_Post_Buy_ActivityMemberShipHandler : HttpPostHandler<C2H_Buy_ActivityMemberShip, H2C_CommonResponse>
    {
        protected override async ETTask Run(Entity domain, C2H_Buy_ActivityMemberShip request, H2C_CommonResponse response, long playerid)
        {
            Scene scene = domain.DomainScene();
            PlayerComponent playerComponent = scene.GetComponent<PlayerComponent>();
            Player player = playerComponent.Get(playerid);
            var character = player.GetMyCharacter();
            var sceneZone = character.GetMyServerZone();
            if (sceneZone != null && sceneZone.ActivityComp != null && character.ActivityComp != null)
            {
                var activity = sceneZone.ActivityComp.GetActivity<TActivityMemberShip>(EActivityType.TActivityMemberShip);
                if (activity != null && activity.IsValid())
                {
                    (response.Error, response.Message) = await activity.BuyMemberShip(character, request.BuyType, request.PayType);
                }
            }
            await ETTask.CompletedTask;
        }
    }
}
