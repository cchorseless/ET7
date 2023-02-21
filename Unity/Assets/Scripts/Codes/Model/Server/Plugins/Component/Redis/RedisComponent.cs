using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StackExchange.Redis;

namespace ET
{
    public class RedisComponent : Entity, IAwake<string, int>, IDestroy
    {
        public ConnectionMultiplexer redisClient;
        public IDatabase database;
        public int RedisDBIndex;
    }
}
