using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace ET.Server
{
    [HttpHandler(SceneType.Gate, "/Use_BagItem")]
    public class Http_Post_Use_BagItemHandler : HttpPostHandler<C2H_Use_BagItem, H2C_CommonResponse>
    {
        protected override async ETTask Run(Entity domain, C2H_Use_BagItem request, H2C_CommonResponse response, long playerid)
        {
            Scene scene = domain.DomainScene();
            PlayerComponent playerComponent = scene.GetComponent<PlayerComponent>();
            Player player = playerComponent.Get(playerid);
            var character = player.GetMyCharacter();
            var sceneZone = character.GetMyServerZone();
            response.Error = ErrorCode.ERR_Error;
            if (sceneZone != null && sceneZone.TItemManageComp != null
                && character.BagComp != null && long.TryParse(request.ItemId, out long ItemId))
            {
                (response.Error, response.Message) = sceneZone.TItemManageComp.ApplyUseBagItem(character, ItemId, request.ItemCount);
            }
            await ETTask.CompletedTask;
        }
    }
}
