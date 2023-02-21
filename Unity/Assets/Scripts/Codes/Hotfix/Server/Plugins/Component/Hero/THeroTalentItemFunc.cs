using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET.Server
{
    [ObjectSystem]
    public class THeroTalentItemAwakeSystem : AwakeSystem<THeroTalentItem, int>
    {
        protected override void Awake(THeroTalentItem self, int ConfigId)
        {
            self.ConfigId = ConfigId;
        }
    }


    public static class THeroTalentItemFunc
    {
        public static void LoadAllChild(this THeroTalentItem self)
        {
            var config = self.TalentConfig();
            var buffComp = self.HeroTalentComp.HeroUnit.Character.BuffComp;
            self.TalentBuff.FindAll(buffconfig =>
            {
                return !config.TalentBuffs.Contains(buffconfig);
            }).ForEach(loseEffectConfig =>
            {
                buffComp.RemoveBuff(loseEffectConfig);
                self.TalentBuff.Remove(loseEffectConfig);
            });
            foreach (var buffConfigId in config.TalentBuffs)
            {
                if (!self.TalentBuff.Contains(buffConfigId))
                {
                    buffComp.AddBuff(buffConfigId);
                    self.TalentBuff.Add(buffConfigId);
                }
            }
        }

        public static cfg.Hero.HeroTalentConfigRecord TalentConfig(this THeroTalentItem self)
        {
            return LuBanConfigComponent.Instance.Config().HeroTalentConfig.GetOrDefault(self.ConfigId);
        }


        public static void ClearBuff(this THeroTalentItem self)
        {
            var buffComp = self.HeroTalentComp.HeroUnit.Character.BuffComp;
            self.TalentBuff.ForEach(buffconfig =>
            {
                buffComp.RemoveBuff(buffconfig);
            });
            self.TalentBuff.Clear();
        }
    }
}
