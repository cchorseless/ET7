﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ET.Server

{
    [HttpHandler(SceneType.GmWeb, "/GMSearchServerZone")]
    public class Http_PostGMSearchServerZoneHandler: HttpPostHandler<C2G_GMSearchServerZone, G2C_GMSearchServerZone>
    {
        protected override async ETTask Run(Entity domain, C2G_GMSearchServerZone request, G2C_GMSearchServerZone response, long playerid)

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

            if (request.ServerLabel != 0 && !Enum.IsDefined(typeof (EServerZoneLabel), request.ServerLabel))
            {
                response.Error = ErrorCode.ERR_Error;
                response.Message = "error ServerLabel";
                return;
            }

            if (request.State != 0 && !Enum.IsDefined(typeof (EServerZoneState), request.State))
            {
                response.Error = ErrorCode.ERR_Error;
                response.Message = "error State";
                return;
            }

            if (request.ZoneId != 0 && !StartZoneConfigCategory.Instance.Contain(request.ZoneId))
            {
                response.Error = ErrorCode.ERR_Error;
                response.Message = "error ZoneId";
                return;
            }

            if (request.PageCount <= 0 || request.PageIndex <= 0)
            {
                response.Error = ErrorCode.ERR_Error;
                response.Message = "no PageCount no PageIndex";
                return;
            }

            if (request.PageCount > 50)
            {
                request.PageCount = 50;
            }

            var db = DBManagerComponent.Instance.GetAccountDB();
            int maxCount = request.PageIndex * request.PageCount;
            Expression<Func<TServerZone, bool>> filter = record =>
                    (request.ServerLabel == 0 || record.ServerLabel.Contains(request.ServerLabel)) &&
                    (request.State == 0 || record.State.Contains(request.State)) &&
                    (request.ZoneId == 0 || record.ZoneID == request.ZoneId);
            List<TServerZone> records = await db.Query(filter, maxCount);
            response.SearchCount = records.Count;
            if (records.Count >= maxCount)
            {
                records = records.GetRange((request.PageIndex - 1) * request.PageCount, request.PageCount);
            }
            else
            {
                records = records.GetRange((records.Count / request.PageCount) * request.PageCount, records.Count % request.PageCount);
            }

            response.SearchResult = new List<string>();
            records.ForEach(record => response.SearchResult.Add(MongoHelper.ToClientJson(record)));
        }
    }
}