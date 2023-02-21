using System;


namespace ET.Server
{
    [MessageHandler(SceneType.Gate)]
    public class C2G_GMLoginGateHandler : AMRpcHandler<C2G_GMLoginGate, G2C_GMLoginGate>
    {
        protected override async ETTask Run(Session session, C2G_GMLoginGate request, G2C_GMLoginGate response)
        {
            Scene scene = session.DomainScene();
            string account = scene.GetComponent<GateSessionKeyComponent>().Get(request.Key);
            TAccountInfo accountInfo = await DBManagerComponent.Instance.GetAccountDB().Query<TAccountInfo>(request.UserId);
            if (account == null || accountInfo == null)
            {
                response.Error = ErrorCore.ERR_ConnectGateKeyError;
                response.Message = "Gate key验证失败!";
                await ETTask.CompletedTask;
                return;
            }
            PlayerComponent playerComponent = scene.GetComponent<PlayerComponent>();
            Player player = playerComponent.Get(request.UserId);
            /// 重复登录
            if (player != null)
            {
                using (await CoroutineLockComponent.Instance.Wait(CoroutineLockType.Login, request.UserId))
                {
                    if (player.UnitId != 0)
                    {
                        //发送重复登陆消息，通知对方客户端下线
                        await ActorLocationSenderComponent.Instance.Call(player.UnitId, new G2M_AgainLogin());
                        player.UnitId = 0;
                    }
                    MessageHelper.SendToClient(player, new G2C_KnockOutClient() { Info = "REPEAT LOGIN" });
                    var _session = player.GetMySession();
                    if (_session != null)
                    {
                        var _sessionPlayer = _session.GetComponent<SessionPlayerComponent>();
                        _sessionPlayer.IsKnockOut = true;
                        _session.Dispose();
                    }
                }
                player.Dispose();
            }
            player = playerComponent.AddChildWithId<Player, string>(request.UserId, account);
            player.IsOnline = true;
            player.GmLevel = accountInfo.GmLevel;
            player.AddComponent<GateMapComponent>();
            player.AddComponent<PlayerLoginOutComponent>().LogOutType = (int)ELogOutHandlerType.LogOutPlayer;
            /// 可以直接发协议给客户端
            player.GateSessionActorId = session.InstanceId;
            playerComponent.Add(player);
            session.RemoveComponent<SessionAcceptTimeoutComponent>();
            var sessionPlayer = session.AddComponent<SessionPlayerComponent>();
            sessionPlayer.PlayerId = player.Id;
            /// 处理断线 直接踢掉
            sessionPlayer.IsKnockOut = true;
            session.AddComponent<MailBoxComponent, MailboxType>(MailboxType.GateSession);
            response.PlayerId = player.Id;
            await ETTask.CompletedTask;
        }
    }
}