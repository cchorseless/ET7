using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace ET.Server
{
    [HttpHandler(SceneType.Gate, "/DrawEnemy_UploadBattleResult")]
    public class Http_Post_DrawEnemy_UploadBattleResult: HttpPostHandler<C2H_DrawEnemy_UploadBattleResult, H2C_CommonResponse>
    {
        protected override async ETTask Run(Entity domain, C2H_DrawEnemy_UploadBattleResult request, H2C_CommonResponse response, long playerid)
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
                if (long.TryParse(request.EnemyEntityId, out var entityid))
                {
                    (response.Error, response.Message) = character.BattleTeamComp.UploadBattleResult(roundIndex,
                        request.BattleScore, entityid);
                }
                else
                {
                    (response.Error, response.Message) = (ErrorCode.ERR_Error, "参数错误");
                }
            }
        }
    }
}