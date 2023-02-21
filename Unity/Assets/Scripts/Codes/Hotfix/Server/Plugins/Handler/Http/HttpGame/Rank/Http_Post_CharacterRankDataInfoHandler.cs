using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace ET.Server
{
    [HttpHandler(SceneType.Gate, "/CharacterRankDataInfo")]
    public class Http_Post_CharacterRankDataInfoHandler : HttpPostHandler<C2H_CharacterRankDataInfo, H2C_CommonResponse>
    {
        protected override async ETTask Run(Entity domain, C2H_CharacterRankDataInfo request, H2C_CommonResponse response, long playerid)
        {
            Scene scene = domain.DomainScene();
            PlayerComponent playerComponent = scene.GetComponent<PlayerComponent>();
            Player player = playerComponent.Get(playerid);
            var character = player.GetMyCharacter();
            var sceneZone = character.GetMyServerZone();
            if (sceneZone != null && sceneZone.RankComp != null && long.TryParse(request.CharacterId, out var characterId))
            {
                if (request.RankType == (int)ERankType.HeroBattleSorceRankGroup)
                {
                    (response.Error, response.Message) = sceneZone.RankComp.GetCurSeasonCharacterRankDataInfo(request.RankType, request.HeroConfigId, characterId);
                }
                else
                {
                    (response.Error, response.Message) = sceneZone.RankComp.GetCurSeasonCharacterRankDataInfo(request.RankType, characterId);
                }
            }
            await ETTask.CompletedTask;
        }
    }
}
