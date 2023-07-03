using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace ET.Server
{
    [HttpHandler(SceneType.GmWeb, "/GMGetServerKeyList")]
    public class Http_GetGMGetServerKeyListHandler: HttpGetHandler<H2C_CommonResponse>
    {
        protected override async ETTask Run(Entity domain, H2C_CommonResponse response, long playerid)
        {
            var db = DBManagerComponent.Instance.GetAccountDB();
            var entitys = await db.Query<TServerKey>(e => true);
            if (entitys.Count > 0)
            {
                response.Message = MongoHelper.ToArrayClientJson(entitys.ToArray());
            }
            else
            {
                response.Message = "[]";
            }
        }
    }
}