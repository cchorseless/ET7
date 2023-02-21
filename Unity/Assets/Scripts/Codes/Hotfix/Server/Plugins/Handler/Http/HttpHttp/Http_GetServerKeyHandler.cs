using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace ET.Server
{
    [HttpHandler(SceneType.Http, "/GetServerKey", false)]
    public class Http_GetServerKeyHandler : HttpGetHandler<H2C_CommonResponse>
    {
        protected override async ETTask Run(Entity domain, H2C_CommonResponse response, long playerid)
        {
            var db = DBManagerComponent.Instance.GetAccountDB();
            var entitys = await db.Query<TServerKey>(e => true);
            response.Message = "";
            entitys.ForEach(e =>
            {
                response.Message += $"<br>[CreateTime=>{e.CreateTime}|Name=>{e.Name}|Label=>{e.Label}|ServerKey=>{e.ServerKey}]<br>";
            });
        }

    }
}
