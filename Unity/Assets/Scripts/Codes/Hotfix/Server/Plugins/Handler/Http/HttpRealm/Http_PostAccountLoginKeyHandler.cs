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
            if (AccountHelper.IsGoodAccountKey(request.Account).Item1 != ErrorCode.ERR_Success)
            {
                response.Error = ErrorCode.ERR_LoginError;
                response.Message = "Account error";
                return;
            }

            response.Error = ErrorCode.ERR_Success;
            response.Key = RandomGenerator.RandInt64().ToString();
            var scene = domain.DomainScene();
            scene.GetComponent<HttpRealmSessionKeyComponent>().Add(request.Account, response.Key);
            await ETTask.CompletedTask;
        }
    }
}