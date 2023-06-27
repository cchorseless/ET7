using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace ET.Server

{
    [HttpHandler(SceneType.Http, "/GMSearchLog")]
    public class Http_PostGMSearchLogHandler: HttpPostHandler<C2G_GMSearchLog, G2C_GMSearchLog>
    {
        protected override async ETTask Run(Entity domain, C2G_GMSearchLog request, G2C_GMSearchLog response, long playerid)
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

            if (request.ProcessId == null || request.ProcessId.Length == 0)
            {
                response.Error = ErrorCode.ERR_Error;
                response.Message = "no ProcessId";
                return;
            }

            if (request.PageCount <= 0 || request.PageIndex <= 0)
            {
                response.Error = ErrorCode.ERR_Error;
                response.Message = "no PageCount no PageIndex";
                return;
            }

            if (!Enum.IsDefined(typeof (DBLogger.EDBLogLevel), request.LogLevel))
            {
                response.Error = ErrorCode.ERR_Error;
                response.Message = "no LogLevel";
                return;
            }

            if (request.Label == null)
            {
                request.Label = "";
            }

            if (request.Title == null)
            {
                request.Title = "";
            }

            if (request.PageCount > 50)
            {
                request.PageCount = 50;
            }

            long startTime;
            long endTime;
            if (request.StartTime == 0)
            {
                startTime = TimeInfo.Instance.Transition(DateTime.Today);
            }
            else
            {
                startTime = (long)request.StartTime * 1000;
            }

            if (request.EndTime == 0)
            {
                endTime = TimeInfo.Instance.Transition(DateTime.Today.AddDays(86399F / 86400));
            }
            else
            {
                endTime = (long)request.EndTime * 1000;
            }

            var start = TimeInfo.Instance.ToDateTime(startTime);
            var end = TimeInfo.Instance.ToDateTime(endTime);
            List<string> dbCollectName = new List<string>();
            while (start < end)
            {
                dbCollectName.Add(DBLogger.GetDBCollectionName(request.ProcessId, request.LogLevel, ref start));
                start = start.AddDays(1);
            }

            var db = DBManagerComponent.Instance.GetLogDB();
            var collectNames = await db.GetCollectionNames();
            int maxCount = request.PageIndex * request.PageCount;
            List<DBLogRecord> records = new List<DBLogRecord>();
            Expression<Func<DBLogRecord, bool>> filter = record =>
                    (record.Time >= startTime) &&
                    (record.Time <= endTime) &&
                    (!record.IsIgnore) &&
                    //(record.Level == request.LogLevel) &&
                    (request.Label == "" || record.Label == request.Label) &&
                    (request.Title == "" || record.Msg.Contains(request.Title));
            foreach (var collectName in dbCollectName)
            {
                if (!collectNames.Contains(collectName))
                {
                    continue;
                }

                if (records.Count < maxCount)
                {
                    int limit = maxCount - records.Count;
                    var r = await db.Query<DBLogRecord>(filter, limit, collectName);
                    records.AddRange(r);
                }

                response.SearchCount += (int)await db.QueryCount<DBLogRecord>(filter, collectName);
            }

            if (records.Count >= maxCount)
            {
                records = records.GetRange((request.PageIndex - 1) * request.PageCount, request.PageCount);
            }
            else
            {
                records = records.GetRange((records.Count / request.PageCount) * request.PageCount, records.Count % request.PageCount);
            }

            player.GetMyCharacter().SyncClientEntity(records.ToArray());
            records.ForEach(record => { response.SearchResult.Add(record.Id.ToString()); });
        }
    }
}