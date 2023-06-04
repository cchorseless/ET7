using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET.Server
{
    public static class TMailFunc
    {

        public static bool IsBelongTo(this TMail self, TCharacter character)
        {
            if (!self.IsValid())
            {
                return false;
            }
            // if (self.Time < character.CreateTime)
            // {
            //     return false;
            // }
            if (self.To.Contains(-1) || self.To.Contains(character.Id))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool IsValid(this TMail self)
        {
            if (self.IsDelete)
            {
                return false;
            }
            if (self.ValidTime < 0)
            {
                return true;
            }
            return self.Time + self.ValidTime > TimeHelper.ServerNow();
        }

        public static TMail CloneToCharacterMail(this TMail self)
        {
            var clone = Entity.CreateOne<TMail>();
            clone.Title = self.Title;
            clone.Content = self.Content;
            clone.State = new HashSet<int>() { (int)EMailState.UnRead };
            clone.From = self.From;
            clone.FromDes = self.FromDes;
            clone.Time = self.Time;
            clone.ValidTime = self.ValidTime;
            if (self.Items != null && self.Items.Count > 0)
            {
                clone.Items = new List<FItemInfo>();
                foreach (var iteminfo in self.Items)
                {
                    clone.Items.Add(new FItemInfo(iteminfo.ItemConfigId, iteminfo.ItemCount));
                }
                clone.State.Add((int)EMailState.UnItemGet);
            }
            return clone;
        }
    }
}
