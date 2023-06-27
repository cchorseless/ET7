using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET.Server

{
    [HttpHandler(SceneType.Http, "/GMGetUserInfo")]
    public class Http_PostGMGetUserInfoHandler: HttpPostHandler<C2G_GMGetUserInfo, H2C_CommonResponse>
    {
        protected override async ETTask Run(Entity domain, C2G_GMGetUserInfo request, H2C_CommonResponse response, long playerid)
        {
            await ETTask.CompletedTask;
            Scene scene = domain.DomainScene();
            PlayerComponent playerComponent = scene.GetComponent<PlayerComponent>();
            Player player = playerComponent.Get(playerid);
            if (player == null)
            {
                response.Error = ErrorCode.ERR_Error;
                response.Message = "no online";
                return;
            }

            var CharacterData = player.GetComponent<GmCharacterDataComponent>();
            if (CharacterData == null)
            {
                response.Error = ErrorCode.ERR_Error;
                response.Message = "no CharacterData";
                return;
            }

            response.Message = MongoHelper.ToClientJson(CharacterData);
        }
    }
}