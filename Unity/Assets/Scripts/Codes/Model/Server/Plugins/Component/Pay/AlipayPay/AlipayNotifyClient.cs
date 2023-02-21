using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using ET.Pay.Alipay.Parser;
using ET.Pay.Alipay.Utility;

namespace ET.Pay.Alipay
{
    public class AlipayNotifyClient : IAlipayNotifyClient
    {
        #region AlipayNotifyClient Constructors

        public AlipayNotifyClient()
        {

        }

        #endregion

        #region IAlipayNotifyClient Members

        public async ETTask<T> ExecuteAsync<T>(HttpListenerRequest request, AlipayOptions options) where T : AlipayNotify
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            if (string.IsNullOrEmpty(options.SignType))
            {
                throw new AlipayException($"options.{nameof(AlipayOptions.SignType)} is Empty!");
            }

            if (string.IsNullOrEmpty(options.AlipayPublicKey))
            {
                throw new AlipayException($"options.{nameof(AlipayOptions.AlipayPublicKey)} is Empty!");
            }

            var parameters = await GetParametersAsync(request);
            return await ExecuteAsync<T>(parameters, options);
        }

        #endregion

        #region IAlipayNotifyClient Members

        public ETTask<T> CertificateExecuteAsync<T>(HttpListenerRequest request, AlipayOptions options) where T : AlipayNotify
        {
            return ExecuteAsync<T>(request, options);
        }

        #endregion

        #region IAlipayNotifyClient Members

        public async ETTask<IDictionary<string, string>> GetParametersAsync(HttpListenerRequest request)
        {
            var parameters = new Dictionary<string, string>();
            if (request.HttpMethod.ToLower() == "post")
            {
                var form = await request.ReadAsFormAsync();
                foreach (var iter in form)
                {
                    parameters.Add(iter.Key, iter.Value);
                }
            }
            else
            {
                foreach (var iter in request.QueryString.AllKeys)
                {
                    parameters.Add(iter, request.QueryString.Get(iter));
                }
            }
            return parameters;
        }

        #endregion

        #region IAlipayNotifyClient Members

        public async ETTask<T> ExecuteAsync<T>(IDictionary<string, string> parameters, AlipayOptions options) where T : AlipayNotify
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            if (string.IsNullOrEmpty(options.SignType))
            {
                throw new AlipayException($"options.{nameof(AlipayOptions.SignType)} is Empty!");
            }

            if (string.IsNullOrEmpty(options.AlipayPublicKey))
            {
                throw new AlipayException($"options.{nameof(AlipayOptions.AlipayPublicKey)} is Empty!");
            }

            var notify = AlipayDictionaryParser.Parse<T>(parameters);
            CheckNotifySign(parameters, options);
            await ETTask.CompletedTask;
            return notify;
        }

        #endregion

        #region IAlipayNotifyClient Members

        public ETTask<T> CertificateExecuteAsync<T>(IDictionary<string, string> parameters, AlipayOptions options) where T : AlipayNotify
        {
            return ExecuteAsync<T>(parameters, options);
        }

        #endregion

        #region Common Method

        private static void CheckNotifySign(IDictionary<string, string> dictionary, AlipayOptions options)
        {
            if (dictionary == null || dictionary.Count == 0)
            {
                throw new AlipayException("sign check fail: dictionary)} is Empty!");
            }

            if (!dictionary.TryGetValue(AlipayConstants.SIGN, out var sign))
            {
                throw new AlipayException("sign check fail: sign)} is Empty!");
            }

            dictionary.Remove(AlipayConstants.SIGN);
            dictionary.Remove(AlipayConstants.SIGN_TYPE);
            var content = AlipaySignature.GetSignContent(dictionary);
            if (!AlipaySignature.RSACheckContent(content, sign, options.AlipayPublicKey, options.SignType))
            {
                throw new AlipayException("sign check fail: check Sign and Data Fail!");
            }
        }

        #endregion
    }
}
