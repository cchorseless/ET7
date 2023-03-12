using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ET;

namespace cfg.Item
{
    public partial class ItemEquipConfig
    {
        public int RandomEquipByHero(string heroConfigName)
        {
            var records = this.DataList.FindAll(record =>
            {
                var itemconfig = LuBanConfigComponent.Instance.Config().ItemConfig.GetOrDefault(record.Id);
                if (itemconfig != null)
                {
                    return itemconfig.BindHeroName == heroConfigName;
                }
                return false;
            });
            return ET.RandomGenerator.RandomArray(records).Id;
        }

        public int RandomEquipBySuit(int suitId)
        {
            var records = this.DataList.FindAll(record => record.SuitId == suitId);
            return ET.RandomGenerator.RandomArray(records).Id;
        }
    }
}