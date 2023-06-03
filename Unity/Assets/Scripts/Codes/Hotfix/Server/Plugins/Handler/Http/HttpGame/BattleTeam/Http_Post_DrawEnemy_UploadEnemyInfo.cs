using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace ET.Server
{
    [HttpHandler(SceneType.Gate, "/DrawEnemy_UploadEnemyInfo")]
    public class Http_Post_DrawEnemy_UploadEnemyInfo: HttpPostHandler<C2H_DrawEnemy_UploadEnemyInfo, H2C_CommonResponse>
    {
        protected override async ETTask Run(Entity domain, C2H_DrawEnemy_UploadEnemyInfo request, H2C_CommonResponse response, long playerid)
        {
            Scene scene = domain.DomainScene();
            PlayerComponent playerComponent = scene.GetComponent<PlayerComponent>();
            Player player = playerComponent.Get(playerid);
            var character = player.GetMyCharacter();
            var sceneZone = character.GetMyServerZone();
            response.Error = ErrorCode.ERR_Error;
            await ETTask.CompletedTask;
            if (sceneZone != null && character.BattleTeamComp != null)
            {
                if (request.TeamInfo == null || request.TeamInfo.Count == 0)
                {
                    response.Error = ErrorCode.ERR_Error;
                    response.Message = "TeamInfo is null";
                    return;
                }

                foreach (var teaminfo in request.TeamInfo)
                {
                    character.BattleTeamComp.UploadBattleTeamRecord(teaminfo).Coroutine();
                }
            }
        }
    }
}