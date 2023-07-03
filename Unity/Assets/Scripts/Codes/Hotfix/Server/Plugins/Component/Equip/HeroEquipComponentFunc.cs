using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET.Server
{
    public static class HeroEquipComponentFunc
    {
        public static void LoadAllChild(this HeroEquipComponent self)
        {
        }

        public static (int, string) DressEquip(this HeroEquipComponent self, long entityid, int slot)
        {
            if (!self.IsValidSlot(slot))
            {
                return (ErrorCode.ERR_Error, "slot is not valid");
            }

            var equip = self.HeroUnit.Character.BagComp.GetChild<TEquipItem>(entityid);
            if (equip == null)
            {
                return (ErrorCode.ERR_Error, "equip not in bag");
            }

            if (self.Equips[slot] != 0)
            {
                self.UnDressEquip(slot);
            }
            else
            {
                self.HeroUnit.Character.BagComp.RemoveTItem(equip, false);
                self.AddChild(equip);
                self.Equips[slot] = entityid;
                var suitId = equip.EquipConfig().SuitId;
                var suitInfo = equip.EquipConfig().SuitInfo;
                var suitCount = self.GetSuitEquipCount(suitId);
                foreach (var suit in suitInfo)
                {
                    if (suit.SuitCount <= suitCount)
                    {
                        suit.SuitBuffs.ForEach(b =>
                        {
                            if (self.HeroUnit.Character.BuffComp.GetBuff(b) == null)
                            {
                                self.HeroUnit.Character.BuffComp.AddBuff(b);
                            }
                        });
                    }
                }
            }

            return (ErrorCode.ERR_Success, "");
        }

        public static (int, string) UnDressEquip(this HeroEquipComponent self, int slot)
        {
            if (!self.IsValidSlot(slot))
            {
                return (ErrorCode.ERR_Error, "slot is not valid");
            }

            var equip = self.GetSlotEquip(slot);
            if (equip == null)
            {
                return (ErrorCode.ERR_Error, "equip not dress up");
            }
            else
            {
                var suitId = equip.EquipConfig().SuitId;
                var suitInfo = equip.EquipConfig().SuitInfo;
                self.HeroUnit.Character.BagComp.AddTItem(equip);
                self.Equips[slot] = 0;
                var suitCount = self.GetSuitEquipCount(suitId);
                foreach (var suit in suitInfo)
                {
                    if (suit.SuitCount > suitCount)
                    {
                        suit.SuitBuffs.ForEach(b => self.HeroUnit.Character.BuffComp.RemoveBuff(b));
                    }
                }

                return (ErrorCode.ERR_Success, "");
            }
        }

        public static bool IsValidSlot(this HeroEquipComponent self, int slot)
        {
            return slot > (int)Conf.EEnum.EEquipSolt.None && slot < (int)Conf.EEnum.EEquipSolt.SlotMax;
        }

        public static TEquipItem GetSlotEquip(this HeroEquipComponent self, int slot)
        {
            if (!self.IsValidSlot(slot))
            {
                return null;
            }

            if (self.Equips[slot] == 0)
            {
                return null;
            }

            return self.GetChild<TEquipItem>(self.Equips[slot]);
        }

        public static int GetSuitEquipCount(this HeroEquipComponent self, int suitId)
        {
            int count = 0;
            for (var i = 0; i < self.Equips.Count; i++)
            {
                if (self.Equips[i] > 0)
                {
                    var equip = self.GetChild<TEquipItem>(self.Equips[i]);
                    if (equip != null && equip.EquipConfig().SuitId == suitId)
                    {
                        count++;
                    }
                }
            }

            return count;
        }
    }
}