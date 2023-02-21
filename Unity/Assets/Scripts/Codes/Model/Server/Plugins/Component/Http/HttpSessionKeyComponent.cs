using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET
{
    public class HttpSessionKeyComponent : Entity, IAwake
    {
        public readonly Dictionary<string, string> sessionKey = new Dictionary<string, string>();
    }

    [FriendOf(typeof(HttpSessionKeyComponent))]
    public static class HttpSessionKeyComponentSystem
    {
        public static void Add(this HttpSessionKeyComponent self, string account, string key)
        {
            if (self.sessionKey.ContainsKey(account))
            {
                self.sessionKey.Remove(account);
            }
            self.sessionKey.Add(account, key);
            self.TimeoutRemoveKey(account, key).Coroutine();
        }

        public static string Get(this HttpSessionKeyComponent self, string account)
        {
            self.sessionKey.TryGetValue(account, out var key);
            return key;
        }

        public static void Remove(this HttpSessionKeyComponent self, string account)
        {
            self.sessionKey.Remove(account);
        }

        private static async ETTask TimeoutRemoveKey(this HttpSessionKeyComponent self, string account, string key)
        {
            await TimerComponent.Instance.WaitAsync(10000);
            if (self.Get(account) == key)
            {
                self.sessionKey.Remove(account);
            }
        }
    }
}
