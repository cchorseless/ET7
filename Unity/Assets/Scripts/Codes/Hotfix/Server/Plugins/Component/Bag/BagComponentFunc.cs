﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET.Server
{
    public static class BagComponentFunc
    {
        public static void LoadAllChild(this BagComponent self)
        {
            // 删除无用物品

            for (int i = 0, len = self.Items.Count; i < len; i++)
            {
                var item = self.GetChild<TItem>(self.Items[i]);
                if (item == null || !self.IsValidItem(item.ConfigId))
                {
                    self.Items.RemoveAt(i);
                    i--;
                    len--;
                }
            }
        }

        public static bool CanOverLayMany(this BagComponent self, int configid)
        {
            var item = LuBanConfigComponent.Instance.Config().ItemConfig.GetOrDefault(configid);
            if (item != null)
            {
                return item.BagSlotType != cfg.EEnum.EBagSlotType.ForbidMany;
            }

            return true;
        }

        public static bool IsSitBagSlot(this BagComponent self, int configid)
        {
            var item = LuBanConfigComponent.Instance.Config().ItemConfig.GetOrDefault(configid);
            if (item != null)
            {
                return item.BagSlotType != cfg.EEnum.EBagSlotType.NoSlot;
            }

            return true;
        }

        public static cfg.EEnum.EItemType GetItemType(this BagComponent self, int configid)
        {
            var item = LuBanConfigComponent.Instance.Config().ItemConfig.GetOrDefault(configid);
            if (item != null)
            {
                return item.ItemType;
            }

            return cfg.EEnum.EItemType.None;
        }

        public static bool IsAutoUse(this BagComponent self, int configid)
        {
            var item = LuBanConfigComponent.Instance.Config().ItemConfig.GetOrDefault(configid);
            if (item != null)
            {
                return item.AutoUse;
            }

            return false;
        }

        public static bool IsValidQuality(this BagComponent self, int quality)
        {
            return quality >= (int)cfg.EEnum.ERarity.C && quality <= (int)cfg.EEnum.ERarity.SS;
        }

        public static bool IsValidItem(this BagComponent self, int configid)
        {
            var item = LuBanConfigComponent.Instance.Config().ItemConfig.GetOrDefault(configid);
            if (item != null)
            {
                return item.IsVaild;
            }

            return false;
        }

        public static bool IsFullForItems(this BagComponent self, List<ValueTupleStruct<int, int>> itemsInfo)
        {
            int countSlot = 0;
            itemsInfo.ForEach(item =>
            {
                if (item.Item1 > EMoneyType.MoneyMax && self.IsSitBagSlot(item.Item1))
                {
                    if (!self.CanOverLayMany(item.Item1))
                    {
                        countSlot += item.Item2;
                    }
                    else if (self.GetTItemCount(item.Item2) == 0)
                    {
                        countSlot += 1;
                    }
                }
            });
            return self.MaxSize - self.Items.Count <= countSlot;
        }

        public static bool IsFullForItems(this BagComponent self, int configid, int count = 1)
        {
            if (configid < EMoneyType.MoneyMax || !self.IsSitBagSlot(configid))
            {
                return true;
            }

            int countSlot = 0;
            if (!self.CanOverLayMany(configid))
            {
                countSlot = count;
            }
            else if (self.GetTItemCount(configid) == 0)
            {
                countSlot = 1;
            }

            return self.MaxSize - self.Items.Count <= countSlot;
        }

        public static T AddTItem<T>(this BagComponent self, T entity) where T : TItem
        {
            var configid = entity.ConfigId;
            var count = entity.ItemCount;
            var canMany = self.CanOverLayMany(configid);
            if (canMany)
            {
                var item = self.GetOneTItem<T>(configid);
                if (item != null)
                {
                    item.ChangeItemCount(count);
                    entity.Dispose();
                    return item;
                }
            }

            self.AddChild(entity);
            if (!self.Items.Contains(entity.Id))
            {
                self.Items.Add(entity.Id);
            }

            return entity;
        }

        public static void RemoveTItem<T>(this BagComponent self, T entity, bool needDispose = true) where T : TItem
        {
            if (entity.IsInBag())
            {
                self.Items.Remove(entity.Id);
                if (needDispose)
                {
                    entity.Dispose();
                }
            }
        }

        public struct FItemPrizeResult
        {
            public string ItemId;
            public int ItemConfigId;
            public int ItemCount;
        }

        public static (int, string) AddTItemOrMoney(this BagComponent self, List<ValueTupleStruct<int, int>> itemsInfo)
        {
            if (self.IsFullForItems(itemsInfo))
            {
                return (ErrorCode.ERR_Error, "Bag IsFull");
            }

            var r = new List<FItemPrizeResult>();
            itemsInfo.ForEach(item =>
            {
                var result = self.AddTItemOrMoney(item.Item1, item.Item2);
                if (result.Item1)
                {
                    r.Add(new FItemPrizeResult() { ItemId = result.Item2.ToString(), ItemConfigId = item.Item1, ItemCount = item.Item2, });
                }
            });
            return (ErrorCode.ERR_Success, JsonHelper.ToLitJson(r));
        }

        public static (bool, long) AddTItemOrMoney(this BagComponent self, int configid, int count = 1)
        {
            if (configid < EMoneyType.MoneyMax)
            {
                self.Character.DataComp.ChangeNumeric(configid, count);
                return (true, 0);
            }
            else
            {
                var item = self.AddTItem(configid, count);
                if (item != null)
                {
                    self.Character.SyncHttpEntity(item);
                    return (true, item.Id);
                }
                else
                {
                    return (false, 0);
                }
            }
        }

        public static TItem AddTItem(this BagComponent self, int configid, int count = 1)
        {
            if (count <= 0)
            {
                return null;
            }

            if (self.IsFullForItems(configid, count))
            {
                return null;
            }

            var itemtype = self.GetItemType(configid);
            switch (itemtype)
            {
                case cfg.EEnum.EItemType.Equip:
                    return self.AddTItem<TEquipItem>(configid, count);

                case cfg.EEnum.EItemType.None:
                default:
                    return self.AddTItem<TItem>(configid, count);
            }
        }

        public static T AddTItem<T>(this BagComponent self, int configid, int count = 1) where T : TItem
        {
            if (self.IsFullForItems(configid, count))
            {
                return null;
            }

            var canMany = self.CanOverLayMany(configid);
            var createtime = TimeHelper.ServerNow();
            T item = null;
            if (canMany)
            {
                item = self.GetOneTItem<T>(configid);
                if (item != null)
                {
                    item.ChangeItemCount(count);
                }
                else
                {
                    item = self.AddChild<T>();
                    item.ConfigId = configid;
                    item.ItemCount = count;
                    item.CharacterId = self.Character.Id;
                    item.CreateTime = createtime;
                    item.ActiveItem();
                    if (!item.IsDisposed)
                    {
                        self.Items.Add(item.Id);
                        if (self.IsAutoUse(configid))
                        {
                            item.ApplyUse(count);
                        }
                    }
                }
            }
            else
            {
                for (var i = 0; i < count; i++)
                {
                    item = self.AddChild<T>();
                    item.ConfigId = configid;
                    item.ItemCount = 1;
                    item.CharacterId = self.Character.Id;
                    item.CreateTime = createtime;
                    item.ActiveItem();
                    if (!item.IsDisposed)
                    {
                        self.Items.Add(item.Id);
                        if (self.IsAutoUse(configid))
                        {
                            item.ApplyUse(1);
                        }
                    }
                }
            }

            return item;
        }

        public static bool RemoveTItem<T>(this BagComponent self, int configid, int count = 1) where T : TItem
        {
            if (count <= 0)
            {
                return false;
            }

            if (self.CanOverLayMany(configid))
            {
                var item = self.GetOneTItem<T>(configid);
                if (item != null && item.ItemCount >= count)
                {
                    item.ChangeItemCount(-count);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                var items = self.GetAllTItem<T>(configid);
                int allCount = 0;
                foreach (var item in items)
                {
                    allCount += item.ItemCount;
                }

                if (allCount >= count)
                {
                    while (count > 0)
                    {
                        var item = items[0];
                        items.RemoveAt(0);
                        var cost = count;
                        if (cost > item.ItemCount)
                        {
                            cost = item.ItemCount;
                        }

                        count -= cost;
                        item.ChangeItemCount(-cost);
                    }

                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public static int GetTItemCount(this BagComponent self, int configid)
        {
            var itemtype = self.GetItemType(configid);
            switch (itemtype)
            {
                case cfg.EEnum.EItemType.Equip:
                    return self.GetTItemCount<TEquipItem>(configid);

                //case EItemType.Money:
                //    return self.GetTItemCount<TMoneyItem>(configid);

                case cfg.EEnum.EItemType.None:
                default:
                    return self.GetTItemCount<TItem>(configid);
            }
        }

        public static int GetTItemCount<T>(this BagComponent self, int configid) where T : TItem
        {
            if (self.CanOverLayMany(configid))
            {
                var item = self.GetOneTItem<T>(configid);
                if (item != null)
                {
                    return item.ItemCount;
                }
            }
            else
            {
                var items = self.GetAllTItem<T>(configid);
                int allCount = 0;
                foreach (var item in items)
                {
                    allCount += item.ItemCount;
                }

                return allCount;
            }

            return 0;
        }

        public static T GetOneTItem<T>(this BagComponent self, int configid) where T : TItem
        {
            foreach (var entityid in self.Items)
            {
                var item = self.GetChild<T>(entityid);
                if (item != null && item.ConfigId == configid)
                {
                    return item;
                }
            }

            return null;
        }

        public static List<T> GetAllTItem<T>(this BagComponent self, int configid) where T : TItem
        {
            var list = new List<T>();
            foreach (var entityid in self.Items)
            {
                var item = self.GetChild<T>(entityid);
                if (item != null && item.ConfigId == configid)
                {
                    list.Add(item);
                }
            }

            return list;
        }
    }

    public static class BagComponent_Equip_Func
    {
        public static TEquipItem AddOneEquip(this BagComponent self, int configid, int quality)
        {
            if (!self.IsValidQuality(quality))
            {
                return null;
            }

            if (self.IsFullForItems(configid, 1))
            {
                return null;
            }

            var item = self.AddChild<TEquipItem>();
            item.ConfigId = configid;
            item.ItemQuality = quality;
            item.ItemCount = 1;
            item.ActiveItem();
            if (!item.IsDisposed)
            {
                self.Items.Add(item.Id);
                if (self.IsAutoUse(configid))
                {
                    item.ApplyUse(1);
                }
            }

            return item;
        }

        public static (int, string) MergeEquip(this BagComponent self, List<long> equips)
        {
            int itemQuality = 0;
            foreach (var equipId in equips)
            {
                var equip = self.GetChild<TEquipItem>(equipId);
                if (equip == null)
                {
                    return (ErrorCode.ERR_Error, "miss equip");
                }

                if (itemQuality == 0)
                {
                    itemQuality = equip.ItemQuality;
                }

                if (itemQuality != equip.ItemQuality)
                {
                    return (ErrorCode.ERR_Error, "equip quality not same");
                }
            }

            var MergeHeroName = self.GetChild<TEquipItem>(RandomGenerator.RandomArray(equips)).Config().BindHeroName;
            if (string.IsNullOrEmpty(MergeHeroName) ||
                LuBanConfigComponent.Instance.Config().BuildingLevelUpConfig.GetOrDefault(MergeHeroName) == null)
            {
                return (ErrorCode.ERR_Error, "MergeHeroName not valid");
            }

            foreach (var equipId in equips)
            {
                var equip = self.GetChild<TEquipItem>(equipId);
                self.RemoveTItem(equip);
            }

            bool IsMergeFail = (RandomGenerator.RandomNumber(1, 6) > equips.Count);
            string result = "";
            if (!IsMergeFail)
            {
                var equipConfigId = LuBanConfigComponent.Instance.Config().ItemEquipConfig.RandomEquipByHero(MergeHeroName);
                var entity = self.AddOneEquip(equipConfigId, itemQuality + 1);
                result = entity.Id.ToString();
            }

            return (ErrorCode.ERR_Success, result);
        }

        public static (int, string) ReplaceEquipProps(this BagComponent self, long ItemId, long ItemPropId, long CostItemId, long CostItemPropId)
        {
            var equip = self.GetChild<TEquipItem>(ItemId);
            if (equip == null)
            {
                return (ErrorCode.ERR_Error, "miss equip");
            }

            return equip.ReplaceEquipProps(ItemPropId, CostItemId, CostItemPropId);
        }
    }

    [ObjectSystem]
    public class BagComponentAwakeSystem: AwakeSystem<BagComponent>
    {
        protected override void Awake(BagComponent self)
        {
            self.MaxSize = TItemConfig.BagMaxSize;
        }
    }
}