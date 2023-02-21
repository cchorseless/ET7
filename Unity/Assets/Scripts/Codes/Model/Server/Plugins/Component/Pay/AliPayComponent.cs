using System;


namespace ET
{

    [ObjectSystem]
    public class AliPayComponentAwakeSystem : AwakeSystem<AliPayComponent>
    {
        protected override void Awake(AliPayComponent self)
        {
            AliPayComponent.Instance = self;
            self.Client = new Pay.Alipay.AlipayClient();
            self.Notify = new Pay.Alipay.AlipayNotifyClient();
            self.PayOptions = new Pay.Alipay.AlipayOptions()
            {
                // 注意: 
                // 若涉及资金类支出接口(如转账、红包等)接入，必须使用“公钥证书”方式。不涉及到资金类接口，也可以使用“普通公钥”方式进行加签。
                // 本示例默认的加签方式为“公钥证书”方式，并调用 CertificateExecuteAsync 方法 执行API。
                // 若使用“普通公钥”方式，除了遵守下方注释的规则外，调用 CertificateExecuteAsync 也需改成 ExecuteAsync。
                // 支付宝后台密钥/证书官方配置教程：https://opendocs.alipay.com/open/291/105971
                // 密钥格式：请选择 PKCS1(非JAVA适用)，切记 切记 切记

                // 应用Id
                // 为支付宝开放平台-APPID
                AppId = "2021003127644680",
                //可设置AES密钥，调用AES加解密相关接口时需要（可选）
                EncryptKey = "wB1tM9fz0pbMHag0GbF0EQ==",
                //可设置异步通知接收服务地址（可选）
                NotifyUrl = "http://henhaoji.games:8080/AliPayNotifyUrl",

                // 支付宝公钥 RSA公钥
                // 为支付宝开放平台-支付宝公钥
                // “公钥证书”方式时，留空
                // “普通公钥”方式时，必填
                AlipayPublicKey = "",

                // 应用私钥 RSA私钥
                // 为“支付宝开放平台开发助手”所生成的应用私钥
                AppPrivateKey = "",

                // 应用公钥证书
                // 可为证书文件路径 / 证书文件的base64字符串
                // “公钥证书”方式时，必填
                // “普通公钥”方式时，留空
                AppPublicCert = "",

                // 支付宝公钥证书
                // 可为证书文件路径 / 证书文件的base64字符串
                // “公钥证书”方式时，必填
                // “普通公钥”方式时，留空
                AlipayPublicCert = "",

                // 支付宝根证书
                // 可为证书文件路径 / 证书文件的base64字符串
                // “公钥证书”方式时，必填
                // “普通公钥”方式时，留空
                AlipayRootCert = ""
            };
        }
    }


    public class AliPayComponent : Entity, IAwake, IDestroy
    {
        public static AliPayComponent Instance;
        public Pay.Alipay.AlipayOptions PayOptions;
        public Pay.Alipay.AlipayClient Client;
        public Pay.Alipay.AlipayNotifyClient Notify;
    }
}
