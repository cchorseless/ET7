﻿using System;

namespace ET.Server
{
    [FriendOf(typeof(DBManagerComponent))]
    public static class DBManagerComponentSystem
    {
        [ObjectSystem]
        public class DBManagerComponentAwakeSystem: AwakeSystem<DBManagerComponent>
        {
            protected override void Awake(DBManagerComponent self)
            {
                DBManagerComponent.Instance = self;
            }
        }

        [ObjectSystem]
        public class DBManagerComponentDestroySystem: DestroySystem<DBManagerComponent>
        {
            protected override void Destroy(DBManagerComponent self)
            {
                DBManagerComponent.Instance = null;
            }
        }
        
        public static DBComponent GetZoneDB(this DBManagerComponent self, int zone)
        {
            DBComponent dbComponent = self.DBComponents[zone];
            if (dbComponent != null)
            {
                return dbComponent;
            }

            StartZoneConfig startZoneConfig = StartZoneConfigCategory.Instance.Get(zone);
            if (startZoneConfig.DBConnection == "")
            {
                throw new Exception($"zone: {zone} not found mongo connect string");
            }

            dbComponent = self.AddChild<DBComponent, string, string, int>(startZoneConfig.DBConnection, startZoneConfig.DBName, zone);
            self.DBComponents[zone] = dbComponent;
            return dbComponent;
        }

        public static DBComponent GetLogDB(this DBManagerComponent self)
        {
            // log 数据库
            return self.GetZoneDB(GameConfig.LogDBZone);
        }

        public static DBComponent GetAccountDB(this DBManagerComponent self)
        {
            // 账号 数据库
            return self.GetZoneDB(GameConfig.AccountDBZone);
        }
    }
}