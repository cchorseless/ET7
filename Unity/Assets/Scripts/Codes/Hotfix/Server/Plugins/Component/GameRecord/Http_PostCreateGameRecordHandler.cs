using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace ET.Server
{
    [HttpHandler(SceneType.Gate, "/CreateGameRecord")]
    public class Http_PostCreateGameRecordHandler : HttpPostHandler<C2H_CreateGameRecord, H2C_CommonResponse>
    {
        protected override async ETTask Run(Entity domain, C2H_CreateGameRecord request, H2C_CommonResponse response, long playerid)
        {
            Scene scene = domain.DomainScene();
            PlayerComponent playerComponent = scene.GetComponent<PlayerComponent>();
            Player player = playerComponent.Get(playerid);
            var character = player.GetMyCharacter();
            character.SyncClientServerZoneData();
            response.Error = ErrorCode.ERR_Error;
            await ETTask.CompletedTask;
            var serverZone = character.GetMyServerZone();
            if (serverZone != null && character.GameRecordComp != null)
            {
                (response.Error, response.Message) = serverZone.GameRecordComp.CreateGameRecord(character, request.Players);
            }
        }
    }

}
