﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET.Server
{
    public static class EMoneyType
    {
        public const int InGame_Gold = 1;
        public const int InGameMax = 100;
        public const int MetaStone = 101;
        public const int StarStone = 102;
        public const int ComHeroExp = 103;

        public const int FireEventMin = 500;

        public const int BattlePassExp = 501;

        public const int AchieveMentMin = 600;

        public const int KillEnemyCount = 601;

        public const int AchieveMentMax = 699;

        public const int DailyDataMin = 700;

        public const int DailyDataMax = 799;

        public const int WeekDataMin = 800;

        public const int WeekDataMax = 899;

        public const int SeasonDataMin = 900;

        public const int SeasonDataMax = 999;

        public const int MoneyMax = 1000;
    }

    [FriendOf(typeof (CharacterDataComponent))]
    [FriendOf(typeof (NumericComponent))]
    [FriendOf(typeof (CharacterInGameDataComponent))]
    public static class CharacterDataComponentFunc
    {
        public static void LoadAllChild(this CharacterDataComponent self)
        {
            foreach (var numericType in self.NumericComp.NumericDic.Keys)
            {
                if (numericType > EMoneyType.DailyDataMin && numericType < EMoneyType.DailyDataMax)
                {
                    if (self.Character.IsFirstLoginToday)
                    {
                        self.NumericComp.SetNoEvent(numericType, 0);
                    }
                }
                else if (numericType > EMoneyType.WeekDataMin && numericType < EMoneyType.WeekDataMax)
                {
                    if (self.Character.IsFirstLoginWeek)
                    {
                        self.NumericComp.SetNoEvent(numericType, 0);
                    }
                }
                else if (numericType > EMoneyType.SeasonDataMin && numericType < EMoneyType.SeasonDataMax)
                {
                    if (self.Character.IsFirstLoginSeason)
                    {
                        self.NumericComp.SetNoEvent(numericType, 0);
                    }
                }
            }

            if (!self.NumericComp.NumericDic.ContainsKey(EMoneyType.MetaStone))
            {
                self.SetNumeric(EMoneyType.MetaStone, 1000000);
            }

            if (!self.NumericComp.NumericDic.ContainsKey(EMoneyType.StarStone))
            {
                self.SetNumeric(EMoneyType.StarStone, 1000000);
            }
        }

        public static void SetNumeric(this CharacterDataComponent self, int numericType, int value)
        {
            if (numericType > EMoneyType.InGameMax)
            {
                if (numericType > EMoneyType.FireEventMin)
                {
                    self.NumericComp.Set(numericType, value);
                }
                else
                {
                    self.NumericComp.SetNoEvent(numericType, value);
                }
            }
            else
            {
                self.ChangeInGameData(numericType, value);
            }
        }

        public static void ChangeNumeric(this CharacterDataComponent self, int numericType, int value)
        {
            if (numericType > EMoneyType.InGameMax)
            {
                var cur = self.NumericComp.GetAsInt(numericType) + value;
                if (numericType > EMoneyType.FireEventMin)
                {
                    self.NumericComp.Set(numericType, cur);
                }
                else
                {
                    self.NumericComp.SetNoEvent(numericType, cur);
                }

                self.Character.SyncHttpEntity(self.NumericComp);
            }
            else
            {
                self.ChangeInGameData(numericType, value);
            }
        }

        public static bool GreaterThan(this CharacterDataComponent self, int numericType, int value)
        {
            return self.NumericComp.GetAsInt(numericType) >= value;
        }

        public static int GetCoinCount(this CharacterDataComponent self, int numericType)
        {
            return self.NumericComp.GetAsInt(numericType);
        }

        public static (int, string) UploadGameRecord(this CharacterDataComponent self, Dictionary<string, string> record)
        {
            foreach (var kv in record)
            {
                if (self.GameRecord.ContainsKey(kv.Key))
                {
                    self.GameRecord[kv.Key] = kv.Value;
                }
                else
                {
                    self.GameRecord.Add(kv.Key, kv.Value);
                }
            }

            self.Character.SyncHttpEntity(self);
            return (ErrorCode.ERR_Success, "");
        }

        public static void ChangeInGameData(this CharacterDataComponent self, int numericType, int value)
        {
            self.InGameDataComp.NumericType = numericType;
            self.InGameDataComp.NumericValue = value;
            self.Character.SyncHttpEntity(self.InGameDataComp);
            self.InGameDataComp.NumericType = 0;
            self.InGameDataComp.NumericValue = 0;
        }
    }

    [ObjectSystem]
    public class CharacterDataComponentAwakeSystem: AwakeSystem<CharacterDataComponent>
    {
        protected override void Awake(CharacterDataComponent self)
        {
            self.AddComponent<NumericComponent>();
            self.AddComponent<CharacterInGameDataComponent>();
        }
    }
}