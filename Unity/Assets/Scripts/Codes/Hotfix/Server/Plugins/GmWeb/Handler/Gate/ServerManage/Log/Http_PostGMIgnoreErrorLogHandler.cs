using System;

namespace ET.Server

{
    [HttpHandler(SceneType.GmWeb, "/GMIgnoreErrorLog")]
    public class Http_PostGMIgnoreErrorLogHandler: HttpPostHandler<C2G_GMIgnoreErrorLog, H2C_CommonResponse>
    {
        protected override async ETTask Run(Entity domain, C2G_GMIgnoreErrorLog request, H2C_CommonResponse response, long playerid)
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

                if (request.LogProcess == null)
                {
                    response.Error = ErrorCode.ERR_Error;
                    response.Message = "no LogProcess";
                    return;
                }

                var date = TimeInfo.Instance.ToDateTime((long)request.LogTime * 1000);
                var collectName = DBLogger.GetDBCollectionName(request.LogProcess, (int)DBLogger.EDBLogLevel.Error, ref date);
                long logid = long.Parse(request.LogId);
                var db = DBManagerComponent.Instance.GetLogDB();
                var log = await db.Query<DBLogRecord>(record => record.Id == logid, collectName);
                if (log.Count > 0)
                {
                    log[0].IsIgnore = true;
                    await db.Save<DBLogRecord>(log[0], collectName);
                }
                else
                {
                    response.Error = ErrorCode.ERR_Error;
                    response.Message = "cant find log";
                }
            }
    }
}