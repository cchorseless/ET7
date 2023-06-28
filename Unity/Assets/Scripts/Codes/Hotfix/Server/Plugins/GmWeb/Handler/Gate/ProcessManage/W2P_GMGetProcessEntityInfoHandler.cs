using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET.Server

{
    [ActorMessageHandler(SceneType.Manager)]
    public class W2P_GMGetProcessEntityInfoHandler: AMActorRpcHandler<Scene, W2P_GMGetProcessEntityInfo, P2W_GMGetProcessEntityInfo>
    {
        protected override async ETTask Run(Scene scene, W2P_GMGetProcessEntityInfo request, P2W_GMGetProcessEntityInfo response)
        {
            await ETTask.CompletedTask;
            var json = JsonHelper.GetLitObject();
            var map = Root.Instance.GetEntityCountMap();
            foreach (var kv in map)
            {
                json[kv.Key] = kv.Value;
            }
            response.Message = JsonHelper.ToLitJson(json);
        }
    }
}