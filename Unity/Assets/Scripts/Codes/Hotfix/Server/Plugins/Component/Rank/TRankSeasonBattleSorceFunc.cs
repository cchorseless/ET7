using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ET.Server
{
    public static class TRankSeasonBattleSorceFunc
    {
        public static void LoadAllChild(this TRankSeasonBattleSorce self)
        {
            self.LoadFakerData(800, 1400);
            self.AutoRankSort(10 * 60).Coroutine();
        }
    }
}