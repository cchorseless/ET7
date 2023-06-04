using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET
{
    public class THeroBanDesign : Entity, IAwake, ISerializeToEntity
    {
        public int Slot;

        public List<string> BanConfigInfo = new List<string>();
    }
}
