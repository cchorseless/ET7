using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ET.Server
{
    [HttpHandler(SceneType.Realm, "/LoginRealm", false)]
    public class Http_PostLoginRealmHandler: HttpPostHandler<C2R_Login, R2C_Login>
    {
        protected override async ETTask Run(Entity domain, C2R_Login request, R2C_Login response, long playerid)
        {
            if (AccountHelper.IsGoodAccountKey(request.Account).Item1 != ErrorCode.ERR_Success)
            {
                response.Error = ErrorCode.ERR_LoginError;
                response.Message = "Account error";
                return;
            }
            var scene = domain.DomainScene();
            var key = scene.GetComponent<HttpRealmSessionKeyComponent>().GetKey(request.Account);
            scene.GetComponent<HttpRealmSessionKeyComponent>().Remove(request.Account);
            if (key == null)
            {
                response.Error = ErrorCode.ERR_LoginError;
                response.Message = "key error";
                return;
            }

            if (String.IsNullOrEmpty(request.Password) || request.Password != MD5Helper.GetMD5(key + ConstValue.DotaDedicatedServerKeyV2))
            {
                response.Error = ErrorCode.ERR_LoginError;
                response.Message = "Password error";
                return;
            }

            var accountDB = DBManagerComponent.Instance.GetAccountDB();
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
                    newAccount.Password = request.Account;
                    newAccount.CreateTime = TimeHelper.ServerNow();
                    newAccount.GmLevel = (int)EGmPlayerRole.GmRole_Player;
                    isCreateNew = true;
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

            if (!isCreateNew && request.GateId == 0)
            {
                request.GateId = (int)newAccount.LastGateId;
            }

            // 随机分配一个Gate
            StartSceneConfig config;
            if (request.GateId != 0 && StartSceneConfigCategory.Instance.Contain(request.GateId))
            {
                config = StartSceneConfigCategory.Instance.Get(request.GateId);
            }
            else
            {
                config = RealmGateAddressHelper.GetGate(domain.DomainZone(), 1);
            }

            if (config == null)
            {
                response.Error = ErrorCode.ERR_LoginError;
                return;
            }

            newAccount.LastLoginTime = TimeHelper.ServerNow();
            newAccount.LastGateId = config.Id;
            // 保存用户数据到数据库
            await accountDB.Save(newAccount);
            // 向gate请求一个key,客户端可以拿着这个key连接gate
            G2R_GetLoginKey g2RGetLoginKey = (G2R_GetLoginKey)await ActorMessageSenderComponent.Instance.Call(
                config.InstanceId, new R2G_GetLoginKey() { Account = request.Account });
            response.Address = config.OuterIPPort.ToString();
            response.Key = g2RGetLoginKey.Key;
            response.GateId = g2RGetLoginKey.GateId;
            response.UserId = newAccount.Id;
            Log.Info($"{request.Account} login in Ream , Go to Gate {response.GateId} ");
            newAccount.Dispose();
        }

        private static async ETTask<long> FindLoginedPlayerGateId(long playerId, int zoneid)
        {
            var configList = StartSceneConfigCategory.Instance.Gates[zoneid];
            foreach (var _config in configList)
            {
                if (_config.Process == GameConfig.GmWebProcessID)
                {
                    continue;
                }

                // 本进程gate
                if (_config.Process == Options.Instance.Process)
                {
                    var scene = ServerSceneManagerComponent.Instance.Get(_config.Id);
                    PlayerComponent playerComponent = scene.GetComponent<PlayerComponent>();
                    Player player = playerComponent.Get(playerId);
                    if (player != null)
                    {
                        return scene.Id;
                    }
                }
                else
                {
                    G2R_CheckHasLogined cbmsg = (G2R_CheckHasLogined)await ActorMessageSenderComponent.Instance.Call(_config.InstanceId,
                        new R2G_CheckHasLogined() { PlayerId = playerId, IsKnockOut = false });
                    // 重复登陆
                    if (cbmsg.Error == ErrorCode.ERR_AccountHasExist)
                    {
                        return cbmsg.GateId;
                    }
                }
            }

            return 0;
        }
    }
}