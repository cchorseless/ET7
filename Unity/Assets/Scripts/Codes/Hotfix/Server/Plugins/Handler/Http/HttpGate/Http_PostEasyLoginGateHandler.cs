using System.Collections.Generic;

namespace ET.Server
{
    [HttpHandler(SceneType.Gate, "/EasyLoginGate", false)]
    public class Http_PostEasyLoginGateHandler: HttpPostHandler<C2G_EasyLoginGate, G2C_EasyLoginGate>
    {
        protected override async ETTask Run(Entity domain, C2G_EasyLoginGate request, G2C_EasyLoginGate response, long playerid)
        {
            TServerZone serverZone = ServerZoneManageComponent.Instance.GetServerZone();
            if (serverZone == null)
            {
                response.Error = ErrorCode.ERR_LoginError;
                response.Message = "ServerZone Miss";
                return;
            }
            if (AccountHelper.IsGoodAccountKey(request.Account).Item1 != ErrorCode.ERR_Success)
            {
                response.Error = ErrorCode.ERR_LoginError;
                response.Message = "Account error";
                return;
            }

            if (string.IsNullOrEmpty(request.Password) ||
                request.Password != MD5Helper.GetMD5(request.Account + "@" + ConstValue.DotaDedicatedServerKeyV2))
            {
                response.Error = ErrorCode.ERR_LoginError;
                response.Message = "Password error";
                return;
            }
            
            Scene scene = domain.DomainScene();
            var sceneConfig = StartSceneConfigCategory.Instance.Get((int)scene.Id);
            var accountDB = DBManagerComponent.Instance.GetAccountDB();
            TAccountInfo newAccount = await accountDB.QueryOne<TAccountInfo>(account => account.Account == request.Account);
            if (newAccount == null)
            {
                bool isAutoRegiste = GameConfig.AutoRegisteAccount;
                if (isAutoRegiste)
                {
                    //新建账号
                    newAccount = Entity.CreateOne<TAccountInfo>(scene);
                    newAccount.Account = request.Account;
                    newAccount.Password = request.Account;
                    newAccount.CreateTime = TimeHelper.ServerNow();
                    newAccount.LastGateId = scene.Id;
                    newAccount.GmLevel = (int)EGmPlayerRole.GmRole_Player;
                }
                else
                {
                    response.Error = ErrorCode.ERR_LoginError;
                    return;
                }
            }
            else if (newAccount.GmLevel > (int)EGmPlayerRole.GmRole_PlayerGm)
            {
                response.Error = ErrorCode.ERR_LoginError;
                return;
            }

            newAccount.LastLoginTime = TimeHelper.ServerNow();
            newAccount.LastGateId = sceneConfig.Id;
            await accountDB.Save(newAccount);
            PlayerComponent playerComponent = scene.GetComponent<PlayerComponent>();
            Player player = playerComponent.Get(newAccount.Id);
            if (player == null || player.IsDisposed)
            {
                player = playerComponent.AddChildWithId<Player, string>(newAccount.Id, newAccount.Account);
                player.AddComponent<GateMapComponent>();
                player.AddComponent<PlayerLoginOutComponent>();
                player.AddComponent<HttpPlayerSessionComponent>();
                playerComponent.Add(player);
            }
            response.PlayerId = player.Id;
            Log.Console($"{player.Account} login in Gate {scene.Id} ,Current Player Count : {playerComponent.idPlayers.Count}");
            player.IsOnline = true;
            player.GateSessionActorId = RandomGenerator.RandInt64();
            player.GmLevel = newAccount.GmLevel;
            response.Message = scene.GetComponent<HttpComponent>().AuthorizeToken(newAccount.Id,  player.GateSessionActorId, 24);
            TCharacter character = player.GetMyCharacter();
            // 重新加载一下数据
            if (character != null)
            {
                await character.Save();
                character.Dispose();
                character = null;
            }

            List<TCharacter> characters = await accountDB.Query<TCharacter>(x => x.Int64PlayerId == player.Id);
            if (characters.Count > 0)
            {
                character = characters[0];
            }
            else if (characters.Count == 0 && GameConfig.AutoCreateDefaultCharacter)
            {
                character = player.AddChild<TCharacter, long>(player.Id);
                character.ZoneID = scene.DomainZone();
                character.ServerID = 1;
                character.CreateTime = TimeHelper.ServerNow();
                // 新增角色
                serverZone.DataStatisticComp.GetCurDataItem().UpdateHoursPlayerNew();
                await accountDB.Save(character);
            }

            if (character == null)
            {
                response.Error = ErrorCode.ERR_LoginError;
                response.Message = "CAN NOT FIND CHARACTER";
                return;
            }


            player.SelectCharacter(character);
            player.GetComponent<PlayerLoginOutComponent>().LogOutType = (int)ELogOutHandlerType.LogOutCharacter;
            character.SyncHttpEntity(character);
            await character.LoadAllComponent();
            serverZone = character.GetMyServerZone();
            serverZone.CharacterComp.Add(character);
            character.SyncClientCharacterData();
        }
    }
}