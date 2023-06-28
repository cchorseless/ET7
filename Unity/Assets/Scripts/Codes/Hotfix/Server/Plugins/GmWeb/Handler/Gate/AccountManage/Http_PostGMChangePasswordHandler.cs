using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET.Server

{
    [HttpHandler(SceneType.GmWeb, "/GMChangePassword")]
    public class Http_PostGMChangePasswordHandler: HttpPostHandler<C2G_GMChangePassword, H2C_CommonResponse>
    {
        protected override async ETTask Run(Entity domain, C2G_GMChangePassword request, H2C_CommonResponse response, long playerid)
        {
            Scene scene = domain.DomainScene();
            PlayerComponent playerComponent = scene.GetComponent<PlayerComponent>();
            Player player = playerComponent.Get(playerid);
            if (request.OldPassWord == null)
            {
                response.Error = ErrorCode.ERR_Error;
                response.Message = "no old password";
                return;
            }

            var checkPassword = AccountHelper.IsGoodPasswordKey(request.NewPassWord);
            if (checkPassword.Item1 != ErrorCode.ERR_Success)
            {
                response.Error = ErrorCode.ERR_Error;
                response.Message = checkPassword.Item2;
                return;
            }

            var accountDB = DBManagerComponent.Instance.GetAccountDB();
            List<TAccountInfo> accountInfos = await accountDB.Query<TAccountInfo>(account => account.Account == player.Account &&
                    account.Password == request.OldPassWord &&
                    account.GmLevel > 0);
            if (accountInfos.Count == 0)
            {
                response.Error = ErrorCode.ERR_Error;
                response.Message = "old password error";
                return;
            }

            accountInfos[0].Password = request.NewPassWord;
            await accountDB.Save(accountInfos[0]);
        }
    }
}