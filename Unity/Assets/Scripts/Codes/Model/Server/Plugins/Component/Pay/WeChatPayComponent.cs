using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;



namespace ET
{
    [ObjectSystem]
    public class WeChatPayComponentAwakeSystem : AwakeSystem<WeChatPayComponent>
    {
        protected override void Awake(WeChatPayComponent self)
        {
            WeChatPayComponent.Instance = self;

            self.ClientV3 = new Pay.WeChatPay.V3.WeChatPayClient();
            self.NotifyV3 = new Pay.WeChatPay.V3.WeChatPayNotifyClient(self.ClientV3, self.ClientV3._platformCertificateManager);
            self.PayOptions = new Pay.WeChatPay.WeChatPayOptions()
            {
                // 应用号
                // 如：微信公众平台AppId、微信开放平台AppId、微信小程序AppId、企业微信CorpId等
                AppId = "wx3963ed233b2e6b4b",
                // 商户号
                // 为微信支付商户平台的商户号
                MchId = "1488002892",
                // 商户API密钥
                // 为微信支付商户平台的API密钥，请注意不是APIv3密钥
                APIKey = "APIv2y4FsSRbUNrhuFy6nWqlZqfRD8KE",
                // 商户APIv3密钥
                // 为微信支付商户平台的APIv3密钥，请注意不是API密钥，v3接口必填
                APIv3Key = "wIkOFc3QwBmwwefDKPgxeIele7JuQygD",
                // 商户API证书
                // 使用V2退款、付款等接口时必填
                // 使用V3接口时必填
                // 可为证书文件路径 / 证书文件的base64字符串
                Certificate = "../Config/Crt/apiclient_cert.p12",
                // 商户API私钥
                // 当配置了P12格式证书时，已包含私钥，不必再单独配置API私钥。
                // PEM格式证书，需要单独配置。
                APIPrivateKey = "",
                // RSA公钥
                // 目前仅调用"企业付款到银行卡API [V2]"时使用，执行本示例中的"获取RSA加密公钥API [V2]"即可获取。
                RsaPublicKey = "",

                NotifyUrl = "http://xiyou.henhaoji.games:8080/WeChatPayNotifyUrl",
            };
        }
    }

    public class WeChatPayComponent : Entity, IAwake, IDestroy
    {
        public bool IsWorking = true;
        public static WeChatPayComponent Instance;
        public Pay.WeChatPay.WeChatPayOptions PayOptions;
        public Pay.WeChatPay.V3.WeChatPayClient ClientV3;
        public Pay.WeChatPay.V3.WeChatPayNotifyClient NotifyV3;

        public Pay.WeChatPay.V2.WeChatPayClient ClientV2;

    }
}
