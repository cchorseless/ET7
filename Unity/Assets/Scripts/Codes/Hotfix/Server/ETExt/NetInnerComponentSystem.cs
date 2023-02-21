using System.Net;
using System.Net.Sockets;

namespace ET.Server
{
    [FriendOf(typeof(NetInnerComponent))]
    public static partial class NetInnerComponentSystem {
        // 这个channelId是由CreateConnectChannelId生成的
        public static Session Create(this NetInnerComponent self, IPEndPoint ipEndPoint)
        {
            long channelId = NetServices.Instance.CreateConnectChannelId();
            Session session = self.CreateInner(channelId, ipEndPoint);
            return session;
        }
    }
}