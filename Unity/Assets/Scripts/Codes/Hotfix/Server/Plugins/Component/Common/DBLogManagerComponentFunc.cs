using System.Collections.Generic;

namespace ET.Server
{
    public enum EDBLogLabel
    {
        ChongZhi,
        XiaoFei,
        Login,
        LoginOut,
        LoginGm,
        LoginOutGm,
    }
    
    [ObjectSystem]
    public class DBLogManagerComponentAwakeSystem: AwakeSystem<DBLogManagerComponent>
    {
        protected override void Awake(DBLogManagerComponent self)
        {
            self.Name = $"{Options.Instance.AppType}_{Options.Instance.Process}";
            self.LogDB = DBManagerComponent.Instance.GetLogDB();
            self.TempLogCache = new Dictionary<string,long>();
            DBLogManagerComponent.Instance = self;
            DBLogger.Instance.RegisterLogDB(self);
            self.IntervalSaveDBLog().Coroutine();
   
        }
    }

    [ObjectSystem]
    public class DBLogManagerComponentDestroySystem: DestroySystem<DBLogManagerComponent>
    {
        protected override void Destroy(DBLogManagerComponent self)
        {
            self.LogDB = null;
            self.TempLogCache.Clear();
            DBLogManagerComponent.Instance = null;
            DBLogger.Instance.RegisterLogDB(null);
        }
    }

    public class DBLogManagerComponentFunc
    {
    }
}