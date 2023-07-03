using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bright.Serialization;


namespace ET.Conf.Dota
{
    public sealed partial class PropRandomConfigRecord : Bright.Config.BeanBase {

        public List<PropPoolBean> GetRandomPropId(int count)
        {
            var weight = new List<int>();
            var r = new List<PropPoolBean>();

            foreach (var data in this.PropPool)
            {
                weight.Add(data.PropWeight);
            }
            for (var i = 0; i < count; i++)
            {
                r.Add(this.PropPool[RandomGenerator.RandomByWeight(weight)]);
            }
            return r;
        }

        public PropPoolBean GetOneRandomPropId()
        {
            var weight = new List<int>();
            foreach (var data in this.PropPool)
            {
                weight.Add(data.PropWeight);
            }
            return this.PropPool[RandomGenerator.RandomByWeight(weight)];
        }
    }
}