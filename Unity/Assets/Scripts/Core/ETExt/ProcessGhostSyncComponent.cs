using MongoDB.Bson.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ET
{
    public enum EGhostChangePropType
    {
        ValueReplace = 1,
        ValueAdd = 2,
        AddChild = 3,
        AddComponent = 4,
        RemoveGhost = 5,
    }

    public class ProcessGhostSyncComponent : Entity, IAwake
    {
        public static ProcessGhostSyncComponent Instance;
        public readonly Dictionary<string, long> GhostEntityInstanceIdList = new Dictionary<string, long>();
        public readonly Dictionary<Type, BsonClassMap> GhostBsonClassMap = new Dictionary<Type, BsonClassMap>();



    }
    public static class ProcessGhostSyncComponentFunc
    {
        public static void MergeProps(this ProcessGhostSyncComponent self, Type t, Entity ghost, Entity merge)
        {
            if (!self.GhostBsonClassMap.TryGetValue(t, out var BsonClassMap))
            {
                BsonClassMap = BsonClassMap.LookupClassMap(t);
                self.GhostBsonClassMap.Add(t, BsonClassMap);
            }
            foreach (var memberMap in BsonClassMap.DeclaredMemberMaps)
            {
                var value = BsonClassMap.GetMemberMap(memberMap.MemberName).Getter(merge);
                BsonClassMap.GetMemberMap(memberMap.MemberName).Setter(ghost, value);
            }
        }

        public static BsonMemberMap GetMemberMap(this ProcessGhostSyncComponent self, Type t, string MemberName)
        {
            if (!self.GhostBsonClassMap.TryGetValue(t, out var BsonClassMap))
            {
                BsonClassMap = BsonClassMap.LookupClassMap(t);
                self.GhostBsonClassMap.Add(t, BsonClassMap);
            }
            return BsonClassMap.GetMemberMap(MemberName);
        }

    }
}
