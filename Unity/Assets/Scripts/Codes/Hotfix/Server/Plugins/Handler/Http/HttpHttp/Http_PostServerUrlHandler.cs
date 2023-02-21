using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace ET.Server
{
    [HttpHandler(SceneType.Http, "/GetServerUrl", false)]
    public class Http_PostServerUrlHandler : HttpPostHandler<C2H_GetServerUrl, H2C_GetServerUrl>
    {
        protected override async ETTask Run(Entity domain, C2H_GetServerUrl request, H2C_GetServerUrl response, long playerid)
        {
            StartSceneConfigCategory.Instance.ClientScenesByName.TryGetValue(request.ZoneId, out var info);
            if (info != null)
            {
                var list = new List<StartSceneConfig>();
                foreach (var kv in info)
                {
                    if (kv.Value.SceneType == SceneType.Realm.ToString() &&
                        kv.Value.ServerMin <= request.ServerId &&
                        kv.Value.ServerMax >= request.ServerId)
                    {
                        list.Add(kv.Value);
                    }
                }
                if (list.Count > 0)
                {
                    var config = RandomGenerator.RandomArray(list);
                    response.Ip = config.StartProcessConfig.OuterIP;
                    response.Port = config.OuterPort;
                }
                else
                {
                    response.Error = ErrorCode.ERR_Error;
                    response.Message = "ServerId error";
                }
            }
            else
            {
                response.Error = ErrorCode.ERR_Error;
                response.Message = "zone error";
            }
            await ETTask.CompletedTask;
        }

    }
}
