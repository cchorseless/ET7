using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET.Server
{
    [MessageHandler(SceneType.Gate)]
    public class Actor_SyncOrderStateHandler : AMRpcHandler<Actor_SyncOrderStateRequest, Actor_SyncOrderStateResponse>
    {
        protected override async ETTask Run(Session session, Actor_SyncOrderStateRequest request, Actor_SyncOrderStateResponse response)
        {
            var order = MongoHelper.Deserialize<TPayOrderItem>(request.Entitys);
            if (order != null)
            {
                Scene scene = session.DomainScene();
                PlayerComponent playerComponent = scene.GetComponent<PlayerComponent>();
                Player player = playerComponent.Get(order.PlayerId);
                if (player != null)
                {
                    TCharacter character = player.GetMyCharacter();
                    var serverZone = character.GetMyServerZone();
                    if (order.Label == EPayOrderLabel.PayForBattlePass)
                    {
                        var activity = serverZone.ActivityComp.GetActivity<TActivityBattlePass>(EActivityType.TActivityBattlePass);
                        activity.OnBuyBattlePass(character, order);
                    }
                    else if (order.Label == EPayOrderLabel.PayForMemberShip)
                    {
                        var activity = serverZone.ActivityComp.GetActivity<TActivityMemberShip>(EActivityType.TActivityMemberShip);
                        activity.OnBuyMemberShip(character, order);
                    }
                    else if (order.Label == EPayOrderLabel.PayForMetaStone)
                    {
                        serverZone.RechargeComp.OnRechargeMetaStone(character, order);
                    }
                    else
                    {


                    }
                }
            }
            await ETTask.CompletedTask;
        }
    }
}
