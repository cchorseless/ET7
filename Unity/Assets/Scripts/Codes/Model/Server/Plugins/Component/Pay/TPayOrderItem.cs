using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ET
{
    public enum EPayOrderState
    {
        CreateSuccess = 1,
        CreateFail = 2,
        CreateError = 4,
        PaySuccess = 8,
        PayFail = 16,
        PayWait = 32,
        PayAddItemSuccess = 64,
        PayAddItemFail = 128,
    }

    public enum EPayOrderSourceType
    {
        AliPay_QrCode = 1,
        WeChat_QrCodeV3 = 2,
        WeChat_H5PayV3 = 3,
    }

    public class TPayOrderItem: Entity, IAwake
    {
        public string Title;

        public string Label;

        public int ShopSellConfigId;

        public int ItemConfigId;

        public int ItemCount;

        public long GateActorId;

        public int TotalAmount;

        public int PayOrderSource;

        // 支付sdk的订单号
        public string TransactionId;

        public long PlayerId;

        public long CharacterId;

        public long CreateTime;

        public long PayTime;

        public string ErrorMsg;

        public HashSet<int> State = new HashSet<int>();
    }
}