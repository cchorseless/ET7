using MongoDB.Driver;
using System;
using System.IO;

namespace ET.Server
{

    public class DBLogger : Singleton<DBLogger>
    {
        private DBComponent LogDB;

        private string Name = "";

        public enum EDBLogLevel
        {

            Debug = 2,
            Info = 3,
            Warning = 4,
            Error = 5,
        }

        public void RegisterLogDB(DBComponent db)
        {
            Name = $"{Options.Instance.AppType}_{Options.Instance.Process}";
            LogDB = db;
        }
        public override void Dispose()
        {
            LogDB = null;
        }

        public void Warning(string message)
        {
            if (LogDB == null) { return; }
            var entity = Entity.CreateOne<DBLogRecord>(true);
            entity.Msg = message;
            entity.Level = (int)EDBLogLevel.Warning;
            SaveDBLog(entity).Coroutine();
        }

        public void Info(string message)
        {
            if (LogDB == null) { return; }
            var entity = Entity.CreateOne<DBLogRecord>(true);
            entity.Msg = message;
            entity.Level = (int)EDBLogLevel.Info;
            SaveDBLog(entity).Coroutine();
        }

        public void Debug(string message)
        {
            if (LogDB == null) { return; }
            var entity = Entity.CreateOne<DBLogRecord>(true);
            entity.Msg = message;
            entity.Level = (int)EDBLogLevel.Debug;
            SaveDBLog(entity).Coroutine();
        }

        public void Error(string message)
        {
            if (LogDB == null) { return; }
            var entity = Entity.CreateOne<DBLogRecord>(true);
            entity.Msg = message;
            entity.Level = (int)EDBLogLevel.Error;
            SaveDBLog(entity).Coroutine();
        }

        private async ETTask SaveDBLog(DBLogRecord entity)
        {
            var date = DateTime.UtcNow;
            entity.Time = TimeHelper.ServerNow();
            entity.Process = Name;
            string collection = GetDBCollectionName(Name, entity.Level, ref date);
            using (await CoroutineLockComponent.Instance.Wait(CoroutineLockType.DB, entity.Id % DBComponent.TaskCount))
            {
                await LogDB.database.GetCollection<Entity>(collection).ReplaceOneAsync(d => d.Id == entity.Id, entity, new ReplaceOptions { IsUpsert = true });
            }
            entity.Dispose();
        }

        public static string GetDBCollectionName(string processName, int loglevel, ref DateTime date)
        {
            var levelDes = ((EDBLogLevel)loglevel).ToString();
            var timestr = date.ToLocalTime().ToString("yyyy-MM-dd");
            return $"{processName}_{timestr}_{levelDes }";

        }
    }
}