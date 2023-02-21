//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using Bright.Serialization;
using System.Collections.Generic;


namespace cfg.Activity
{
public sealed partial class TActivityBattlePassRecord :  Bright.Config.BeanBase 
{
    public TActivityBattlePassRecord(ByteBuf _buf) 
    {
        Id = _buf.ReadInt();
        ActivityStartTime = _buf.ReadLong();
        ActivityEndTime = _buf.ReadLong();
        {int n0 = System.Math.Min(_buf.ReadSize(), _buf.Size);ItemPrize = new System.Collections.Generic.List<Activity.BattlePassBean>(n0);for(var i0 = 0 ; i0 < n0 ; i0++) { Activity.BattlePassBean _e0;  _e0 = Activity.BattlePassBean.DeserializeBattlePassBean(_buf); ItemPrize.Add(_e0);}}
        PostInit();
    }

    public static TActivityBattlePassRecord DeserializeTActivityBattlePassRecord(ByteBuf _buf)
    {
        return new Activity.TActivityBattlePassRecord(_buf);
    }

    /// <summary>
    /// 活动id
    /// </summary>
    public int Id { get; private set; }
    /// <summary>
    /// 开启时间
    /// </summary>
    public long ActivityStartTime { get; private set; }
    public long ActivityStartTime_Millis => ActivityStartTime * 1000L;
    /// <summary>
    /// 结束时间
    /// </summary>
    public long ActivityEndTime { get; private set; }
    public long ActivityEndTime_Millis => ActivityEndTime * 1000L;
    public System.Collections.Generic.List<Activity.BattlePassBean> ItemPrize { get; private set; }

    public const int __ID__ = -456028610;
    public override int GetTypeId() => __ID__;

    public  void Resolve(Dictionary<string, object> _tables)
    {
        foreach(var _e in ItemPrize) { _e?.Resolve(_tables); }
        PostResolve();
    }

    public  void TranslateText(System.Func<string, string, string> translator)
    {
        foreach(var _e in ItemPrize) { _e?.TranslateText(translator); }
    }

    public override string ToString()
    {
        return "{ "
        + "Id:" + Id + ","
        + "ActivityStartTime:" + ActivityStartTime + ","
        + "ActivityEndTime:" + ActivityEndTime + ","
        + "ItemPrize:" + Bright.Common.StringUtil.CollectionToString(ItemPrize) + ","
        + "}";
    }
    
    partial void PostInit();
    partial void PostResolve();
}

}