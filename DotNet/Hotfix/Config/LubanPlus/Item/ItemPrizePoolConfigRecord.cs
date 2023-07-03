using System;
using System.Collections.Generic;
using System.Linq;
using ET;



namespace ET.Conf.Item
{
    public sealed partial class ItemPrizePoolConfigRecord
    {
        public List<ItemPrizePoolBean> GetRandomItemId(int count)
        {
            var weight = new List<int>();
            var r = new List<ItemPrizePoolBean>();

            foreach (var data in this.Itempool)
            {
                if (data.IsVaild) weight.Add(data.ItemWeight);
            }
            for (var i = 0; i < count; i++)
            {
                r.Add(this.Itempool[RandomGenerator.RandomByWeight(weight)]);
            }
            return r;
        }

        public ItemPrizePoolBean GetOneRandomItemId()
        {
            var weight = new List<int>();
            foreach (var data in this.Itempool)
            {
                if (data.IsVaild) weight.Add(data.ItemWeight);
            }
            return this.Itempool[RandomGenerator.RandomByWeight(weight)];
        }
    }
}
