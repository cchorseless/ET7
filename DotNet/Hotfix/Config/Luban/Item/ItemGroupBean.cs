//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using Bright.Serialization;
using System.Collections.Generic;


namespace ET.Conf.Item
{
public sealed partial class ItemGroupBean :  Bright.Config.BeanBase 
{
    public ItemGroupBean(ByteBuf _buf) 
    {
        Index = _buf.ReadInt();
        {int n0 = System.Math.Min(_buf.ReadSize(), _buf.Size);ItemGroup = new System.Collections.Generic.List<Item.ItemInfoBean>(n0);for(var i0 = 0 ; i0 < n0 ; i0++) { Item.ItemInfoBean _e0;  _e0 = Item.ItemInfoBean.DeserializeItemInfoBean(_buf); ItemGroup.Add(_e0);}}
        PostInit();
    }

    public static ItemGroupBean DeserializeItemGroupBean(ByteBuf _buf)
    {
        return new Item.ItemGroupBean(_buf);
    }

    /// <summary>
    /// 道具组索引
    /// </summary>
    public int Index { get; private set; }
    /// <summary>
    /// 道具组
    /// </summary>
    public System.Collections.Generic.List<Item.ItemInfoBean> ItemGroup { get; private set; }

    public const int __ID__ = -775563935;
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
        + "Index:" + Index + ","
        + "ItemGroup:" + Bright.Common.StringUtil.CollectionToString(ItemGroup) + ","
        + "}";
    }
    
    partial void PostInit();
    partial void PostResolve();
}

}