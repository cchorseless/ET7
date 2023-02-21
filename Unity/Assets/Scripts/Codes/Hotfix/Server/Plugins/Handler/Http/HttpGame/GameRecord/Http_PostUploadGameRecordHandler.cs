using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace ET.Server
{
    [HttpHandler(SceneType.Gate, "/UploadGameRecord")]
    public class Http_PostUploadGameRecordHandler : HttpPostHandler<C2H_UploadGameRecord, H2C_CommonResponse>
    {
        protected override async ETTask Run(Entity domain, C2H_UploadGameRecord request, H2C_CommonResponse response, long playerid)
        {
            Scene scene = domain.DomainScene();
            PlayerComponent playerComponent = scene.GetComponent<PlayerComponent>();
            Player player = playerComponent.Get(playerid);
            var character = player.GetMyCharacter();
            response.Error = ErrorCode.ERR_Error;
            await ETTask.CompletedTask;
            if (character.GameRecordComp != null)
            {
                var record = character.GameRecordComp.GetCurGameRecord();
                if (record != null && request.Keys.Count == request.Values.Count)
                {
                    var info = new Dictionary<string, string>();
                    for (var i = 0; i < request.Keys.Count; i++)
                    {
                        info.Add(request.Keys[i], request.Values[i]);
                    }
                    (response.Error, response.Message) = record.UploadGameRecord(info);
                }
            }

        }
    }
}
