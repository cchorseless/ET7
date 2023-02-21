using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET.Server
{
    public static class TRankHeroBattleScoreFunc
    {
        public static void LoadAllChild(this TRankHeroBattleScore self)
        {
            self.RankSort();
        }
    }
}
