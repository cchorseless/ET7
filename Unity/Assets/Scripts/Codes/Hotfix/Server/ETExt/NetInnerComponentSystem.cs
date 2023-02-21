using System.Net;
using System.Net.Sockets;

namespace ET.Server
{
    [FriendOf(typeof(NetInnerComponent))]
    public static partial class NetInnerComponentSystem {
        // ���channelId����CreateConnectChannelId���ɵ�
        public static Session Create(this NetInnerComponent self, IPEndPoint ipEndPoint)
        {
            long channelId = NetServices.Instance.CreateConnectChannelId();
            Session session = self.CreateInner(channelId, ipEndPoint);
            return session;
        }
    }
}