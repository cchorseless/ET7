﻿using System.Text.Json.Serialization;

namespace ET.Pay.Alipay.Domain
{
    /// <summary>
    /// SignMerchantParams Data Structure.
    /// </summary>
    public class SignMerchantParams : AlipayObject
    {
        /// <summary>
        /// 子商户的商户id
        /// </summary>
        [JsonPropertyName("sub_merchant_id")]
        public string SubMerchantId { get; set; }

        /// <summary>
        /// 子商户的商户名称
        /// </summary>
        [JsonPropertyName("sub_merchant_name")]
        public string SubMerchantName { get; set; }

        /// <summary>
        /// 子商户的服务描述
        /// </summary>
        [JsonPropertyName("sub_merchant_service_description")]
        public string SubMerchantServiceDescription { get; set; }

        /// <summary>
        /// 子商户的服务名称
        /// </summary>
        [JsonPropertyName("sub_merchant_service_name")]
        public string SubMerchantServiceName { get; set; }
    }
}
