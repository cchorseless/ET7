using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET
{
    public class TimerEntityComponent : Entity, IAwake, IDestroy
    {
        public List<long> TimerIdList = new List<long>();
    }
}
