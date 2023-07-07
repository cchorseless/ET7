using System.Collections.Generic;

namespace ET.Server
{
    [HttpHandler(SceneType.Gate, "/LoginGate", false)]
    public class Http_PostLoginGateHandler: HttpPostHandler<C2G_LoginGate, G2C_LoginGate>
    {
        protected override async ETTask Run(Entity domain, C2G_LoginGate request, G2C_LoginGate response, long playerid)
        {
            Scene scene = domain.DomainScene();
            var sceneConfig = StartSceneConfigCategory.Instance.Get((int)scene.Id);
            if (sceneConfig.ServerMin > request.ServerId || sceneConfig.ServerMax < request.ServerId)
            {
                response.Error = ErrorCode.ERR_LoginError;
                await ETTask.CompletedTask;
                return;
            }

            string account = scene.GetComponent<GateSessionKeyComponent>().Get(request.Key);
            var accountDB = DBManagerComponent.Instance.GetAccountDB();
            TAccountInfo accountInfo = await accountDB.Query<TAccountInfo>(request.UserId);
            if (account == null || accountInfo == null)
            {
                response.Error = ErrorCore.ERR_ConnectGateKeyError;
                response.Message = "Gate key验证失败!";
                return;
            }

            // 更新一下
            accountInfo.LastGateId = sceneConfig.Id;
            await accountDB.Save(accountInfo);
            TServerZone serverZone = ServerZoneManageComponent.Instance.GetServerZone(request.ServerId);
            if (serverZone == null)
            {
                response.Error = ErrorCode.ERR_LoginError;
                return;
            }

            PlayerComponent playerComponent = scene.GetComponent<PlayerComponent>();
            Player player = playerComponent.Get(request.UserId);
            if (player == null || player.IsDisposed)
            {
                player = playerComponent.AddChildWithId<Player, string>(request.UserId, account);
                player.AddComponent<GateMapComponent>();
                player.AddComponent<PlayerLoginOutComponent>();
                player.AddComponent<HttpPlayerSessionComponent>();
                playerComponent.Add(player);
            }

            Log.Console($"{player.Account} login in Gate {scene.Id} ,Current Player Count : {playerComponent.idPlayers.Count}");
            player.IsOnline = true;
            player.GateSessionActorId = request.Key;
            player.GmLevel = accountInfo.GmLevel;
            // 可以直接发协议给客户端
            response.PlayerId = player.Id;
            response.Message = scene.GetComponent<HttpComponent>().AuthorizeToken(request.UserId, request.Key, 24);
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