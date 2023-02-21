using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET
{
    [FriendOf(typeof(ET.TStatefulTimer))]
    public static class TStatefulTimerFunc
    {
        public static async ETTask CheckTimer(this TStatefulTimer self)
        {
            var parent = self.GetParent<Entity>();
            if (parent == null)
            {
                await TimerComponent.Instance.WaitFrameAsync(self.cancellationToken);
                self.Dispose();
                return;
            }
            if (TimeHelper.ServerNow() >= self.OperateTime)
            {
                await TimerComponent.Instance.WaitFrameAsync(self.cancellationToken);
                if (!self.IsDisposed)
                {
                    await StatefulTimerComponent.Instance.RunTimer(parent, self.Label);
                    self.Dispose();
                }
            }
            else
            {
                await TimerComponent.Instance.WaitTillAsync(self.OperateTime, self.cancellationToken);
                if (!self.IsDisposed)
                {
                    await StatefulTimerComponent.Instance.RunTimer(parent, self.Label);
                    self.Dispose();
                }
            }
        }

        public static void Enable(this TStatefulTimer self)
        {
            self.Clear();
            self.cancellationToken = new ETCancellationToken();
            self.CheckTimer().Coroutine();
        }
        public static void Clear(this TStatefulTimer self)
        {
            if (self.cancellationToken != null)
            {
                self.cancellationToken.Cancel();
                self.cancellationToken = null;
            }
        }
    }
    [ObjectSystem]
    public class TStatefulTimerAwakeSystem : AwakeSystem<TStatefulTimer, long>
    {
        protected override void Awake(TStatefulTimer self, long operateTime)
        {
            self.OperateTime = operateTime;
            self.Label = 0;
            self.Enable();
        }
    }

    [ObjectSystem]
    public class TStatefulTimerAwakeSystem1 : AwakeSystem<TStatefulTimer, long, int>
    {
        protected override void Awake(TStatefulTimer self, long operateTime, int label)
        {
            self.OperateTime = operateTime;
            self.Label = label;
            self.Enable();
        }
    }

    [ObjectSystem]
    public class TStatefulTimerDestroySystem : DestroySystem<TStatefulTimer>
    {
        protected override void Destroy(TStatefulTimer self)
        {
            self.Clear();
        }
    }

    [ObjectSystem]
    public class TStatefulTimerEnableSystem : EnableSystem<TStatefulTimer>
    {
        protected override void Enable(TStatefulTimer self)
        {
            self.Enable();
        }
    }

    public class TStatefulTimer : Entity, IAwake<long>, IAwake<long, int>, IEnable, IDestroy, ISerializeToEntity
    {
        public long OperateTime;

        public int Label;

        [BsonIgnore]
        public ETCancellationToken cancellationToken;
    }
}
