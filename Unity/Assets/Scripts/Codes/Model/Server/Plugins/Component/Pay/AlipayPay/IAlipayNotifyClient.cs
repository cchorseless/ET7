using System.Collections.Generic;
using System.Net;

namespace ET.Pay.Alipay
{
    /// <summary>
    /// Alipay 通知客户端
    /// </summary>
    public interface IAlipayNotifyClient
    {
        /// <summary>
        /// 执行 Alipay 通知请求解析。
        /// </summary>
        /// <typeparam name="T">领域对象</typeparam>
        /// <param name="request">控制器的请求</param>
        /// <param name="options">配置选项</param>
        /// <returns>领域对象</returns>
        ETTask<T> ExecuteAsync<T>(HttpListenerRequest request, AlipayOptions options) where T : AlipayNotify;

        /// <summary>
        /// 执行 Alipay 通知请求解析。
        /// </summary>
        /// <typeparam name="T">领域对象</typeparam>
        /// <param name="request">控制器的请求</param>
        /// <param name="options">配置选项</param>
        /// <returns>领域对象</returns>
        ETTask<T> CertificateExecuteAsync<T>(HttpListenerRequest request, AlipayOptions options) where T : AlipayNotify;

        /// <summary>
        /// 获取通知参数
        /// </summary>
        /// <param name="request"></param>
        ETTask<IDictionary<string, string>> GetParametersAsync(HttpListenerRequest request);

        /// <summary>
        /// 执行 Alipay 通知请求解析。
        /// </summary>
        /// <typeparam name="T">领域对象</typeparam>
        /// <param name="parameters">通知参数</param>
        /// <param name="options">配置选项</param>
        /// <returns>领域对象</returns>
        ETTask<T> ExecuteAsync<T>(IDictionary<string, string> parameters, AlipayOptions options) where T : AlipayNotify;

        /// <summary>
        /// 执行 Alipay 通知请求解析。
        /// </summary>
        /// <typeparam name="T">领域对象</typeparam>
        /// <param name="parameters">通知参数</param>
        /// <param name="options">配置选项</param>
        /// <returns>领域对象</returns>
        ETTask<T> CertificateExecuteAsync<T>(IDictionary<string, string> parameters, AlipayOptions options) where T : AlipayNotify;
    }
}
