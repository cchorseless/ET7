using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace ET.Server

{
    public enum EPlayerBagHandleType
    {
        AddItem = 0,
        DeleteItem = 1,
    }

    [HttpHandler(SceneType.GmWeb, "/GMHandlePlayerBagItem")]
    public class Http_PostGMHandlePlayerBagItemHandler: HttpPostHandler<C2G_GMHandlePlayerBagItem, H2C_CommonResponse>
    {
        protected override async ETTask Run(Entity domain, C2G_GMHandlePlayerBagItem request, H2C_CommonResponse response, long playerid)
        {
            Scene scene = domain.DomainScene();
            PlayerComponent playerComponent = scene.GetComponent<PlayerComponent>();
            Player player = playerComponent.Get(playerid);

            if (!player.HasGmRolePermission(EGmPlayerRole.GmRole_WebPlayerDataEditer))
            {
                response.Error = ErrorCode.ERR_Error;
                response.Message = "no Permission";
                return;
            }
            if (string.IsNullOrEmpty(request.Account))
            {
                response.Error = ErrorCode.ERR_Error;
                response.Message = "Account error";
                return;
            }

            if (LuBanConfigComponent.Instance.Config().ItemConfig.GetOrDefault(request.ItemConfigId) == null)
            {
                response.Error = ErrorCode.ERR_Error;
                response.Message = "ItemConfigId not valid";
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
                var cbmsg = (P2G_GMHandlePlayerBagItem)await ActorMessageSenderComponent.Instance.Call(sceneConfig.InstanceId,
                    new G2P_GMHandlePlayerBagItem()
                    {
                        ItemConfigId = request.ItemConfigId,
                        ItemCount = request.ItemCount,
                        HandleType = request.HandleType,
                        Account = request.Account,
                    });

                response.Error = ErrorCode.ERR_Success;
                response.Message = cbmsg.Message;
            }
        }
    }
}