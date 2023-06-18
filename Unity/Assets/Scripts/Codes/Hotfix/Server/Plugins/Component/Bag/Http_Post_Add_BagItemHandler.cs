using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace ET.Server
{
    [HttpHandler(SceneType.Gate, "/Add_BagItem")]
    public class Http_Post_Add_BagItemHandler: HttpPostHandler<C2H_Add_BagItem, H2C_CommonResponse>
    {
        protected override async ETTask Run(Entity domain, C2H_Add_BagItem request, H2C_CommonResponse response, long playerid)
        {
            Scene scene = domain.DomainScene();
            PlayerComponent playerComponent = scene.GetComponent<PlayerComponent>();
            Player player = playerComponent.Get(playerid);
            var character = player.GetMyCharacter();
            var sceneZone = character.GetMyServerZone();
            response.Error = ErrorCode.ERR_Error;
            if (sceneZone != null && character.BagComp != null)
            {
                var items = new List<FItemInfo>();
                foreach (var _des in request.ItemDes)
                {
                    var a = _des.Split("|");
                    if (a.Length == 2)
                    {
                        if (int.TryParse(a[0], out var itemConfigId) && int.TryParse(a[1], out var itemCount))
                        {
                            items.Add(new FItemInfo(itemConfigId, itemCount));
                        }
                    }
                }
                if (items.Count > 0)
                {
                    (response.Error, response.Message) = character.BagComp.AddTItemOrMoney(items);
                }
            }
            await ETTask.CompletedTask;
        }
    }
}