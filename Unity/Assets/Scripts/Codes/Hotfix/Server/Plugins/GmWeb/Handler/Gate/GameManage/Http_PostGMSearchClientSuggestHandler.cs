using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace ET.Server

{
    [HttpHandler(SceneType.GmWeb, "/GMSearchClientSuggest")]
    public class Http_PostGMSearchClientSuggestHandler: HttpPostHandler<C2G_GMSearchClientSuggest, G2C_GMSearchClientSuggest>
    {
        protected override async ETTask Run(Entity domain, C2G_GMSearchClientSuggest request, G2C_GMSearchClientSuggest response, long playerid)
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

            if (request.PageCount <= 0 || request.PageIndex <= 0)
            {
                response.Error = ErrorCode.ERR_Error;
                response.Message = "no PageCount no PageIndex";
                return;
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
                dbCollectName.Add(DBLogManagerComponent.Instance.GetDBCollectionName("", (int)DBLogger.EDBLogLevel.ClientSuggest, start));
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
                    (request.Label == "" || record.Label.Contains(request.Label)) &&
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

            response.SearchResult = new List<string>();
            records.ForEach(record => { response.SearchResult.Add(MongoHelper.ToClientJson(record)); });
        }
    }
}