using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET.Server

{

    [MessageHandler(SceneType.Gate)]
    public class W2G_GMGetProcessEntityInfoHandler : AMRpcHandler<W2G_GMGetProcessEntityInfo, G2W_GMGetProcessEntityInfo>
    {
        protected override async ETTask Run(Session session, W2G_GMGetProcessEntityInfo request, G2W_GMGetProcessEntityInfo response)
        {
            await ETTask.CompletedTask;
            response.Message = Root.Instance.ToString();
        }
    }
}
