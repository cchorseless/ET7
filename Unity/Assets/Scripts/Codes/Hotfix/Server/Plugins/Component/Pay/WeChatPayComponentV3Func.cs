﻿using System;
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
        public static async ETTask<TPayOrderItem> GetOrder(this WeChatPayComponent self, long orderid)
        {
            if (self.GetChild<TPayOrderItem>(orderid) != null)
            {
                return self.GetChild<TPayOrderItem>(orderid);
            }
            else
            {
                var order = await DBManagerComponent.Instance.GetAccountDB().Query<TPayOrderItem>(orderid);
                if (order != null)
                {
                    self.AddChild(order);
                }

                return order;
            }
        }

        public static async ETTask<(long, string)> GetQrCodePayV3(this WeChatPayComponent self, TCharacter character, string title, int money,
        FItemInfo itemInfo, string label = "")
        {
            string QrCode = "";
            long orderid = 0;
            if (!self.IsWorking)
            {
                return (orderid, "WeChatPay Not Working");
            }

            var order = self.AddChild<TPayOrderItem>();
            order.LoadData(character, title, money, itemInfo, (int)EPayOrderSourceType.WeChat_QrCodeV3, label);
            var model = new WeChatPayTransactionsNativeBodyModel();
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
                    QrCode = response.CodeUrl;
                }
                else
                {
                    order.State.Add((int)EPayOrderState.CreateFail);
                    order.ErrorMsg = $"{response.Code} {response.Message} | {response.Detail} |{MongoHelper.ToJson(response)}";
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                order.State.Add((int)EPayOrderState.CreateError);
            }

            if (string.IsNullOrEmpty(QrCode))
            {
                await order.SaveAndExit();
            }
            else
            {
                orderid = order.Id;
                await order.SaveAndExit(false);
                order.CheckOrderState().Coroutine();
            }

            return (orderid, QrCode);
        }

        /// <summary>
        /// 公众号支付-JSAPI下单 todo
        /// </summary>
        public static async ETTask<string> GetJSPubPayV3(this WeChatPayComponent self, TCharacter character, string openid, string title, int money,
        FItemInfo itemInfo, string label = "")
        {
            string qrcode = "";
            var order = self.AddChild<TPayOrderItem>();
            order.LoadData(character, title, money, itemInfo, (int)EPayOrderSourceType.WeChat_QrCodeV3, label);
            var model = new WeChatPayTransactionsJsApiBodyModel();
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
                    var req = new WeChatPayJsApiSdkRequest { Package = "prepay_id=" + response.PrepayId };
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

        public static async ETTask<(long, string)> GetH5PayV3(this WeChatPayComponent self, TCharacter character, string title, int money,
        FItemInfo itemInfo, string label = "")
        {
            string h5url = "";
            long orderid = 0;
            if (!self.IsWorking)
            {
                return (orderid, "WeChatPay Not Working");
            }

            var order = self.AddChild<TPayOrderItem>();
            order.LoadData(character, title, money, itemInfo, (int)EPayOrderSourceType.WeChat_H5PayV3, label);
            string payerClientIp = character.GetMyPlayer().GetMySession().RemoteAddress.Address.ToString();
            var model = new WeChatPayTransactionsH5BodyModel();
            model.AppId = self.PayOptions.AppId;
            model.MchId = self.PayOptions.MchId;
            model.Amount = new Amount { Total = money, Currency = "CNY" };
            model.Description = title;
            model.NotifyUrl = self.PayOptions.NotifyUrl;
            model.OutTradeNo = order.Id.ToString();
            model.SceneInfo = new SceneInfo { PayerClientIp = payerClientIp, H5Info = new H5Info { Type = "Wap" } };
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
            if (string.IsNullOrEmpty(h5url))
            {
                await order.SaveAndExit();
            }
            else
            {
                orderid = order.Id;
                await order.SaveAndExit(false);
                order.CheckOrderState().Coroutine();
            }

            return (orderid, h5url);
        }

        /// <summary>
        /// 查询订单
        /// </summary>
        /// <param name="self"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        public static async ETTask<bool> QueryOrderStateV3(this WeChatPayComponent self, TPayOrderItem order)
        {
            var model = new WeChatPayPartnerTransactionsOutTradeNoQueryModel() { SpMchId = self.PayOptions.MchId, };
            var request = new WeChatPayPartnerTransactionsOutTradeNoRequest();
            request.OutTradeNo = order.Id.ToString();
            request.SetQueryModel(model);
            var response = await self.ClientV3.ExecuteAsync(request, self.PayOptions);
            if (response != null && !response.IsError)
            {
                var TradeStatus = response.TradeState.ToUpper();
                return order.UpdateOrderState(TradeStatus);
            }

            return false;
        }

        public static async ETTask CloseOrderV3(this WeChatPayComponent self, TPayOrderItem order)
        {
            var model = new WeChatPayTransactionsOutTradeNoCloseBodyModel() { MchId = self.PayOptions.MchId, };
            var request = new WeChatPayTransactionsOutTradeNoCloseRequest();
            request.OutTradeNo = order.Id.ToString();
            request.SetBodyModel(model);
            order.State.Add((int)EPayOrderState.PayFail);
            order.ErrorMsg = "CloseOrder";
            await order.SaveAndExit();
            await self.ClientV3.ExecuteAsync(request, self.PayOptions);
        }
    }
}