using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET.Server

{

    [MessageHandler(SceneType.Gate)]
    public class G2W_GMGetProcessInfoHandler : AMRpcHandler<G2W_GMGetProcessInfo, W2G_GMGetProcessInfo>
    {
        protected override async ETTask Run(Session session, G2W_GMGetProcessInfo request, W2G_GMGetProcessInfo response)
        {
            await ETTask.CompletedTask;
            var watchComp = WatcherComponent.Instance;
            if (watchComp != null)
            {
                var json = JsonHelper.GetLitObject();

                var allMeInfo = StartMachineConfigCategory.Instance.GetAll();
                foreach (var kv in allMeInfo)
                {
                    if (kv.Key != WatcherHelper.GetThisMachineConfig().Id)
                    {
                        //kv.Value.
                    }
                }

                // Todo 获取其他机器上的信息
                foreach (var process in watchComp.Processes)
                {
                    var _json = JsonHelper.GetLitObject();
                    _json["Id"] = process.Key.ToString();
                    _json["HasExited"] = process.Value.HasExited;
                    _json["StartTime"] = process.Value.StartTime.ToString("yyyy-MM-dd-HH-mm-ss");
                    _json["MemorySize"] = $"{(process.Value.WorkingSet64 / 1024 / 1024)}MB";
                    json[process.Key.ToString()] = _json;
                }
                foreach (var process in watchComp.Processes)
                {
                    if (StartProcessConfigCategory.Instance.Get(process.Key).AppName == "Robot") { continue; }
                    Session serverSession = NetInnerComponent.Instance.Get(process.Key);
                    var cbmsg = await serverSession.Call(new W2G_GMGetProcessEntityInfo());
                    json[process.Key.ToString()]["EntityInfo"] = cbmsg.Message;
                }
                response.Message =JsonHelper.ToLitJson(json);
            }
            else
            {
                response.Error = ErrorCode.ERR_Error;
            }
        }
    }
}
