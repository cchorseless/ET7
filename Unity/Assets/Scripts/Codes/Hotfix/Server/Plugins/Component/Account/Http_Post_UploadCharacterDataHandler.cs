using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET.Server
{
    [HttpHandler(SceneType.Gate, "/UploadCharacterData")]
    public class Http_Post_UploadCharacterDataHandler : HttpPostHandler<C2H_UploadCharacterData, H2C_CommonResponse>
    {
        protected override async ETTask Run(Entity domain, C2H_UploadCharacterData request, H2C_CommonResponse response, long playerid)
        {
            Scene scene = domain.DomainScene();
            PlayerComponent playerComponent = scene.GetComponent<PlayerComponent>();
            Player player = playerComponent.Get(playerid);
            var character = player.GetMyCharacter();
            response.Error = ErrorCode.ERR_Error;
            await ETTask.CompletedTask;
            if (character.DataComp != null)
            {
                if ( request.Keys.Count == request.Values.Count)
                {
                    var info = new Dictionary<string, string>();
                    for (var i = 0; i < request.Keys.Count; i++)
                    {
                        info.Add(request.Keys[i], request.Values[i]);
                    }
                    (response.Error, response.Message) = character.DataComp.UploadGameRecord(info);
                }
            }
        }
    }
}
