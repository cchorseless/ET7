using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET.Server

{
    [HttpHandler(SceneType.Http, "/GMGetLogDBInfo")]
    public class Http_PostGMGetLogDBInfoHandler: HttpPostHandler<C2G_GMGetLogDBInfo, G2C_GMGetLogDBInfo>
    {
        protected override async ETTask Run(Entity domain, C2G_GMGetLogDBInfo request, G2C_GMGetLogDBInfo response, long playerid)
        {
            await ETTask.CompletedTask;
            Scene scene = domain.DomainScene();
            PlayerComponent playerComponent = scene.GetComponent<PlayerComponent>();
            Player player = playerComponent.Get(playerid);
            if (!player.HasGmRolePermission(EGmPlayerRole.GmRole_WebServerManager))
            {
                response.Error = ErrorCode.ERR_Error;
                response.Message = "no Permission";
                return;
            }

            var json = JsonHelper.GetLitObject();
            var jsonLabel = JsonHelper.GetLitArray();
            foreach (var label in Enum.GetNames(typeof (EDBLogLabel)))
            {
                jsonLabel.Add(label);
            }

            var jsonProcess = JsonHelper.GetLitArray();
            var allProcess = StartProcessConfigCategory.Instance.GetAll();
            foreach (var processInfo in allProcess)
            {
                var _data = JsonHelper.GetLitObject();
                _data["Id"] = processInfo.Value.Id.ToString();
                _data["AppName"] = processInfo.Value.AppName;
                _data["ProcessDes"] = processInfo.Value.ProcessDes;
                jsonProcess.Add(_data);
            }

            json["labels"] = jsonLabel;
            json["Process"] = jsonProcess;
            response.Message = JsonHelper.ToLitJson(json);
            StartZoneConfig startZoneConfig = StartZoneConfigCategory.Instance.Get(GameConfig.LogDBZone);
            var url = startZoneConfig.DBConnection;
            url = url.Replace("mongodb://", "");
            int endIndex = url.LastIndexOf("/");
            int startIndex = 0;
            if (endIndex < 0)
            {
                endIndex = url.Length;
            }

            if (url.Contains("@"))
            {
                startIndex = url.IndexOf("@") + 1;
            }

            url = url.Substring(startIndex, endIndex - startIndex);

            if (url.Contains(":"))
            {
                response.Ip = url.Split(":")[0];
                response.Port = url.Split(":")[1];
            }
            else
            {
                response.Ip = url;
                response.Port = "27017";
            }

            response.DbName = startZoneConfig.DBName;
        }
    }
}