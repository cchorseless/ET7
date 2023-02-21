using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET.Server
{
    public static class SeedRandomComponentFunc
    {
        public static void SetSeed(this SeedRandomComponent self, int seed)
        {
            self.Seed = seed;
            self.SeedCount++;
        }

        public static int Next(this SeedRandomComponent self)
        {
            return self.NextInt(0, int.MaxValue);
        }

        public static double NextDouble(this SeedRandomComponent self, double min, double max)
        {
            return min + Math.Abs(self.Sample() * (max - min));
        }

        public static int NextInt(this SeedRandomComponent self, int min, int max)
        {
            return (int)Math.Floor(self.NextDouble(min, max));
        }

        public static double Sample(this SeedRandomComponent self)
        {
            long r = self.Seed * 9301 + 49297;
            self.SetSeed((int)(r % 233280));
            return self.Seed / 233280.0;
        }

        public static int Rand(this SeedRandomComponent self, int n)
        {
            // 注意，返回值是左闭右开，所以maxValue要加1
            return self.NextInt(1, n + 1);
        }
        /// <summary>
        /// 随机从数组中取若干个不重复的元素，
        /// 为了降低算法复杂度，所以是伪随机，对随机要求不是非常高的逻辑可以用
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sourceList"></param>
        /// <param name="destList"></param>
        /// <param name="randCount"></param>
        public static List<T> GetRandListByCount<T>(this SeedRandomComponent self, List<T> sourceList, int randCount)
        {
            List<T> destList = new List<T>();
            if (sourceList == null || randCount < 0 || randCount == 0)
            {
                return destList;
            }
            if (randCount >= sourceList.Count)
            {
                destList.AddRange(sourceList);
                return destList;
            }
            var copy = new List<T>();
            copy.AddRange(sourceList);
            while (randCount > 0)
            {
                if (copy.Count == 0) { break; }
                int beginIndex = self.NextInt(0, copy.Count - 1);
                if (beginIndex < 0) { beginIndex = 0; }
                destList.Add(copy[beginIndex]);
                copy.RemoveAt(beginIndex);
                randCount--;
            }
            return destList;
        }


        public static int RandomByWeight(this SeedRandomComponent self, List<int> weights)
        {
            if (weights.Count == 0)
            {
                return -1;
            }

            if (weights.Count == 1)
            {
                return 0;
            }

            int sum = 0;
            for (int i = 0; i < weights.Count; i++)
            {
                sum += weights[i];
            }

            int number_rand = self.NextInt(1, sum + 1);

            int sum_temp = 0;

            for (int i = 0; i < weights.Count; i++)
            {
                sum_temp += weights[i];
                if (number_rand <= sum_temp)
                {
                    return i;
                }
            }

            return -1;
        }


        public static bool RandomBool(this SeedRandomComponent self)
        {
            return self.NextInt(0, 2) == 0;
        }
    }



    [ObjectSystem]
    public class SeedRandomComponentAwakeSystem : AwakeSystem<SeedRandomComponent>
    {
        protected override void Awake(SeedRandomComponent self)
        {
            int seed = RandomGenerator.RandomNumber(0, 100000);
            self.BeginSeed = seed;
            self.Seed = seed;
        }
    }
}
