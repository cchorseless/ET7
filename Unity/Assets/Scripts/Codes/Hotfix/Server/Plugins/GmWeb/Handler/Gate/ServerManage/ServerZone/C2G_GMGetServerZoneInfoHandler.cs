using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET.Server

{
    [MessageHandler(SceneType.Gate)]
    public class C2G_GMGetServerZoneInfoHandler : AMRpcHandler<C2G_GMGetServerZoneInfo, G2C_GMGetServerZoneInfo>
    {
        protected override async ETTask Run(Session session, C2G_GMGetServerZoneInfo request, G2C_GMGetServerZoneInfo response)
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
            catch (Exception e)
            {
                ReplyError(response, e);
            }

        }
    }
}
