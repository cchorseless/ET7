using System.Text.Json.Serialization;

namespace ET.Pay.Alipay
{
    public class AlipayOpenAppAlipaycertDownloadResponse : AlipayResponse
    {
        [JsonPropertyName("alipay_cert_content")]
        public string AlipayCertContent { get; set; }
    }
}
