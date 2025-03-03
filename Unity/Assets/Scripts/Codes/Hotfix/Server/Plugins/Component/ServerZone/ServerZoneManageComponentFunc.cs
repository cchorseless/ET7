﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ET.Server
{
    public enum EServerZoneState
    {
        Working = 1,
        Closing = 2,
        WaitForWorking = 4,
        WaitForClosing = 8,
        StopResigter = 16,
        Hot = 32,
    }

    public enum EServerZoneLabel
    {
        Develop = 1,
        Publish = 2,
        Android = 4,
        IOS = 8,
    }

    public static class ServerZoneManageComponentFunc
    {
        public static List<int> GetProcessByServerId(this ServerZoneManageComponent self, SceneType sceneType, int serverid)
        {
            var r = new List<int>();
            foreach (var sceneconfig in StartSceneConfigCategory.Instance.ProcessScenes)
            {
                foreach (var sceneConfig in sceneconfig.Value)
                {
                    if (sceneConfig.Type == sceneType && sceneConfig.ServerMin <= serverid && sceneConfig.ServerMax >= serverid)
                    {
                        if (!r.Contains(sceneconfig.Key))
                        {
                            r.Add(sceneconfig.Key);
                        }

                        break;
                    }
                }
            }

            return r;
        }

        public static async ETTask LoadAllChild(this ServerZoneManageComponent self)
        {
            await self.LoadServerZone();
            if (GameConfig.IsAccountProcess())
            {
                await self.LoadNoticeRecord();
            }
        }

        public static async ETTask SaveAllChild(this ServerZoneManageComponent self)
        {
            foreach (var kv in self.ServerZoneDict)
            {
                var serverZone = self.GetChild<TServerZone>(kv.Value);
                if (serverZone != null)
                {
                    await serverZone.Save();
                }
            }
        }

        public static async ETTask LoadServerZone(this ServerZoneManageComponent self)
        {
            var accountDB = DBManagerComponent.Instance.GetAccountDB();
            var ProcessID = Options.Instance.Process;
            var serverZoneList = await accountDB.Query<TServerZone>(a => a.ProcessID == ProcessID);
            TServerZone serverZone = null;
            if (serverZoneList.Count == 0)
            {
                serverZone = self.AddChild<TServerZone, int, int, string>(GameConfig.AccountDBZone, 1, $"Server_Process{ProcessID}");
                serverZone.ProcessID = ProcessID;
                serverZone.State.Add((int)EServerZoneState.Working);
                serverZone.CreateTime = TimeHelper.ServerNow();
            }
            else
            {
                serverZone = serverZoneList[0];
                self.AddChild(serverZone);
            }

            await serverZone.LoadAllComponent();
            await serverZone.Save();
            self.ServerZoneDict.Add(serverZone.ServerID, serverZone.Id);
        }

        public static async ETTask LoadNoticeRecord(this ServerZoneManageComponent self)
        {
            var accountDB = DBManagerComponent.Instance.GetAccountDB();
            int isValid = (int)EStatefulTimer.Enable;
            int isWaitValid = (int)EStatefulTimer.WaitEnable;
            var noticeRecord = await accountDB.Query<TServerNoticeRecord>(a =>
                    a.State.Contains(isValid) || a.State.Contains(isWaitValid));
            if (noticeRecord.Count == 0)
            {
                await self.AddNewNotice("first notice");
            }
            else
            {
                foreach (var notice in noticeRecord)
                {
                    self.AddChild(notice);
                    if (notice.State.Contains(isValid))
                    {
                        self.LastServerNoticeID = notice.Id;
                    }
                }
            }
        }

        public static TServerZone GetServerZone(this ServerZoneManageComponent self, int ServerID = 1)
        {
            if (self.ServerZoneDict.TryGetValue(ServerID, out var serverid))
            {
                return self.GetChild<TServerZone>(serverid);
            }

            return null;
        }

        public static async ETTask AddNewServer(this ServerZoneManageComponent self, int zoneId,
        string serverName, int serverLabel = 0, long operateTime = 0)
        {
            bool isWait = operateTime > TimeHelper.ServerNow();
            var item = self.AddChild<TServerZone, int, int, string>(zoneId, self.ServerZoneDict.Count + 1, serverName);
            await item.LoadAllComponent();
            foreach (EServerZoneLabel labels in Enum.GetValues(typeof (EServerZoneLabel)))
            {
                int _v = (int)labels;
                if ((serverLabel & _v) == _v)
                {
                    item.ServerLabel.Add(_v);
                }
            }

            if (!isWait)
            {
                item.State.Add((int)EServerZoneState.Working);
                item.CreateTime = TimeHelper.ServerNow();
            }
            else
            {
                item.State.Add((int)EServerZoneState.Closing);
                item.State.Add((int)EServerZoneState.WaitForWorking);
                item.CreateTime = operateTime;
                item.AddChild<TStatefulTimer, long>(operateTime);
            }

            self.ServerZoneDict.Add(item.ServerID, item.Id);
            await item.Save();
        }

        public static async ETTask<int> EditServer(this ServerZoneManageComponent self, long serverZoneid,
        string serverName, int serverLabel = 0, int serverstate = 0)
        {
            var item = self.GetChild<TServerZone>(serverZoneid);
            if (item == null)
            {
                await ETTask.CompletedTask;
                return ErrorCode.ERR_Error;
            }

            if (serverName != null && serverName.Length > 5 && serverName.Length < 20)
            {
                item.ServerName = serverName;
            }

            foreach (EServerZoneLabel labels in Enum.GetValues(typeof (EServerZoneLabel)))
            {
                int _v = (int)labels;
                if ((serverLabel & _v) == _v)
                {
                    item.ServerLabel.Add(_v);
                }
                else
                {
                    item.ServerLabel.Remove(_v);
                }
            }

            foreach (EServerZoneLabel labels in Enum.GetValues(typeof (EServerZoneState)))
            {
                int _v = (int)labels;
                if (_v > (int)EServerZoneState.WaitForClosing)
                {
                    if ((serverstate & _v) == _v)
                    {
                        item.State.Add(_v);
                    }
                    else
                    {
                        item.State.Remove(_v);
                    }
                }
            }

            var accountDB = DBManagerComponent.Instance.GetAccountDB();
            await accountDB.Save(item);
            return ErrorCode.ERR_Success;
        }

        public static async ETTask<int> ManageServer(this ServerZoneManageComponent self, long serverZoneid,
        int operate, long operateTime)
        {
            bool isWait = operateTime > TimeHelper.ServerNow();
            var item = self.GetChild<TServerZone>(serverZoneid);
            if (item == null)
            {
                await ETTask.CompletedTask;
                return ErrorCode.ERR_Error;
            }

            item.GetChilds<TStatefulTimer>().ForEach(timer => timer.Dispose());
            item.State.Remove((int)EServerZoneState.WaitForWorking);
            item.State.Remove((int)EServerZoneState.WaitForClosing);
            // 开启
            if (operate == 1)
            {
                if (!isWait)
                {
                    item.State.Remove((int)EServerZoneState.Closing);
                    item.State.Remove((int)EServerZoneState.WaitForWorking);
                    item.State.Add((int)EServerZoneState.Working);
                }
                else
                {
                    item.State.Add((int)EServerZoneState.Closing);
                    item.State.Add((int)EServerZoneState.WaitForWorking);
                    item.State.Remove((int)EServerZoneState.Working);
                    item.AddChild<TStatefulTimer, long>(operateTime);
                }
            }
            // 关闭
            else if (operate == 2)
            {
                if (!isWait)
                {
                    item.State.Remove((int)EServerZoneState.Working);
                    item.State.Remove((int)EServerZoneState.WaitForClosing);
                    item.State.Add((int)EServerZoneState.Closing);
                }
                else
                {
                    item.State.Add((int)EServerZoneState.Working);
                    item.State.Add((int)EServerZoneState.WaitForClosing);
                    item.State.Remove((int)EServerZoneState.Closing);
                    item.AddChild<TStatefulTimer, long>(operateTime);
                }
            }

            var accountDB = DBManagerComponent.Instance.GetAccountDB();
            await accountDB.Save(item);
            return ErrorCode.ERR_Success;
        }

        public static async ETTask AddNewNotice(this ServerZoneManageComponent self, string serverNotice, long operateTime = 0)
        {
            bool isWait = operateTime > TimeHelper.ServerNow();
            var accountDB = DBManagerComponent.Instance.GetAccountDB();
            if (!isWait && self.LastServerNoticeID > 0)
            {
                var last = self.GetChild<TServerNoticeRecord>(self.LastServerNoticeID);
                if (last != null)
                {
                    last.State.Clear();
                    last.State.Add((int)EStatefulTimer.Disable);
                    await accountDB.Save(last);
                    last.Dispose();
                }

                self.LastServerNoticeID = 0;
            }

            var item = self.AddChild<TServerNoticeRecord>();
            item.Notice = serverNotice;
            if (!isWait)
            {
                item.State.Add((int)EStatefulTimer.Enable);
                item.CreateTime = TimeHelper.ServerNow();
                self.LastServerNoticeID = item.Id;
            }
            else
            {
                item.State.Add((int)EStatefulTimer.Disable);
                item.State.Add((int)EStatefulTimer.WaitEnable);
                item.CreateTime = operateTime;
                item.AddChild<TStatefulTimer, long>(operateTime);
            }

            await accountDB.Save(item);
        }

        public static async ETTask EnableNotice(this ServerZoneManageComponent self, TServerNoticeRecord notice)
        {
            var accountDB = DBManagerComponent.Instance.GetAccountDB();
            if (self.LastServerNoticeID > 0)
            {
                var last = self.GetChild<TServerNoticeRecord>(self.LastServerNoticeID);
                if (last != null)
                {
                    last.State.Clear();
                    last.State.Add((int)EStatefulTimer.Disable);
                    await accountDB.Save(last);
                    last.Dispose();
                }

                self.LastServerNoticeID = 0;
            }

            if (notice.Parent != self)
            {
                self.AddChild(notice);
            }

            notice.State.Clear();
            notice.State.Add((int)EStatefulTimer.Enable);
            self.LastServerNoticeID = notice.Id;
            await accountDB.Save(notice);
        }
    }

    [ObjectSystem]
    public class ServerZoneManageComponentAwakeSystem: AwakeSystem<ServerZoneManageComponent>
    {
        protected override void Awake(ServerZoneManageComponent self)
        {
            ServerZoneManageComponent.Instance = self;
            self.ServerZoneDict = new Dictionary<int, long>();
        }
    }
}