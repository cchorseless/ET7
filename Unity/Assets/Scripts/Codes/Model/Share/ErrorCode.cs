namespace ET
{
    public static partial class ErrorCode
    {
        // 1-11004 是SocketError请看SocketError定义
        //-----------------------------------
        // 100000-109999是Core层的错误

        // 110000以下的错误请看ErrorCore.cs

        // 这里配置逻辑层的错误码
        // 110000 - 200000是抛异常的错误
        // 200001以上不抛异常

        public const int ERR_Error = 300000;
        /// <summary>
        /// 账号或密码不存在
        /// </summary>
        public const int ERR_LoginError = 300001;

        /// <summary>
        /// 账号不存在
        /// </summary>
        public const int ERR_AccountNotExist = 300002;

        /// <summary>
        /// 账号已被注册
        /// </summary>
        public const int ERR_AccountHasExist = 300003;

        /// <summary>
        /// 房间不存在
        /// </summary>
        public const int ERR_RoomNotExist = 300004;

        /// <summary>
        /// 开局失败
        /// </summary>
        public const int ERR_StartGameFail = 300005;
    }
}