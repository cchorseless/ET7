using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace ET.Server
{
    [HttpHandler(SceneType.Gate, "/ChangeHeroTalentState")]
    public class Http_Post_ChangeHeroTalentStateHandler : HttpPostHandler<C2H_ChangeHeroTalentState, H2C_CommonResponse>
    {
        protected override async ETTask Run(Entity domain, C2H_ChangeHeroTalentState request, H2C_CommonResponse response, long playerid)
        {
            Scene scene = domain.DomainScene();
            PlayerComponent playerComponent = scene.GetComponent<PlayerComponent>();
            Player player = playerComponent.Get(playerid);
            var character = player.GetMyCharacter();
            var sceneZone = character.GetMyServerZone();
            if (sceneZone != null && character.HeroManageComp != null)
            {
                var hero = character.HeroManageComp.GetHeroUnit(request.HeroId);
                if (hero == null)
                {
                    (response.Error, response.Message) = (ErrorCode.ERR_Error, "cant find hero");
                }
                else
                {
                    (response.Error, response.Message) = hero.HeroTalentComp.ChangeTalentState(request.TalentLevel, request.TalentIndex);
                }
            }
            await ETTask.CompletedTask;
        }
    }
}
