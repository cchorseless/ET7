using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ET.Server
{
    [HttpHandler(SceneType.Realm, "/AccountLoginKey", false)]
    public class Http_PostAccountLoginKeyHandler: HttpPostHandler<C2H_GetAccountLoginKey, H2C_GetAccountLoginKey>
    {
        protected override async ETTask Run(Entity domain, C2H_GetAccountLoginKey request, H2C_GetAccountLoginKey response, long playerid)
        {
            await ETTask.CompletedTask;
            if (AccountHelper.IsGoodAccountKey(request.Account).Item1 != ErrorCode.ERR_Success)
            {
                response.Error = ErrorCode.ERR_LoginError;
                response.Message = "Account error";
                return;
            }

            var accountDB = DBManagerComponent.Instance.GetAccountDB();
            TAccountInfo newAccount = await accountDB.QueryOne<TAccountInfo>(account => account.Account == request.Account);
            if (newAccount == null)
            {
                response.Error = ErrorCode.ERR_AccountNotExist;
                response.Message = "Account CANT FIND";
            }

            response.Key = RandomGenerator.RandInt64().ToString();
            domain.GetComponent<HttpSessionKeyComponent>().Add(request.Account, response.Key);
        }
    }
}