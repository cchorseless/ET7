using System;
using System.Collections.Generic;
using System.Linq;
using ET;



namespace cfg.Item
{
    public sealed partial class ItemPrizePoolGroupConfigRecord
    {
        public List<ItemPrizePoolBean> GetRandomItemId()
        {
            var count = new List<int>();
            var weight = new List<int>();
            int randomTimes = this.RandomCountInfo[0].RandomCount;
            if (this.RandomCountInfo.Count > 1)
            {
                foreach (var data in this.RandomCountInfo)
                {
                    if (data.RandomWeight > 0 && data.RandomCount > 0)
                    {
                        count.Add(data.RandomCount);
                        weight.Add(data.RandomWeight);
                    }
                }
                randomTimes = count[RandomGenerator.RandomByWeight(weight)];
            }
            count.Clear();
            weight.Clear();
            foreach (var data in this.ItemPoolGroup)
            {
                if (data.ItemPoolConfigId > 0 && data.ItemPoolWeight > 0)
                {
                    count.Add(data.ItemPoolConfigId);
                    weight.Add(data.ItemPoolWeight);
                }
            }
            var r = new List<ItemPrizePoolBean>();
            if (this.IsRandomRepeat == 1)
            {
                for (var i = 0; i < randomTimes; i++)
                {
                    var poolConfigId = count[RandomGenerator.RandomByWeight(weight)];
                    var poolConfig = LuBanConfigComponent.Instance.Config().ItemPrizePoolConfig.GetOrDefault(poolConfigId);
                    if (poolConfig != null)
                    {
                        r.Add(poolConfig.GetOneRandomItemId());
                    }
                }
            }
            else
            {
                var quo = randomTimes / count.Count;
                var mod = randomTimes % count.Count;
                for (var i = 0; i < quo; i++)
                {
                    count.ForEach(poolConfigId =>
                    {
                        var poolConfig = LuBanConfigComponent.Instance.Config().ItemPrizePoolConfig.GetOrDefault(poolConfigId);
                        if (poolConfig != null)
                        {
                            r.Add(poolConfig.GetOneRandomItemId());
                        }
                    });
                }
                for (var i = 0; i < mod; i++)
                {
                    var index = RandomGenerator.RandomByWeight(weight);
                    var poolConfigId = count[index];
                    count.RemoveAt(index);
                    weight.RemoveAt(index);
                    var poolConfig = LuBanConfigComponent.Instance.Config().ItemPrizePoolConfig.GetOrDefault(poolConfigId);
                    if (poolConfig != null)
                    {
                        r.Add(poolConfig.GetOneRandomItemId());
                    }
                }
            }
            return r;
        }
    }
}
