using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET
{
    public class TActivityGiftCommondItem : Entity, IAwake<string>, ISerializeToEntity
    {
        public string ConfigId;
        public int GiftCost;
        public int GiftMax;
    }
}
