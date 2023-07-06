using System;

namespace ET
{
    public static class GameConfig
    {
        /// <summary>
        /// 数据同步客户端是否压缩
        /// </summary>
        public static bool SyncClientCompress = true;

        /// <summary>
        /// 数据同步客户端转成base64字符串
        /// </summary>
        public static bool SyncClientToBase64 = true;

        /// <summary>
        /// 处理同步给客户端字符串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string DealSyncClientString(string str)
        {
            if (SyncClientCompress || SyncClientToBase64)
            {
                var bytes = SyncClientCompress? ZipHelper.Compress(str.ToByteArray()) : str.ToByteArray();
                str = SyncClientToBase64? Convert.ToBase64String(bytes) : bytes.Utf8ToStr();
                str = "\"" + str + "\"";
            }

            return str;
        }

        public const short AccountProcessID = 1000;

        public const short GmWebProcessID = 1900;

        /// <summary>
        /// 账号数据库
        /// </summary>
        public const short AccountDBZone = 1;

        /// <summary>
        /// 日志数据库
        /// </summary>
        public const short LogDBZone = 100;

        /// <summary>
        /// 自动创建账号
        /// </summary>
        public const bool AutoRegisteAccount = true;

        /// <summary>
        /// 自动创建默认角色信息
        /// </summary>
        public const bool AutoCreateDefaultCharacter = true;

        /// <summary>
        /// 单服最大角色数
        /// </summary>
        public const short MaxCharacterCount = 4;

        /// <summary>
        /// 短线重连时间
        /// </summary>
        public const int WaitReConnectTime = 10000;

        /// <summary>
        /// http检查是否断线和定期保存数据
        /// </summary>
        public const int HttpPlayerOnlineCheckInterval = 15000;

        /// <summary>
        /// HTTP轮询间隔
        /// </summary>
        public const int HttpPollingCheckInterval = 10000;

        /// <summary>
        /// 自动保存角色数据间隔
        /// </summary>
        public const int AutoSaveCharacterInterval = 10 * 60 * 1000;

        public static bool IsAccountProcess()
        {
            return Options.Instance.Process == AccountProcessID;
        }
    }

    public static class HttpConfig
    {
        public const int PostMaxDataLength = 4096;
        public const string Exp = "exp";
        public const string UUID = "uuid";
        public const string Key = "key";
        public const string TokenPrefix = "Bearer ";
        public const string Authorization = "Authorization";
        public const string CustomAuthorization = "CustomAuthorization";
    }
}