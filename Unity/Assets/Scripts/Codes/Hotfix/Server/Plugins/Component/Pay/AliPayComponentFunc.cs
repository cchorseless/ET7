using System;
using ET.Pay.Alipay;
using ET.Pay.Alipay.Domain;
using ET.Pay.Alipay.Request;
using ET.Pay.Alipay.Response;

namespace ET.Server
{
    public static class AliPayComponentFunc
    {
        public static async ETTask<T> ExecuteAsync<T>(this AliPayComponent self, IAlipayRequest<T> request) where T : AlipayResponse
        {
            if (!string.IsNullOrEmpty(self.PayOptions.AlipayPublicKey))
            {
                return await self.Client.ExecuteAsync(request, self.PayOptions);
            }
            else
            {
                return await self.Client.CertificateExecuteAsync(request, self.PayOptions);
            }
        }

        public static async ETTask<TPayOrderItem> GetOrder(this AliPayComponent self, long orderid)
        {
            if (self.GetChild<TPayOrderItem>(orderid) != null)
            {
                return self.GetChild<TPayOrderItem>(orderid);
            }
            else
            {
                var order = await DBManagerComponent.Instance.GetAccountDB().Query<TPayOrderItem>(orderid);
                self.AddChild(order);
                return order;
            }
        }

        public static async ETTask<(int, string)> GetQrCodePay(this AliPayComponent self, TCharacter character, string title, int money_fen,
        FItemInfo itemInfo, string label = "")
        {
            await ETTask.CompletedTask;
            string QrCode = "";
            int errorCode = ErrorCode.ERR_Error;
            if (!self.IsWorking)
            {
                return (errorCode, "AliPay Not Working");
            }

            var order = self.AddChild<TPayOrderItem>();
            order.LoadData(character, title, money_fen, itemInfo, (int)EPayOrderSourceType.AliPay_QrCode, label);
            try
            {
                var model = new AlipayTradePrecreateModel()
                {
                    OutTradeNo = order.Id.ToString(), 
                    Subject = title, 
                    TotalAmount = (money_fen / 100.0).ToString("F2"), 
                    Body = label,
                };
                var req = new AlipayTradePrecreateRequest();
                req.SetBizModel(model);
                req.SetNotifyUrl(self.PayOptions.NotifyUrl);
                var response = await self.ExecuteAsync(req);
                if (!response.IsError)
                {
                    QrCode = response.QrCode;
                    order.State.Add((int)EPayOrderState.CreateSuccess);
                }
                else
                {
                    order.State.Add((int)EPayOrderState.CreateFail);
                    order.ErrorMsg = $"{response.Msg} | {response.SubMsg}";
                }
            }
            catch (Exception ex)
            {
                order.State.Add((int)EPayOrderState.CreateError);
                order.ErrorMsg = $"{ex.Message}";
                Log.Error(ex);
            }

            if (string.IsNullOrEmpty(QrCode))
            {
                await order.SaveAndExit();
            }
            else
            {
                errorCode = ErrorCode.ERR_Success;
                await order.SaveAndExit(false);
                order.CheckOrderState().Coroutine();
            }

            return (errorCode, QrCode);
        }

        /// <summary>
        /// APP支付异步通知
        /// </summary>
        public static async ETTask<string> GetAppPay(this AliPayComponent self, TCharacter character, string title, int money, FItemInfo itemInfo,
        string label = "")
        {
            var html = "";
            var order = self.AddChild<TPayOrderItem>();
            order.LoadData(character, title, money, itemInfo, (int)EPayOrderSourceType.AliPay_QrCode, label);
            try
            {
                var model = new AlipayTradeAppPayModel()
                {
                    OutTradeNo = order.Id.ToString(),
                    Subject = title,
                    //ProductCode = viewModel.ProductCode,
                    TotalAmount = (money / 100.0).ToString("F2"),
                    Body = label
                };
                var req = new AlipayTradeAppPayRequest();
                req.SetBizModel(model);
                req.SetNotifyUrl(self.PayOptions.NotifyUrl);
                var response = await self.ExecuteAsync(req);
                // 将response.Body给 ios、android端，由其去完成调起支付宝APP。
                // 客户端 Android 集成流程: https://opendocs.alipay.com/open/204/105296/
                // 客户端 iOS 集成流程: https://opendocs.alipay.com/open/204/105295/
                if (!response.IsError)
                {
                    order.State.Add((int)EPayOrderState.CreateSuccess);
                    html = response.Body;
                }
                else
                {
                    order.State.Add((int)EPayOrderState.CreateFail);
                    order.ErrorMsg = $"{response.Msg} | {response.SubMsg}";
                }
            }
            catch (Exception ex)
            {
                order.State.Add((int)EPayOrderState.CreateError);
                order.ErrorMsg = $"{ex.Message}";
                Log.Error(ex);
            }

            if (string.IsNullOrEmpty(html))
            {
                await order.SaveAndExit();
            }
            else
            {
                await order.SaveAndExit(false);
                order.CheckOrderState().Coroutine();
            }

            return html;
        }

        public static async ETTask<bool> FinishPayFaceToFace(this AliPayComponent self, long orderid)
        {
            var db = DBManagerComponent.Instance.GetAccountDB();
            TPayOrderItem order = await db.Query<TPayOrderItem>(orderid);
            if (order == null)
            {
                return false;
            }

            order.State.Add((int)EPayOrderState.PaySuccess);
            await order.SaveAndExit();
            return true;
        }

        /// <summary>
        /// 查询订单
        /// </summary>
        /// <param name="self"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        public static async ETTask<bool> QueryOrderState(this AliPayComponent self, TPayOrderItem order)
        {
            var model = new AlipayTradeQueryModel() { OutTradeNo = order.Id.ToString(), };
            var request = new AlipayTradeQueryRequest();
            request.SetBizModel(model);
            var response = await self.ExecuteAsync(request);
            if (response != null && !response.IsError)
            {
                var TradeStatus = response.TradeStatus.ToUpper();
                return order.UpdateOrderState(TradeStatus);
            }

            return false;
        }

        public static async ETTask CloseOrder(this AliPayComponent self, TPayOrderItem order)
        {
            order.State.Add((int)EPayOrderState.PayFail);
            var model = new AlipayTradeCloseModel() { OutTradeNo = order.Id.ToString(), };
            var request = new AlipayTradeCloseRequest();
            request.SetBizModel(model);
            await order.SaveAndExit();
            await self.ExecuteAsync(request);
        }
    }
}