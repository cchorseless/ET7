using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET.Server
{
    public static class EPayOrderResult
    {
        //微信
        public static readonly string SUCCESS = "SUCCESS";
        public static readonly string REFUND = "REFUND";
        public static readonly string NOTPAY = "NOTPAY";
        public static readonly string CLOSED = "CLOSED";
        public static readonly string REVOKED = "REVOKED";
        public static readonly string USERPAYING = "USERPAYING";
        public static readonly string PAYERROR = "PAYERROR";

        // 支付宝   
        public static readonly string WAIT_BUYER_PAY = "WAIT_BUYER_PAY";
        public static readonly string TRADE_CLOSED = "TRADE_CLOSED";
        public static readonly string TRADE_SUCCESS = "TRADE_SUCCESS";
        public static readonly string TRADE_FINISHED = "TRADE_FINISHED";
    }

    public static class TPayOrderItemFunc
    {
        public static void LoadData(this TPayOrderItem self, TCharacter character, string title, int money, FItemInfo itemInfo, int payOrderSource,
        string label = "")
        {
            self.Title = title;
            self.GateActorId = character.DomainScene().InstanceId;
            self.CreateTime = TimeHelper.ServerNow();
            self.TotalAmount = money;
            self.ItemConfigId = itemInfo.ItemConfigId;
            self.ItemCount = itemInfo.ItemCount;
            self.Label = label;
            self.PayOrderSource = payOrderSource;
            self.CharacterId = character.Id;
            self.PlayerId = character.Int64PlayerId;
            self.Account = character.Name;
        }

        public static async ETTask CheckOrderState(this TPayOrderItem self)
        {
            await TimerComponent.Instance.WaitAsync(5000);
            if (self.IsDisposed)
            {
                return;
            }

            var isfinish = true;
            switch (self.PayOrderSource)
            {
                case (int)EPayOrderSourceType.AliPay_QrCode:
                    isfinish = await AliPayComponent.Instance.QueryOrderState(self);
                    break;
                case (int)EPayOrderSourceType.WeChat_QrCodeV3:
                    isfinish = await WeChatPayComponent.Instance.QueryOrderStateV3(self);
                    break;
            }

            if (isfinish)
            {
                self.PayFinishAddItem();
                await self.SaveAndExit();
            }
            // 订单超时 最大15min
            else if (TimeHelper.ServerNow() - self.CreateTime >= 15 * 60 * 1000)
            {
                switch (self.PayOrderSource)
                {
                    case (int)EPayOrderSourceType.AliPay_QrCode:
                        await AliPayComponent.Instance.CloseOrder(self);
                        return;
                    case (int)EPayOrderSourceType.WeChat_QrCodeV3:
                        await WeChatPayComponent.Instance.CloseOrderV3(self);
                        return;
                }
            }
            else
            {
                self.CheckOrderState().Coroutine();
            }
        }

        public static async ETTask SaveAndExit(this TPayOrderItem self, bool bexit = true)
        {
            await DBManagerComponent.Instance.GetAccountDB().Save(self);
            if (bexit)
            {
                self.Dispose();
            }
        }

        /// <summary>
        /// 更新订单状态
        /// </summary>
        /// <param name="self"></param>
        /// <param name="TradeStatus"></param>
        /// <returns>订单是否结束</returns>
        public static bool UpdateOrderState(this TPayOrderItem self, string TradeStatus)
        {
            self.ErrorMsg = TradeStatus;
            if (self.PayOrderSource == (int)EPayOrderSourceType.AliPay_QrCode)
            {
                if (TradeStatus == EPayOrderResult.TRADE_SUCCESS)
                {
                    self.State.Add((int)EPayOrderState.PaySuccess);
                    self.PayTime = TimeHelper.ServerNow();
                    return true;
                }
                else if (TradeStatus == EPayOrderResult.WAIT_BUYER_PAY)
                {
                    self.State.Add((int)EPayOrderState.PayWait);
                    return false;
                }
                else if (TradeStatus == EPayOrderResult.TRADE_CLOSED)
                {
                    self.State.Add((int)EPayOrderState.PayFail);
                    return true;
                }
                else if (TradeStatus == EPayOrderResult.TRADE_FINISHED)
                {
                    return true;
                }
            }

            else if (self.PayOrderSource == (int)EPayOrderSourceType.WeChat_QrCodeV3)
            {
                if (TradeStatus == EPayOrderResult.SUCCESS)
                {
                    self.State.Add((int)EPayOrderState.PaySuccess);
                    self.PayTime = TimeHelper.ServerNow();
                    return true;
                }
                else if (TradeStatus == EPayOrderResult.NOTPAY)
                {
                    self.State.Add((int)EPayOrderState.PayWait);
                    return false;
                }
                else if (TradeStatus == EPayOrderResult.USERPAYING)
                {
                    self.State.Add((int)EPayOrderState.PayWait);
                    return false;
                }
                else if (TradeStatus == EPayOrderResult.CLOSED)
                {
                    self.State.Add((int)EPayOrderState.PayFail);
                    return true;
                }
                else if (TradeStatus == EPayOrderResult.PAYERROR)
                {
                    self.State.Add((int)EPayOrderState.PayFail);
                    return true;
                }
                else if (TradeStatus == EPayOrderResult.REFUND)
                {
                    self.State.Add((int)EPayOrderState.PayFail);
                    return true;
                }
                else if (TradeStatus == EPayOrderResult.REVOKED)
                {
                    self.State.Add((int)EPayOrderState.PayFail);
                    return true;
                }
            }

            return false;
        }

        public static void SyncOrderState(this TPayOrderItem self, string state)
        {
            ActorMessageSenderComponent.Instance.Send(self.GateActorId,
                new Actor_SyncOrderStateRequest() { OrderId = self.Id, OrderPaySource = self.PayOrderSource, OrderState = state, });
        }

        public static void PayFinishAddItem(this TPayOrderItem self)
        {
            if (!self.State.Contains((int)EPayOrderState.PaySuccess))
            {
                return;
            }

            if (self.State.Contains((int)EPayOrderState.PayAddItemSuccess))
            {
                Log.Error("pay success repeat add item");
                return;
            }

            var scene = self.DomainScene();
            PlayerComponent playerComponent = scene.GetComponent<PlayerComponent>();
            if (playerComponent == null)
            {
                return;
            }

            Player player = playerComponent.Get(self.PlayerId);
            if (player == null)
            {
                return;
            }

            TCharacter character = player.GetMyCharacter();
            var r = character.BagComp.AddTItemOrMoney(self.ItemConfigId, self.ItemCount);
            if (r.Item1 == ErrorCode.ERR_Success)
            {
                self.State.Add((int)EPayOrderState.PayAddItemSuccess);
            }
            else
            {
                self.State.Add((int)EPayOrderState.PayAddItemFail);
                Log.Error($"PayFinishAddItem Fail , Order = {self.Id} , ItemConfigId = {self.ItemConfigId} ItemCount = {self.ItemCount}");
            }
            // 统计订单收入
            character.GetMyServerZone().DataStatisticComp.GetCurDataItem().UpdateOrderIncome(self.PayOrderSource, self.TotalAmount);
            // 统计订单物品
            character.GetMyServerZone().DataStatisticComp.GetCurDataItem().UpdateShopSellItem(self.ItemConfigId, self.ItemCount);
        }
    }
}