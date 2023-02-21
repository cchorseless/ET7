using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace ET.Server
{
    [HttpHandler(SceneType.Gate, "/Mentorship_ChangeApplyState")]
    public class Http_Post_Mentorship_ChangeApplyStateHandler : HttpPostHandler<C2H_Mentorship_ChangeApplyState, H2C_CommonResponse>
    {
        protected override async ETTask Run(Entity domain, C2H_Mentorship_ChangeApplyState request, H2C_CommonResponse response, long playerid)
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
                    (response.Error, response.Message) = await activity.ChangeApplyForMasterState(character, request.EntityId, request.IsAgree);
                }
            }
            await ETTask.CompletedTask;
        }
    }
}
