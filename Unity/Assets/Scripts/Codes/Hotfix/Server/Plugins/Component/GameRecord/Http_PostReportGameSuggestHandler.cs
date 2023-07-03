using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace ET.Server
{
    [HttpHandler(SceneType.Gate, "/ReportGameSuggest")]
    public class Http_PostReportGameSuggestHandler: HttpPostHandler<C2H_ReportGameSuggest, H2C_CommonResponse>
    {
        protected override async ETTask Run(Entity domain, C2H_ReportGameSuggest request, H2C_CommonResponse response, long playerid)
        {
            Scene scene = domain.DomainScene();
            PlayerComponent playerComponent = scene.GetComponent<PlayerComponent>();
            Player player = playerComponent.Get(playerid);
            var character = player.GetMyCharacter();
            await ETTask.CompletedTask;
            if (character.GameRecordComp != null)
            {
                var record = character.GameRecordComp.GetCurGameRecord();
                if (record != null && record.SuggestCount < 10)
                {
                    var json = JsonHelper.GetLitObject();
                    json["AccountId"] = request.AccountId;
                    json["Label"] = request.Label;
                    DBLogger.Instance.ClientSuggest(JsonHelper.ToLitJson(json), request.SuggestMsg);
                    record.SuggestCount++;
                    response.Error = ErrorCode.ERR_Success;
                    return;
                }
            }
            response.Error = ErrorCode.ERR_Error;
            await ETTask.CompletedTask;
        }
    }
}