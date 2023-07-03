using MongoDB.Driver;
using System;
using System.IO;

namespace ET
{
    public interface ILogDBHandler
    {
        void HandleDBLog(int level, string message, string label = "");
    }

    public class DBLogger: Singleton<DBLogger>
    {
        private ILogDBHandler LogDB;

        public enum EDBLogLevel
        {
            Debug = 2,
            Info = 3,
            Warning = 4,
            Error = 5,
            Important = 9,
            //前端报错记录
            ClientError = 10,
            //前端反馈建议
            ClientSuggest = 11,
        }

        public void RegisterLogDB(ILogDBHandler db)
        {
            LogDB = db;
        }

        public override void Dispose()
        {
            LogDB = null;
        }

        public void Warning(string message)
        {
            if (LogDB == null)
            {
                return;
            }

            LogDB.HandleDBLog((int)EDBLogLevel.Warning, message);
        }

        public void Important(string label, string message)
        {
            if (LogDB == null)
            {
                return;
            }

            LogDB.HandleDBLog((int)EDBLogLevel.Important, message, label);
        }

        public void ClientError(string label, string message)
        {
            if (LogDB == null)
            {
                return;
            }

            LogDB.HandleDBLog((int)EDBLogLevel.ClientError, message, label);
        }
        
        public void ClientSuggest(string label, string message)
        {
            if (LogDB == null)
            {
                return;
            }

            LogDB.HandleDBLog((int)EDBLogLevel.ClientSuggest, message, label);
        }
        
        public void Info(string message)
        {
            if (LogDB == null)
            {
                return;
            }

            LogDB.HandleDBLog((int)EDBLogLevel.Info, message);
        }

        public void Debug(string message)
        {
            if (LogDB == null)
            {
                return;
            }

            LogDB.HandleDBLog((int)EDBLogLevel.Debug, message);
        }

        public void Error(string message)
        {
            if (LogDB == null)
            {
                return;
            }

            LogDB.HandleDBLog((int)EDBLogLevel.Error, message);
        }

        public void Error(Exception e)
        {
            if (LogDB == null)
            {
                return;
            }

            string message = "";
            if (e.Data.Contains("StackTrace"))
            {
                message = $"{e.Data["StackTrace"]}\n{e}";
            }

            message = e.ToString();
            LogDB.HandleDBLog((int)EDBLogLevel.Error, message);
        }
    }
}