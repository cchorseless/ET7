using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET.Server

{
    [MessageHandler(SceneType.Gate)]
    public class C2G_GMRegisteHandler : AMRpcHandler<C2G_GMRegiste, G2C_GMGetLogDBInfo>
    {
        protected override async ETTask Run(Session session, C2G_GMRegiste request, G2C_GMGetLogDBInfo response)
        {
            await ETTask.CompletedTask;
            try
            {
                Player player = session.GetComponent<SessionPlayerComponent>().GetMyPlayer();
                if (!player.HasGmRolePermission(EGmPlayerRole.GmRole_Admin))
                {
                    response.Error = ErrorCode.ERR_Error;
                    response.Message = "no Permission";
                    return;
                }
                var checkAccount = AccountHelper.IsGoodAccountKey(request.Account);
                if (checkAccount.Item1 != ErrorCode.ERR_Success)
                {
                    response.Error = ErrorCode.ERR_Error;
                    response.Message = checkAccount.Item2;
                    return;
                }
                if (request.GmLevel < (int)EGmPlayerRole.GmRole_PlayerGm ||
                    request.GmLevel >= (int)EGmPlayerRole.GmRole_Admin)
                {
                    response.Error = ErrorCode.ERR_Error;
                    response.Message = "error gm level";
                    return;
                }
                if (request.Des != null && request.Des.Length >= 100)
                {
                    response.Error = ErrorCode.ERR_Error;
                    response.Message = "error Des length";
                    return;
                }
                if (request.Routes != null && request.Routes.Count == 0 || request.Routes.Count > 100)
                {
                    response.Error = ErrorCode.ERR_Error;
                    response.Message = "error Routes length";
                    return;
                }
                var db = DBManagerComponent.Instance.GetZoneDB(session.DomainZone());
                var accountDB = DBManagerComponent.Instance.GetAccountDB();

                TAccountInfo newAccount = await accountDB.QueryOne<TAccountInfo>(account => account.Account == request.Account);
                if (newAccount != null)
                {
                    response.Error = ErrorCode.ERR_Error;
                    response.Message = "same account error";
                    return;
                }
                //新建账号
                newAccount = Entity.CreateOne<TAccountInfo>(session.DomainScene());
                newAccount.Account = request.Account;
                newAccount.Password = "123456abc";
                newAccount.CreateTime = TimeHelper.ServerNow();
                newAccount.GmLevel = request.GmLevel;
                await accountDB.Save(newAccount);
                TCharacter newCharacter = Entity.CreateOne<TCharacter, long>(session.DomainScene(), newAccount.Id);
                var gmDataComp = newCharacter.AddComponent<GmCharacterDataComponent>();
                gmDataComp.Description = request.Des;
                gmDataComp.Routes = request.Routes;
                await db.Save(newCharacter);
                newAccount.Dispose();
                newCharacter.Dispose();
                await ETTask.CompletedTask;
            }
            catch (Exception e)
            {
                ReplyError(response, e);
            }

        }
    }
}