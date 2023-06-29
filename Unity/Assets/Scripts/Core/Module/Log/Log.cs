using System;

namespace ET
{
    public static class Log
    {
        public static void Trace(string msg)
        {
            Logger.Instance.Trace(msg);
        }

        public static void Debug(string msg)
        {
            Logger.Instance.Debug(msg);
            DBLogger.Instance.Debug(msg);
        }

        public static void Info(string msg)
        {
            Logger.Instance.Info(msg);
            DBLogger.Instance.Info(msg);

        }
   
        public static void TraceInfo(string msg)
        {
            Logger.Instance.Trace(msg);
        }

        public static void Warning(string msg)
        {
            Logger.Instance.Warning(msg);
            DBLogger.Instance.Warning(msg);
        }

        public static void Error(string msg)
        {
            Logger.Instance.Error(msg);
            DBLogger.Instance.Error(msg);
        }

        public static void Error(Exception e)
        {
            Logger.Instance.Error(e);
            DBLogger.Instance.Error(e);
        }

        public static void Console(string message)
        {
            Logger.Instance.Console(message);
        }
        
#if DOTNET
        public static void Trace(ref System.Runtime.CompilerServices.DefaultInterpolatedStringHandler message)
        {
            Logger.Instance.Trace(message.ToStringAndClear());
        }

        public static void Warning(ref System.Runtime.CompilerServices.DefaultInterpolatedStringHandler message)
        {
            var str = message.ToStringAndClear();
            Logger.Instance.Warning(str);
            DBLogger.Instance.Warning(str);
        }

        public static void Info(ref System.Runtime.CompilerServices.DefaultInterpolatedStringHandler message)
        {
            var str = message.ToStringAndClear();
            Logger.Instance.Info(str);
            DBLogger.Instance.Info(str);
        }

        public static void Debug(ref System.Runtime.CompilerServices.DefaultInterpolatedStringHandler message)
        {
            var str = message.ToStringAndClear();
            Logger.Instance.Debug(str);
            DBLogger.Instance.Debug(str);
        }

        public static void Error(ref System.Runtime.CompilerServices.DefaultInterpolatedStringHandler message)
        {
            var str = message.ToStringAndClear();
            Logger.Instance.Error(str);
            DBLogger.Instance.Error(str);
        }
        
        public static void Console(ref System.Runtime.CompilerServices.DefaultInterpolatedStringHandler message)
        {
            Logger.Instance.Console(message.ToStringAndClear());
        }
#endif
    }
}