using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ET.Server
{
    public class WatcherSessionComponentSystemAwakeSystem : AwakeSystem<WatcherSessionComponent>
    {
        protected override void Awake(WatcherSessionComponent self)
        {
            WatcherSessionComponent.Instance = self;
        }
    }

    public class WatcherSessionComponentSystemDestroySystem : DestroySystem<WatcherSessionComponent>
    {
        protected override void Destroy(WatcherSessionComponent self)
        {
            WatcherSessionComponent.Instance = null;
        }
    }

    public static class WatcherSessionComponentSystem
    {
        public static Session GetThisMachineWatcherSession(this WatcherSessionComponent self)
        {
            Session session;
            if (self.WatcherSessionId != 0)
            {
                session = NetInnerComponent.Instance.GetChild<Session>(self.WatcherSessionId);
                if (session != null)
                {
                    return session;
                }
                self.WatcherSessionId = 0;
            }
            var startMachineConfig = WatcherHelper.GetThisMachineConfig();
            var ipPoint = NetworkHelper.ToIPEndPoint($"{startMachineConfig.InnerIP}:{startMachineConfig.WatcherPort}");
            session = NetInnerComponent.Instance.Create(ipPoint);
            self.WatcherSessionId = session.Id;
            return session;

        }

        public static Session GetOtherMachineWatcherSession(this WatcherSessionComponent self)
        {
            Session session;
            if (self.WatcherSessionId != 0)
            {
                session = NetInnerComponent.Instance.GetChild<Session>(self.WatcherSessionId);
                if (session != null)
                {
                    return session;
                }
                self.WatcherSessionId = 0;
            }
            var startMachineConfig = WatcherHelper.GetThisMachineConfig();
            var ipPoint = NetworkHelper.ToIPEndPoint($"{startMachineConfig.InnerIP}:{startMachineConfig.WatcherPort}");
            session = NetInnerComponent.Instance.Create(ipPoint);
            self.WatcherSessionId = session.Id;
            return session;

        }


    }
}