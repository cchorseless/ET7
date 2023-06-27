using System;
using System.Collections.Generic;

namespace ET.Server
{
    [HttpHandler(SceneType.Http, "/GMLogin", false)]
    public class Http_PostGMLoginHandler: HttpPostHandler<C2G_GMLogin, G2C_GMLogin>
    {
        protected override async ETTask Run(Entity domain, C2G_GMLogin request, G2C_GMLogin response, long playerid)
        {
            Scene scene = domain.DomainScene();
            var accountDB = DBManagerComponent.Instance.GetAccountDB();
            TAccountInfo newAdminAccount = await accountDB.QueryOne<TAccountInfo>(
                account => account.Account == "admin" && account.GmLevel == (int)EGmPlayerRole.GmRole_Admin);
            if (newAdminAccount == null)
            {
                //新建admin账号
                newAdminAccount = Entity.CreateOne<TAccountInfo>(scene);
                newAdminAccount.Account = "admin";
                newAdminAccount.Password = "admin123";
                newAdminAccount.GmLevel = (int)EGmPlayerRole.GmRole_Admin;
                // 保存用户数据到数据库
                newAdminAccount.CreateTime = TimeHelper.ServerNow();
                await accountDB.Save(newAdminAccount);
                response.Error = ErrorCode.ERR_LoginError;
                return;
            }

            TAccountInfo newAccount = await accountDB.QueryOne<TAccountInfo>(account => account.Account == request.Account &&
                    account.Password == request.Password &&
                    account.GmLevel > (int)EGmPlayerRole.GmRole_PlayerGm);
            if (newAccount == null)
            {
                response.Error = ErrorCode.ERR_LoginError;
                return;
            }

            newAccount.LastLoginTime = TimeHelper.ServerNow();
            // 保存用户数据到数据库
            await accountDB.Save(newAccount);

            PlayerComponent playerComponent = scene.GetComponent<PlayerComponent>();
            Player player = playerComponent.Get(newAccount.Id);
            if (player == null)
            {
                player = playerComponent.AddChildWithId<Player, string>(newAccount.Id, newAccount.Account);
                player.IsOnline = true;
                player.GmLevel = newAccount.GmLevel;
                playerComponent.Add(player);
            }

            var GmCharacterData = player.GetComponent<GmCharacterDataComponent>();
            if (GmCharacterData == null)
            {
                GmCharacterData = await accountDB.Query<GmCharacterDataComponent>(player.Id);
                if (GmCharacterData != null)
                {
                    player.AddComponent(GmCharacterData);
                }
            }
            if (GmCharacterData == null)
            {
                GmCharacterData = player.AddComponent<GmCharacterDataComponent, long>(player.Id);
                if (!GmCharacterData.Roles.Contains(player.GmLevel))
                {
                    GmCharacterData.Roles.Add(player.GmLevel);
                }

                await accountDB.Save(GmCharacterData);
            }

            player.GateSessionActorId = RandomGenerator.RandInt64();
            response.Token = scene.GetComponent<HttpComponent>().AuthorizeToken(newAccount.Id, player.GateSessionActorId, 24);
        }
    }
}