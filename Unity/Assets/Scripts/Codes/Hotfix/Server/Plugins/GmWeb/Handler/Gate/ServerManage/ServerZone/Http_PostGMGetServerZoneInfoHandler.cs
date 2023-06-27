using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET.Server

{
    [HttpHandler(SceneType.Http, "/GMGetServerZoneInfo")]
    public class Http_PostGMGetServerZoneInfoHandler: HttpPostHandler<C2G_GMGetServerZoneInfo, H2C_CommonResponse>
    {
        protected override async ETTask Run(Entity domain, C2G_GMGetServerZoneInfo request, H2C_CommonResponse response, long playerid)

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

            var zoneInfo = JsonHelper.GetLitArray();
            StartZoneConfigCategory.Instance.GetAll().Keys.ToList().ForEach(key => zoneInfo.Add(key));
            json["ZoneInfo"] = zoneInfo;
            var jsonLabel = JsonHelper.GetLitObject();
            foreach (var label in Enum.GetValues<EServerZoneLabel>())
            {
                jsonLabel[label.ToString()] = (int)label;
            }

            json["ServerLabel"] = jsonLabel;

            var jsonState = JsonHelper.GetLitObject();
            foreach (var state in Enum.GetValues<EServerZoneState>())
            {
                jsonState[state.ToString()] = (int)state;
            }

            json["ServerState"] = jsonState;
            response.Message = JsonHelper.ToLitJson(json);
        }
    }
}