using System;
using System.Collections.Generic;
using System.IO;

namespace ET
{

    [ObjectSystem]
    public class LuBanConfigAwakeSystem : AwakeSystem<LuBanConfigComponent>
    {
        protected override void Awake(LuBanConfigComponent self)
        {
            LuBanConfigComponent.Instance = self;
        }
    }

    [ObjectSystem]
    public class LuBanConfigDestroySystem : DestroySystem<LuBanConfigComponent>
    {
        protected override void Destroy(LuBanConfigComponent self)
        {
            LuBanConfigComponent.Instance = null;
        }
    }
    public static class LuBanConfigComponentFunc
    {
        public static async ETTask LoadAsync(this LuBanConfigComponent self)
        {
            Dictionary<string, byte[]> output = new Dictionary<string, byte[]>();
            foreach (string file in Directory.GetFiles($"../Config/Luban", "*.bytes"))
            {
                string key = Path.GetFileNameWithoutExtension(file);
                output[key] = await File.ReadAllBytesAsync(file);
            }
            Type type = EventSystem.Instance.GetType("cfg.Tables");
            // == luban ==
            Func<string, Bright.Serialization.ByteBuf> loader = (file => new Bright.Serialization.ByteBuf(output[file]));
            self.LuBanConfig = Activator.CreateInstance(type, loader);
            // == luban ==
            await self.LoadClientSyncConfig();
        }

        public static cfg.Tables Config(this LuBanConfigComponent self)
        {
            // == luban ==
            return self.LuBanConfig as cfg.Tables;
            // == luban ==
        }

        public static async ETTask Reload(this LuBanConfigComponent self)
        {
            await self.LoadAsync();
            Log.Console("reload LuBanConfig finish!");
        }

        /// <summary>
        /// 重载需要同步给客户端的表
        /// </summary>
        /// <param name="self"></param>
        public static async ETTask LoadClientSyncConfig(this LuBanConfigComponent self)
        {
            self.ClientSyncConfig.Clear();
            var ClientJsonConfig = new List<string>()
            {
            /// 商店表
                "shop_shopconfig",
            /// 道具表
                "item_itemconfig",
            };
            foreach (var filename in ClientJsonConfig)
            {
                var filepath = $"../Config/Luban/{filename}.json";
                if (!File.Exists(filepath))
                {
                    Log.Error("miss client config :" + filename);
                    continue;
                }
                var _json = await File.ReadAllTextAsync(filepath, System.Text.Encoding.UTF8);
                self.ClientSyncConfig.Add(filename, _json);
            }
            /// 添加其他表
            var str = MongoHelper.ToClientJson(self);
            LuBanConfigComponent.InstanceBase64 = GameConfig.DealSyncClientString(str);
        }


    }



}
