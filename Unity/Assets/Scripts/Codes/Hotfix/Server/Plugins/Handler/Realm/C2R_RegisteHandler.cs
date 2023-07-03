using System;
using System.Collections.Generic;

namespace ET.Server
{
    [MessageHandler(SceneType.Realm)]
    public class C2R_RegisteHandler : AMRpcHandler<C2R_Registe, R2C_Registe>
    {
        protected override async ETTask Run(Session session, C2R_Registe request, R2C_Registe response)
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
            var accountDB = DBManagerComponent.Instance.GetAccountDB();
            //查询账号是否存在
            List<TAccountInfo> result = await accountDB.Query<TAccountInfo>(_account => _account.Account == request.Account);
            if (result.Count > 0)
            {
                response.Error = ErrorCode.ERR_AccountHasExist;
                return;
            }
            //新建账号
            TAccountInfo newAccount = Entity.CreateOne<TAccountInfo>(scene);
            newAccount.Account = request.Account;
            newAccount.Password = request.Password;
            // 保存用户数据到数据库
            await accountDB.Save(newAccount);
        }
    }
}