using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET
{

    public class ServerZoneManageComponent : Entity, IAwake
    {
        public static ServerZoneManageComponent Instance;
        public Dictionary<int, long> ServerZoneDict;
        public long LastServerNoticeID;
    }
}
