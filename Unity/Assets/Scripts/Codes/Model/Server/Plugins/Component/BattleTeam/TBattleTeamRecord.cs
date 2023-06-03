using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson.Serialization.Options;

namespace ET
{
    public class TBattleTeamRecord: Entity, IAwake
    {
        public string SteamAccountId;
        public string SteamAccountName;      
        public int RoundIndex;   
        public int RoundCharpter;   
        public int Score; 
        public int BattleWinCount; 
        public int BattleLoseCount; 
        public int BattleDrawCount;
        public bool IsRecord=false;
        public List<string> SectInfo = new List<string>();  
        
        public List<FBattleUnitInfoItem> UnitInfo = new List<FBattleUnitInfoItem>();

        public int GetRoundKey()
        {
            return this.RoundIndex + this.RoundCharpter * 1000;
        }
    }
}