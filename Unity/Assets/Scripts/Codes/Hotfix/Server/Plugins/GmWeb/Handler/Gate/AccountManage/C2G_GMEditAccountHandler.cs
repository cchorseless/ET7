using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET.Server

{
    [MessageHandler(SceneType.Gate)]
    public class C2G_GMEditAccountHandler : AMRpcHandler<C2G_GMEditAccount, G2C_GMEditAccount>
    {
        enum EEditAccountOperate
        {
            Update,
            ResertPassword,
            Delete
        }

        protected override async ETTask Run(Session session, C2G_GMEditAccount request, G2C_GMEditAccount response)
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
                if (request.Account == null || request.Account == "admin")
                {
                    response.Error = ErrorCode.ERR_Error;
                    response.Message = "error Account ";
                    return;
                }
                if (request.Operate == (int)EEditAccountOperate.Update)
                {
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

                    if (request.Routes == null || request.Routes.Count == 0 || request.Routes.Count > 100)
                    {
                        response.Error = ErrorCode.ERR_Error;
                        response.Message = "error Routes length";
                        return;
                    }
                }
                var accountDB = DBManagerComponent.Instance.GetAccountDB();
                var db = DBManagerComponent.Instance.GetZoneDB(session.DomainZone());
                TAccountInfo newAccount = await accountDB.QueryOne<TAccountInfo>(account => account.Account == request.Account);
                if (newAccount == null)
                {
                    response.Error = ErrorCode.ERR_Error;
                    response.Message = "no account error";
                    return;
                }
                switch (request.Operate)
                {
                    case (int)EEditAccountOperate.ResertPassword:
                        newAccount.Password = "123456abc";
                        await accountDB.Save(newAccount);
                        break;
                    case (int)EEditAccountOperate.Delete:
                        await accountDB.Remove<TAccountInfo>(newAccount.Id);
                        await db.Remove<TCharacter>(character => character.Int64PlayerId == newAccount.Id);
                        break;
                    case (int)EEditAccountOperate.Update:
                        newAccount.GmLevel = request.GmLevel;
                        var comp = await db.QueryOne<TCharacter>(character => character.Int64PlayerId == newAccount.Id);
                        if (comp != null)
                        {
                            var dataComp = comp.GetUnActiveComponent<GmCharacterDataComponent>();
                            dataComp.Description = request.Des;
                            dataComp.Routes = request.Routes;
                            await db.Save(comp);
                        }
                        await db.Save(newAccount);
                        break;
                }
            }
            catch (Exception e)
            {
                ReplyError(response, e);
            }

        }
    }
}
