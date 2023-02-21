using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET
{
    public enum EEquipPropSlot
    {
        MinSlot = 0,

        MaxSlot = 5,
    }

    public class TEquipItem : TItem
    {
        public long[] Props = new long[(int)EEquipPropSlot.MaxSlot];

    }
}
