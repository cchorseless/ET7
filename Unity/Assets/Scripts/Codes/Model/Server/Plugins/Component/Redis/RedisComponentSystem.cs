using System;
using System.Collections.Generic;
using System.Linq;
using StackExchange.Redis;

namespace ET
{
    [ObjectSystem]
    public class RedisComponentAwakeSystem : AwakeSystem<RedisComponent, string, int>
    {
        protected override void Awake(RedisComponent self, string dbConnection, int redisDbIndex)
        {
            self.redisClient = ConnectionMultiplexer.Connect(dbConnection);
            self.database = self.redisClient.GetDatabase(redisDbIndex);
            self.RedisDBIndex = redisDbIndex;
        }
    }

    [ObjectSystem]
    public class RedisComponentDestroySystem : DestroySystem<RedisComponent>
    {
        protected override void Destroy(RedisComponent self)
        {
            self.redisClient.Dispose();
            self.redisClient = null;
            self.database = null;
        }
    }
    [FriendOf(typeof(ET.RedisComponent))]
    public static class RedisComponentSystem
    {
        #region 三、操作String类型方法封装
        /// <summary>
        /// 设置 key 并保存字符串（如果 key 已存在，则覆盖值）
        /// </summary>
        /// <param name="redisKey">名称</param>
        /// <param name="redisValue">值</param>
        /// <param name="expiry">时间</param>
        /// <returns></returns>
        public static bool StringSet(this RedisComponent self, string redisKey, string redisValue, TimeSpan? expiry = null)
        {
            return self.database.StringSet(redisKey, redisValue, expiry);
        }

        /// <summary>
        /// 获取字符串
        /// </summary>
        /// <param name="redisKey">名称</param>
        /// <param name="expiry">时间</param>
        /// <returns></returns>
        public static string StringGet(this RedisComponent self, string redisKey, TimeSpan? expiry = null)
        {
            return self.database.StringGet(redisKey);
        }

        /// <summary>
        /// 存储一个对象（该对象会被序列化保存）
        /// </summary>
        /// <param name="redisKey">名称</param>
        /// <param name="redisValue">值</param>
        /// <param name="expiry">时间</param>
        /// <returns></returns>
        public static bool StringSet<T>(this RedisComponent self, string redisKey, T redisValue, TimeSpan? expiry = null)
        {
            var json = Serialize(redisValue);
            return self.database.StringSet(redisKey, json, expiry);
        }

        /// <summary>
        /// 获取一个对象（会进行反序列化）
        /// </summary>
        /// <param name="redisKey">名称</param>
        /// <param name="expiry">时间</param>
        /// <returns></returns>
        public static T StringGet<T>(this RedisComponent self, string redisKey, TimeSpan? expiry = null)
        {
            return Deserialize<T>(self.database.StringGet(redisKey));
        }
        #endregion

        #region 四、操作Hash类型方法封装

        /// <summary>
        /// 判断该字段是否存在 hash 中
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="hashField"></param>
        /// <returns></returns>
        public static bool HashExists(this RedisComponent self, string redisKey, string hashField)
        {
            return self.database.HashExists(redisKey, hashField);
        }

        /// <summary>
        /// 从 hash 中移除指定字段
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="hashField"></param>
        /// <returns></returns>
        public static bool HashDelete(this RedisComponent self, string redisKey, string hashField)
        {
            return self.database.HashDelete(redisKey, hashField);
        }

        /// <summary>
        /// 从 hash 中移除指定字段（多个删除）
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="hashField"></param>
        /// <returns></returns>
        public static long HashDelete(this RedisComponent self, string redisKey, IEnumerable<RedisValue> hashField)
        {
            return self.database.HashDelete(redisKey, hashField.ToArray());
        }

        /// <summary>
        /// 在 hash 设定值
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="hashField"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool HashSet(this RedisComponent self, string redisKey, string hashField, string value)
        {
            return self.database.HashSet(redisKey, hashField, value);
        }

        /// <summary>
        /// 在 hash 中设定值（多个）
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="hashFields"></param>
        public static void HashSet(this RedisComponent self, string redisKey, IEnumerable<HashEntry> hashFields)
        {
            self.database.HashSet(redisKey, hashFields.ToArray());
        }

        /// <summary>
        /// 在 hash 中获取值
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="hashField"></param>
        /// <returns></returns>
        public static RedisValue HashGet(this RedisComponent self, string redisKey, string hashField)
        {
            return self.database.HashGet(redisKey, hashField);
        }

        /// <summary>
        /// 在 hash 中获取值（多个）
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="hashField"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static RedisValue[] HashGet(this RedisComponent self, string redisKey, RedisValue[] hashField, string value)
        {
            return self.database.HashGet(redisKey, hashField);
        }

        /// <summary>
        /// 从 hash 返回所有的字段值
        /// </summary>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        public static IEnumerable<RedisValue> HashKeys(this RedisComponent self, string redisKey)
        {
            return self.database.HashKeys(redisKey);
        }

        /// <summary>
        /// 返回 hash 中的所有值
        /// </summary>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        public static RedisValue[] HashValues(this RedisComponent self, string redisKey)
        {
            return self.database.HashValues(redisKey);
        }

        /// <summary>
        /// 在 hash 设定值（序列化）
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="hashField"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool HashSet<T>(this RedisComponent self, string redisKey, string hashField, T value)
        {
            var json = Serialize(value);
            return self.database.HashSet(redisKey, hashField, json);
        }

        /// <summary>
        /// 在 hash 中获取值（反序列化）
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="hashField"></param>
        /// <returns></returns>
        public static T HashGet<T>(this RedisComponent self, string redisKey, string hashField)
        {
            return Deserialize<T>(self.database.HashGet(redisKey, hashField));
        }
        #endregion

        #region 五、操作List类型方法封装

        /// <summary>
        /// 移除并返回存储在该键列表的第一个元素
        /// </summary>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        public static string ListLeftPop(this RedisComponent self, string redisKey)
        {
            return self.database.ListLeftPop(redisKey);
        }

        /// <summary>
        /// 移除并返回存储在该键列表的最后一个元素
        /// </summary>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        public static string ListRightPop(this RedisComponent self, string redisKey)
        {
            return self.database.ListRightPop(redisKey);
        }

        /// <summary>
        /// 移除列表指定键上与该值相同的元素
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="redisValue"></param>
        /// <returns></returns>
        public static long ListRemove(this RedisComponent self, string redisKey, string redisValue)
        {
            return self.database.ListRemove(redisKey, redisValue);
        }

        /// <summary>
        /// 在列表尾部插入值。如果键不存在，先创建再插入值
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="redisValue"></param>
        /// <returns></returns>
        public static long ListRightPush(this RedisComponent self, string redisKey, string redisValue)
        {
            return self.database.ListRightPush(redisKey, redisValue);
        }

        /// <summary>
        /// 在列表头部插入值。如果键不存在，先创建再插入值
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="redisValue"></param>
        /// <returns></returns>
        public static long ListLeftPush(this RedisComponent self, string redisKey, string redisValue)
        {
            return self.database.ListLeftPush(redisKey, redisValue);
        }

        /// <summary>
        /// 返回列表上该键的长度，如果不存在，返回 0
        /// </summary>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        public static long ListLength(this RedisComponent self, string redisKey)
        {
            return self.database.ListLength(redisKey);
        }

        /// <summary>
        /// 返回在该列表上键所对应的元素
        /// </summary>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        public static IEnumerable<RedisValue> ListRange(this RedisComponent self, string redisKey)
        {
            return self.database.ListRange(redisKey);
        }

        /// <summary>
        /// 返回在该列表上键所对应的元素
        /// </summary>
        /// <param name="redisKey">开始行</param>
        /// <param name="startRow">结束行</param>
        /// <returns></returns>
        public static IEnumerable<RedisValue> ListRange(this RedisComponent self, string redisKey, int startRow, int endRow)
        {
            return self.database.ListRange(redisKey, startRow, endRow);
        }

        /// <summary>
        /// 移除并返回存储在该键列表的第一个元素
        /// </summary>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        public static T ListLeftPop<T>(this RedisComponent self, string redisKey)
        {
            return Deserialize<T>(self.database.ListLeftPop(redisKey));
        }

        /// <summary>
        /// 移除并返回存储在该键列表的最后一个元素
        /// </summary>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        public static T ListRightPop<T>(this RedisComponent self, string redisKey)
        {
            return Deserialize<T>(self.database.ListRightPop(redisKey));
        }

        /// <summary>
        /// 在列表尾部插入值。如果键不存在，先创建再插入值
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="redisValue"></param>
        /// <returns></returns>
        public static long ListRightPush<T>(this RedisComponent self, string redisKey, T redisValue)
        {
            return self.database.ListRightPush(redisKey, Serialize(redisValue));
        }

        /// <summary>
        /// 在列表头部插入值。如果键不存在，先创建再插入值
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="redisValue"></param>
        /// <returns></returns>
        public static long ListLeftPush<T>(this RedisComponent self, string redisKey, T redisValue)
        {
            return self.database.ListLeftPush(redisKey, Serialize(redisValue));
        }
        #endregion

        #region 六、操作SortedSet类型方法封装

        /// <summary>
        /// SortedSet 新增
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="member"></param>
        /// <param name="score"></param>
        /// <returns></returns>
        public static bool SortedSetAdd(this RedisComponent self, string redisKey, string member, double score)
        {
            return self.database.SortedSetAdd(redisKey, member, score);
        }

        /// <summary>
        /// 在有序集合中返回指定范围的元素，默认情况下从低到高。
        /// </summary>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        public static IEnumerable<RedisValue> SortedSetRangeByRank(this RedisComponent self, string redisKey)
        {
            return self.database.SortedSetRangeByRank(redisKey);
        }

        /// <summary>
        /// 返回有序集合的元素个数
        /// </summary>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        public static long SortedSetLength(this RedisComponent self, string redisKey)
        {
            return self.database.SortedSetLength(redisKey);
        }

        /// <summary>
        /// 返回有序集合的元素个数
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="memebr"></param>
        /// <returns></returns>
        public static bool SortedSetLength(this RedisComponent self, string redisKey, string memebr)
        {
            return self.database.SortedSetRemove(redisKey, memebr);
        }

        /// <summary>
        /// SortedSet 新增
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="member"></param>
        /// <param name="score"></param>
        /// <returns></returns>
        public static bool SortedSetAdd<T>(this RedisComponent self, string redisKey, T member, double score)
        {
            var json = Serialize(member);

            return self.database.SortedSetAdd(redisKey, json, score);
        }
        #endregion

        #region SortedSet-Async

        /// <summary>
        /// SortedSet 新增
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="member"></param>
        /// <param name="score"></param>
        /// <returns></returns>
        public static async ETTask<bool> SortedSetAddAsync(this RedisComponent self, string redisKey, string member, double score)
        {
            return await self.database.SortedSetAddAsync(redisKey, member, score);
        }

        /// <summary>
        /// 在有序集合中返回指定范围的元素，默认情况下从低到高。
        /// </summary>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        public static async ETTask<IEnumerable<RedisValue>> SortedSetRangeByRankAsync(this RedisComponent self, string redisKey)
        {
            return await self.database.SortedSetRangeByRankAsync(redisKey);
        }

        /// <summary>
        /// 返回有序集合的元素个数
        /// </summary>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        public static async ETTask<long> SortedSetLengthAsync(this RedisComponent self, string redisKey)
        {
            return await self.database.SortedSetLengthAsync(redisKey);
        }

        /// <summary>
        /// 返回有序集合的元素个数
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="memebr"></param>
        /// <returns></returns>
        public static async ETTask<bool> SortedSetRemoveAsync(this RedisComponent self, string redisKey, string memebr)
        {
            return await self.database.SortedSetRemoveAsync(redisKey, memebr);
        }

        /// <summary>
        /// SortedSet 新增
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="member"></param>
        /// <param name="score"></param>
        /// <returns></returns>
        public static async ETTask<bool> SortedSetAddAsync<T>(this RedisComponent self, string redisKey, T member, double score)
        {
            var json = Serialize(member);

            return await self.database.SortedSetAddAsync(redisKey, json, score);
        }
        #endregion

        #region 七、操作key类型方法封装

        /// <summary>
        /// 移除指定 Key
        /// </summary>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        public static bool KeyDelete(this RedisComponent self, string redisKey)
        {
            return self.database.KeyDelete(redisKey);
        }

        /// <summary>
        /// 移除指定 Key
        /// </summary>
        /// <param name="redisKeys"></param>
        /// <returns></returns>
        public static long KeyDelete(this RedisComponent self, IEnumerable<string> redisKeys)
        {
            var keys = redisKeys.Select(x => (RedisKey)x);
            return self.database.KeyDelete(keys.ToArray());
        }

        /// <summary>
        /// 校验 Key 是否存在
        /// </summary>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        public static bool KeyExists(this RedisComponent self, string redisKey)
        {
            return self.database.KeyExists(redisKey);
        }

        /// <summary>
        /// 重命名 Key
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="redisNewKey"></param>
        /// <returns></returns>
        public static bool KeyRename(this RedisComponent self, string redisKey, string redisNewKey)
        {
            return self.database.KeyRename(redisKey, redisNewKey);
        }

        /// <summary>
        /// 设置 Key 的时间
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="expiry"></param>
        /// <returns></returns>
        public static bool KeyExpire(this RedisComponent self, string redisKey, TimeSpan? expiry)
        {
            return self.database.KeyExpire(redisKey, expiry);
        }
        #endregion

        #region key-async

        /// <summary>
        /// 移除指定 Key
        /// </summary>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        public static async ETTask<bool> KeyDeleteAsync(this RedisComponent self, string redisKey)
        {
            return await self.database.KeyDeleteAsync(redisKey);
        }

        /// <summary>
        /// 移除指定 Key
        /// </summary>
        /// <param name="redisKeys"></param>
        /// <returns></returns>
        public static async ETTask<long> KeyDeleteAsync(this RedisComponent self, IEnumerable<string> redisKeys)
        {
            var keys = redisKeys.Select(x => (RedisKey)x);
            return await self.database.KeyDeleteAsync(keys.ToArray());
        }

        /// <summary>
        /// 校验 Key 是否存在
        /// </summary>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        public static async ETTask<bool> KeyExistsAsync(this RedisComponent self, string redisKey)
        {
            return await self.database.KeyExistsAsync(redisKey);
        }

        /// <summary>
        /// 重命名 Key
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="redisNewKey"></param>
        /// <returns></returns>
        public static async ETTask<bool> KeyRenameAsync(this RedisComponent self, string redisKey, string redisNewKey)
        {
            return await self.database.KeyRenameAsync(redisKey, redisNewKey);
        }

        /// <summary>
        /// 设置 Key 的时间
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="expiry"></param>
        /// <returns></returns>
        public static async ETTask<bool> KeyExpireAsync(this RedisComponent self, string redisKey, TimeSpan? expiry)
        {
            return await self.database.KeyExpireAsync(redisKey, expiry);
        }
        #endregion

        #region 八、发布订阅方法封装
        /// <summary>
        /// 订阅
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="handle"></param>
        public static void Subscribe(this RedisComponent self, RedisChannel channel, Action<RedisChannel, RedisValue> handle)
        {
            var sub = self.redisClient.GetSubscriber();
            sub.Subscribe(channel, handle);
        }

        /// <summary>
        /// 发布
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static long Publish(this RedisComponent self, RedisChannel channel, RedisValue message)
        {
            var sub = self.redisClient.GetSubscriber();
            return sub.Publish(channel, message);
        }

        /// <summary>
        /// 发布（使用序列化）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="channel"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static long Publish<T>(this RedisComponent self, RedisChannel channel, T message)
        {
            var sub = self.redisClient.GetSubscriber();
            return sub.Publish(channel, Serialize(message));
        }
        #endregion

        #region 发布订阅-async

        /// <summary>
        /// 订阅
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="handle"></param>
        public static async ETTask SubscribeAsync(this RedisComponent self, RedisChannel channel, Action<RedisChannel, RedisValue> handle)
        {
            var sub = self.redisClient.GetSubscriber();
            await sub.SubscribeAsync(channel, handle);
        }

        /// <summary>
        /// 发布
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static async ETTask<long> PublishAsync(this RedisComponent self, RedisChannel channel, RedisValue message)
        {
            var sub = self.redisClient.GetSubscriber();
            return await sub.PublishAsync(channel, message);
        }

        /// <summary>
        /// 发布（使用序列化）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="channel"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static async ETTask<long> PublishAsync<T>(this RedisComponent self, RedisChannel channel, T message)
        {
            var sub = self.redisClient.GetSubscriber();
            return await sub.PublishAsync(channel, Serialize(message));
        }
        #endregion

        #region 九、事件方法封装

        /// <summary>
        /// 添加注册事件
        /// </summary>
        private static void AddRegisterEvent(this RedisComponent self)
        {
            //self.redisClient.ConnectionRestored += ConnMultiplexer_ConnectionRestored;
            //self.redisClient.ConnectionFailed += ConnMultiplexer_ConnectionFailed;
            //self.redisClient.ErrorMessage += ConnMultiplexer_ErrorMessage;
            //self.redisClient.ConfigurationChanged += ConnMultiplexer_ConfigurationChanged;
            //self.redisClient.HashSlotMoved += ConnMultiplexer_HashSlotMoved;
            //self.redisClient.InternalError += ConnMultiplexer_InternalError;
            //self.redisClient.ConfigurationChangedBroadcast += ConnMultiplexer_ConfigurationChangedBroadcast;
        }

        /// <summary>
        /// 重新配置广播时（通常意味着主从同步更改）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void ConnMultiplexer_ConfigurationChangedBroadcast(this RedisComponent self, object sender, EndPointEventArgs e)
        {
            Log.Debug($"{nameof(ConnMultiplexer_ConfigurationChangedBroadcast)}: {e.EndPoint}");
        }

        /// <summary>
        /// 发生内部错误时（主要用于调试）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void ConnMultiplexer_InternalError(this RedisComponent self, object sender, InternalErrorEventArgs e)
        {
            Log.Debug($"{nameof(ConnMultiplexer_InternalError)}: {e.Exception}");
        }

        /// <summary>
        /// 更改集群时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void ConnMultiplexer_HashSlotMoved(this RedisComponent self, object sender, HashSlotMovedEventArgs e)
        {
            Log.Debug(
                $"{nameof(ConnMultiplexer_HashSlotMoved)}: {nameof(e.OldEndPoint)}-{e.OldEndPoint} To {nameof(e.NewEndPoint)}-{e.NewEndPoint}, ");
        }

        /// <summary>
        /// 配置更改时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void ConnMultiplexer_ConfigurationChanged(this RedisComponent self, object sender, EndPointEventArgs e)
        {
            Log.Debug($"{nameof(ConnMultiplexer_ConfigurationChanged)}: {e.EndPoint}");
        }

        /// <summary>
        /// 发生错误时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void ConnMultiplexer_ErrorMessage(this RedisComponent self, object sender, RedisErrorEventArgs e)
        {
            Log.Debug($"{nameof(ConnMultiplexer_ErrorMessage)}: {e.Message}");
        }

        /// <summary>
        /// 物理连接失败时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void ConnMultiplexer_ConnectionFailed(this RedisComponent self, object sender, ConnectionFailedEventArgs e)
        {
            Log.Debug($"{nameof(ConnMultiplexer_ConnectionFailed)}: {e.Exception}");
        }

        /// <summary>
        /// 建立物理连接时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void ConnMultiplexer_ConnectionRestored(this RedisComponent self, object sender, ConnectionFailedEventArgs e)
        {
            Log.Debug($"{nameof(ConnMultiplexer_ConnectionRestored)}: {e.Exception}");
        }
        #endregion

        #region 十、序列化方法封装

        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private static string Serialize(object obj)
        {
            if (obj == null)
                return null;
            return MongoHelper.ToJson(obj);
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        private static T Deserialize<T>(string data)
        {
            if (data == null)
                return default(T);
            return MongoHelper.FromJson<T>(data);

        }
        #endregion
    }
}