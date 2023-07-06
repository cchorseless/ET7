using System;
using System.Collections.Generic;

namespace ET.Server
{
    // 直接查询Gate上的playerinfo
    [ActorMessageHandler(SceneType.Gate)]
    public class G2P_GMHandlePlayerBagItemHandler: AMActorRpcHandler<Scene, G2P_GMHandlePlayerBagItem, P2G_GMHandlePlayerBagItem>
    {
        protected override async ETTask Run(Scene scene, G2P_GMHandlePlayerBagItem request, P2G_GMHandlePlayerBagItem response)
        {
            var playercomp = scene.GetComponent<PlayerComponent>();
            if (playercomp == null)
            {
                response.Error = ErrorCode.ERR_Error;
                response.Message = " no PlayerComponent";
                return;
            }

            if (string.IsNullOrEmpty(request.Account))
            {
                response.Error = ErrorCode.ERR_Error;
                response.Message = "Account error";
                return;
            }

            Player player = null;
            var allplayer = playercomp.GetAll();
            foreach (var _player in allplayer)
            {
                if (_player.Account == request.Account)
                {
                    player = _player;
                    break;
                }
            }

            if (player != null && player.GetMyCharacter() != null)
            {
                var character = player.GetMyCharacter();
                var BagComp = character.BagComp;
                if (request.HandleType == (int)EPlayerBagHandleType.AddItem)
                {
                    (response.Error, response.Message) = BagComp.AddTItemOrMoney(request.ItemConfigId, request.ItemCount);
                }
                else if (request.HandleType == (int)EPlayerBagHandleType.DeleteItem)
                {
                    (response.Error, response.Message) = BagComp.RemoveTItemOrMoney(request.ItemConfigId, request.ItemCount);
                }
            }
            else
            {
                var accountDB = DBManagerComponent.Instance.GetAccountDB();
                List<TCharacter> characters = await accountDB.Query<TCharacter>(x => x.Name == request.Account);
                if (characters != null && characters.Count > 0)
                {
                    var character = characters[0];
                    var serverzone = ServerZoneManageComponent.Instance.GetServerZone();
                    if (request.HandleType == (int)EPlayerBagHandleType.AddItem)
                    {
                        serverzone.MailComp.AddCharacterPrizeMail(character.Id, "GM", "AddItem", (int)TimeHelper.OneDay * 10,
                            new List<FItemInfo>() { new FItemInfo(request.ItemConfigId, request.ItemCount) });
                        (response.Error, response.Message) = (ErrorCode.ERR_Success, "");
                    }
                    else if (request.HandleType == (int)EPlayerBagHandleType.DeleteItem)
                    {
                        (response.Error, response.Message) = (ErrorCode.ERR_Error, "not support remove item when unonline");
                    }
                }
            }
        }
    }
}