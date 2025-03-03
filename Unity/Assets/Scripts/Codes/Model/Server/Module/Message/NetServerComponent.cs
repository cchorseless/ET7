﻿using System.Collections.Generic;
using System.Net;

namespace ET.Server
{
    public struct NetServerComponentOnRead
    {
        public Session Session;
        public object Message;
    }
    
    [ComponentOf(typeof(Scene))]
    public class NetServerComponent: Entity, IAwake<IPEndPoint>, IAwake<IEnumerable<string>>, IDestroy
    {
        public int ServiceId;
    }
}