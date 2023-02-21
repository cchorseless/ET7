using System;


namespace ET.Server
{
    [MessageHandler(SceneType.Gate)]
    public class C2G_ReLoginGateHandler : AMRpcHandler<C2G_ReLoginGate, G2C_ReLoginGate>
    {
        protected override async ETTask Run(Session session, C2G_ReLoginGate request, G2C_ReLoginGate response)
        {
            await ETTask.CompletedTask;
            Scene scene = session.DomainScene();
            PlayerComponent playerComponent = scene.GetComponent<PlayerComponent>();
            Player player = playerComponent.GetChild<Player>(request.UserId);
            if (player == null || player.IsDisposed || player.IsOnline)
            {
                response.Error = ErrorCore.ERR_ConnectGateKeyError;
                response.Message = "cant find player";
                return;
            }
            var sceneConfig = StartSceneConfigCategory.Instance.GetBySceneName(scene.Zone, scene.Name);
            if (sceneConfig.ServerMin > request.ServerId || sceneConfig.ServerMax < request.ServerId)
            {
                response.Error = ErrorCode.ERR_LoginError;
                return;
            }
            var ReConnectKey = player.GetComponent<PlayerLoginOutComponent>().ReConnectKey;
            if (ReConnectKey == 0 || ReConnectKey != request.Key)
            {
                response.Error = ErrorCore.ERR_ConnectGateKeyError;
                response.Message = "Gate key验证失败!";
                return;
            }
            var accountDB = DBManagerComponent.Instance.GetAccountDB();
            if (string.IsNullOrEmpty(request.Password) ||
                 string.IsNullOrEmpty(request.Account))
            {
                response.Error = ErrorCode.ERR_LoginError;
                return;
            }
            TAccountInfo newAccount = await accountDB.QueryOne<TAccountInfo>(account => account.Account == request.Account);
            if (newAccount.Password != request.Password ||
                   newAccount.GmLevel > (int)EGmPlayerRole.GmRole_PlayerGm)
            {
                response.Error = ErrorCode.ERR_LoginError;
                return;
            }

            TServerZone serverZone = await accountDB.QueryOne<TServerZone>(server =>
            server.ServerID == request.ServerId &&
            server.State.Contains((int)EServerZoneState.Working));
            if (serverZone == null)
            {
                response.Error = ErrorCode.ERR_LoginError;
                return;
            }
            if (player.UnitId != 0)
            {
                // 断线重连，更新session
                await ActorLocationSenderComponent.Instance.Call(player.UnitId, new G2M_AgainLogin()
                {
                    IsKnockOut = false,
                    ActorId = session.InstanceId
                });
            }
            player.IsOnline = true;
            // 可以直接发协议给客户端
            player.GateSessionActorId = session.InstanceId;
            session.RemoveComponent<SessionAcceptTimeoutComponent>();
            var sessionPlayer = session.AddComponent<SessionPlayerComponent>();
            sessionPlayer.PlayerId = player.Id;
            sessionPlayer.ServerId = request.ServerId;
            session.AddComponent<MailBoxComponent, MailboxType>(MailboxType.GateSession);
            response.PlayerId = player.Id;
            player.GetComponent<PlayerLoginOutComponent>().CancelLogOutGate();
        }


    }
}