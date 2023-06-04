using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace ET.Server
{
    [HttpHandler(SceneType.Gate, "/MergeBagEquip")]
    public class Http_Post_MergeBagEquipHandler : HttpPostHandler<C2H_MergeBagEquip, H2C_CommonResponse>
    {
        protected override async ETTask Run(Entity domain, C2H_MergeBagEquip request, H2C_CommonResponse response, long playerid)
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
                var itemIdList = new List<long>();
                foreach (var itemIdStr in request.ItemId)
                {
                    if (!long.TryParse(itemIdStr, out long ItemId))
                    {
                        (response.Error, response.Message) = (ErrorCode.ERR_Error, "ItemId error");
                        return;
                    }
                    itemIdList.Add(ItemId);
                }
                (response.Error, response.Message) = character.BagComp.MergeEquip(itemIdList);
            }
        }
    }
}
