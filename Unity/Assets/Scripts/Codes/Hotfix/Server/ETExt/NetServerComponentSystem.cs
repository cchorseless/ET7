using System.Collections.Generic;
using System.Net;

namespace ET.Server
{
    [FriendOf(typeof(NetServerComponent))]
    public static partial class NetServerComponentSystem
    {
        [ObjectSystem]
        public class NetServerWebSocketComponentAwakeSystem : AwakeSystem<NetServerComponent, IEnumerable<string>>
        {
            protected override void Awake(NetServerComponent self, IEnumerable<string> address)
            {
                self.ServiceId = NetServices.Instance.AddService(new WService(address));
                NetServices.Instance.RegisterAcceptCallback(self.ServiceId, self.OnAccept);
                NetServices.Instance.RegisterReadCallback(self.ServiceId, self.OnRead);
                NetServices.Instance.RegisterErrorCallback(self.ServiceId, self.OnError);
            }
        }
    }

}