using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace ET.Server
{
    [HttpHandler(SceneType.Gate, "/DrawEnemy_GetEnemyInfo")]
    public class Http_Post_DrawEnemy_GetEnemyInfo: HttpPostHandler<C2H_DrawEnemy_GetEnemyInfo, H2C_CommonResponse>
    {
        protected override async ETTask Run(Entity domain, C2H_DrawEnemy_GetEnemyInfo request, H2C_CommonResponse response, long playerid)
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
                var roundIndex = request.RoundIndex + request.RoundCharpter * 1000;
                (response.Error, response.Message) = character.BattleTeamComp.SearchBattleTeamRecord(roundIndex,
                    request.Score, request.EnemyCount);
            }
        }
    }
}