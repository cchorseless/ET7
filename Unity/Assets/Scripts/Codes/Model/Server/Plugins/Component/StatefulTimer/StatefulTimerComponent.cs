using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET
{
    public enum EStatefulTimer
    {
        Enable = 1,
        Disable = 2,
        WaitEnable = 4,
        WaitDisable = 8,
    }

    public class StatefulTimerAttribute : BaseAttribute
    {
        public int Label { get; }

        public StatefulTimerAttribute(int label = 0)
        {
            this.Label = label;
        }
    }

    [ObjectSystem]
    public class StatefulTimerComponentAwakeSystem : AwakeSystem<StatefulTimerComponent>
    {
        protected override void Awake(StatefulTimerComponent self)
        {
            StatefulTimerComponent.Instance = self;
            self.Awake();
        }
    }


    [ObjectSystem]
    public class StatefulTimerComponentLoadSystem : LoadSystem<StatefulTimerComponent>
    {
        protected override void Load(StatefulTimerComponent self)
        {
            self.Awake();
        }
    }

    [ObjectSystem]
    public class StatefulTimerComponentDestroySystem : DestroySystem<StatefulTimerComponent>
    {
        protected override void Destroy(StatefulTimerComponent self)
        {
            StatefulTimerComponent.Instance = null;
        }
    }

    public interface IStatefulTimer
    {
        ETTask Handle(object args);

        Type GetArgsType();
    }

    public abstract class AStatefulTimer<T> : IStatefulTimer where T : Entity
    {
        public async ETTask Handle(object args)
        {
            await this.Run(args as T);
        }

        public Type GetArgsType()
        {
            return typeof(T);
        }
        public abstract ETTask Run(T t);
    }

    public class StatefulTimerComponent : Entity, IAwake, ILoad, IDestroy
    {
        public static StatefulTimerComponent Instance
        {
            get;
            set;
        }
        public Dictionary<Type, Dictionary<int, IStatefulTimer>> timerActions { get; set; }

    }

    public static class StatefulTimerComponentFunc
    {
        public static void Awake(this StatefulTimerComponent self)
        {
            self.timerActions = new Dictionary<Type, Dictionary<int, IStatefulTimer>>();
            var types = EventSystem.Instance.GetTypes(typeof(StatefulTimerAttribute));

            foreach (Type type in types)
            {
                IStatefulTimer iTimer = Activator.CreateInstance(type) as IStatefulTimer;
                if (iTimer == null)
                {
                    Log.Error($"StatefulTimer Action {type.Name} 需要继承 IStatefulTimer");
                    continue;
                }
                object[] attrs = type.GetCustomAttributes(typeof(StatefulTimerAttribute), false);
                if (attrs.Length == 0)
                {
                    continue;
                }
                Type argstype = iTimer.GetArgsType();
                if (!self.timerActions.ContainsKey(argstype))
                {
                    self.timerActions.Add(argstype, new Dictionary<int, IStatefulTimer>());
                }

                foreach (object attr in attrs)
                {
                    StatefulTimerAttribute timerAttribute = attr as StatefulTimerAttribute;
                    self.timerActions[argstype].Add(timerAttribute.Label, iTimer);
                }
            }
        }

        public static async ETTask RunTimer<T>(this StatefulTimerComponent self, T entity, int label) where T : Entity
        {
            Type type = entity.GetType();
            self.timerActions.TryGetValue(type, out var iTimerDict);
            if (iTimerDict != null)
            {
                iTimerDict.TryGetValue(label, out var iTimer);
                if (iTimer != null)
                {
                    await iTimer.Handle(entity);
                }

            }
            await ETTask.CompletedTask;
        }
    }
}
