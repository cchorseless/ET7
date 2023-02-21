using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET.Server
{
    public static class CharacterMailComponentFunc
    {
        public static void LoadAllChild(this CharacterMailComponent self)
        {
            if (self.IsFull())
            {
                return;
            }
            var zoneMailComp = self.Character.GetMyServerZone().MailComp;
            int lastindex = 0;
            if (self.LastMailId > 0)
            {
                lastindex = zoneMailComp.Mails.LastIndexOf(self.LastMailId);
            }
            else
            {
                for (var i = zoneMailComp.Mails.Count - 1; i > -1; i--)
                {
                    var mail = zoneMailComp.GetChild<TMail>(zoneMailComp.Mails[i]);
                    if (mail != null && mail.IsValid() && mail.Time > self.Character.CreateTime)
                    {
                        lastindex = i;
                        break;
                    }
                }
            }
            for (var i = lastindex + 1; i < zoneMailComp.Mails.Count; i++)
            {
                if (self.IsFull())
                {
                    return;
                }
                var mail = zoneMailComp.GetChild<TMail>(zoneMailComp.Mails[i]);
                self.AddOneMail(mail);
            }
        }
       
        public static (int, string) AddOneMail(this CharacterMailComponent self, TMail mail)
        {
            if (self.IsFull())
            {
                return (ErrorCode.ERR_Error, " mail is full");
            }
            if (mail == null || !mail.IsBelongTo(self.Character))
            {
                return (ErrorCode.ERR_Error, " mail is error");
            }
            var clone = mail.CloneToCharacterMail();
            self.AddChild(clone);
            self.Mails.Add(clone.Id);
            self.LastMailId = mail.Id;
            return (ErrorCode.ERR_Success, "");
        }

        public static (int, string) ReadOneMail(this CharacterMailComponent self, long mailId)
        {
            var mail = self.GetChild<TMail>(mailId);
            if (mail == null)
            {
                return (ErrorCode.ERR_Error, "cant find mail");
            }
            if (!mail.IsValid())
            {
                return (ErrorCode.ERR_Error, "mail isnot valid");
            }
            if (!mail.State.Contains((int)EMailState.UnRead))
            {
                return (ErrorCode.ERR_Error, "mail has read");
            }
            mail.State.Remove((int)EMailState.UnRead);
            mail.State.Add((int)EMailState.Read);
            return (ErrorCode.ERR_Success, "");
        }
        public static (int, string) ReadAllMail(this CharacterMailComponent self)
        {
            foreach (var mailId in self.Mails)
            {
                var mail = self.GetChild<TMail>(mailId);
                if (mail == null)
                {
                    continue;
                }
                if (!mail.IsValid())
                {
                    continue;
                }
                if (mail.State.Contains((int)EMailState.UnRead))
                {
                    mail.State.Remove((int)EMailState.UnRead);
                    mail.State.Add((int)EMailState.Read);
                }
            }
            return (ErrorCode.ERR_Success, "");
        }

        public static (int, string) GetItemOneMail(this CharacterMailComponent self, long mailId)
        {
            var mail = self.GetChild<TMail>(mailId);
            if (mail == null)
            {
                return (ErrorCode.ERR_Error, "cant find mail");
            }
            if (!mail.IsValid())
            {
                return (ErrorCode.ERR_Error, "mail isnot valid");
            }
            if (mail.Items == null || mail.Items.Count <= 0)
            {
                return (ErrorCode.ERR_Error, "mail no items");
            }
            if (!mail.State.Contains((int)EMailState.UnItemGet))
            {
                return (ErrorCode.ERR_Error, "mail had get items");
            }
            if (self.Character.BagComp.IsFullForItems(mail.Items))
            {
                return (ErrorCode.ERR_Error, "bad is full");
            }
            foreach (var iteminfo in mail.Items)
            {
                self.Character.BagComp.AddTItemOrMoney(iteminfo.Item1, iteminfo.Item2);
            }
            mail.State.Remove((int)EMailState.UnItemGet);
            mail.State.Add((int)EMailState.ItemGet);
            return (ErrorCode.ERR_Success, "");
        }
        public static (int, string) GetItemAllMail(this CharacterMailComponent self)
        {
            foreach (var mailId in self.Mails)
            {
                var mail = self.GetChild<TMail>(mailId);
                if (mail == null)
                {
                    continue;
                }
                if (!mail.IsValid())
                {
                    continue;
                }
                if (mail.State.Contains((int)EMailState.UnItemGet))
                {
                    var result = self.GetItemOneMail(mail.Id);
                    if (result.Item1 == ErrorCode.ERR_Error)
                    {
                        return result;
                    }
                }
            }
            return (ErrorCode.ERR_Success, "");
        }

        public static (int, string) DeleteOneMail(this CharacterMailComponent self, long mailId)
        {
            var mail = self.GetChild<TMail>(mailId);
            if (mail == null)
            {
                return (ErrorCode.ERR_Error, "cant find mail");
            }
            if (!mail.IsValid())
            {
                return (ErrorCode.ERR_Error, "mail isnot valid");
            }
            if (mail.Items != null && mail.Items.Count > 0 && mail.State.Contains((int)EMailState.UnItemGet))
            {
                return (ErrorCode.ERR_Error, "mail has items not get");
            }
            self.Mails.Remove(mailId);
            mail.Dispose();
            return (ErrorCode.ERR_Success, "");

        }
        public static (int, string) DeleteAllMail(this CharacterMailComponent self)
        {
            for (var i = 0; i < self.Mails.Count; i++)
            {
                var mailId = self.Mails[i];
                var mail = self.GetChild<TMail>(mailId);
                if (mail == null)
                {
                    continue;
                }
                if (mail.Items != null && mail.Items.Count > 0 && mail.State.Contains((int)EMailState.UnItemGet))
                {
                    continue;
                }
                self.Mails.Remove(mailId);
                mail.Dispose();
                i--;
            }
            return (ErrorCode.ERR_Success, "");

        }


        public static bool IsFull(this CharacterMailComponent self)
        {
            return self.Mails.Count >= self.MaxSize;
        }



    }



    [ObjectSystem]
    public class CharacterMailComponentAwakeSystem : AwakeSystem<CharacterMailComponent>
    {
        protected override void Awake(CharacterMailComponent self)
        {
            self.MaxSize = TMailConfig.MaxSize;

        }
    }
}
