using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace ET.Server
{
    [HttpHandler(SceneType.Gate, "/Use_AddHeroLevelByComHeroExp")]
    public class Http_Post_Use_AddHeroLevelByComHeroExpHandler : HttpPostHandler<C2H_Use_AddHeroLevelByComHeroExp, H2C_CommonResponse>
    {
        protected override async ETTask Run(Entity domain, C2H_Use_AddHeroLevelByComHeroExp request, H2C_CommonResponse response, long playerid)
        {
            Scene scene = domain.DomainScene();
            PlayerComponent playerComponent = scene.GetComponent<PlayerComponent>();
            Player player = playerComponent.Get(playerid);
            var character = player.GetMyCharacter();
            var sceneZone = character.GetMyServerZone();
            if (sceneZone != null && character.HeroManageComp != null)
            {
                (response.Error, response.Message) = character.HeroManageComp.AddHeroLevelByComHeroExp(request.HeroId);
            }
            await ETTask.CompletedTask;
        }
    }
}
