using System;
using ET.Pay.Alipay.Domain;
using ET.Pay.Alipay.Request;
using ET.Pay.Alipay.Response;

namespace ET.Server
{
    public static class AliPayComponentFunc
    {
        public static async ETTask<TPayOrderItem> QueryOrder(this AliPayComponent self, long orderid)
        {
            if (self.GetChild<TPayOrderItem>(orderid) != null)
            {
                await ETTask.CompletedTask;
                return self.GetChild<TPayOrderItem>(orderid);
            }
            else
            {
                var order = await DBManagerComponent.Instance.GetAccountDB().Query<TPayOrderItem>(orderid);
                self.AddChild(order);
                return order;
            }
        }
        public static async ETTask<string> GetQrCodePay(this AliPayComponent self, TCharacter character, string title, int money_fen, string label = "")
        {
            string QrCode = "";
            var order = TPayOrderItemFunc.CreateOrder(character, title, money_fen, (int)EPayOrderSourceType.AliPay_QrCode, label);
            try
            {
                var model = new AlipayTradePrecreateModel
                {
                    OutTradeNo = order.Id.ToString(),
                    Subject = title,
                    TotalAmount = (money_fen / 100.0).ToString("F2"),
                    Body = label,
                };
                var req = new AlipayTradePrecreateRequest();
                req.SetBizModel(model);
                req.SetNotifyUrl(self.PayOptions.NotifyUrl);
                AlipayTradePrecreateResponse response;
                if (self.PayOptions.AlipayPublicKey != null &&
                    self.PayOptions.AlipayPublicKey.Length > 0)
                {
                    response = await self.Client.ExecuteAsync(req, self.PayOptions);
                }
                else
                {
                    response = await self.Client.CertificateExecuteAsync(req, self.PayOptions);
                }
                if (!response.IsError)
                {
                    QrCode = response.QrCode;
                    order.State.Add((int)EPayOrderState.CreateSuccess);
                }
                else
                {
                    order.State.Add((int)EPayOrderState.CreateFail);
                    order.ErrorMsg = $"{response.Body}";
                }
            }
            catch (Exception ex)
            {
                order.State.Add((int)EPayOrderState.CreateError);
                order.ErrorMsg = $"{ex.Message}";
                Log.Error(ex);
            }
            await order.SaveAndExit();
            return QrCode;
        }



        /// <summary>
        /// APP支付异步通知
        /// </summary>
        public static async ETTask<string> GetAppPay(this AliPayComponent self, TCharacter character, string title, int money, string label = "")
        {
            var html = "";
            var order = TPayOrderItemFunc.CreateOrder(character, title, money, (int)EPayOrderSourceType.AliPay_QrCode, label);
            try
            {
                var model = new AlipayTradeAppPayModel
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
                var response = await self.Client.SdkExecuteAsync(req, self.PayOptions);
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
                    order.ErrorMsg = $"{response.Body}";
                }
            }
            catch (Exception ex)
            {
                order.State.Add((int)EPayOrderState.CreateError);
                order.ErrorMsg = $"{ex.Message}";
                Log.Error(ex);
            }
            await order.SaveAndExit();
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
    }
}
