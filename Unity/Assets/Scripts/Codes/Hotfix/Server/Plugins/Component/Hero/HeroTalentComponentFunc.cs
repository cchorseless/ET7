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
            var config = self.HeroUnit.HeroConfig();
            var talentInfo = config.TalentInfo;
            foreach (var talent in talentInfo)
            {
                if (self.TalentLearn.TryGetValue(talent.TalentLevel, out var learnData))
                {
                    for (int i = 0; i < learnData.Length; i++)
                    {
                        var old_talentConfigId = learnData[i];
                        var new_talentConfigId = talent.TalentGroup[i];
                        if (old_talentConfigId != 0 && old_talentConfigId != new_talentConfigId)
                        {
                            var old_talent = self.GetChild<THeroTalentItem>(self.Talents[old_talentConfigId]);
                            if (old_talent != null)
                            {
                                self.TalentPoint += old_talent.CostTalentPoint;
                                learnData[i] = 0;
                                self.Talents.Remove(old_talentConfigId);
                                old_talent.ClearBuff();
                                old_talent.Dispose();
                            }
                        }
                    }
                }
                else
                {
                    self.TalentLearn.Add(talent.TalentLevel, new int[3] { 0, 0, 0 });
                }
            }
            foreach (var entityId in self.Talents.Values)
            {
                var talent = self.GetChild<THeroTalentItem>(entityId);
                if (talent != null)
                {
                    talent.LoadAllChild();
                }
            }
        }


        public static (int, string) ChangeTalentState(this HeroTalentComponent self, int level, int index, bool isLearn)
        {
            if (!self.TalentLearn.TryGetValue(level, out var talentInfo))
            {
                return (ErrorCode.ERR_Error, "level miss");
            }
            if (talentInfo.Count() <= index + 1)
            {
                return (ErrorCode.ERR_Error, "index out of range");
            }
            int talentConfigId = 0;
            var config = self.HeroUnit.HeroConfig();
            foreach (var _talent in config.TalentInfo)
            {
                if (_talent.TalentLevel == level)
                {
                    if (_talent.TalentGroup.Count <= index + 1)
                    {
                        return (ErrorCode.ERR_Error, "TalentGroup index out of range");
                    }
                    talentConfigId = _talent.TalentGroup[index];
                    break;
                }
            }
            if (talentConfigId == 0)
            {
                return (ErrorCode.ERR_Error, "index out of range");
            }
            var talentConfig = LuBanConfigComponent.Instance.Config().HeroTalentConfig.GetOrDefault(talentConfigId);
            if (talentConfig == null)
            {
                return (ErrorCode.ERR_Error, "talent config cant find");
            }
            var needTalentPoint = talentConfig.NeedTalentPoint;
            if (isLearn)
            {
                if (talentInfo[index] != 0 || self.Talents.ContainsKey(talentConfigId))
                {
                    return (ErrorCode.ERR_Error, "state is same ,no need change");
                }
                if (needTalentPoint < self.TalentPoint)
                {
                    return (ErrorCode.ERR_Error, "TalentPoint no enough");
                }
                self.TalentPoint -= needTalentPoint;
                talentInfo[index] = talentConfigId;
                var entity = self.AddChild<THeroTalentItem, int>(talentConfigId);
                self.Talents.Add(talentConfigId, entity.Id);
                entity.CostTalentPoint = needTalentPoint;
                entity.LoadAllChild();
            }
            else
            {
                if (talentInfo[index] == 0 || !self.Talents.ContainsKey(talentConfigId))
                {
                    return (ErrorCode.ERR_Error, "state is same ,no need change");
                }
                var talent = self.GetChild<THeroTalentItem>(self.Talents[talentConfigId]);
                if (talent == null)
                {
                    return (ErrorCode.ERR_Error, "cant find talent entity");
                }
                self.TalentPoint += talent.CostTalentPoint;
                talentInfo[index] = 0;
                self.Talents.Remove(talentConfigId);
                talent.ClearBuff();
                talent.Dispose();
            }
            return (ErrorCode.ERR_Success, "");
        }

    }
}
