using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ET.Server

{
    public struct TProcessStateInfo
    {
        public string Id { get; set; }
        public bool HasExited { get; set; }
        public string StartTime { get; set; }
        public string MemorySize { get; set; }
        public string EntityInfo { get; set; }
    }

    [HttpHandler(SceneType.GmWeb, "/GMGetProcessState")]
    public class Http_PostGMGetProcessStateHandler: HttpPostHandler<C2G_GMGetProcessState, H2C_CommonResponse>
    {
        protected override async ETTask Run(Entity domain, C2G_GMGetProcessState request, H2C_CommonResponse response, long playerid)
        {
            Scene scene = domain.DomainScene();
            PlayerComponent playerComponent = scene.GetComponent<PlayerComponent>();
            Player player = playerComponent.Get(playerid);

            if (!player.HasGmRolePermission(EGmPlayerRole.GmRole_WebServerManager))
            {
                response.Error = ErrorCode.ERR_Error;
                response.Message = "no Permission";
                return;
            }

            var cbmsg = await WatcherHelper.CallWatcher(new G2W_GMGetProcessInfo());
            Dictionary<string, TProcessStateInfo> cbjson = JsonHelper.FromLitJson<Dictionary<string, TProcessStateInfo>>(cbmsg.Message);
            var json = JsonHelper.GetLitArray();
            var allProcess = StartProcessConfigCategory.Instance.GetAll();
            foreach (var processInfo in allProcess)
            {
                var machineInfo = StartMachineConfigCategory.Instance.Get(processInfo.Value.MachineId);
                var _data = JsonHelper.GetLitObject();
                var key = processInfo.Value.Id.ToString();
                _data["Id"] = key;
                _data["InnerPort"] = processInfo.Value.InnerPort.ToString();
                _data["AppName"] = processInfo.Value.AppName;
                _data["ProcessDes"] = processInfo.Value.ProcessDes;
                _data["OuterIP"] = machineInfo.OuterIP;
                bool hasInfo = cbjson.ContainsKey(key);
                _data["ServerTime"] = hasInfo? cbjson[key].StartTime : "??";
                _data["MemorySize"] = hasInfo? cbjson[key].MemorySize : "??";
                _data["Status"] = hasInfo && (!cbjson[key].HasExited);
                _data["EntityInfo"] = hasInfo? cbjson[key].EntityInfo : "";
                json.Add(_data);
            }

            response.Message = JsonHelper.ToLitJson(json);
        }
    }
}