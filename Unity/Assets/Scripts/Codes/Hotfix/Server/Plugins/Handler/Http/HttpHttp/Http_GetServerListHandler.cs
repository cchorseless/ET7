using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;


namespace ET.Server
{
    [HttpHandler(SceneType.Http, "/GetServerList", false)]
    public class Http_GetServerListHandler : HttpGetHandler<H2C_GetServerList>
    {
        protected override async ETTask Run(Entity domain, H2C_GetServerList response, long playerid)
        {
            var accountDB = DBManagerComponent.Instance.GetAccountDB();
            var serverZoneList = await accountDB.Query<TServerZone>(a => true);
            foreach (var server in serverZoneList)
            {
                var _json = new ServerInfo()
                {
                    ServerID = server.ServerID,
                    ServerName = server.ServerName,
                    ZoneID = server.ZoneID,
                };
                _json.State.AddRange(server.State.ToArray());
                response.ServerList.Add(_json);
            }
        }
    }
}
