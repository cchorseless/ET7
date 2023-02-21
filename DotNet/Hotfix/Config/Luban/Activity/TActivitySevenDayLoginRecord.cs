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
public sealed partial class TActivitySevenDayLoginRecord :  Bright.Config.BeanBase 
{
    public TActivitySevenDayLoginRecord(ByteBuf _buf) 
    {
        Id = _buf.ReadInt();
        ActivityStartTime = _buf.ReadLong();
        ActivityEndTime = _buf.ReadLong();
        {int n0 = System.Math.Min(_buf.ReadSize(), _buf.Size);PrizeItemGroup = new System.Collections.Generic.List<Item.ItemGroupBean>(n0);for(var i0 = 0 ; i0 < n0 ; i0++) { Item.ItemGroupBean _e0;  _e0 = Item.ItemGroupBean.DeserializeItemGroupBean(_buf); PrizeItemGroup.Add(_e0);}}
        PostInit();
    }

    public static TActivitySevenDayLoginRecord DeserializeTActivitySevenDayLoginRecord(ByteBuf _buf)
    {
        return new Activity.TActivitySevenDayLoginRecord(_buf);
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
    public System.Collections.Generic.List<Item.ItemGroupBean> PrizeItemGroup { get; private set; }

    public const int __ID__ = -2065087705;
    public override int GetTypeId() => __ID__;

    public  void Resolve(Dictionary<string, object> _tables)
    {
        foreach(var _e in PrizeItemGroup) { _e?.Resolve(_tables); }
        PostResolve();
    }

    public  void TranslateText(System.Func<string, string, string> translator)
    {
        foreach(var _e in PrizeItemGroup) { _e?.TranslateText(translator); }
    }

    public override string ToString()
    {
        return "{ "
        + "Id:" + Id + ","
        + "ActivityStartTime:" + ActivityStartTime + ","
        + "ActivityEndTime:" + ActivityEndTime + ","
        + "PrizeItemGroup:" + Bright.Common.StringUtil.CollectionToString(PrizeItemGroup) + ","
        + "}";
    }
    
    partial void PostInit();
    partial void PostResolve();
}

}