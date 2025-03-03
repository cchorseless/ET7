﻿
namespace ET.Pay.WeChatPay.V3
{
    /// <summary>
    /// WeChatPay 客户端
    /// </summary>
    public interface IWeChatPayClient
    {
        /// <summary>
        /// 执行 WeChatPay V3 Sdk请求。
        /// </summary>
        /// <param name="request">请求对象</param>
        /// <param name="options">配置选项</param>
        /// <returns>响应字典</returns>
        ETTask<WeChatPayDictionary> ExecuteAsync(IWeChatPaySdkRequest request, WeChatPayOptions options);

        /// <summary>
        /// 执行 WeChatPay V3 Get请求。
        /// </summary>
        /// <param name="request">请求对象</param>
        /// <param name="options">配置选项</param>
        /// <returns>响应对象</returns>
        ETTask<T> ExecuteAsync<T>(IWeChatPayGetRequest<T> request, WeChatPayOptions options) where T : WeChatPayResponse;

        /// <summary>
        /// 执行 WeChatPay V3 Post请求。
        /// </summary>
        /// <param name="request">请求对象</param>
        /// <param name="options">配置选项</param>
        /// <returns>响应对象</returns>
        ETTask<T> ExecuteAsync<T>(IWeChatPayPostRequest<T> request, WeChatPayOptions options) where T : WeChatPayResponse;

        /// <summary>
        /// 执行 WeChatPay V3 Post敏感信息请求。
        /// </summary>
        /// <param name="request">请求对象</param>
        /// <param name="options">配置选项</param>
        /// <returns>响应对象</returns>
        ETTask<T> ExecuteAsync<T>(IWeChatPayPrivacyPostRequest<T> request, WeChatPayOptions options) where T : WeChatPayResponse;
    }
}
