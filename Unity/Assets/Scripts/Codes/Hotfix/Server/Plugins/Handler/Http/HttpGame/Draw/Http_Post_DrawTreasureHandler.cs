using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace ET.Server
{
    [HttpHandler(SceneType.Gate, "/DrawTreasure")]
    public class Http_Post_DrawTreasureHandler : HttpPostHandler<C2H_DrawTreasure, H2C_CommonResponse>
    {
        protected override async ETTask Run(Entity domain, C2H_DrawTreasure request, H2C_CommonResponse response, long playerid)
        {
            Scene scene = domain.DomainScene();
            PlayerComponent playerComponent = scene.GetComponent<PlayerComponent>();
            Player player = playerComponent.Get(playerid);
            var character = player.GetMyCharacter();
            var sceneZone = character.GetMyServerZone();
            if (sceneZone != null && sceneZone.DrawTreasureComp != null && character.DrawTreasureComp != null )
            {
                (response.Error, response.Message) = sceneZone.DrawTreasureComp.DrawTreasure(character, request.TreasureConfigId, request.TreasureCount);
            }
            await ETTask.CompletedTask;
        }
    }
}
