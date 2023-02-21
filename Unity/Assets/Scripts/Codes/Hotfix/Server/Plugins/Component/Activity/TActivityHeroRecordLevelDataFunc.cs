using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET.Server
{
    [ObjectSystem]
    public class TActivityHeroRecordLevelDataAwakeSystem : AwakeSystem<TActivityHeroRecordLevelData>
    {
        protected override void Awake(TActivityHeroRecordLevelData self)
        {
            self.HeroSumLevel = 0;
        }
    }

    public static class TActivityHeroRecordLevelDataFunc
    {
        public static void LoadAllChild(this TActivityHeroRecordLevelData self)
        {
            self.UpdateHeroSumLevel();
        }

        public static void UpdateHeroSumLevel(this TActivityHeroRecordLevelData self)
        {
            var heroManage = self.CharacterActivity.Character.HeroManageComp;
            int levelSum = 0;
            heroManage.GetAllHeroUnit().ForEach(heroUnit => { levelSum += heroUnit.Level; });
            self.HeroSumLevel = levelSum;
        }
        public static void AddHeroSumLevel(this TActivityHeroRecordLevelData self)
        {
            self.HeroSumLevel += 1;

        }

    }
}
