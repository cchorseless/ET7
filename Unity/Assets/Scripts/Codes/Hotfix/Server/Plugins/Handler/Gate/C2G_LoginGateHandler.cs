using System;


namespace ET.Server
{
    [MessageHandler(SceneType.Gate)]
    public class C2G_LoginGateHandler : AMRpcHandler<C2G_LoginGate, G2C_LoginGate>
    {
        protected override async ETTask Run(Session session, C2G_LoginGate request, G2C_LoginGate response)
        {
            await ETTask.CompletedTask;
            Scene scene = session.DomainScene();
            var sceneConfig = StartSceneConfigCategory.Instance.GetBySceneName(scene.Zone, scene.Name);
            if (sceneConfig.ServerMin > request.ServerId || sceneConfig.ServerMax < request.ServerId)
            {
                response.Error = ErrorCode.ERR_LoginError;
                return;
            }
            string account = scene.GetComponent<GateSessionKeyComponent>().Get(request.Key);
            var accountDB = DBManagerComponent.Instance.GetAccountDB();
            TAccountInfo accountInfo = await accountDB.Query<TAccountInfo>(request.UserId);
            if (account == null || accountInfo == null)
            {
                response.Error = ErrorCore.ERR_ConnectGateKeyError;
                response.Message = "Gate key验证失败!";
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
            PlayerComponent playerComponent = scene.GetComponent<PlayerComponent>();
            Player player = playerComponent.Get(request.UserId);
            /// 重复登录
            if (player != null)
            {
                if (player.IsOnline)
                {
                    using (await CoroutineLockComponent.Instance.Wait(CoroutineLockType.Login, request.UserId))
                    {
                        if (player.UnitId != 0)
                        {
                            //发送重复登陆消息，通知对方客户端下线
                            await ActorLocationSenderComponent.Instance.Call(player.UnitId, new G2M_AgainLogin()
                            {
                                IsKnockOut = true
                            });
                            player.UnitId = 0;
                        }
                        var _session = player.GetMySession();
                        if (_session != null)
                        {
                            MessageHelper.SendToClient(player, new G2C_KnockOutClient() { Info = "REPEAT LOGIN" });
                            player.GetMySession()?.Dispose();
                        }
                        await player.GetComponent<PlayerLoginOutComponent>().KnockOutGate();
                    }
                }
                else
                {
                    if (player.UnitId != 0)
                    {
                        // 断线重连，更新session
                        await ActorLocationSenderComponent.Instance.Call(player.UnitId, new G2M_AgainLogin()
                        {
                            IsKnockOut = false,
                            ActorId = session.InstanceId
                        });
                    }
                }
            }
            if (player == null || player.IsDisposed)
            {
                player = playerComponent.AddChildWithId<Player, string>(request.UserId, account);
                player.AddComponent<GateMapComponent>();
                player.AddComponent<PlayerLoginOutComponent>().LogOutType = (int)ELogOutHandlerType.LogOutPlayer;
                playerComponent.Add(player);
            }
            player.GetComponent<PlayerLoginOutComponent>().ReConnectKey = request.Key;
            player.IsOnline = true;
            player.GmLevel = accountInfo.GmLevel;
            /// 可以直接发协议给客户端
            player.GateSessionActorId = session.InstanceId;
            session.RemoveComponent<SessionAcceptTimeoutComponent>();
            var sessionPlayer = session.AddComponent<SessionPlayerComponent>();
            sessionPlayer.PlayerId = player.Id;
            sessionPlayer.ServerId = request.ServerId;
            session.AddComponent<MailBoxComponent, MailboxType>(MailboxType.GateSession);
            response.PlayerId = player.Id;
        }



    }
}