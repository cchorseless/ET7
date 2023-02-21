
using System.Collections.Generic;
using System.IO;

namespace ET.Server
{
    public static partial class MessageHelper
    {
        public static void SendToClient(Player player, IMessage message)
        {
            Session session = player.GetMySession();
            if (session != null)
            {
                session.Send(message);
            }
        }

        public static void SendToMap(Player player, IActorMessage message)
        {
            SendActor(player.UnitId, message);
        }

        public static void SendToGate(Unit unit, IActorMessage message)
        {
            SendActor(unit.GetComponent<UnitGateComponent>().GatePlayerActorId, message);
        }
    }
}