using System;
using System.Collections.Generic;

namespace ET.Server

{
    // 直接查询Gate上的playerinfo
    [ActorMessageHandler(SceneType.Gate)]
    public class G2P_GMSearchPlayerInfoHandler: AMActorRpcHandler<Scene, G2P_GMSearchPlayerInfo, P2G_GMSearchPlayerInfo>
    {
        protected override async ETTask Run(Scene scene, G2P_GMSearchPlayerInfo request, P2G_GMSearchPlayerInfo response)
        {
            var playercomp = scene.GetComponent<PlayerComponent>();
            if (playercomp == null)
            {
                response.Error = ErrorCode.ERR_Error;
                response.Message = " no PlayerComponent";
                return;
            }

            if (string.IsNullOrEmpty(request.Account))
            {
                response.Error = ErrorCode.ERR_Error;
                response.Message = "Account error";
                return;
            }

            Player player = null;
            var allplayer = playercomp.GetAll();
            foreach (var _player in allplayer)
            {
                if (_player.Account == request.Account)
                {
                    player = _player;
                    break;
                }
            }

            var json = JsonHelper.GetLitObject();
            json["online"] = player != null;
            TCharacter character = null;
            CharacterDataComponent DataComp = null;
            BagComponent BagComp = null;
            CharacterBattlePassComponent BattlePassComp = null;
            HeroManageComponent HeroManageComp = null;
            CharacterRankComponent RankComp = null;
            CharacterBattleTeamComponent BattleTeamComp = null;
            if (player != null && player.GetMyCharacter() != null)
            {
                character = player.GetMyCharacter();
                DataComp = character.DataComp;
                BagComp = character.BagComp;
                BattlePassComp = character.BattlePassComp;
                HeroManageComp = character.HeroManageComp;
                RankComp = character.RankComp;
                BattleTeamComp= character.BattleTeamComp;
            }
            else
            {
                var accountDB = DBManagerComponent.Instance.GetAccountDB();
                List<TCharacter> characters = await accountDB.Query<TCharacter>(x => x.Name == request.Account);
                if (characters != null && characters.Count > 0)
                {
                    character = characters[0];
                    DataComp = await accountDB.Query<CharacterDataComponent>(character.Int64PlayerId);
                    BagComp = await accountDB.Query<BagComponent>(character.Int64PlayerId);
                    BattlePassComp = await accountDB.Query<CharacterBattlePassComponent>(character.Int64PlayerId);
                    HeroManageComp = await accountDB.Query<HeroManageComponent>(character.Int64PlayerId);
                    RankComp = await accountDB.Query<CharacterRankComponent>(character.Int64PlayerId);
                    BattleTeamComp= await accountDB.Query<CharacterBattleTeamComponent>(character.Int64PlayerId);
                }
            }
            var ListEntity = new List<Entity>()
            {
                character,
                DataComp,
                BagComp,
                BattlePassComp,
                HeroManageComp,
                RankComp,
                BattleTeamComp
            };
            json["Child"] = MongoHelper.ToArrayClientJson(ListEntity.ToArray());
            response.Message = JsonHelper.ToLitJson(json);
        }
    }
}