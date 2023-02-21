using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET.Server
{
    public class WatcherSessionComponent : Entity, IAwake, IDestroy
    {
        public static WatcherSessionComponent Instance { get; set; }

        public long WatcherSessionId { get; set; }

    }
}