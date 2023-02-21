using System;
using System.Collections.Generic;
using ET.JWT;
using ET.JWT.Algorithms;
using ET.JWT.Builder;
using ET.JWT.Serializers;

namespace ET.Server
{
    [HttpHandler(SceneType.Http, "/GetServerNotice", false)]
    public class Http_GetServerNoticeHandler : HttpGetHandler<H2C_GetServerNotice>
    {
        protected override async ETTask Run(Entity domain, H2C_GetServerNotice response, long playerid)
        {
            var accountDB = DBManagerComponent.Instance.GetAccountDB();
            int _state = (int)EStatefulTimer.Enable;
            var noticeRecord = await accountDB.QueryOne<TServerNoticeRecord>(a => a.State.Contains(_state));
            if (noticeRecord != null)
            {
                response.Message = noticeRecord.Notice;
            }
            else
            {
                response.Message = "error";
            }
        }
    }
}
