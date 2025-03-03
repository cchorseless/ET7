﻿using ET.Pay.WeChatPay.V3.Response;

namespace ET.Pay.WeChatPay.V3.Request
{
    /// <summary>
    /// 基础支付(服务商) - JSAPI支付、小程序支付 - 统一下单
    /// </summary>
    /// <remarks>
    /// <para><a href="https://pay.weixin.qq.com/wiki/doc/apiv3_partner/apis/chapter4_1_1.shtml">JSAPI支付 - 统一下单API</a> - 最新更新时间：2020.05.26</para>
    /// <para><a href="https://pay.weixin.qq.com/wiki/doc/apiv3_partner/apis/chapter4_5_1.shtml">小程序支付 - 统一下单API</a> - 最新更新时间：2020.05.26</para>
    /// </remarks>
    public class WeChatPayPartnerTransactionsJsApiRequest : IWeChatPayPostRequest<WeChatPayPartnerTransactionsJsApiResponse>
    {
        private WeChatPayObject bodyModel;

        public string GetRequestUrl()
        {
            return "https://api.mch.weixin.qq.com/v3/pay/partner/transactions/jsapi";
        }

        public WeChatPayObject GetBodyModel()
        {
            return bodyModel;
        }

        public void SetBodyModel(WeChatPayObject bodyModel)
        {
            this.bodyModel = bodyModel;
        }
    }
}
