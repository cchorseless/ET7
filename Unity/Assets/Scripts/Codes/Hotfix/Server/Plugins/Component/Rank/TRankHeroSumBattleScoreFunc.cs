using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET.Server
{
    public static class TRankHeroSumBattleScoreFunc
    {
        public static void LoadAllChild(this TRankHeroSumBattleScore self)
        {
            self.LoadFakerData(1000,4000);
            self.AutoRankSort(10 * 60).Coroutine();
        }
    }
}
