using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET.Server
{
    public static class EPayOrderLabel
    {
        public static readonly string PayForBattlePass = "PayForBattlePass";
        public static readonly string PayForMemberShip = "PayForMemberShip";
        public static readonly string PayForMetaStone = "PayForMetaStone";
        //public static readonly string PayForBattlePass = "PayForBattlePass";
        //public static readonly string PayForBattlePass = "PayForBattlePass";
        //public static readonly string PayForBattlePass = "PayForBattlePass";
        //public static readonly string PayForBattlePass = "PayForBattlePass";
    }

    public static class TPayOrderItemFunc
    {
        public static TPayOrderItem CreateOrder(TCharacter character, string title, int money, int payOrderSource, string label = "")
        {
            var order = Entity.CreateOne<TPayOrderItem>();
            order.Title = title;
            order.ProcessId = Options.Instance.Process;
            order.CreateTime = TimeHelper.ServerNow();
            order.TotalAmount = money;
            order.Label = label;
            order.PayOrderSource = payOrderSource;
            order.CharacterId = character.Id;
            order.PlayerId = character.Int64PlayerId;
            return order;
        }

        public static async ETTask SaveAndExit(this TPayOrderItem self)
        {
            await DBManagerComponent.Instance.GetAccountDB().Save(self);
            self.Dispose();
        }

        public static async ETTask SyncOrderState(this TPayOrderItem self)
        {
            Session serverSession = NetInnerComponent.Instance.Get(self.ProcessId);
            await serverSession.Call(new Actor_SyncOrderStateRequest()
            {
                Entitys = MongoHelper.Serialize(self)
            });
        }

        public static bool IsPaySuccess(this TPayOrderItem self)
        {
            return self.State.Contains((int)EPayOrderState.PaySuccess);
        }
    }
}
