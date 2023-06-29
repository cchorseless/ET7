using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ET.Server
{
    public class DBLogManagerComponent: Entity, IAwake, IDestroy, ILogDBHandler
    {
        public static DBLogManagerComponent Instance;
        public string Name;
        public DBComponent LogDB;
        public Dictionary<string, long> TempLogCache;

        private bool CheckLogLevel(int level)
        {
            if (Options.Instance == null)
            {
                return true;
            }
            return Options.Instance.LogLevel <= level;
        }
        public void HandleDBLog(int level, string message, string label = "")
        {
            if (this.LogDB == null || this.IsDisposed || string.IsNullOrEmpty(message) || !CheckLogLevel(level))
            {
                return;
            }
            DBLogRecord entity;
            if (TempLogCache.TryGetValue(message, out var entityid))
            {
                entity = this.GetChild<DBLogRecord>(entityid);
                if (entity != null)
                {
                    entity.Count++;
                    return;
                }
            }

            entity = this.AddChild<DBLogRecord>();
            entity.Msg = message;
            entity.Level = level;
            entity.Label = label;
            entity.Time = TimeHelper.ServerNow();
            entity.Process = Name;
            if (level == (int)DBLogger.EDBLogLevel.Important)
            {
                SaveDBLog(entity).Coroutine();
            }
            else
            {
                TempLogCache.Add(message, entity.Id);
            }
        }

        public async ETTask IntervalSaveDBLog()
        {
            await TimerComponent.Instance.WaitAsync(1 * 60 * 1000);
            if (this.IsDisposed)
            {
                return;
            }

            if (this.TempLogCache.Count > 0)
            {
                var copyDic = new Dictionary<string, long>();
                foreach (var kv in this.TempLogCache)
                {
                    copyDic.Add(kv.Key, kv.Value);
                }

                using (await CoroutineLockComponent.Instance.Wait(CoroutineLockType.DB, this.Id % DBComponent.TaskCount))
                {
                    foreach (var kv in copyDic)
                    {
                        var entity = this.GetChild<DBLogRecord>(kv.Value);
                        if (entity != null)
                        {
                            string collection = GetDBCollectionName(Name, entity.Level, TimeInfo.Instance.ToDateTime(entity.Time));
                            await LogDB.database.GetCollection<Entity>(collection)
                                    .ReplaceOneAsync(d => d.Id == entity.Id, entity, new ReplaceOptions { IsUpsert = true });
                            entity.Dispose();
                        }

                        this.TempLogCache.Remove(kv.Key);
                    }
                }
            }

            this.IntervalSaveDBLog().Coroutine();
        }

        private async ETTask SaveDBLog(DBLogRecord entity)
        {
            string collection = GetDBCollectionName(Name, entity.Level, TimeInfo.Instance.ToDateTime(entity.Time));
            using (await CoroutineLockComponent.Instance.Wait(CoroutineLockType.DB, entity.Id % DBComponent.TaskCount))
            {
                await LogDB.database.GetCollection<Entity>(collection)
                        .ReplaceOneAsync(d => d.Id == entity.Id, entity, new ReplaceOptions { IsUpsert = true });
            }

            entity.Dispose();
        }

        public string GetDBCollectionName(string processName, int loglevel, DateTime date)
        {
            var levelDes = ((DBLogger.EDBLogLevel)loglevel).ToString();
            var timestr = date.ToLocalTime().ToString("yyyy-MM-dd");
            return $"{processName}_{timestr}_{levelDes}";
        }
    }
}