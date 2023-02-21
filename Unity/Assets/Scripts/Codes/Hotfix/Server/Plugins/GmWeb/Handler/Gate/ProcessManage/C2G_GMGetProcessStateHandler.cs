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

    [MessageHandler(SceneType.Gate)]
    public class C2G_GMGetProcessStateHandler : AMRpcHandler<C2G_GMGetProcessState, G2C_GMGetProcessState>
    {
        protected override async ETTask Run(Session session, C2G_GMGetProcessState request, G2C_GMGetProcessState response)
        {
            await ETTask.CompletedTask;
            try
            {
                Player player = session.GetComponent<SessionPlayerComponent>().GetMyPlayer();
                if (!player.HasGmRolePermission(EGmPlayerRole.GmRole_WebServerManager))
                {
                    response.Error = ErrorCode.ERR_Error;
                    response.Message = "no Permission";
                    return;
                }
                if (WatcherSessionComponent.Instance == null)
                {
                    response.Error = ErrorCode.ERR_Error;
                    response.Message = "cant find watch process";
                    return;
                }
                var cbmsg = await WatcherSessionComponent.Instance.GetThisMachineWatcherSession().Call(new G2W_GMGetProcessInfo());
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
                    _data["ServerTime"] = hasInfo ? cbjson[key].StartTime : "??";
                    _data["MemorySize"] = hasInfo ? cbjson[key].MemorySize : "??";
                    _data["Status"] = hasInfo ? (!cbjson[key].HasExited) : false;
                    _data["EntityInfo"] = hasInfo ? cbjson[key].EntityInfo : "";
                    json.Add(_data);
                }
                response.Message = JsonHelper.ToLitJson(json);
            }
            catch (Exception e)
            {
                ReplyError(response, e);
            }

        }
    }
}
