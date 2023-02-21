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
        }


        public static void AddServerZoneMail(this ServerZoneMailComponent self,
            string title,
            string content,
            long from,
            string FromDes,
            string ToDes,
            int ValidTime,
            List<ValueTupleStruct<int, int>> Items = null)
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
                clone.Items = new List<ValueTupleStruct<int, int>>();
                Items.CopyTo(clone.Items.ToArray());
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
            List<ValueTupleStruct<int, int>> Items)
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
                clone.Items = new List<ValueTupleStruct<int, int>>();
                Items.CopyTo(clone.Items.ToArray());
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
            List<ValueTupleStruct<int, int>> Items = null
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
                    clone.Items = new List<ValueTupleStruct<int, int>>();
                    Items.CopyTo(clone.Items.ToArray());
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



        public static void RemoveMail(this ServerZoneMailComponent self, long mailId)
        {
            self.Mails.Remove(mailId);
            self.GetChild<TMail>(mailId)?.Dispose();
        }
    }
}
