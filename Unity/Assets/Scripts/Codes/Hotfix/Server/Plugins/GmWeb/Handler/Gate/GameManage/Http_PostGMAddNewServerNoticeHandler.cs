﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET.Server

{
    [HttpHandler(SceneType.GmWeb, "/GMAddNewServerNotice")]
    public class Http_PostGMAddNewServerNoticeHandler: HttpPostHandler<C2G_GMAddNewServerNotice, H2C_CommonResponse>
    {
        protected override async ETTask Run(Entity domain, C2G_GMAddNewServerNotice request, H2C_CommonResponse response, long playerid)
        {
            Scene scene = domain.DomainScene();
            PlayerComponent playerComponent = scene.GetComponent<PlayerComponent>();
            Player player = playerComponent.Get(playerid);
            if (!player.HasGmRolePermission(EGmPlayerRole.GmRole_WebActivityEditer))
            {
                response.Error = ErrorCode.ERR_Error;
                response.Message = "no Permission";
                return;
            }

            if (string.IsNullOrEmpty(request.NOTICE))
            {
                response.Error = ErrorCode.ERR_Error;
                response.Message = "error NOTICE ";
                return;
            }

            if (GameConfig.IsAccountProcess())
            {
                await ServerZoneManageComponent.Instance.AddNewNotice(request.NOTICE, (long)request.OperateTime * 1000);
            }
            else
            {
                var cbmsg = (P2G_GMAddNewNotice)await WatcherHelper.CallManage(GameConfig.AccountProcessID,
                    new G2P_GMAddNewNotice() { NOTICE = request.NOTICE, OperateTime = request.OperateTime });
                response.Error = cbmsg.Error;
                response.Message = cbmsg.Message;
            }
        }
    }
}