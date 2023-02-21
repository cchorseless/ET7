using StackExchange.Redis;
using System;
using System.Collections.Generic;

namespace ET
{
    public class RedisManagerComponent : Entity, IAwake, IDestroy
    {
        public static RedisManagerComponent Instance;

        public const int RedisMaxIndex = 14;

        public RedisComponent[] RedisComponents = new RedisComponent[RedisMaxIndex];

    }

    [ObjectSystem]
    public class RedisManagerComponentAwakeSystem : AwakeSystem<RedisManagerComponent>
    {
        protected override void Awake(RedisManagerComponent self)
        {
            RedisManagerComponent.Instance = self;
        }
    }

    [ObjectSystem]
    public class RedisManagerComponentDestroySystem : DestroySystem<RedisManagerComponent>
    {
        protected override void Destroy(RedisManagerComponent self)
        {
            RedisManagerComponent.Instance = null;
        }
    }

    [FriendOf(typeof(ET.RedisManagerComponent))]
    public static class RedisManagerComponentSystem
    {
        public static RedisComponent GetRedisDB(this RedisManagerComponent self, int index = 0)
        {
            RedisComponent dbComponent = self.RedisComponents[index];
            if (dbComponent != null)
            {
                return dbComponent;
            }
            var startRedisConfig = StartRedisConfigCategory.Instance.Get(index);
            if (startRedisConfig.DBConnection == "")
            {
                throw new Exception($"index: {index} not found redis connect string");
            }
            dbComponent = self.AddChild<RedisComponent, string, int>(startRedisConfig.DBConnection, index);
            self.RedisComponents[index] = dbComponent;
            return dbComponent;
        }
    }
}