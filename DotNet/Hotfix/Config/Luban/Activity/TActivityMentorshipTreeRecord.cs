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
public sealed partial class TActivityMentorshipTreeRecord :  Bright.Config.BeanBase 
{
    public TActivityMentorshipTreeRecord(ByteBuf _buf) 
    {
        Id = _buf.ReadInt();
        IsValid = _buf.ReadBool();
        TreeLevel = _buf.ReadInt();
        PrizeConditionType = _buf.ReadString();
        PrizeCondition = _buf.ReadInt();
        {int n0 = System.Math.Min(_buf.ReadSize(), _buf.Size);ItemGroup = new System.Collections.Generic.List<Item.ItemInfoBean>(n0);for(var i0 = 0 ; i0 < n0 ; i0++) { Item.ItemInfoBean _e0;  _e0 = Item.ItemInfoBean.DeserializeItemInfoBean(_buf); ItemGroup.Add(_e0);}}
        PostInit();
    }

    public static TActivityMentorshipTreeRecord DeserializeTActivityMentorshipTreeRecord(ByteBuf _buf)
    {
        return new Activity.TActivityMentorshipTreeRecord(_buf);
    }

    /// <summary>
    /// 奖励Id
    /// </summary>
    public int Id { get; private set; }
    /// <summary>
    /// 是否有效
    /// </summary>
    public bool IsValid { get; private set; }
    /// <summary>
    /// 树奖励层级
    /// </summary>
    public int TreeLevel { get; private set; }
    /// <summary>
    /// 奖励条件类型
    /// </summary>
    public string PrizeConditionType { get; private set; }
    /// <summary>
    /// 奖励条件
    /// </summary>
    public int PrizeCondition { get; private set; }
    public System.Collections.Generic.List<Item.ItemInfoBean> ItemGroup { get; private set; }

    public const int __ID__ = 9183376;
    public override int GetTypeId() => __ID__;

    public  void Resolve(Dictionary<string, object> _tables)
    {
        foreach(var _e in ItemGroup) { _e?.Resolve(_tables); }
        PostResolve();
    }

    public  void TranslateText(System.Func<string, string, string> translator)
    {
        foreach(var _e in ItemGroup) { _e?.TranslateText(translator); }
    }

    public override string ToString()
    {
        return "{ "
        + "Id:" + Id + ","
        + "IsValid:" + IsValid + ","
        + "TreeLevel:" + TreeLevel + ","
        + "PrizeConditionType:" + PrizeConditionType + ","
        + "PrizeCondition:" + PrizeCondition + ","
        + "ItemGroup:" + Bright.Common.StringUtil.CollectionToString(ItemGroup) + ","
        + "}";
    }
    
    partial void PostInit();
    partial void PostResolve();
}

}