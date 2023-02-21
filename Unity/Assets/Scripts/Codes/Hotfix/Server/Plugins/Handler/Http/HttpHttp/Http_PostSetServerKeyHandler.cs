using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace ET.Server
{
    [HttpHandler(SceneType.Http, "/SetServerKey", false)]
    public class Http_PostSetServerKeyHandler : HttpPostHandler<C2H_SetServerKey, H2C_CommonResponse>
    {
        protected override async ETTask Run(Entity domain, C2H_SetServerKey request, H2C_CommonResponse response, long playerid)
        {
            await ETTask.CompletedTask;
            if (string.IsNullOrEmpty(request.ServerKey) || string.IsNullOrEmpty(request.Name) || string.IsNullOrEmpty(request.Label))
            {
                throw new Exception("message is null or empty");
            }
            var db = DBManagerComponent.Instance.GetAccountDB();
            var entity = Entity.CreateOne<TServerKey>();
            entity.ServerKey = request.ServerKey;
            entity.Name = request.Name;
            entity.Label = request.Label;
            entity.CreateTime = TimeHelper.ServerNow();
            await db.Save(entity);
            entity.Dispose();
        }

    }
}
