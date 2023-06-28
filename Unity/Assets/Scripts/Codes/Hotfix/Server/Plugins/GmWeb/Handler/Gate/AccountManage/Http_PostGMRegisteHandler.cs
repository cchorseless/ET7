using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET.Server

{
    [HttpHandler(SceneType.GmWeb, "/GMRegiste")]
    public class Http_PostGMRegisteHandler: HttpPostHandler<C2G_GMRegiste, H2C_CommonResponse>
    {
        protected override async ETTask Run(Entity domain, C2G_GMRegiste request, H2C_CommonResponse response, long playerid)
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

            if (request.Roles != null && request.Roles.Count == 0 )
            {
                response.Error = ErrorCode.ERR_Error;
                response.Message = "error Routes length";
                return;
            }

            var accountDB = DBManagerComponent.Instance.GetAccountDB();

            TAccountInfo newAccount = await accountDB.QueryOne<TAccountInfo>(account => account.Account == request.Account);
            if (newAccount != null)
            {
                response.Error = ErrorCode.ERR_Error;
                response.Message = "same account error";
                return;
            }

            //新建账号
            newAccount = Entity.CreateOne<TAccountInfo>(scene);
            newAccount.Account = request.Account;
            newAccount.Password = "123456abc";
            newAccount.CreateTime = TimeHelper.ServerNow();
            newAccount.GmLevel = request.GmLevel;
            await accountDB.Save(newAccount);
            var CharacterData = Entity.CreateOne<GmCharacterDataComponent, long>(scene, newAccount.Id);
            CharacterData.Description = request.Des;
            CharacterData.Roles = request.Roles;
            await accountDB.Save(CharacterData);
            newAccount.Dispose();
            CharacterData.Dispose();
            await ETTask.CompletedTask;
        }
    }
}