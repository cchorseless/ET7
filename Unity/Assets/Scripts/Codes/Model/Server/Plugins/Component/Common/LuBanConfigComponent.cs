using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET
{
    public class LuBanConfigComponent : Entity, IAwake, IDestroy
    {
        public static LuBanConfigComponent Instance;
        /// <summary>
        /// 实例的base64字符串
        /// </summary>
        public static string InstanceBase64;

        [BsonIgnore]
        public object LuBanConfig;
        /// <summary>
        /// 同步给客户端的表数据
        /// </summary>
        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
        public Dictionary<string, string> ClientSyncConfig = new Dictionary<string, string>();

    }


}
