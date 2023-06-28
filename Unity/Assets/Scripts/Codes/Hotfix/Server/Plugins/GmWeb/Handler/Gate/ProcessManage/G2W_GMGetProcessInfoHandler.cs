using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET.Server

{
    [ActorMessageHandler(SceneType.Manager)]
    public class G2W_GMGetProcessInfoHandler: AMActorRpcHandler<Scene, G2W_GMGetProcessInfo, W2G_GMGetProcessInfo>
    {
        protected override async ETTask Run(Scene scene, G2W_GMGetProcessInfo request, W2G_GMGetProcessInfo response)
        {
            await ETTask.CompletedTask;
            var watchComp = WatcherComponent.Instance;
            if (watchComp == null)
            {
                response.Error = ErrorCode.ERR_Error;
                response.Message = "this is no watcher";
                return;
            }

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
                var config = StartProcessConfigCategory.Instance.Get(process.Key);
                if (config.IsWatcher() || config.IsRobot())
                {
                    continue;
                }

                // if (config.Id == GameConfig.GmWebProcessID)
                // {
                //     continue;
                // }

                var cbmsg = await WatcherHelper.CallManage(process.Key, new W2P_GMGetProcessEntityInfo());
                json[process.Key.ToString()]["EntityInfo"] = cbmsg.Message;
            }

            response.Message = JsonHelper.ToLitJson(json);
        }
    }
}