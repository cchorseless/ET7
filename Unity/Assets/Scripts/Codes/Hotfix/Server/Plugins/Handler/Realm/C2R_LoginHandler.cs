using System;
using System.Collections.Generic;
using System.Linq;

namespace ET.Server
{
    [MessageHandler(SceneType.Realm)]
    public class C2R_LoginHandler : AMRpcHandler<C2R_Login, R2C_Login>
    {
        protected override async ETTask Run(Session session, C2R_Login request, R2C_Login response)
        {
            Scene scene = session.DomainScene();
            var sceneCloseComp = ServerSceneManagerComponent.Instance;
            if (sceneCloseComp != null && sceneCloseComp.IsClosing)
            {
                response.Error = ErrorCode.ERR_Error;
                response.Message = "Server IsClosing";
                await ETTask.CompletedTask;
                return;
            }
            var sceneConfig = StartSceneConfigCategory.Instance.GetBySceneName(scene.Zone, scene.Name);
            if (sceneConfig.ServerMin > request.ServerId || sceneConfig.ServerMax < request.ServerId)
            {
                response.Error = ErrorCode.ERR_LoginError;
                await ETTask.CompletedTask;
                return;
            }
            var accountDB = DBManagerComponent.Instance.GetAccountDB();
            TServerZone serverZone = await accountDB.QueryOne<TServerZone>(server =>
            server.ServerID == request.ServerId &&
            server.State.Contains((int)EServerZoneState.Working));
            if (serverZone == null)
            {
                response.Error = ErrorCode.ERR_LoginError;
                return;
            }
            TAccountInfo newAccount = await accountDB.QueryOne<TAccountInfo>(account => account.Account == request.Account);
            bool isCreateNew = false;
            if (newAccount == null)
            {
                bool isAutoRegiste = GameConfig.AutoRegisteAccount;
                if (isAutoRegiste)
                {
                    //新建账号
                    newAccount = Entity.CreateOne<TAccountInfo>(scene);
                    newAccount.Account = request.Account;
                    newAccount.Password = request.Password;
                    newAccount.CreateTime = TimeHelper.ServerNow();
                    isCreateNew = true;
                }
                else
                {
                    response.Error = ErrorCode.ERR_LoginError;
                    return;
                }
            }
            else
            {
                if (newAccount.Password != request.Password ||
                    newAccount.GmLevel > (int)EGmPlayerRole.GmRole_PlayerGm)
                {
                    response.Error = ErrorCode.ERR_LoginError;
                    return;
                }
            }
            if (!isCreateNew)
            {
                await KnockOutLoginedPlayer(newAccount.Id, session.DomainZone());
            }
            // 随机分配一个Gate
            StartSceneConfig config = RealmGateAddressHelper.GetGate(session.DomainZone(), request.ServerId);
            if (config == null)
            {
                response.Error = ErrorCode.ERR_LoginError;
                return;
            }
            newAccount.LastLoginTime = TimeHelper.ServerNow();
            // 保存用户数据到数据库
            await accountDB.Save(newAccount);
            Log.Debug($"gate address: {MongoHelper.ToJson(config)}");
            // 向gate请求一个key,客户端可以拿着这个key连接gate
            G2R_GetLoginKey g2RGetLoginKey = (G2R_GetLoginKey)await ActorMessageSenderComponent.Instance.Call(
                config.InstanceId, new R2G_GetLoginKey() { Account = request.Account });
            response.Address = config.OuterIPPort.ToString();
            response.Key = g2RGetLoginKey.Key;
            response.GateId = g2RGetLoginKey.GateId;
            response.UserId = newAccount.Id;
            newAccount.Dispose();
        }

        private static async ETTask KnockOutLoginedPlayer(long playerId, int zoneid)
        {
            var configList = StartSceneConfigCategory.Instance.Gates[zoneid];
            foreach (var _config in configList)
            {
                if (_config.InstanceId != 0 && _config.Process != GameConfig.GmWebProcessID)
                {
                    G2R_CheckHasLogined cbmsg = (G2R_CheckHasLogined)await ActorMessageSenderComponent.Instance.Call(_config.InstanceId, new R2G_CheckHasLogined() { PlayerId = playerId });
                    // 重复登陆
                    if (cbmsg.Error == ErrorCode.ERR_AccountHasExist)
                    {
                        return;
                    }
                }
            }
        }

    }
}