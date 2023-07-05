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
                // EncryptKey = "wB1tM9fz0pbMHag0GbF0EQ==",
                //可设置异步通知接收服务地址（可选）
                NotifyUrl = "http://xiyou.henhaoji.games:8080/AliPayNotifyUrl",

                // 支付宝公钥 RSA公钥
                // 为支付宝开放平台-支付宝公钥
                // “公钥证书”方式时，留空
                // “普通公钥”方式时，必填
                AlipayPublicKey = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAwo57w9RzfTzmCCkAQ5W9Dr9GOyIie18FK41ffXYlYRnUzRf//TH8Z3DeXB8lH4xLH8QF21KmhKWe/lujHA1WUoAbFecUv1tZIu+hvQ8jF+ZyohRuDw+soGdrJ+kJrM3FyGFnnQvWqP5/APPRNU5tISeuKOOjrH6hlYlRwga9CujYb+aeCUs4G0zNH/rx/fKeuD28dsU/vVjkMumSBl2fj4ZlmS2g4/VCmriwoR1ja3VNkm9K8ladO1QdRW3R3awmM8PkXK7e6OUUQQZEMi9Kx85MYUoFLUUr06vH23uZSKn5MqXx1en+QMAjAIMrbK4IAh4Bbiphw4GubCDPX+U4SwIDAQAB",

                // 支付宝公钥证书
                // 可为证书文件路径 / 证书文件的base64字符串
                // “公钥证书”方式时，必填
                // “普通公钥”方式时，留空
                // AlipayPublicCert = "",
                
                // 应用私钥 RSA私钥
                // 为“支付宝开放平台开发助手”所生成的应用私钥
                AppPrivateKey = "MIIEvAIBADANBgkqhkiG9w0BAQEFAASCBKYwggSiAgEAAoIBAQDCjnvD1HN9POYIKQBDlb0Ov0Y7IiJ7XwUrjV99diVhGdTNF//9MfxncN5cHyUfjEsfxAXbUqaEpZ7+W6McDVZSgBsV5xS/W1ki76G9DyMX5nKiFG4PD6ygZ2sn6QmszcXIYWedC9ao/n8A89E1Tm0hJ64o46OsfqGViVHCBr0K6Nhv5p4JSzgbTM0f+vH98p64Pbx2xT+9WOQy6ZIGXZ+PhmWZLaDj9UKauLChHWNrdU2Sb0ryVp07VB1FbdHdrCYzw+Rcrt7o5RRBBkQyL0rHzkxhSgUtRSvTq8fbe5lIqfkypfHV6f5AwCMAgytsrggCHgFuKmHDga5sIM9f5ThLAgMBAAECggEAJRQWjknNAM88X83AmSDOeSMG9XoZ7D09tQEqc7SyhwDvR28NgGmoWuZt2kytPIf2QUWQgC4OQjV2Sa+ZNF1uWCbGArSZhaaZJElbH7bkz0dCDZWrK/+mvKM5DtAg4egNi5TUtF9vN6HY/ot5EZmyvqDbVjucE+HGVcNn63xxRsSobq6UPFsvO5saLDnBj1ZrvGnPHaNGx85oyJPhOioMZ9mj2JL5/Zjap4XV0CfTY5SA1ep/Mf35dSeOFsf7G12FaHOeTza9vFBtV8UeAzr1kYlvU2XP4lqWDf6+3pq7NUdQp4kjo71GfKnmwKE1ZiDO3aKtf0Eh6qAxjhDbtC87AQKBgQDmkE4IO9n3e2ux4bQo/jq0BmWihw3ylmrHymuLzyAtZXa/t13fLQOVN1ynx4dVTk91TLWAZlsoUQ+XSDCj7Uk6J3Y8O93bVixRWWW9EV85YawsVp+K+gy0eMJCvu2Xquptdqlzszgg4crbsOwIAev/DrnbipuCWbZoPpFak7y0wQKBgQDYBT9oMwjPrFWSTNSVaFUaJbL4s9vGwWhW4nkmb2o9SSH6XSRasB83Ez/i2Y10uyLigxCM17aEohl/GHTuk9pRdvyOshLJte5jD0H2QKTtWyrrwj/ULQVhfno/b+a387MLXlJuNlpCLGfyhuP1UFA39dwxU8iRCjzSCkHSE2J0CwKBgH2K08Jt/IhiHsjz8epkS8ictxihWzndJ3V0Rc0R0h6F4fwQNz//PbUxOPVuksUjZ+aiBy5MDZTNVqT0PO/1k2rj8+BmZK46pNUCzX/+hpAzG9HktOiysNpP6s73MV3lRdKmyyvhyU02RQQMuOi/SyZNeWwOdBCtEsJ+Vx0v1o/BAoGAevn5z9sF0BweluvwNaIwmHMPwO+7VRnzyUqih5Pz2jHRCxONR6duDc+CliUdl2+Ve3f7qwJ+oGEbvLPylYNMTQY83wtXEMfmjzQ3a/X/LjSxaYerCKIcpxT2iTiuEtjEe9tVd/KvTW60Omg6TARNtp3bnaVBz/gRCc3XDL4GVWMCgYB/FtjQWwLwEelLYonpn8MhijV9VG9bvJeMZfTaNRwEnEIoyIvFv5xcPkswYcp+BJfm9PChCfs03YJ3ZZUGQVqcPtTsX+Jz/dlD/qGWRUR9c2jF/+LRcOXRGQs/pP6CruippUdPPgff8Olz+cH77OV7Uq6WFwHhVi8q3xgLAEqPdg==",

                // 应用公钥证书
                // 可为证书文件路径 / 证书文件的base64字符串
                // “公钥证书”方式时，必填
                // “普通公钥”方式时，留空
                // AppPublicCert = "",

                // 支付宝根证书
                // 可为证书文件路径 / 证书文件的base64字符串
                // “公钥证书”方式时，必填
                // “普通公钥”方式时，留空
                // AlipayRootCert = ""
            };
        }
    }


    public class AliPayComponent : Entity, IAwake, IDestroy
    {
        public bool IsWorking = true;
        public static AliPayComponent Instance;
        public Pay.Alipay.AlipayOptions PayOptions;
        public Pay.Alipay.AlipayClient Client;
        public Pay.Alipay.AlipayNotifyClient Notify;
    }
}
