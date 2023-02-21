using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace ET.Server
{
    [HttpHandler(SceneType.Gate, "/Mentorship_ApplyForMaster")]
    public class Http_Post_Mentorship_ApplyForMasterHandler : HttpPostHandler<C2H_Mentorship_ApplyForMaster, H2C_CommonResponse>
    {
        protected override async ETTask Run(Entity domain, C2H_Mentorship_ApplyForMaster request, H2C_CommonResponse response, long playerid)
        {
            Scene scene = domain.DomainScene();
            PlayerComponent playerComponent = scene.GetComponent<PlayerComponent>();
            Player player = playerComponent.Get(playerid);
            var character = player.GetMyCharacter();
            var sceneZone = character.GetMyServerZone();
            if (sceneZone != null && sceneZone.ActivityComp != null && character.ActivityComp != null)
            {
                var activity = sceneZone.ActivityComp.GetActivity<TActivityMentorshipTree>(EActivityType.TActivityMentorshipTree);
                if (activity != null && activity.IsValid())
                {
                    (response.Error, response.Message) = await activity.ApplyForMaster(character, request.MasterX64str, request.ApplyMessage);
                }
            }
            await ETTask.CompletedTask;
        }
    }
}
