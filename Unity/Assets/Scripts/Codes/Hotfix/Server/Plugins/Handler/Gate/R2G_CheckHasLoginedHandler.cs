using System;


namespace ET.Server
{
    [ActorMessageHandler(SceneType.Gate)]
    public class R2G_CheckHasLoginedHandler : AMActorRpcHandler<Scene, R2G_CheckHasLogined, G2R_CheckHasLogined>
    {
        protected override async ETTask Run(Scene scene, R2G_CheckHasLogined request, G2R_CheckHasLogined response)
        {
            PlayerComponent playerComponent = scene.GetComponent<PlayerComponent>();
            Player player = playerComponent.Get(request.PlayerId);

            if (player != null)
            {
                if (request.IsKnockOut)
                {
                    if (player.GetComponent<HttpPlayerSessionComponent>() != null)
                    {
                        await HttpKnockOut(player);
                    }
                    else
                    {
                        using (await CoroutineLockComponent.Instance.Wait(CoroutineLockType.Login, request.PlayerId))
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
                            if (player.IsOnline)
                            {
                                MessageHelper.SendToClient(player, new G2C_KnockOutClient() { Info = "REPEAT LOGIN" });
                                player.GetMySession()?.Dispose();
                            }
                            await player.GetComponent<PlayerLoginOutComponent>().KnockOutGate();
                        }
                    }
                }
                else
                {
                    response.GateId = scene.Id;
                }
                response.Error = ErrorCode.ERR_AccountHasExist;
            }
            await ETTask.CompletedTask;
        }


        private async ETTask HttpKnockOut(Player player)
        {
            using (await CoroutineLockComponent.Instance.Wait(CoroutineLockType.Login, player.Id))
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
                if (player.IsOnline)
                {
                    await player.GetComponent<PlayerLoginOutComponent>().KnockOutGate();
                }
            }
        }
    }


}