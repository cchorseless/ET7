using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET
{
    public class TEquipItemProp : Entity, IAwake, ISerializeToEntity
    {
        public int PropQuality { get; set; }
        public int PropId;
        public string PropName;
        public int PropValue;
        public int PropMin;
        public int PropMax;
    }
}
