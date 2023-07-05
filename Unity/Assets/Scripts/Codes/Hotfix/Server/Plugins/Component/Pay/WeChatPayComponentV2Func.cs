using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ET.Pay.WeChatPay.V2;
using ET.Pay.WeChatPay.V2.Request;

namespace ET.Server
{
    public struct TWeChatPayQrCodePay
    {
        public string OutTradeNo { get; set; }

        public string Body { get; set; }

        public int TotalFee { get; set; }

        public string SpBillCreateIp { get; set; }

        public string NotifyUrl { get; set; }

        public string TradeType { get; set; }

        public string ProfitSharing { get; set; }
    }

    public static class WeChatPayComponentV2Func
    {
        public static async ETTask<string> GetQrCodePayV2(this WeChatPayComponent self, TWeChatPayQrCodePay viewModel)
        {
            string qrcode = "";
            var request = new WeChatPayUnifiedOrderRequest()
            {
                Body = viewModel.Body,
                OutTradeNo = viewModel.OutTradeNo,
                TotalFee = viewModel.TotalFee,
                SpBillCreateIp = viewModel.SpBillCreateIp,
                NotifyUrl = viewModel.NotifyUrl,
                TradeType = viewModel.TradeType,
                ProfitSharing = viewModel.ProfitSharing
            };
            var response = await self.ClientV2.ExecuteAsync(request, self.PayOptions);
            // response.CodeUrl 给前端生成二维码
            qrcode = response.CodeUrl;
            //ViewData["response"] = response.Body;
            return qrcode;
        }

        /// <summary>
        /// 查询订单
        /// </summary>
        /// <param name="self"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        public static async ETTask<bool> QueryOrderStateV2(this WeChatPayComponent self, TPayOrderItem order)
        {
            var request = new WeChatPayOrderQueryRequest();
            request.OutTradeNo = order.Id.ToString();
            var response = await self.ClientV2.ExecuteAsync(request, self.PayOptions);
            if (response != null)
            {
                if (response.ReturnCode.ToUpper() == EPayOrderResult.SUCCESS &&
                    response.ResultCode.ToUpper() == EPayOrderResult.SUCCESS &&
                    response.TradeState.ToUpper() == EPayOrderResult.SUCCESS)
                {
                    return true;
                }
            }

            return false;
        }
    }
}