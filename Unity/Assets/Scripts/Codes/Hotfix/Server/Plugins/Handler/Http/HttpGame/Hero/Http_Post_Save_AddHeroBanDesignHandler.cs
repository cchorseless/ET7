using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace ET.Server
{
    [HttpHandler(SceneType.Gate, "/Save_AddHeroBanDesign")]
    public class Http_Post_Save_AddHeroBanDesignHandler : HttpPostHandler<C2H_Save_AddHeroBanDesign, H2C_CommonResponse>
    {
        protected override async ETTask Run(Entity domain, C2H_Save_AddHeroBanDesign request, H2C_CommonResponse response, long playerid)
        {
            Scene scene = domain.DomainScene();
            PlayerComponent playerComponent = scene.GetComponent<PlayerComponent>();
            Player player = playerComponent.Get(playerid);
            var character = player.GetMyCharacter();
            var sceneZone = character.GetMyServerZone();
            if (sceneZone != null && character.HeroManageComp != null)
            {
                (response.Error, response.Message) = character.HeroManageComp.AddHeroBanDesign(request.Slot, request.BanHeroList);
            }
            await ETTask.CompletedTask;
        }
    }
}
