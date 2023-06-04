using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace ET.Server
{
    [HttpHandler(SceneType.Gate, "/ReplaceEquipProps")]
    public class Http_Post_ReplaceEquipPropsHandler : HttpPostHandler<C2H_ReplaceEquipProps, H2C_CommonResponse>
    {
        protected override async ETTask Run(Entity domain, C2H_ReplaceEquipProps request, H2C_CommonResponse response, long playerid)
        {
            Scene scene = domain.DomainScene();
            PlayerComponent playerComponent = scene.GetComponent<PlayerComponent>();
            Player player = playerComponent.Get(playerid);
            var character = player.GetMyCharacter();
            var sceneZone = character.GetMyServerZone();
            response.Error = ErrorCode.ERR_Error;
            await ETTask.CompletedTask;
            if (sceneZone != null && character.BagComp != null)
            {
                if (!long.TryParse(request.ItemId, out long ItemId))
                {
                    (response.Error, response.Message) = (ErrorCode.ERR_Error, "ItemId error");
                    return;
                }
                if (!long.TryParse(request.ItemPropId, out long ItemPropId))
                {
                    (response.Error, response.Message) = (ErrorCode.ERR_Error, "ItemPropId error");
                    return;
                }
                if (!long.TryParse(request.CostItemId, out long CostItemId))
                {
                    (response.Error, response.Message) = (ErrorCode.ERR_Error, "CostItemId error");
                    return;
                }
                if (!long.TryParse(request.CostItemPropId, out long CostItemPropId))
                {
                    (response.Error, response.Message) = (ErrorCode.ERR_Error, "CostItemPropId error");
                    return;
                }
                (response.Error, response.Message) = character.BagComp.ReplaceEquipProps(ItemId, ItemPropId, CostItemId, CostItemPropId);
            }
        }
    }
}
