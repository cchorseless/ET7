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

            var role_json=JsonHelper.GetLitArray();
            var accountDB = DBManagerComponent.Instance.GetAccountDB();
            var GmCharacterDatas = await accountDB.Query<GmCharacterDataComponent>(data => true);
            foreach (var CharacterData in GmCharacterDatas)
            {
                var _json = JsonHelper.FromEntity(CharacterData);
                var account = await accountDB.QueryOne<TAccountInfo>(accountInfo => CharacterData.Int64PlayerId == accountInfo.Id);
                if (account != null)
                {
                    _json["LastLoginTime"] = account.LastLoginTime;
                    _json["GmLevel"] = account.GmLevel;
                }
                role_json.Add(_json);
            }
            response.Message = JsonHelper.ToLitJson(role_json);
        }
    }
}