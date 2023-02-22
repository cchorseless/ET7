using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET.Server
{
    public static class HttpPlayerSessionComponentFunc
    {
        public static async ETTask CheckHttpLogOut(this HttpPlayerSessionComponent self)
        {
            long instanceId = self.InstanceId;
            while (self.InstanceId == instanceId)
            {
                await TimerComponent.Instance.WaitAsync(GameConfig.HttpPlayerOnlineCheckInterval);
                if (self.IsDisposed)
                {
                    return;
                }
                Player player = self.GetParent<Player>();
                long timeNow = TimeHelper.ServerNow();
                if (timeNow - self.LastRecvTime < GameConfig.HttpPlayerOnlineCheckInterval)
                {
                    // 定期保存数据
                    TCharacter character = player.GetMyCharacter();
                    if (character != null)
                    {
                        await character.Save();
                    }
                }
                else
                {
                    await player.GetComponent<PlayerLoginOutComponent>().KnockOutGate();
                    return;
                }
            }
        }

        public static void SendToClient(this HttpPlayerSessionComponent self, string _str = null)
        {
            if (!string.IsNullOrEmpty(_str))
            {
                self.SyncString.Add(_str);
            }

            if (self.Response != null && self.SyncString.Count > 0)
            {
                string message = "[";
                //while (self.SyncString.Count > 0 && message.Length + self.SyncString[0].Length < 6000)
                //{
                //    message += self.SyncString[0];
                //    message += ",";
                //    self.SyncString.RemoveAt(0);
                //}
                while (self.SyncString.Count > 0)
                {
                    message += self.SyncString[0];
                    self.SyncString.RemoveAt(0);
                    if (self.SyncString.Count > 0)
                    {
                        message += ",";
                    }
                }

                message += "]";
                self.Response.Message = message;
                self.Response = null;
                self.CancelWaiting();
            }
        }

        public static bool IsWaiting(this HttpPlayerSessionComponent self)
        {
            return self.CancelTimer != null;
        }
        public static void CancelWaiting(this HttpPlayerSessionComponent self)
        {
            if (self.CancelTimer != null)
            {
                self.CancelTimer.Cancel();
                self.CancelTimer = null;
            }
        }
    }

    [ObjectSystem]
    public class HttpPlayerSessionComponentAwakeSystem: AwakeSystem<HttpPlayerSessionComponent>
    {
        protected override void Awake(HttpPlayerSessionComponent self)
        {
            self.CheckHttpLogOut().Coroutine();
        }
    }

    [ObjectSystem]
    public class HttpPlayerSessionComponentDestroySystem: DestroySystem<HttpPlayerSessionComponent>
    {
        protected override void Destroy(HttpPlayerSessionComponent self)
        {
            self.Response = null;
            if (self.CancelTimer != null)
            {
                self.CancelTimer.Cancel();
                self.CancelTimer = null;
            }
        }
    }
}