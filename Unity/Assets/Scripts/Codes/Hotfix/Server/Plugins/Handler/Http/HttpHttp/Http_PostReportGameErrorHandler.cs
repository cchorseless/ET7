using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace ET.Server
{
    [HttpHandler(SceneType.Http, "/ReportGameError", false)]
    public class Http_PostReportGameErrorHandler: HttpPostHandler<C2H_ReportGameError, H2C_CommonResponse>
    {
        protected override async ETTask Run(Entity domain, C2H_ReportGameError request, H2C_CommonResponse response, long playerid)
        {
            var json = JsonHelper.GetLitObject();
            json["GameId"] = request.GameId;
            json["GameTime"] = request.GameTime;
            json["Label"] = request.Label;
            json["TimeSpan"] = TimeHelper.ServerNow().ToString();
            DBLogger.Instance.ClientError(JsonHelper.ToLitJson(json), request.ErrorMsg);
            response.Error = ErrorCode.ERR_Success;
            await ETTask.CompletedTask;
        }
    }
}