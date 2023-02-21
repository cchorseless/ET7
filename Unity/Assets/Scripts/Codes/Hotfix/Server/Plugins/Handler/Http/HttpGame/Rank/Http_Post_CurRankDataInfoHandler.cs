using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace ET.Server
{
    [HttpHandler(SceneType.Gate, "/CurRankDataInfo")]
    public class Http_Post_CurRankDataInfoHandler : HttpPostHandler<C2H_CurRankDataInfo, H2C_CommonResponse>
    {
        protected override async ETTask Run(Entity domain, C2H_CurRankDataInfo request, H2C_CommonResponse response, long playerid)
        {
            Scene scene = domain.DomainScene();
            PlayerComponent playerComponent = scene.GetComponent<PlayerComponent>();
            Player player = playerComponent.Get(playerid);
            var character = player.GetMyCharacter();
            var sceneZone = character.GetMyServerZone();
            if (sceneZone != null && sceneZone.RankComp != null)
            {
                if (request.RankType == (int)ERankType.HeroBattleSorceRankGroup)
                {
                    (response.Error, response.Message) = sceneZone.RankComp.GetCurSeasonRankDataInfo(request.RankType, request.HeroConfigId, request.Page, 10);
                }
                else
                {
                    (response.Error, response.Message) = sceneZone.RankComp.GetCurSeasonRankDataInfo(request.RankType, request.Page, 10);
                }
            }
            await ETTask.CompletedTask;
        }
    }
}
