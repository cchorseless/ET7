using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ET.Server

{
    [HttpHandler(SceneType.GmWeb, "/GMSearchDailyDataStatistic")]
    public class Http_PostGMSearchDailyDataStatisticHandler: HttpPostHandler<C2G_GMSearchDailyDataStatistic, G2C_GMSearchDailyDataStatistic>
    {
        protected override async ETTask Run(Entity domain, C2G_GMSearchDailyDataStatistic request, G2C_GMSearchDailyDataStatistic response, long playerid)
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

            var db = DBManagerComponent.Instance.GetAccountDB();
            Expression<Func<TServerZoneDailyDataStatisticItem, bool>> filter = record =>
                    (record.Time >= startTime) &&
                    (record.Time <= endTime);
            var records = await db.Query<TServerZoneDailyDataStatisticItem>(filter);
            var tempDic = new Dictionary<string, TServerZoneDailyDataStatisticItem>();
            foreach (var entity in records)
            {
                string key = $"{entity.Year}_{entity.Month}_{entity.Day}";
                if (tempDic.TryGetValue(key, out var _entity))
                {
                    _entity.MergeData(entity);
                }
                else
                {
                    tempDic.Add(key, entity);
                }
            }

            records = tempDic.Values.ToList();
            response.Error = ErrorCode.ERR_Success;
            response.SearchCount = records.Count;
            response.SearchResult = new List<string>();
            records.ForEach(record => { response.SearchResult.Add(MongoHelper.ToClientJson(record)); });
            // 今日数据
            var now = TimeHelper.DateTimeNow();
            var year = now.Year;
            var month = now.Month;
            var day = now.Day;
            response.Message = "";
            Expression<Func<TServerZoneDailyDataStatisticItem, bool>> filterToday = record =>
                    record.Year == year && record.Month == month && record.Day == day;
            var recordToday = await db.Query<TServerZoneDailyDataStatisticItem>(filterToday);
            if (recordToday.Count > 0)
            {
                var todayRecord = recordToday[0];
                foreach (var _record in recordToday)
                {
                    if (todayRecord != _record)
                    {
                        todayRecord.MergeData(_record);
                    }
                }

                response.Message = MongoHelper.ToClientJson(todayRecord);
            }
        }
    }
}