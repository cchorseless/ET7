//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using Bright.Serialization;
using System.Collections.Generic;


namespace ET.Conf.Activity
{
public sealed partial class TActivityMonthLoginRecord :  Bright.Config.BeanBase 
{
    public TActivityMonthLoginRecord(ByteBuf _buf) 
    {
        Id = _buf.ReadInt();
        ActivityStartTime = _buf.ReadLong();
        ActivityEndTime = _buf.ReadLong();
        {int n0 = System.Math.Min(_buf.ReadSize(), _buf.Size);LoginPrize = new System.Collections.Generic.List<Item.ItemInfoWithIndexBean>(n0);for(var i0 = 0 ; i0 < n0 ; i0++) { Item.ItemInfoWithIndexBean _e0;  _e0 = Item.ItemInfoWithIndexBean.DeserializeItemInfoWithIndexBean(_buf); LoginPrize.Add(_e0);}}
        {int n0 = System.Math.Min(_buf.ReadSize(), _buf.Size);TotalLoginPrize = new System.Collections.Generic.List<Item.ItemInfoWithIndexBean>(n0);for(var i0 = 0 ; i0 < n0 ; i0++) { Item.ItemInfoWithIndexBean _e0;  _e0 = Item.ItemInfoWithIndexBean.DeserializeItemInfoWithIndexBean(_buf); TotalLoginPrize.Add(_e0);}}
        PostInit();
    }

    public static TActivityMonthLoginRecord DeserializeTActivityMonthLoginRecord(ByteBuf _buf)
    {
        return new Activity.TActivityMonthLoginRecord(_buf);
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
    public System.Collections.Generic.List<Item.ItemInfoWithIndexBean> LoginPrize { get; private set; }
    public System.Collections.Generic.List<Item.ItemInfoWithIndexBean> TotalLoginPrize { get; private set; }

    public const int __ID__ = -2097445346;
    public override int GetTypeId() => __ID__;

    public  void Resolve(Dictionary<string, object> _tables)
    {
        foreach(var _e in LoginPrize) { _e?.Resolve(_tables); }
        foreach(var _e in TotalLoginPrize) { _e?.Resolve(_tables); }
        PostResolve();
    }

    public  void TranslateText(System.Func<string, string, string> translator)
    {
        foreach(var _e in LoginPrize) { _e?.TranslateText(translator); }
        foreach(var _e in TotalLoginPrize) { _e?.TranslateText(translator); }
    }

    public override string ToString()
    {
        return "{ "
        + "Id:" + Id + ","
        + "ActivityStartTime:" + ActivityStartTime + ","
        + "ActivityEndTime:" + ActivityEndTime + ","
        + "LoginPrize:" + Bright.Common.StringUtil.CollectionToString(LoginPrize) + ","
        + "TotalLoginPrize:" + Bright.Common.StringUtil.CollectionToString(TotalLoginPrize) + ","
        + "}";
    }
    
    partial void PostInit();
    partial void PostResolve();
}

}