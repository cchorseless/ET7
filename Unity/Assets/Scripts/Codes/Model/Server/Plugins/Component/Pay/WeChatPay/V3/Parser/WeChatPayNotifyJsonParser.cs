﻿using System;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using ET.Pay.Security;
using ET.Pay.WeChatPay.V3.Domain;

namespace ET.Pay.WeChatPay.V3.Parser
{
    public class WeChatPayNotifyJsonParser<T> where T : WeChatPayNotify
    {

        private static readonly JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions() { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };

        /// <summary>
        /// 将加密报文解密并反序列化
        /// </summary>
        /// <remarks>
        /// <a href="https://pay.weixin.qq.com/wiki/doc/apiv3/wechatpay/wechatpay4_2.shtml">证书和回调报文解密</a>
        /// </remarks>
        public T Parse(string body, string v3Key)
        {
            T result = null;
            NotifyCiphertext notifyCiphertext = default;
            string resourcePlaintext;

            try
            {
                if (body.StartsWith("{") && body.EndsWith("}"))
                {
                    notifyCiphertext = JsonSerializer.Deserialize<NotifyCiphertext>(body, jsonSerializerOptions);
                }
            }
            catch { }

            switch (notifyCiphertext.Resource.Algorithm)
            {
                case nameof(AEAD_AES_256_GCM):
                    {
                        resourcePlaintext = AEAD_AES_256_GCM.Decrypt(notifyCiphertext.Resource.Nonce, notifyCiphertext.Resource.Ciphertext, notifyCiphertext.Resource.AssociatedData, v3Key);
                    }
                    break;
                default:
                    throw new WeChatPayException("Unknown algorithm!");
            }

            try
            {
                result = JsonSerializer.Deserialize<T>(resourcePlaintext, jsonSerializerOptions);
            }
            catch { }

            if (result == null)
            {
                result = Activator.CreateInstance<T>();
            }

            result.Body = body;
            result.NotifyCiphertext = notifyCiphertext;
            result.ResourcePlaintext = resourcePlaintext;

            return result;
        }
    }
}
