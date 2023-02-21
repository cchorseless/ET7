using System;
using System.Collections.Generic;
using Random = System.Random;

namespace ET
{
    // 支持多线程
    public static partial class RandomGenerator
    {


        private static int Rand(int n)
        {
            // 注意，返回值是左闭右开，所以maxValue要加1
            return RandomNumber(1, n + 1);
        }
        /// <summary>
        /// 通过权重随机
        /// </summary>
        /// <param name="weights"></param>
        /// <returns></returns>
        public static int RandomByWeight(int[] weights)
        {
            int sum = 0;
            for (int i = 0; i < weights.Length; i++)
            {
                sum += weights[i];
            }

            int number_rand = Rand(sum);

            int sum_temp = 0;

            for (int i = 0; i < weights.Length; i++)
            {
                sum_temp += weights[i];
                if (number_rand <= sum_temp)
                {
                    return i;
                }
            }

            return -1;
        }

        public static int RandomByWeight(List<int> weights)
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

            int number_rand = Rand(sum);

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

        public static int RandomByWeight(List<int> weights, int weightRandomMinVal)
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

            int number_rand = Rand(Math.Max(sum, weightRandomMinVal));

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

      
    }
}
