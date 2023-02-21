using System.Collections.Generic;
using ET.Pay.Alipay.Utility;

namespace ET.Pay.Alipay
{
    /// <summary>
    /// Alipay 上传请求接口
    /// </summary>
    public interface IAlipayUploadRequest<T> : IAlipayRequest<T> where T : AlipayResponse
    {
        /// <summary>
        /// 获取文件请求参数字典
        IDictionary<string, FileItem> GetFileParameters();
    }
}
