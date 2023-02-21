using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using ET.Pay.Security;
using ET.Pay.WeChatPay.V3.Parser;

namespace ET.Pay.WeChatPay.V3
{
    public class WeChatPayNotifyClient : IWeChatPayNotifyClient
    {
        #region WeChatPayNotifyClient Constructors

        private readonly IWeChatPayClient _client;
        private readonly WeChatPayPlatformCertificateManager _platformCertificateManager;

        public WeChatPayNotifyClient(IWeChatPayClient client, WeChatPayPlatformCertificateManager platformCertificateManager)
        {
            _client = client;
            _platformCertificateManager = platformCertificateManager;
        }
        #endregion

        #region IWeChatPayNotifyClient Members

        public async ETTask<T> ExecuteAsync<T>(HttpListenerRequest request, WeChatPayOptions options) where T : WeChatPayNotify
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            var headers = GetWeChatPayHeadersFromRequest(request);
            using (var reader = new StreamReader(request.InputStream, Encoding.UTF8, true, 1024, true))
            {
                var body = await reader.ReadToEndAsync();
                return await ExecuteAsync<T>(headers, body, options);
            }
        }

        private static WeChatPayHeaders GetWeChatPayHeadersFromRequest(HttpListenerRequest request)
        {
            var headers = new WeChatPayHeaders();
            headers.Serial = request.Headers.Get(WeChatPayConsts.Wechatpay_Serial) ?? null;
            headers.Timestamp = request.Headers.Get(WeChatPayConsts.Wechatpay_Timestamp) ?? null;
            headers.Nonce = request.Headers.Get(WeChatPayConsts.Wechatpay_Nonce) ?? null;
            headers.Signature = request.Headers.Get(WeChatPayConsts.Wechatpay_Signature) ?? null;
            return headers;
        }

        #endregion

        #region IWeChatPayNotifyClient Members

        public async ETTask<T> ExecuteAsync<T>(WeChatPayHeaders headers, string body, WeChatPayOptions options) where T : WeChatPayNotify
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            if (string.IsNullOrEmpty(options.APIv3Key))
            {
                throw new WeChatPayException($"options.{nameof(options.APIv3Key)} is Empty!");
            }

            await CheckNotifySignAsync(headers, body, options);

            var parser = new WeChatPayNotifyJsonParser<T>();
            var notify = parser.Parse(body, options.APIv3Key);

            return notify;
        }

        #endregion

        #region Check Notify Method

        private async ETTask CheckNotifySignAsync(WeChatPayHeaders headers, string body, WeChatPayOptions options)
        {
            if (string.IsNullOrEmpty(headers.Serial))
            {
                throw new WeChatPayException($"sign check fail: {nameof(headers.Serial)} is empty!");
            }

            if (string.IsNullOrEmpty(headers.Signature))
            {
                throw new WeChatPayException($"sign check fail: {nameof(headers.Signature)} is empty!");
            }

            if (string.IsNullOrEmpty(body))
            {
                throw new WeChatPayException("sign check fail: body is empty!");
            }

            var cert = await _platformCertificateManager.GetCertificateAsync(_client, options, headers.Serial);
            var signSourceData = WeChatPayUtility.BuildSignatureSourceData(headers.Timestamp, headers.Nonce, body);
            var signCheck = SHA256WithRSA.Verify(cert.Certificate.GetRSAPublicKey(), signSourceData, headers.Signature);
            if (!signCheck)
            {
                throw new WeChatPayException("sign check fail: check Sign and Data Fail!");
            }
        }

        #endregion
    }
}
