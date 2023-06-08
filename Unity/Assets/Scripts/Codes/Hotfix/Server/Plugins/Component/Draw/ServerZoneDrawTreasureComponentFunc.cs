using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET.Server
{
    public enum EDrawTreasureType
    {
        HeroExpTreasure = 10013,
        MingWenTreasure= 10014,
        CourierRareTreasure= 10015,
        HeroDressRareTreasure= 10016,
    }
    public static class ServerZoneDrawTreasureComponentFunc
    {
        public static void LoadAllChild(this ServerZoneDrawTreasureComponent self)
        {

        }
        public static (int, string) DrawTreasure(this ServerZoneDrawTreasureComponent self, TCharacter character, int treasureId, int times)
        {
            if (!Enum.IsDefined(typeof(EDrawTreasureType), treasureId))
            {
                return (ErrorCode.ERR_Error, "treasureId not valid");
            }
            if (times <= 0)
            {
                return (ErrorCode.ERR_Error, "times not valid");
            }
            var config = LuBanConfigComponent.Instance.Config().DrawTreasureConfig.GetOrDefault(treasureId);
            if (config == null)
            {
                return (ErrorCode.ERR_Error, "treasureId not valid");
            }
            if (!character.DrawTreasureComp.IsDrawItemEnough(treasureId, times))
            {
                return (ErrorCode.ERR_Error, "item not enough");
            }
            var r = new List<FItemInfo>();
            for (var i = 0; i < times; i++)
            {
                var items = character.DrawTreasureComp.DrawTreasureOnce(treasureId);
                if (items != null)
                {
                    r.AddRange(items);
                }
            }
            return (ErrorCode.ERR_Success, JsonHelper.ToLitJson(r));
        }
    }
}
