using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET.Server
{
    public static class CharacterTitleComponentFunc
    {
        public static void LoadAllChild(this CharacterTitleComponent self)
        {
            foreach (var titleId in self.Titles.Values)
            {
                var title = self.GetChild<CharacterTitleItem>(titleId);
                title.LoadAllChild();
            }
        }

        public static void UnDressCurTitle(this CharacterTitleComponent self)
        {
            if (self.DressTitleConfigId != 0)
            {
                var title = self.GetChild<CharacterTitleItem>(self.DressTitleConfigId);
                if (title != null)
                {
                    title.ClearBuff();
                }
            }
            self.DressTitleConfigId = 0;
        }


        public static CharacterTitleItem AddTitle(this CharacterTitleComponent self, int titleConfigId)
        {
            CharacterTitleItem title;
            if (self.Titles.TryGetValue(titleConfigId, out var titleId))
            {
                title = self.GetChild<CharacterTitleItem>(titleId);
            }
            else
            {
                title = self.AddChild<CharacterTitleItem, int>(titleConfigId);
                self.Titles.Add(titleConfigId, title.Id);
            }
            if (title != null)
            {
                title.IsValid = true;
                int validTime = title.TitleConfig().TitleValidTime;
                if (validTime <= 0)
                {
                    title.DisabledTime = -1;
                }
                else
                {
                    title.DisabledTime = TimeHelper.ServerNow() + validTime;
                }
            }
            return title;
        }


        public static (int, string) ChangeTitleState(this CharacterTitleComponent self, int titleConfigId, bool state)
        {
            if (!self.Titles.TryGetValue(titleConfigId, out var titleId))
            {
                return (ErrorCode.ERR_Error, "cant find title");
            }
            var title = self.GetChild<CharacterTitleItem>(titleId);
            if (state)
            {
                if (!title.IsValid)
                {
                    return (ErrorCode.ERR_Error, "not valid title");
                }
                if (self.DressTitleConfigId == title.ConfigId)
                {
                    return (ErrorCode.ERR_Error, "dress had title");
                }
                self.UnDressCurTitle();
                var buffComp = self.Character.BuffComp;
                title.TitleBuff.AddRange(title.TitleConfig().TitleBuffs);
                title.TitleBuff.ForEach(buffid =>
                {
                    buffComp.AddBuff(buffid);
                });
                self.DressTitleConfigId = title.ConfigId;
            }
            else
            {
                if (self.DressTitleConfigId != title.ConfigId)
                {
                    return (ErrorCode.ERR_Error, "not dress this title");
                }
                self.UnDressCurTitle();
            }
            return (ErrorCode.ERR_Success, "");
        }

    }
}
