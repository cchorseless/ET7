using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET.Server

{
    [HttpHandler(SceneType.Http, "/GMEditAccount")]
    public class Http_PostGMEditAccountHandler: HttpPostHandler<C2G_GMEditAccount, H2C_CommonResponse>
    {
        enum EEditAccountOperate
        {
            Update,
            ResertPassword,
            Delete
        }

        protected override async ETTask Run(Entity domain, C2G_GMEditAccount request, H2C_CommonResponse response, long playerid)
        {
            Scene scene = domain.DomainScene();
            PlayerComponent playerComponent = scene.GetComponent<PlayerComponent>();
            Player player = playerComponent.Get(playerid);
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

                if (request.Roles == null || request.Roles.Count == 0)
                {
                    response.Error = ErrorCode.ERR_Error;
                    response.Message = "error Routes length";
                    return;
                }
            }

            var accountDB = DBManagerComponent.Instance.GetAccountDB();
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
                    await accountDB.Remove<GmCharacterDataComponent>(CharacterData => CharacterData.Int64PlayerId == newAccount.Id);
                    break;
                case (int)EEditAccountOperate.Update:
                    newAccount.GmLevel = request.GmLevel;
                    var dataComp = await accountDB.QueryOne<GmCharacterDataComponent>(CharacterData => CharacterData.Int64PlayerId == newAccount.Id);
                    if (dataComp != null)
                    {
                        dataComp.Description = request.Des;
                        dataComp.Roles = request.Roles;
                        await accountDB.Save(dataComp);
                        response.Message = MongoHelper.ToClientJson(dataComp);
                    }

                    await accountDB.Save(newAccount);
                    break;
            }
        }
    }
}