using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET
{
    public class SeedRandomComponent : Entity, IAwake, ISerializeToEntity
    {
        public int BeginSeed;

        public int Seed;

        public int SeedCount;

    }

}
