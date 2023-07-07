using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET
{
    public class HttpRealmSessionKeyComponent: Entity, IAwake
    {
        public readonly Dictionary<string, (string, long)> sessionKey = new Dictionary<string, (string, long)>();
    }

    [ObjectSystem]
    public class HttpRealmSessionKeyComponentAwakeSystem: AwakeSystem<HttpRealmSessionKeyComponent>
    {
        protected override void Awake(HttpRealmSessionKeyComponent self)
        {
            self.RemoveTimeoutKey().Coroutine();
        }
    }

    [FriendOf(typeof (HttpRealmSessionKeyComponent))]
    public static class HttpSessionKeyComponentSystem
    {
        public static void Add(this HttpRealmSessionKeyComponent self, string account, string key)
        {
            if (self.sessionKey.ContainsKey(account))
            {
                self.sessionKey.Remove(account);
            }

            self.sessionKey.Add(account, (key, TimeHelper.ServerNow()));
        }

        public static string GetKey(this HttpRealmSessionKeyComponent self, string account)
        {
            self.sessionKey.TryGetValue(account, out var key);
            return key.Item1;
        }

        public static void Remove(this HttpRealmSessionKeyComponent self, string account)
        {
            self.sessionKey.Remove(account);
        }

        public static async ETTask RemoveTimeoutKey(this HttpRealmSessionKeyComponent self)
        {
            await TimerComponent.Instance.WaitAsync(10 * 1000);
            if (self.IsDisposed)
            {
                return;
            }

            var keys = self.sessionKey.Keys.ToList();
            var now = TimeHelper.ServerNow();
            foreach (var key in keys)
            {
                if (now - self.sessionKey[key].Item2 >= 10 * 1000)
                {
                    self.Remove(key);
                }
            }

            self.RemoveTimeoutKey().Coroutine();
        }
    }
}