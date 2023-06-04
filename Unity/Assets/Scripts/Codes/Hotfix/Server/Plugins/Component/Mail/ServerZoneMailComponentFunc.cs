using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET.Server
{
    public static class ServerZoneMailComponentFunc
    {
        public static void LoadAllChild(this ServerZoneMailComponent self)
        {
            if (self.Mails.Count == 0)
            {
                self.AddServerZoneMail("欢迎来到棋盘西游", "这里是一点小心意，请笑纳。", (int)EMailType.SystemMail, "系统", "每位玩家", -1,
                    new List<FItemInfo>() { new FItemInfo(10017, 10), new FItemInfo(10018, 10), });
                self.AddServerZoneMail("测试邮件1", "这里是一点小心意，请笑纳。", (int)EMailType.PersonMail, "系统", "每位玩家", -1,
                    new List<FItemInfo>() { new FItemInfo(10019, 10), new FItemInfo(10018, 10), });
                self.AddServerZoneMail("测试邮件2", "这里是一点小心意，请笑纳。", (int)EMailType.PersonMail, "系统", "每位玩家", -1,
                    new List<FItemInfo>() { new FItemInfo(10017, 10), new FItemInfo(10018, 10), });
                self.AddServerZoneMail("测试邮件3", "这里是一点小心意，请笑纳。", (int)EMailType.SystemMail, "系统", "每位玩家", -1,
                    new List<FItemInfo>() { new FItemInfo(10017, 10), new FItemInfo(10018, 10), });
                self.AddServerZoneMail("测试邮件4", "这里是一点小心意，请笑纳。", (int)EMailType.SystemMail, "系统", "每位玩家", -1,
                    new List<FItemInfo>() { new FItemInfo(10017, 10), new FItemInfo(10018, 10), });
            }
        }

        /// <summary>
        /// 全服邮件
        /// </summary>
        /// <param name="self"></param>
        /// <param name="title"></param>
        /// <param name="content"></param>
        /// <param name="from"></param>
        /// <param name="FromDes"></param>
        /// <param name="ToDes"></param>
        /// <param name="ValidTime"></param>
        /// <param name="Items"></param>
        public static void AddServerZoneMail(this ServerZoneMailComponent self,
        string title,
        string content,
        long from,
        string FromDes,
        string ToDes,
        int ValidTime,
        List<FItemInfo> Items = null)
        {
            var clone = self.AddChild<TMail>();
            clone.Title = title;
            clone.Content = content;
            clone.From = from;
            clone.FromDes = FromDes;
            clone.To = new List<long>() { -1 };
            clone.ToDes = ToDes;
            clone.ValidTime = ValidTime;
            clone.Time = TimeHelper.ServerNow();
            if (Items != null && Items.Count > 0)
            {
                clone.Items = new List<FItemInfo>();
                foreach (var iteminfo in Items)
                {
                    clone.Items.Add(new FItemInfo(iteminfo.ItemConfigId, iteminfo.ItemCount));
                }
            }

            self.Mails.Add(clone.Id);
            var characters = self.ServerZone.CharacterComp.GetAll();
            foreach (var character in characters)
            {
                if (character != null)
                {
                    character.MailComp.AddOneMail(clone);
                }
            }
        }

        public static void AddCharacterPrizeMail(this ServerZoneMailComponent self,
        long characterid,
        string title,
        string content,
        int ValidTime,
        List<FItemInfo> Items)
        {
            var clone = self.AddChild<TMail>();
            clone.Title = title;
            clone.Content = content;
            clone.From = 0;
            clone.FromDes = "system mail";
            clone.To = new List<long>() { characterid };
            clone.ToDes = "";
            clone.ValidTime = ValidTime;
            clone.Time = TimeHelper.ServerNow();
            if (Items != null && Items.Count > 0)
            {
                clone.Items = new List<FItemInfo>();
                foreach (var iteminfo in Items)
                {
                    clone.Items.Add(new FItemInfo(iteminfo.ItemConfigId, iteminfo.ItemCount));
                }
            }

            self.Mails.Add(clone.Id);
            var character = self.ServerZone.CharacterComp.Get(characterid);
            if (character != null)
            {
                character.MailComp.AddOneMail(clone);
            }
        }

        public static void AddCharacterMail(this ServerZoneMailComponent self,
        List<long> characteridList,
        string title,
        string content,
        long from,
        string FromDes,
        string ToDes,
        int ValidTime,
        List<FItemInfo> Items = null
        )
        {
            if (characteridList != null && characteridList.Count > 0)
            {
                var clone = self.AddChild<TMail>();
                clone.Title = title;
                clone.Content = content;
                clone.From = from;
                clone.FromDes = FromDes;
                clone.To = characteridList;
                clone.ToDes = ToDes;
                clone.ValidTime = ValidTime;
                clone.Time = TimeHelper.ServerNow();
                if (Items != null && Items.Count > 0)
                {
                    clone.Items = new List<FItemInfo>();
                    foreach (var iteminfo in Items)
                    {
                        clone.Items.Add(new FItemInfo(iteminfo.ItemConfigId, iteminfo.ItemCount));
                    }
                }

                self.Mails.Add(clone.Id);
                foreach (var characterid in characteridList)
                {
                    var character = self.ServerZone.CharacterComp.Get(characterid);
                    if (character != null)
                    {
                        character.MailComp.AddOneMail(clone);
                    }
                }
            }
        }

        // public static void RemoveMail(this ServerZoneMailComponent self, long mailId)
        // {
        //     self.Mails.Remove(mailId);
        //     self.GetChild<TMail>(mailId)?.Dispose();
        // }
    }
}