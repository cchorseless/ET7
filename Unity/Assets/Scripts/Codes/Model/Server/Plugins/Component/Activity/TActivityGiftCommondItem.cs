using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET
{
    public class TActivityGiftCommondItem : Entity, IAwake<int>, ISerializeToEntity
    {
        public int ConfigId;
        public int GiftCost;
        public int GiftMax;
    }
}
