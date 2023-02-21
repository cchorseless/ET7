using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET
{
    public class GhostEntityComponent : Entity, IAwake<int>, IDestroy, ISerializeToEntity
    {
        public int ServerId;

        public string EntityType;

    }
}
