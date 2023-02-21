using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET.Server
{
    [Invoke(TimerInvokeType.CharacterTitleItem)]
    public class CharacterTitleItemCheckTimer : ATimer<CharacterTitleItem>
    {
        protected override void Run(CharacterTitleItem self)
        {
            try
            {
                self.ClearBuff();
                self.IsValid = false;
                if (self.CharacterTitleComp.DressTitleConfigId == self.ConfigId)
                {
                    self.CharacterTitleComp.DressTitleConfigId = 0;
                }
            }
            catch (Exception e)
            {
                Log.Error($"CharacterTitleItem error: {self.Id}\n{e}");
            }
        }
    }
    public static class CharacterTitleItemFunc
    {
        public static void LoadAllChild(this CharacterTitleItem self)
        {
            var time = TimeHelper.ServerNow();
            if (self.IsValid && self.DisabledTime > 0)
            {
                if (self.DisabledTime <= time)
                {
                    self.ClearBuff();
                    self.IsValid = false;
                    if (self.CharacterTitleComp.DressTitleConfigId == self.ConfigId)
                    {
                        self.CharacterTitleComp.DressTitleConfigId = 0;
                    }
                }
                else
                {
                    self.CharacterTitleComp.Character.TimerEntityComp.AddOnceTimer(self.DisabledTime - time + 100, TimerInvokeType.CharacterTitleItem, self);
                }
            }
        }

        public static void ClearBuff(this CharacterTitleItem self)
        {
            var buffComp = self.CharacterTitleComp.Character.BuffComp;
            self.TitleBuff.ForEach(buffconfig =>
            {
                buffComp.RemoveBuff(buffconfig);
            });
            self.TitleBuff.Clear();
        }

        public static cfg.Title.TitleConfigRecord TitleConfig(this CharacterTitleItem self)
        {
            return LuBanConfigComponent.Instance.Config().TitleConfig.GetOrDefault(self.ConfigId);
        }



    }
}
