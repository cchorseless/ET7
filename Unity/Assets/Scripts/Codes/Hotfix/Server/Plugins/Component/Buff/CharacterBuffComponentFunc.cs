using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ET.Server
{
    public static class CharacterBuffComponentFunc
    {
        public static void LoadAllChild(this CharacterBuffComponent self)
        {
            var buffs = self.GetAllBuffs();
            buffs.ForEach(buff =>
            {
                if (buff.IsOutOfDate())
                {
                    self.RemoveBuff(buff.ConfigId);
                }
            });
            buffs = self.GetAllBuffs();
            var dealBuff = new List<int>();
            var buffList = new List<TBuffItem>();
            foreach (var buff in buffs)
            {
                if (dealBuff.Contains(buff.ConfigId))
                {
                    continue;
                }
                buffList.Clear();
                buff.Config().BuffGroupMember.ForEach(m =>
                {
                    var _buff = self.GetBuff(m);
                    if (_buff != null)
                    {
                        buffList.Add(_buff);
                        dealBuff.Add(m);
                    }
                });
                buffList.Sort((buffa, buffb) =>
                {
                    return buffa.Config().BuffGroupPriority - buffb.Config().BuffGroupPriority;

                });
                for (int i = 0; i < buffList.Count; i++)
                {
                    buffList[i].IsValid = (buffList.Count - 1 == i);
                }
            }
        }


        public static List<TBuffItem> GetAllBuffs(this CharacterBuffComponent self)
        {
            var r = new List<TBuffItem>();
            foreach (var itemKv in self.Buffs)
            {
                var entity = self.GetChild<TBuffItem>(itemKv.Value);
                if (entity != null)
                {
                    r.Add(entity);
                }
            }
            return r;
        }


        public static void RemoveBuff(this CharacterBuffComponent self, int configId)
        {
            var buff = self.GetBuff(configId);
            if (buff != null)
            {
                buff.BuffLayerCount--;
                if (buff.BuffLayerCount <= 0)
                {
                    self.Buffs.Remove(configId);
                    buff.Dispose();
                    self.RefreshBuffGroup(configId);
                }
            }
        }

        public static TBuffItem GetBuff(this CharacterBuffComponent self, int configId)
        {
            if (self.Buffs.TryGetValue(configId, out var item))
            {
                return self.GetChild<TBuffItem>(item);
            }
            return null;
        }

        public static void RefreshBuffGroup(this CharacterBuffComponent self, int configId)
        {
            var config = LuBanConfigComponent.Instance.Config().ItemBuffConfig.GetOrDefault(configId);
            if (config == null) { return; }
            var buffList = new List<TBuffItem>();
            config.BuffGroupMember.ForEach(m =>
            {
                var _buff = self.GetBuff(m);
                if (_buff != null)
                {
                    buffList.Add(_buff);
                }
            });
            buffList.Sort((buffa, buffb) =>
            {
                return buffa.Config().BuffGroupPriority - buffb.Config().BuffGroupPriority;

            });
            for (int i = 0; i < buffList.Count; i++)
            {
                buffList[i].IsValid = (buffList.Count - 1 == i);
            }
        }
        public static TBuffItem AddBuff(this CharacterBuffComponent self, int configId)
        {
            var config = LuBanConfigComponent.Instance.Config().ItemBuffConfig.GetOrDefault(configId);
            if (config == null) { return null; }
            var buff = self.GetBuff(configId);
            if (buff == null)
            {
                buff = self.AddChild<TBuffItem>();
                buff.ConfigId = configId;
                self.Buffs.Add(configId, buff.Id);
            }
            if (config.BuffValidTime == 0)
            {
                buff.DisabledTime = -1;
                buff.BuffLayerCount += 1;
            }
            else
            {
                buff.DisabledTime += config.BuffValidTime * 1000;
                buff.BuffLayerCount = 1;
            }
            self.RefreshBuffGroup(configId);
            return buff;
        }
    }
}
