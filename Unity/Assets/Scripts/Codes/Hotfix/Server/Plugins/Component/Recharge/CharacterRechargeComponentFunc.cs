using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET.Server
{
    public static class CharacterRechargeComponentFunc
    {
        public static void LoadAllChild(this CharacterRechargeComponent self)
        {
        }

        public static void RechargeMetaStoneSuccess(this CharacterRechargeComponent self, TPayOrderItem payOrder)
        {
            self.Character.BagComp.AddTItemOrMoney(EMoneyType.MetaStone, payOrder.TotalAmount / 10);
            self.TotalCharge += payOrder.TotalAmount / 100;
        }
    }
}
