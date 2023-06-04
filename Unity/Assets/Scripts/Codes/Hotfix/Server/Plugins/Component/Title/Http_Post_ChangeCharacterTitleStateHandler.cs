using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace ET.Server
{
    [HttpHandler(SceneType.Gate, "/ChangeCharacterTitleState")]
    public class Http_Post_ChangeCharacterTitleStateHandler : HttpPostHandler<C2H_ChangeCharacterTitleState, H2C_CommonResponse>
    {
        protected override async ETTask Run(Entity domain, C2H_ChangeCharacterTitleState request, H2C_CommonResponse response, long playerid)
        {
            Scene scene = domain.DomainScene();
            PlayerComponent playerComponent = scene.GetComponent<PlayerComponent>();
            Player player = playerComponent.Get(playerid);
            var character = player.GetMyCharacter();
            var sceneZone = character.GetMyServerZone();
            if (sceneZone != null && character.TitleComp != null)
            {
                (response.Error, response.Message) = character.TitleComp.ChangeTitleState(request.TitleConfigId, request.IsDress);

            }
            await ETTask.CompletedTask;
        }
    }
}
