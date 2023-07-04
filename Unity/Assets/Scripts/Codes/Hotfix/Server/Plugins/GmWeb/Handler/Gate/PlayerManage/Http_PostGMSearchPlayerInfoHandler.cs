using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace ET.Server

{
    [HttpHandler(SceneType.GmWeb, "/GMSearchPlayerInfo")]
    public class Http_PostGMSearchPlayerInfoHandler: HttpPostHandler<C2G_GMSearchPlayerInfo, G2C_GMSearchPlayerInfo>
    {
        protected override async ETTask Run(Entity domain, C2G_GMSearchPlayerInfo request, G2C_GMSearchPlayerInfo response, long playerid)
        {
            Scene scene = domain.DomainScene();
            PlayerComponent playerComponent = scene.GetComponent<PlayerComponent>();
            Player player = playerComponent.Get(playerid);

            if (!player.HasGmRolePermission(EGmPlayerRole.GmRole_WebServerManager))
            {
                response.Error = ErrorCode.ERR_Error;
                response.Message = "no Permission";
                return;
            }

            if (string.IsNullOrEmpty(request.Account))
            {
                response.Error = ErrorCode.ERR_Error;
                response.Message = "Account not valid";
                return;
            }

            var accountDB = DBManagerComponent.Instance.GetAccountDB();
            var accountInfos = await accountDB.Query<TAccountInfo>(v => v.Account == request.Account);
            if (accountInfos == null || accountInfos.Count == 0)
            {
                response.Error = ErrorCode.ERR_Error;
                response.Message = "Account cant find";
                return;
            }

            var accountInfo = accountInfos[0];
            var LastGateId = accountInfo.LastGateId;
            var sceneConfig = StartSceneConfigCategory.Instance.Get((int)LastGateId);
            if (sceneConfig == null)
            {
                response.Error = ErrorCode.ERR_Error;
                response.Message = "Account cant LastGateId";
            }
            else
            {
                var cbmsg = (P2G_GMSearchPlayerInfo)await ActorMessageSenderComponent.Instance.Call(sceneConfig.InstanceId,
                    new G2P_GMSearchPlayerInfo() { Account = request.Account });

                response.Error = ErrorCode.ERR_Success;
                response.Message = cbmsg.Message;
            }
        }
    }
}