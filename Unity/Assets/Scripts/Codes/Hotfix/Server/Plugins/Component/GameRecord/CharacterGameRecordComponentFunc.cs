using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET.Server
{
    [ObjectSystem]
    public class CharacterGameRecordComponentDestroySystem : DestroySystem<CharacterGameRecordComponent>
    {
        protected override void Destroy(CharacterGameRecordComponent self)
        {
            self.GetCurGameRecord()?.PlayerLogOut(self.Character.Int64PlayerId);
            self.CurRecordID = 0;
        }
    }
    public static class CharacterGameRecordComponentFunc
    {
        public static void LoadAllChild(this CharacterGameRecordComponent self)
        {
            self.CurRecordID = 0;
        }

        public static TGameRecordItem GetCurGameRecord(this CharacterGameRecordComponent self)
        {
            if (self.CurRecordID != 0)
            {
                return self.Character.GetMyServerZone().GameRecordComp.GetChild<TGameRecordItem>(self.CurRecordID);
            }
            return null;
        }

        public static void AddGameRecord(this CharacterGameRecordComponent self, long recordid)
        {
            self.CurRecordID = recordid;
            if (self.Records.Count >= EGameRecordConfig.CharacterMaxGameRecordCount)
            {
                self.Records.RemoveAt(0);


            }
            self.Records.Add(recordid);
            self.Character.SyncHttpEntity(self);
        }
    }
}
