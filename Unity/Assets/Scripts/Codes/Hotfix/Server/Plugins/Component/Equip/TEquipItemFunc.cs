using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ET.Server
{

    public static class TEquipItemFunc
    {
        public static cfg.Item.ItemEquipConfigRecord EquipConfig(this TEquipItem self)
        {
            return LuBanConfigComponent.Instance.Config().ItemEquipConfig.GetOrDefault(self.ConfigId);
        }



        public static void BindEquip(this TEquipItem self)
        {
            if (self.Props.Length > 0) { return; }
            self.AddRandomProp(self.EquipConfig().EquipRandomProp, self.ItemQuality);
        }




        public static void AddRandomProp(this TEquipItem self, List<int> randomPropPoolid, int PropCount)
        {
            if (randomPropPoolid.Count == 0) return;
            int i = 0;
            int index = 0;
            var PropWeight = new List<int>() { 20, 20, 30, 20, 10 };
            double PropWeightMax = 0;
            PropWeight.ForEach(i => PropWeightMax += i);
            var randomQuality = PropWeight.GetRange(0, self.ItemQuality);
            while (index < PropCount)
            {
                if (i >= randomPropPoolid.Count)
                {
                    i = 0;
                }
                int configid = randomPropPoolid[i];
                var config = LuBanConfigComponent.Instance.Config().PropRandomConfig.GetOrDefault(configid);
                if (config != null)
                {
                    var propInfo = config.GetOneRandomPropId();
                    var prop = self.AddChild<TEquipItemProp>();
                    prop.PropId = propInfo.Propid;
                    prop.PropName = propInfo.PropDes;
                    prop.PropMin = propInfo.PropMin;
                    prop.PropMax = propInfo.PropMax;
                    if (propInfo.PropMin == propInfo.PropMax)
                    {
                        prop.PropQuality = self.ItemQuality;
                        prop.PropValue = propInfo.PropMin;
                        continue;
                    }
                    if (index == 0)
                    {
                        prop.PropQuality = self.ItemQuality;
                    }
                    else
                    {
                        prop.PropQuality = RandomGenerator.RandomByWeight(randomQuality) + 1;
                    }
                    int propPectMax = 0;
                    for (int j = 0; j < prop.PropQuality; j++)
                    {
                        propPectMax += PropWeight[j];
                    }
                    int propPectMin = propPectMax - PropWeight[prop.PropQuality - 1];
                    double offset = Math.Abs(prop.PropMax - prop.PropMin) * RandomGenerator.RandomNumber(propPectMin + 1, propPectMax + 1) / PropWeightMax;
                    prop.PropValue = Math.Min(prop.PropMax, prop.PropMin) + (int)Math.Ceiling(offset);
                    self.Props[index] = prop.Id;
                }
                i++;
                index++;
            }
        }


        public static bool ChangeSingleEquipProps(this TEquipItem self, int slot)
        {
            if (!self.IsValidSlot(slot)) { return false; }
            var item = self.AddChild<TEquipItemProp>();
            self.Props[slot] = item.Id;
            return true;
        }

        public static (int, string) ReplaceEquipProps(this TEquipItem self, long ItemPropId, long CostItemId, long CostItemPropId)
        {
            if (!self.IsInBag())
            {
                return (ErrorCode.ERR_Error, "not in bag");
            }
            var prop = self.GetChild<TEquipItemProp>(ItemPropId);
            if (prop == null)
            {
                return (ErrorCode.ERR_Error, "cant find props");
            }
            var costEquip = self.BagComp.GetChild<TEquipItem>(CostItemId);
            if (costEquip == null)
            {
                return (ErrorCode.ERR_Error, "cant find costEquip");
            }
            var costProp = costEquip.GetChild<TEquipItemProp>(CostItemPropId);
            if (costProp == null)
            {
                return (ErrorCode.ERR_Error, "cant find CostItemProp");
            }
            var slot = self.Props.ToList().IndexOf(ItemPropId);
            prop.Dispose();
            self.AddChild(costProp);
            self.Props[slot] = costProp.Id;
            self.BagComp.RemoveTItem(costEquip);
            return (ErrorCode.ERR_Error, "");
        }

        public static bool IsValidSlot(this TEquipItem self, int slot)
        {
            return slot >= (int)EEquipPropSlot.MinSlot && slot <= (int)EEquipPropSlot.MaxSlot;
        }

        public static TEquipItemProp GetSlotEquipProp(this TEquipItem self, int slot)
        {
            if (!self.IsValidSlot(slot)) { return null; }
            if (self.Props[slot] == 0) { return null; }
            return self.GetChild<TEquipItemProp>(self.Props[slot]);
        }

    }
}
