using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace ET
{
    [ProtoContract]
    [Config]
    public partial class StartRedisConfigCategory : ConfigSingleton<StartRedisConfigCategory>, IMerge
    {
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, StartRedisConfig> dict = new Dictionary<int, StartRedisConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<StartRedisConfig> list = new List<StartRedisConfig>();
		
        public void Merge(object o)
        {
            StartRedisConfigCategory s = o as StartRedisConfigCategory;
            this.list.AddRange(s.list);
        }
		
		[ProtoAfterDeserialization]        
        public void ProtoEndInit()
        {
            foreach (StartRedisConfig config in list)
            {
                config.AfterEndInit();
                this.dict.Add(config.Id, config);
            }
            this.list.Clear();
            
            this.AfterEndInit();
        }
		
        public StartRedisConfig Get(int id)
        {
            this.dict.TryGetValue(id, out StartRedisConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (StartRedisConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, StartRedisConfig> GetAll()
        {
            return this.dict;
        }

        public StartRedisConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class StartRedisConfig: ProtoObject, IConfig
	{
		/// <summary>Id</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>数据库地址</summary>
		[ProtoMember(2)]
		public string DBConnection { get; set; }
		/// <summary>数据库名</summary>
		[ProtoMember(3)]
		public string DBName { get; set; }

	}
}
