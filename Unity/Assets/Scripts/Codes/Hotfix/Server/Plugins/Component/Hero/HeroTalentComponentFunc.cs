using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET.Server
{
    public static class HeroTalentComponentFunc
    {
        public static void LoadAllChild(this HeroTalentComponent self)
        {
        }

        public static (int, string) ChangeTalentState(this HeroTalentComponent self, int level, int index, bool isLearn = true)
        {
            if (index < 0 || index > 1)
            {
                return (ErrorCode.ERR_Error, "index out of range");
            }

            if (level < 0 || level > 4)
            {
                return (ErrorCode.ERR_Error, "level out of range");
            }

            if (isLearn)
            {
                if (self.TalentPoint == 0)
                {
                    return (ErrorCode.ERR_Error, "TalentPoint no enough");
                }

                int talentId = level * 100 + index;
                if (self.TalentLearn.Contains(talentId))
                {
                    return (ErrorCode.ERR_Error, "TalentState is learned");
                }

                self.TalentLearn.Add(talentId);
            }

            return (ErrorCode.ERR_Success, "");
        }

        public static void OnHeroLevelUp(this HeroTalentComponent self)
        {
            var level = self.HeroUnit.Level;
            var talentPoint = LuBanConfigComponent.Instance.Config().HeroLevelUpConfig.GetOrDefault(level).TotalTalentPoint;
            if (talentPoint > self.TotalTalentPoint)
            {
                self.TalentPoint += talentPoint - self.TotalTalentPoint;
                self.TotalTalentPoint = talentPoint;
            }
        }
    }
}