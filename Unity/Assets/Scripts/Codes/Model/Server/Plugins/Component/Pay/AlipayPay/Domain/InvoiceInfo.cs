﻿using System.Text.Json.Serialization;

namespace ET.Pay.Alipay.Domain
{
    /// <summary>
    /// InvoiceInfo Data Structure.
    /// </summary>
    public class InvoiceInfo : AlipayObject
    {
        /// <summary>
        /// 开票内容  注：json数组格式
        /// </summary>
        [JsonPropertyName("details")]
        public string Details { get; set; }

        /// <summary>
        /// 开票关键信息
        /// </summary>
        [JsonPropertyName("key_info")]
        public InvoiceKeyInfo KeyInfo { get; set; }
    }
}
