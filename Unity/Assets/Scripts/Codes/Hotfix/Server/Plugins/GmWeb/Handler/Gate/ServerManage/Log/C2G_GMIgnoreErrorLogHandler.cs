using System;


namespace ET.Server

{

    [MessageHandler(SceneType.Gate)]
    public class C2G_GMIgnoreErrorLogHandler : AMRpcHandler<C2G_GMIgnoreErrorLog, G2C_GMIgnoreErrorLog>
    {
        protected override async ETTask Run(Session session, C2G_GMIgnoreErrorLog request, G2C_GMIgnoreErrorLog response)
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
            catch (Exception e)
            {
                ReplyError(response, e);
            }
        }
    }
}
