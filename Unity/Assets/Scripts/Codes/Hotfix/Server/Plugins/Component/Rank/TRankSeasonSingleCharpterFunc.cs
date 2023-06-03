using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ET.Server
{
    public static class TRankSeasonSingleCharpterFunc
    {
        public static void LoadAllChild(this TRankSeasonSingleCharpter self)
        {
            self.LoadFakerData(1000,6000);
            self.AutoRankSort(10 * 60).Coroutine();
        }

    }
}
