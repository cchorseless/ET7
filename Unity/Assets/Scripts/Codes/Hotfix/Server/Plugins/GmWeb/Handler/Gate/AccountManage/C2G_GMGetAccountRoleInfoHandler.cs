using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET.Server

{
    [MessageHandler(SceneType.Gate)]
    public class C2G_GMGetAccountRoleInfoHandler : AMRpcHandler<C2G_GMGetAccountRoleInfo, G2C_GMGetAccountRoleInfo>
    {
        protected override async ETTask Run(Session session, C2G_GMGetAccountRoleInfo request, G2C_GMGetAccountRoleInfo response)
        {
            await ETTask.CompletedTask;
            try
            {
                Player player = session.GetComponent<SessionPlayerComponent>().GetMyPlayer();
                if (!player.HasGmRolePermission(EGmPlayerRole.GmRole_Admin))
                {
                    response.Error = ErrorCode.ERR_Error;
                    response.Message = "no Permission";
                    return;
                }
                var json = JsonHelper.GetLitObject();
                var enumRoleInfo = JsonHelper.GetLitObject();
                foreach (EGmPlayerRole role in Enum.GetValues(typeof(EGmPlayerRole)))
                {
                    if ((int)role > (int)EGmPlayerRole.GmRole_PlayerGm)
                    {
                        enumRoleInfo[role.ToString()] = (int)role;
                    }
                }
                var gmAccountInfo = JsonHelper.GetLitArray();
                var accountDB = DBManagerComponent.Instance.GetAccountDB();
                var db = DBManagerComponent.Instance.GetZoneDB(session.DomainZone());
                List<TAccountInfo> accountInfos = await accountDB.Query<TAccountInfo>(account =>
                account.Account != "admin" &&
                account.GmLevel > (int)EGmPlayerRole.GmRole_PlayerGm);
                foreach (var accountInfo in accountInfos)
                {
                    var comp = await db.QueryOne<TCharacter>(character => character.Int64PlayerId == accountInfo.Id);
                    if (comp != null)
                    {
                        var dataComp = comp.GetUnActiveComponent<GmCharacterDataComponent>();
                        var _jsonInfo = JsonHelper.GetLitObject();
                        _jsonInfo["Id"] = comp.Id.ToString();
                        _jsonInfo["Account"] = accountInfo.Account;
                        _jsonInfo["GmLevel"] = accountInfo.GmLevel;
                        _jsonInfo["LastLoginTime"] = accountInfo.LastLoginTime;
                        _jsonInfo["Description"] = dataComp.Description;
                        if (dataComp.Routes != null)
                        {
                            var routesInfo = JsonHelper.GetLitArray();
                            dataComp.Routes.ForEach(route => routesInfo.Add(route));
                            _jsonInfo["Routes"] = routesInfo;
                        }
                        gmAccountInfo.Add(_jsonInfo);
                    }
                }
                json["Role"] = enumRoleInfo;
                json["Account"] = gmAccountInfo;
                response.Message = JsonHelper.ToLitJson(json);
            }
            catch (Exception e)
            {
                ReplyError(response, e);
                Log.Error(e);
            }

        }
    }
}
