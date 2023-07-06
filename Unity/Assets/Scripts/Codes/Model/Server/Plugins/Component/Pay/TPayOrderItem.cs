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
        PayAddItemByEmail = 256,
    }

    public enum EPayOrderSourceType
    {
        AliPay_QrCode = 1000,
        WeChat_QrCodeV3 = 2000,
        WeChat_H5PayV3 = 2001,
        Paypal_H5Pay = 3000,
        YooMoney_H5Pay = 4000,
    }

    public class TPayOrderItem: Entity, IAwake
    {
        public string Title;

        public string Label;

        public int ShopSellConfigId;

        public int ItemConfigId;

        public int ItemCount;

        public int GateId;

        public int TotalAmount;

        public int PayOrderSource;

        // 支付sdk的订单号
        public string TransactionId;

        public long PlayerId;

        public long CharacterId;

        public long CreateTime;

        public string Account;

        public long PayTime;

        public string ErrorMsg;

        public HashSet<int> State = new HashSet<int>();
    }
}