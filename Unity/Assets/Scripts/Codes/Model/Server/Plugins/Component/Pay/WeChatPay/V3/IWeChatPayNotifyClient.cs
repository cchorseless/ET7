﻿using System.Net;

namespace ET.Pay.WeChatPay.V3
{
    /// <summary>
    /// WeChatPay 通知客户端
    /// </summary>
    public interface IWeChatPayNotifyClient
    {
        /// <summary>
        /// 执行 WeChatPay V3 通知请求解析
        /// </summary>
        /// <typeparam name="T">领域对象</typeparam>
        /// <param name="request">控制器的请求</param>
        /// <param name="options">配置选项</param>
        /// <returns>领域对象</returns>
        ETTask<T> ExecuteAsync<T>(HttpListenerRequest request, WeChatPayOptions options) where T : WeChatPayNotify;
        /// <summary>
        /// 执行 WeChatPay V3 通知请求解析
        /// </summary>
        /// <typeparam name="T">领域对象</typeparam>
        /// <param name="headers">微信Http头信息</param>
        /// <param name="body">通知内容</param>
        /// <param name="options">配置选项</param>
        /// <returns>领域对象</returns>
        ETTask<T> ExecuteAsync<T>(WeChatPayHeaders headers, string body, WeChatPayOptions options) where T : WeChatPayNotify;
    }
}
