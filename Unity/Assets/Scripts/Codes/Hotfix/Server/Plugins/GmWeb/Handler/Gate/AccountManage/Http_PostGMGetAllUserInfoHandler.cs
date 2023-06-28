using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET.Server

{
    [HttpHandler(SceneType.GmWeb, "/GMGetAllUserInfo")]
    public class Http_PostGMGetAllUserInfoHandler: HttpPostHandler<C2G_GMGetAllUserInfo, H2C_CommonResponse>
    {
        protected override async ETTask Run(Entity domain, C2G_GMGetAllUserInfo request, H2C_CommonResponse response, long playerid)
        {
            Scene scene = domain.DomainScene();
            PlayerComponent playerComponent = scene.GetComponent<PlayerComponent>();
            Player player = playerComponent.Get(playerid);
            if (!player.HasGmRolePermission(EGmPlayerRole.GmRole_Admin))
            {
                response.Error = ErrorCode.ERR_Error;
                response.Message = "no Permission";
                return;
            }

            var json = JsonHelper.GetLitObject();
            var gmAccountInfo = JsonHelper.GetLitArray();
            var gmCharacterDataInfo = JsonHelper.GetLitArray();
            var accountDB = DBManagerComponent.Instance.GetAccountDB();
            List<TAccountInfo> accountInfos = await accountDB.Query<TAccountInfo>(account =>
                    account.Account != "admin" &&
                    account.GmLevel > (int)EGmPlayerRole.GmRole_PlayerGm);
            foreach (var accountInfo in accountInfos)
            {
                gmAccountInfo.Add(MongoHelper.ToClientJson(accountInfo));
                var dataComp = await accountDB.QueryOne<GmCharacterDataComponent>(CharacterData => CharacterData.Int64PlayerId == accountInfo.Id);
                if (dataComp != null)
                {
                    gmCharacterDataInfo.Add(MongoHelper.ToClientJson(dataComp));
                }
            }
            json["Account"] = gmAccountInfo;
            json["Character"] = gmCharacterDataInfo;
            response.Message = JsonHelper.ToLitJson(json);
        }
    }
}