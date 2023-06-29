using MongoDB.Bson.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ET
{
    public partial class Entity
    {
        public bool IsGhost()
        {
            return this.GetComponent<GhostEntityComponent>() != null;
        }

        public int GhostServerId()
        {
            return this.GetComponent<GhostEntityComponent>().ServerId;
        }

        public void Merge(Entity merge, bool mergeDestroy = true)
        {
            if (merge == null || merge is GhostEntityComponent)
            {
                return;
            }

            Type t = this.GetType();
            if (t.Name != merge.GetType().Name)
            {
                return;
            }

            ProcessGhostSyncComponent.Instance.MergeProps(t, this, merge);
            if (this.childrenDB != null)
            {
                var _list = this.childrenDB.ToList();
                foreach (var _entity in _list)
                {
                    if (merge.childrenDB == null)
                    {
                        _entity.Dispose();
                    }
                    else
                    {
                        if (!merge.childrenDB.Any(e => { return e.Id == _entity.Id; }))
                        {
                            _entity.Dispose();
                        }
                    }
                }
            }

            if (this.componentsDB != null)
            {
                var _list = this.componentsDB.ToList();
                foreach (var _entity in _list)
                {
                    if (merge.componentsDB == null)
                    {
                        _entity.Dispose();
                    }
                    else
                    {
                        if (!merge.componentsDB.Any(e => { return e.GetType().Name == _entity.GetType().Name; }))
                        {
                            _entity.Dispose();
                        }
                    }
                }
            }

            if (merge.childrenDB != null)
            {
                var _list = merge.childrenDB.ToList();
                foreach (var _entity in _list)
                {
                    if (this.Children.TryGetValue(_entity.Id, out var child))
                    {
                        child.Merge(_entity, false);
                    }
                    else
                    {
                        this.AddChild(_entity);
                    }
                }
            }

            if (merge.componentsDB != null)
            {
                var _list = merge.componentsDB.ToList();
                foreach (var _component in _list)
                {
                    if (this.Components.TryGetValue(_component.GetType(), out var component))
                    {
                        component.Merge(_component, false);
                    }
                    else
                    {
                        this.AddComponent(_component);
                    }
                }
            }

            if (mergeDestroy)
            {
                merge.Dispose();
            }
        }

        public List<K> GetChilds<K>() where K : Entity
        {
            var _list = new List<K>();
            if (this.children == null)
            {
                return _list;
            }

            Type type = typeof (K);
            foreach (var component in this.Children)
            {
                if (component.Value.GetType().Equals(type))
                {
                    _list.Add((K)component.Value);
                }
            }

            return _list;
        }

        public K GetUnActiveChild<K>(long entityId) where K : Entity
        {
            if (this.childrenDB == null)
            {
                return null;
            }

            var _list = this.childrenDB.ToList();
            Type type = typeof (K);
            foreach (var _child in _list)
            {
                if (_child.Id == entityId)
                {
                    return (K)_child;
                }
            }

            return null;
        }

        public K GetUnActiveComponent<K>() where K : Entity
        {
            if (this.componentsDB == null)
            {
                return null;
            }

            var _list = this.componentsDB.ToList();
            Type type = typeof (K);
            foreach (var component in _list)
            {
                if (component.GetType().Equals(type))
                {
                    return (K)component;
                }
            }

            return null;
        }

        public static void EntityWithNewId<T>(T entity, Dictionary<string, string> idReplace) where T : Entity
        {
            string oldId = entity.Id.ToString();
            if (!idReplace.ContainsKey(oldId))
            {
                idReplace.Add(oldId, IdGenerater.Instance.GenerateId().ToString());
            }

            foreach (var _child in entity.Children)
            {
                EntityWithNewId(_child.Value, idReplace);
            }

            foreach (var _child in entity.Components)
            {
                EntityWithNewId(_child.Value, idReplace);
            }
        }

        public static T CloneWithNewId<T>(T entity) where T : Entity
        {
            string json = MongoHelper.ToJson(entity);
            var idReplace = new Dictionary<string, string>();
            EntityWithNewId(entity, idReplace);
            foreach (var kv in idReplace)
            {
                json = json.Replace(kv.Key, kv.Value);
            }

            T t = MongoHelper.FromJson<T>(json);
            t.Domain = entity.Domain;
            return t;
        }

        public static Entity CreateOne(Scene scene, Type type, bool isFromPool = false)
        {
            Entity component = Entity.Create(type, isFromPool) as Entity;
            component.Id = IdGenerater.Instance.GenerateId();
            component.Domain = scene.Domain;
            return component;
        }

        public static T CreateOne<T>(bool isFromPool = false) where T : Entity
        {
            Type type = typeof (T);
            T component = (T)Entity.Create(type, isFromPool);
            component.Id = IdGenerater.Instance.GenerateId();
            return component;
        }

        public static T CreateOne<T>(Scene scene, bool isFromPool = false) where T : Entity
        {
            Type type = typeof (T);
            T component = (T)Entity.Create(type, isFromPool);
            component.Id = IdGenerater.Instance.GenerateId();
            component.Domain = scene.Domain;
            return component;
        }

        public static T CreateOne<T, A>(Scene scene, A p1, bool isFromPool = false) where T : Entity, IAwake<A>
        {
            Type type = typeof (T);
            T component = (T)Entity.Create(type, isFromPool);
            component.Id = IdGenerater.Instance.GenerateId();
            component.Domain = scene.Domain;
            EventSystem.Instance.Awake(component, p1);
            return component;
        }

        public static T CreateOne<T, A, B>(Scene scene, A p1, B p2, bool isFromPool = false) where T : Entity, IAwake<A, B>
        {
            Type type = typeof (T);
            T component = (T)Entity.Create(type, isFromPool);
            component.Id = IdGenerater.Instance.GenerateId();
            component.Domain = scene.Domain;
            EventSystem.Instance.Awake(component, p1, p2);
            return component;
        }
    }
}