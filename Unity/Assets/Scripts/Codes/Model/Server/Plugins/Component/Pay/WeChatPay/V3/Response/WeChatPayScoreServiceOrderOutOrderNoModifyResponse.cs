﻿using System.Collections.Generic;
using System.Text.Json.Serialization;
using ET.Pay.WeChatPay.V3.Domain;

namespace ET.Pay.WeChatPay.V3.Response
{
    /// <summary>
    /// 微信支付分 - 修改订单金额 - 返回参数
    /// </summary>
    /// <remarks>
    /// <para><a href="https://pay.weixin.qq.com/wiki/doc/apiv3/apis/chapter6_1_17.shtml">微信支付分 - 修改订单金额API</a> - 最新更新时间：2020.03.05</para>
    /// </remarks>
    public class WeChatPayScoreServiceOrderOutOrderNoModifyResponse : WeChatPayResponse
    {
        /// <summary>
        /// 应用ID
        /// </summary>
        /// <remarks>
        /// 调用接口提交的公众账号ID
        /// <para>示例值：wxd678efh567hg6787</para>
        /// </remarks>
        [JsonPropertyName("appid")]
        public string AppId { get; set; }

        /// <summary>
        /// 商户号
        /// </summary>
        /// <remarks>
        /// 调用接口提交的商户号。
        /// <para>示例值：1230000109</para>
        /// </remarks>
        [JsonPropertyName("mchid")]
        public string MchId { get; set; }

        /// <summary>
        /// 服务ID
        /// </summary>
        /// <remarks>
        /// 调用该接口提交的服务ID。
        /// <para>示例值：500001</para>
        /// </remarks>
        [JsonPropertyName("service_id")]
        public string ServiceId { get; set; }

        /// <summary>
        /// 商户服务订单号
        /// </summary>
        /// <remarks>
        /// 调用接口提交的商户服务订单号。
        /// <para>示例值：1234323JKHDFE1243252</para>
        /// </remarks>
        [JsonPropertyName("out_order_no")]
        public string OutOrderNo { get; set; }

        /// <summary>
        /// 服务信息
        /// </summary>
        /// <remarks>
        /// 服务信息，用于介绍本订单所提供的服务 ，当参数长度超过20个字符时，报错处理。
        /// <para>示例值：某某酒店</para>
        /// </remarks>
        [JsonPropertyName("service_introduction")]
        public string ServiceIntroduction { get; set; }

        /// <summary>
        /// 服务订单状态
        /// </summary>
        /// <remarks>
        /// 表示当前单据状态。
        /// 枚举值：
        /// 1、CREATED：商户已创建服务订单
        /// 2、DOING：服务订单进行中
        /// 3、DONE：服务订单完成
        /// 4、REVOKED：商户取消服务订单
        /// 5、EXPIRED：服务订单已失效，"商户已创建服务订单"状态超过30天未变动，则订单失效
        /// <para>示例值：CREATED</para>
        /// </remarks>
        [JsonPropertyName("state")]
        public string State { get; set; }

        /// <summary>
        /// 订单状态说明
        /// </summary>
        /// <remarks>
        /// 对服务订单"进行中"状态的附加说明。
        /// 1、USER_CONFIRM：用户确认
        /// 2、MCH_COMPLETE：商户完结
        /// <para>示例值：MCH_COMPLETE</para>
        /// </remarks>
        [JsonPropertyName("state_description")]
        public string StateDescription { get; set; }

        /// <summary>
        /// 商户收款总金额
        /// </summary>
        /// <remarks>
        /// 总金额，大于等于0的数字，单位为分，只能为整数，详见支付金额。
        /// 此参数需满足：总金额=后付费项目金额之和-后付费商户优惠项目金额之和，且小于等于订单风险金额。取消订单时，该字段必须为0。
        /// <para>示例值：40000</para>
        /// </remarks>
        [JsonPropertyName("total_amount")]
        public long? TotalAmount { get; set; }

        /// <summary>
        /// 后付费项目
        /// </summary>
        /// <remarks>
        /// 后付费项目列表，最多包含100条付费项目。
        /// 如果传入，用户侧则显示此参数。
        /// </remarks>
        [JsonPropertyName("post_payments")]
        public List<PostPayment> PostPayments { get; set; }

        /// <summary>
        /// 后付费商户优惠
        /// </summary>
        /// <remarks>
        /// 后付费商户优惠列表，最多包含30条商户优惠。
        /// 如果传入，用户侧则显示此参数。
        /// </remarks>
        [JsonPropertyName("post_discounts")]
        public List<PostDiscount> PostDiscounts { get; set; }

        /// <summary>
        /// 订单风险金
        /// </summary>
        /// <remarks>
        /// 订单风险金信息
        /// </remarks>
        [JsonPropertyName("risk_fund")]
        public RiskFund RiskFund { get; set; }

        /// <summary>
        /// 服务时间段
        /// </summary>
        /// <remarks>
        /// 服务时间范围
        /// </remarks>
        [JsonPropertyName("time_range")]
        public TimeRange TimeRange { get; set; }

        /// <summary>
        /// 服务位置
        /// </summary>
        /// <remarks>
        /// 服务位置信息
        /// 如果传入，用户侧则显示此参数。
        /// </remarks>
        [JsonPropertyName("location")]
        public Location Location { get; set; }

        /// <summary>
        /// 商户数据包
        /// </summary>
        /// <remarks>
        /// 商户数据包可存放本订单所需信息，需要先urlencode后传入。
        /// 当商户数据包总长度超出256字符时，报错处理。商户接收回包是根据场景，决定是否需要做安全过滤(XSS/CSRF)。
        /// <para>示例值：Easdfowealsdkjfnlaksjdlfkwqoi&amp;wl3l2sald</para>
        /// </remarks>
        [JsonPropertyName("attach")]
        public string Attach { get; set; }

        /// <summary>
        /// 商户回调地址
        /// </summary>
        /// <remarks>
        /// 商户接收用户确认订单和扣款成功回调通知的地址。
        /// <para>示例值：https://api.test.com</para>
        /// </remarks>
        [JsonPropertyName("notify_url")]
        public string NotifyUrl { get; set; }

        /// <summary>
        /// 微信支付服务订单号
        /// </summary>
        /// <remarks>
        /// 微信支付服务订单号，每个微信支付服务订单号与商户号下对应的商户服务订单号一一对应。
        /// <para>示例值：15646546545165651651</para>
        /// </remarks>
        [JsonPropertyName("order_id")]
        public string OrderId { get; set; }

        /// <summary>
        /// 是否需要收款
        /// </summary>
        /// <remarks>
        /// true：微信支付分代收款
        /// false：无需微信支付分代收款
        /// <para>示例值：true</para>
        /// </remarks>
        [JsonPropertyName("need_collection")]
        public bool NeedCollection { get; set; }

        /// <summary>
        /// 收款信息
        /// </summary>
        /// <remarks>
        /// 服务使用信息
        /// </remarks>
        [JsonPropertyName("collection")]
        public Collection Collection { get; set; }
    }
}
