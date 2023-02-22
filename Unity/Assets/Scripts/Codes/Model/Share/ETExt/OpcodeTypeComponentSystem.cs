using System;
using System.Collections.Generic;

namespace ET
{
    [FriendOf(typeof (OpcodeTypeComponent))]
    public static partial class OpcodeTypeComponentSystem
    {
        public static void RefreshMessageType(this OpcodeTypeComponent self)
        {
            self.requestResponse.Clear();
            self.outrActorMessage.Clear();
            HashSet<Type> types = EventSystem.Instance.GetTypes(typeof (MessageAttribute));
            foreach (Type type in types)
            {
                object[] att = type.GetCustomAttributes(typeof (MessageAttribute), false);
                if (att.Length == 0)
                {
                    continue;
                }
                MessageAttribute messageAttribute = att[0] as MessageAttribute;
                if (messageAttribute == null)
                {
                    continue;
                }
                ushort opcode = messageAttribute.Opcode;

                if (OpcodeHelper.IsOuterMessage(opcode) && typeof (IActorMessage).IsAssignableFrom(type))
                {
                    self.outrActorMessage.Add(opcode);
                }
                
                // 检查request response
                if (typeof (IRequest).IsAssignableFrom(type))
                {
                    if (typeof (IActorLocationMessage).IsAssignableFrom(type))
                    {
                        self.requestResponse.Add(type, typeof(ActorResponse));
                        continue;
                    }
                    
                    var attrs = type.GetCustomAttributes(typeof (ResponseTypeAttribute), false);
                    if (attrs.Length == 0)
                    {
                        Log.Error($"not found responseType: {type}");
                        continue;
                    }

                    ResponseTypeAttribute responseTypeAttribute = attrs[0] as ResponseTypeAttribute;
                    self.requestResponse.Add(type, EventSystem.Instance.GetType($"ET.{responseTypeAttribute.Type}"));
                }
            }
        }
    }
}