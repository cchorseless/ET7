using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace ET
{
    [ProtoContract]
    [Config]
    public partial class CommonConfigCategory : ConfigSingleton<CommonConfigCategory>, IMerge
    {
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, CommonConfig> dict = new Dictionary<int, CommonConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<CommonConfig> list = new List<CommonConfig>();
		
        public void Merge(object o)
        {
            CommonConfigCategory s = o as CommonConfigCategory;
            this.list.AddRange(s.list);
        }
		
		[ProtoAfterDeserialization]        
        public void ProtoEndInit()
        {
            foreach (CommonConfig config in list)
            {
                config.AfterEndInit();
                this.dict.Add(config.Id, config);
            }
            this.list.Clear();
            
            this.AfterEndInit();
        }
		
        public CommonConfig Get(int id)
        {
            this.dict.TryGetValue(id, out CommonConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (CommonConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, CommonConfig> GetAll()
        {
            return this.dict;
        }

        public CommonConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class CommonConfig: ProtoObject, IConfig
	{
		/// <summary>Id</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>名字</summary>
		[ProtoMember(2)]
		public string Name { get; set; }
		/// <summary>描述</summary>
		[ProtoMember(3)]
		public string Desc { get; set; }

	}
}
