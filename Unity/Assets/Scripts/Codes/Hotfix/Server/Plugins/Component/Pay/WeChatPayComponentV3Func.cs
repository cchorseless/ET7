using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ET.Pay.WeChatPay.V3;
using ET.Pay.WeChatPay.V3.Domain;
using ET.Pay.WeChatPay.V3.Request;

namespace ET.Server
{
    public static class WeChatPayComponentV3Func
    {
       
        public static async ETTask<TPayOrderItem> QueryOrder(this WeChatPayComponent self, long orderid)
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

        public static async ETTask<string> GetQrCodePayV3(this WeChatPayComponent self, TCharacter character, string title, int money, string label = "")
        {
            string qrcode = "";
            var order = TPayOrderItemFunc.CreateOrder(character, title, money, (int)EPayOrderSourceType.WeChat_QrCodeV3, label);
            var model = ObjectPool.Instance.Fetch<WeChatPayTransactionsNativeBodyModel>();
            model.AppId = self.PayOptions.AppId;
            model.MchId = self.PayOptions.MchId;
            model.Amount = new Amount { Total = money, Currency = "CNY" };
            model.Description = title;
            model.NotifyUrl = self.PayOptions.NotifyUrl;
            model.OutTradeNo = order.Id.ToString();
            try
            {
                var request = new WeChatPayTransactionsNativeRequest();
                request.SetBodyModel(model);
                var response = await self.ClientV3.ExecuteAsync(request, self.PayOptions);
                if (!response.IsError)
                {
                    order.State.Add((int)EPayOrderState.CreateSuccess);
                    qrcode = response.CodeUrl;
                }
                else
                {
                    order.State.Add((int)EPayOrderState.CreateFail);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                order.State.Add((int)EPayOrderState.CreateError);
            }
            ObjectPool.Instance.Recycle(model);
            await order.SaveAndExit();
            return qrcode;
        }

        /// <summary>
        /// 公众号支付-JSAPI下单 todo
        /// </summary>
        public static async ETTask<string> GetJSPubPayV3(this WeChatPayComponent self, TCharacter character, string openid, string title, int money, string label = "")
        {
            string qrcode = "";
            var order = TPayOrderItemFunc.CreateOrder(character, title, money, (int)EPayOrderSourceType.WeChat_QrCodeV3, label);
            var model = ObjectPool.Instance.Fetch<WeChatPayTransactionsJsApiBodyModel>();
            model.AppId = self.PayOptions.AppId;
            model.MchId = self.PayOptions.MchId;
            model.Amount = new Amount { Total = money, Currency = "CNY" };
            model.Description = title;
            model.NotifyUrl = self.PayOptions.NotifyUrl;
            model.OutTradeNo = order.Id.ToString();
            model.Payer = new PayerInfo { OpenId = openid };
            try
            {
                var request = new WeChatPayTransactionsJsApiRequest();
                request.SetBodyModel(model);
                var response = await self.ClientV3.ExecuteAsync(request, self.PayOptions);
                if (!response.IsError)
                {
                    order.State.Add((int)EPayOrderState.CreateSuccess);
                    var req = new WeChatPayJsApiSdkRequest
                    {
                        Package = "prepay_id=" + response.PrepayId
                    };
                    var parameter = await self.ClientV3.ExecuteAsync(req, self.PayOptions);

                    // 将参数(parameter)给 公众号前端
                    // https://pay.weixin.qq.com/wiki/doc/apiv3/apis/chapter3_1_4.shtml
                    //ViewData["parameter"] = JsonSerializer.Serialize(parameter);
                    //ViewData["response"] = response.Body;
                }
                else
                {
                    order.State.Add((int)EPayOrderState.CreateFail);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                order.State.Add((int)EPayOrderState.CreateError);
            }

            ObjectPool.Instance.Recycle(model);
            await order.SaveAndExit();
            return qrcode;
        }



        public static async ETTask<string> GetH5PayV3(this WeChatPayComponent self, TCharacter character, string title, int money, string label = "")
        {
            string h5url = "";
            var order = TPayOrderItemFunc.CreateOrder(character, title, money, (int)EPayOrderSourceType.WeChat_H5PayV3, label);
            string payerClientIp = character.GetMyPlayer().GetMySession().RemoteAddress.Address.ToString();
            var model = ObjectPool.Instance.Fetch<WeChatPayTransactionsH5BodyModel>();
            model.AppId = self.PayOptions.AppId;
            model.MchId = self.PayOptions.MchId;
            model.Amount = new Amount { Total = money, Currency = "CNY" };
            model.Description = title;
            model.NotifyUrl = self.PayOptions.NotifyUrl;
            model.OutTradeNo = order.Id.ToString();
            model.SceneInfo = new SceneInfo
            {
                PayerClientIp = payerClientIp,
                H5Info = new H5Info { Type = "Wap" }
            };
            try
            {
                var request = new WeChatPayTransactionsH5Request();
                request.SetBodyModel(model);
                var response = await self.ClientV3.ExecuteAsync(request, self.PayOptions);
                if (!response.IsError)
                {
                    order.State.Add((int)EPayOrderState.CreateSuccess);
                    h5url = response.H5Url;
                }
                else
                {
                    order.State.Add((int)EPayOrderState.CreateFail);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                order.State.Add((int)EPayOrderState.CreateError);
            }
            ObjectPool.Instance.Recycle(model);
            await order.SaveAndExit();
            return h5url;
        }

    }
}
