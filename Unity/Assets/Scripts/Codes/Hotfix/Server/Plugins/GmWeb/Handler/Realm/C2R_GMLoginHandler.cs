using System;
using System.Collections.Generic;


namespace ET.Server
{
    [MessageHandler(SceneType.Realm)]
    public class C2R_GMLoginHandler : AMRpcHandler<C2R_GMLogin, R2C_GMLogin>
    {
        protected override async ETTask Run(Session session, C2R_GMLogin request, R2C_GMLogin response)
        {
            Scene scene = session.DomainScene();
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

            TAccountInfo newAccount = await accountDB.QueryOne<TAccountInfo>(
                account => account.Account == request.Account &&
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
            // 随机分配一个Gate
            StartSceneConfig config = RealmGateAddressHelper.GetGMWebGate();
            // 向gate请求一个key,客户端可以拿着这个key连接gate
            G2R_GetLoginKey g2RGetLoginKey = (G2R_GetLoginKey)await ActorMessageSenderComponent.Instance.Call(
                config.InstanceId, new R2G_GetLoginKey() { Account = request.Account });
            response.Address = config.OuterIPPort.ToString();
            response.Key = g2RGetLoginKey.Key;
            response.GateId = g2RGetLoginKey.GateId;
            response.UserId = newAccount.Id;
        }
    }
}