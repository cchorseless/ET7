using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cfg.Item
{
    public partial class ItemEquipConfig
    {
        public int RandomEquipByHero(int heroConfigId)
        {
            var records = this.DataList.FindAll(record => record.BindHeroId == heroConfigId);
            return ET.RandomGenerator.RandomArray(records).Id;
        }

        public int RandomEquipBySuit(int suitId)
        {
            var records = this.DataList.FindAll(record => record.SuitId == suitId);
            return ET.RandomGenerator.RandomArray(records).Id;
        }
    }
}
