using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET.Server
{
    public static class ServerZoneTItemManageComponentFunc
    {
        public static void LoadAllChild(this ServerZoneTItemManageComponent self)
        {

        }


        public static (int, string) ApplyUseBagItem(this ServerZoneTItemManageComponent self,
            TCharacter character, long itemId, int count)
        {
            var item = character.BagComp.GetChild<TItem>(itemId);
            if (item == null)
            {
                return (ErrorCode.ERR_Error, "cant find item");
            }
            return item.ApplyUse(count);
        }
    }
}
