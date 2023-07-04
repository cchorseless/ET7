using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace ET.Server

{
    [HttpHandler(SceneType.GmWeb, "/GMSearchOrder")]
    public class Http_PostGMSearchOrderHandler: HttpPostHandler<C2G_GMSearchOrder, G2C_GMSearchOrder>
    {
        protected override async ETTask Run(Entity domain, C2G_GMSearchOrder request, G2C_GMSearchOrder response, long playerid)
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

            if (request.Account == null)
            {
                request.Account = "";
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

            var db = DBManagerComponent.Instance.GetAccountDB();
            int maxCount = request.PageIndex * request.PageCount;
            Expression<Func<TPayOrderItem, bool>> filter = record =>
                    (record.CreateTime >= startTime) &&
                    (record.CreateTime <= endTime) &&
                    (request.Account == "" || record.Account == request.Account) &&
                    (request.ItemConfigId == 0 || record.ItemConfigId == request.ItemConfigId) &&
                    (request.PayType == 0 || record.PayOrderSource == request.PayType) &&
                    (request.OrderState == 0 || record.State.Contains(request.OrderState));
            var records = await db.Query<TPayOrderItem>(filter, maxCount);
            response.SearchCount = (int)await db.QueryCount<TPayOrderItem>(filter);
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