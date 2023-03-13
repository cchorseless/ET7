using System;
using System.Collections.Generic;
using System.Linq;

namespace ET.Server
{
    [ObjectSystem]
    public class TCharacterDestroySystem: DestroySystem<TCharacter>
    {
        protected override void Destroy(TCharacter self)
        {
            self.GetMyServerZone()?.CharacterComp.Remove(self.Id);
            self.PartialOnlineTime += TimeHelper.ServerNow() - self.LastLoginTime;
        }
    }

    public static class TCharacterFunc
    {
        public static async ETTask<T> LoadOrAddComponent<T>(this TCharacter self) where T : Entity, IAwake, new()
        {
            var comp = self.GetComponent<T>();
            if (comp != null)
            {
                return comp;
            }

            DBComponent db = DBManagerComponent.Instance.GetZoneDB(self.DomainZone());
            comp = await db.Query<T>(self.Id);
            if (comp == null)
            {
                comp = self.AddComponent<T>();
                await db.Save(comp);
            }
            else
            {
                self.AddComponent(comp);
            }

            return comp;
        }

        public static async ETTask LoadAllComponent(this TCharacter self)
        {
            self.AddComponent<TimerEntityComponent>();
            await self.LoadOrAddComponent<BagComponent>();
            self.BagComp.LoadAllChild();
            await self.LoadOrAddComponent<CharacterDataComponent>();
            self.DataComp.LoadAllChild();
            await self.LoadOrAddComponent<CharacterSteamComponent>();
            self.SteamComp.LoadAllChild();
            await self.LoadOrAddComponent<CharacterShopComponent>();
            self.ShopComp.LoadAllChild();
            await self.LoadOrAddComponent<CharacterTaskComponent>();
            self.TaskComp.LoadAllChild();
            await self.LoadOrAddComponent<CharacterMailComponent>();
            self.MailComp.LoadAllChild();
            await self.LoadOrAddComponent<CharacterActivityComponent>();
            self.ActivityComp.LoadAllChild();
            await self.LoadOrAddComponent<HeroManageComponent>();
            self.HeroManageComp.LoadAllChild();
            await self.LoadOrAddComponent<CharacterDrawTreasureComponent>();
            self.DrawTreasureComp.LoadAllChild();
            await self.LoadOrAddComponent<CharacterRechargeComponent>();
            self.RechargeComp.LoadAllChild();
            await self.LoadOrAddComponent<CharacterBuffComponent>();
            self.BuffComp.LoadAllChild();
            await self.LoadOrAddComponent<CharacterTitleComponent>();
            self.TitleComp.LoadAllChild();
            await self.LoadOrAddComponent<CharacterAchievementComponent>();
            self.AchievementComp.LoadAllChild();
            await self.LoadOrAddComponent<CharacterGameRecordComponent>();
            self.GameRecordComp.LoadAllChild();
        }

        public static async ETTask Save(this TCharacter self)
        {
            DBComponent db = DBManagerComponent.Instance.GetZoneDB(self.DomainZone());
            if (self.GmLevel <= (int)EGmPlayerRole.GmRole_PlayerGm)
            {
                await db.Save(self.Id, new List<Entity>()
                {
                    self.BagComp,
                    self.DataComp,
                    self.SteamComp,
                    self.ShopComp,
                    self.TaskComp,
                    self.MailComp,
                    self.ActivityComp,
                    self.HeroManageComp,
                    self.DrawTreasureComp,
                    self.RechargeComp,
                    self.BuffComp,
                    self.TitleComp,
                    self.AchievementComp,
                    self.GameRecordComp,
                });
            }

            await db.Save(self);
            // Log.Console("save TCharacter :" + self.Name);
        }

        public static void SyncClientCharacterData(this TCharacter self)
        {
            self.SyncHttpEntity(new Entity[]
            {
                self.BagComp, self.DataComp, self.ShopComp, self.TaskComp, self.MailComp, self.ActivityComp, self.HeroManageComp,
                self.DrawTreasureComp, self.RechargeComp, self.BuffComp, self.TitleComp, self.AchievementComp, self.GameRecordComp,
            });
        }

        public static void SyncClientServerZoneData(this TCharacter self)
        {
            // self.SyncHttpEntity(LuBanConfigComponent.InstanceBase64);
            var serverZone = self.GetMyServerZone();
            if (serverZone != null)
            {
                self.SyncHttpEntity(serverZone);
                self.SyncHttpEntity(new Entity[]
                {
                    serverZone.SeasonComp, serverZone.ShopComp, serverZone.ActivityComp, serverZone.RankComp, serverZone.BuffComp,
                    serverZone.GameRecordComp,
                });
            }
        }

        public static long TodayTotalOnlineTime(this TCharacter self)
        {
            return self.PartialOnlineTime + TimeHelper.ServerNow() - self.LastLoginTime;
        }

        public static Player GetMyPlayer(this TCharacter self)
        {
            return self.GetParent<Player>();
        }

        public static TServerZone GetMyServerZone(this TCharacter self)
        {
            return ServerZoneManageComponent.Instance.GetServerZone(self.ServerID);
        }

        public static void SendToClient(this TCharacter self, IMessage message)
        {
            MessageHelper.SendToClient(self.GetMyPlayer(), message);
        }

        public static void SyncClientEntity<T>(this TCharacter self, T entity) where T : Entity
        {
            var str = MongoHelper.ToClientJson(entity);
            var bytes = ZipHelper.Compress(str.ToByteArray());
            self.SendToClient(new Actor_SyncEntity() { Entity = bytes, });
        }

        public static void SyncClientEntity(this TCharacter self, Entity[] entitys)
        {
            string str = "[";
            for (int i = 0; i < entitys.Length; i++)
            {
                str += MongoHelper.ToClientJson(entitys[i]);
                if (i < entitys.Length - 1)
                {
                    str += ",";
                }
            }

            str += "]";
            var bytes = ZipHelper.Compress(str.ToByteArray());
            self.SendToClient(new Actor_SyncEntity() { Entity = bytes, });
        }

        public static void RemoveClientEntity<T>(this TCharacter self, T[] entitys) where T : Entity
        {
            var msg = new Actor_RemoveEntity();
            foreach (var entity in entitys)
            {
                msg.EntityId.Add(entity.Id.ToString());
                msg.EntityType.Add(entity.GetType().Name);
            }

            self.SendToClient(msg);
        }

        public static void SyncClientEntityProps<T>(this TCharacter self, T entity, string[] propsName) where T : Entity
        {
            if (entity != null)
            {
                var str = MongoHelper.ToClientJson(entity);
                var litjson = JsonHelper.FromLitJson(str).AsObject();
                var keys = JsonHelper.Keys(litjson);
                foreach (var propName in keys)
                {
                    if (propName != "_t" && propName != "_id" && !propsName.Contains(propName))
                    {
                        litjson.Remove(propName);
                    }
                }

                if (litjson.Count > 0)
                {
                    self.SendToClient(new Actor_SyncEntityProps() { EntityProps = ZipHelper.Compress(JsonHelper.ToLitJson(litjson).ToByteArray()) });
                }
            }
        }

        public static void SyncHttpEntity(this TCharacter self, string base64str)
        {
            var player = self.GetMyPlayer();
            var HttpPlayerSession = player.GetComponent<HttpPlayerSessionComponent>();
            HttpPlayerSession.SendToClient(base64str);
        }

        public static void SyncHttpEntity<T>(this TCharacter self, T entity, bool includeChild = true) where T : Entity
        {
            if (entity == null)
            {
                Log.Error("entity is null");
                return;
            }

            var str = MongoHelper.ToClientJson(entity);
            var player = self.GetMyPlayer();
            var HttpPlayerSession = player.GetComponent<HttpPlayerSessionComponent>();
            if (includeChild == false)
            {
                var litjson = JsonHelper.FromLitJson(str).AsObject();
                litjson.Remove("Children");
                litjson.Remove("C");
                str = JsonHelper.ToLitJson(litjson);
            }

            HttpPlayerSession.SendToClient(GameConfig.DealSyncClientString(str));
        }

        public static void SyncHttpEntity(this TCharacter self, Entity[] entitys)
        {
            var player = self.GetMyPlayer();
            var HttpPlayerSession = player.GetComponent<HttpPlayerSessionComponent>();
            for (int i = 0; i < entitys.Length; i++)
            {
                if (entitys[i] == null)
                {
                    Log.Error($"entity is null, index : {i}");
                    continue;
                }

                var str = MongoHelper.ToClientJson(entitys[i]);
                HttpPlayerSession.SyncString.Add(GameConfig.DealSyncClientString(str));
            }

            HttpPlayerSession.SendToClient();
        }

        public static void SyncHttpEntityAndChild<T>(this TCharacter self, T entity, long childId) where T : Entity
        {
            self.SyncHttpEntityAndChilds(entity, new long[] { childId });
        }
        public static void SyncHttpEntityAndChilds<T>(this TCharacter self, T entity, long[] childIds) where T : Entity
        {
            if (entity == null)
            {
                Log.Error("entity is null");
                return;
            }

            var str = MongoHelper.ToClientJson(entity);
            var litjson = JsonHelper.FromLitJson(str).AsObject();
            var player = self.GetMyPlayer();
            var HttpPlayerSession = player.GetComponent<HttpPlayerSessionComponent>();
            if (litjson.ContainsKey("Children"))
            {
                var childs = litjson["Children"].AsArray();
                var _childs= childs.ToArray();
                foreach (var child in _childs)
                {
                    var childId = long.TryParse(child["_id"].ToString(), out var id)? id : 0;
                    if (!childIds.Contains(childId))
                    {
                        childs.Remove(child);
                    }
                }
            }
            str = JsonHelper.ToLitJson(litjson);
            HttpPlayerSession.SendToClient(GameConfig.DealSyncClientString(str));
        }

        public static void SyncHttpEntityProps<T>(this TCharacter self, T entity, string[] propsName) where T : Entity
        {
            if (entity == null)
            {
                Log.Error("entity is null");
                return;
            }

            var jsonstr = MongoHelper.ToClientJson(entity);
            var litjson = JsonHelper.FromLitJson(jsonstr).AsObject();
            var keys = JsonHelper.Keys(litjson);
            foreach (var propName in keys)
            {
                if (propName != "_t" && propName != "_id" && !propsName.Contains(propName))
                {
                    litjson.Remove(propName);
                }
            }

            if (litjson.Count > 0)
            {
                var player = self.GetMyPlayer();
                var HttpPlayerSession = player.GetComponent<HttpPlayerSessionComponent>();
                var str = JsonHelper.ToLitJson(litjson);
                HttpPlayerSession.SendToClient(GameConfig.DealSyncClientString(str));
            }
        }
    }

    [ObjectSystem]
    public class TCharacterAwakeSystem: AwakeSystem<TCharacter, long>
    {
        protected override void Awake(TCharacter self, long playerId)
        {
            self.Int64PlayerId = playerId;
            self.AddComponent<SeedRandomComponent>();
        }
    }
}