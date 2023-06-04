using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace ET.Server
{
    [HttpHandler(SceneType.Gate, "/Buy_ActivityBattlePass")]
    public class Http_Post_Buy_ActivityBattlePassHandler : HttpPostHandler<C2H_Buy_ActivityBattlePass,H2C_CommonResponse>
    {
        protected override async ETTask Run(Entity domain, C2H_Buy_ActivityBattlePass request, H2C_CommonResponse response, long playerid)
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
                    (response.Error, response.Message) = await activity.BuyBattlePass(character ,request.PayType);
                }
            }
            await ETTask.CompletedTask;
        }
    }
}
