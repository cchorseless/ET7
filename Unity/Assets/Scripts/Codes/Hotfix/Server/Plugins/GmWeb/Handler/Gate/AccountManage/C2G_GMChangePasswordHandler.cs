using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET.Server

{
    [MessageHandler(SceneType.Gate)]
    public class C2G_GMChangePasswordHandler : AMRpcHandler<C2G_GMChangePassword, G2C_GMChangePassword>
    {
        protected override async ETTask Run(Session session, C2G_GMChangePassword request, G2C_GMChangePassword response)
        {
            await ETTask.CompletedTask;
            try
            {
                Player player = session.GetComponent<SessionPlayerComponent>().GetMyPlayer();
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
                List<TAccountInfo> accountInfos = await accountDB.Query<TAccountInfo>(
             account => account.Account == player.Account &&
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
            catch (Exception e)
            {
                ReplyError(response, e);
            }

        }
    }
}
