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
public sealed partial class ItemInfoBean :  Bright.Config.BeanBase 
{
    public ItemInfoBean(ByteBuf _buf) 
    {
        ItemConfigId = _buf.ReadInt();
        ItemCount = _buf.ReadInt();
        PostInit();
    }

    public static ItemInfoBean DeserializeItemInfoBean(ByteBuf _buf)
    {
        return new Item.ItemInfoBean(_buf);
    }

    /// <summary>
    /// 道具索引
    /// </summary>
    public int ItemConfigId { get; private set; }
    /// <summary>
    /// 道具数量
    /// </summary>
    public int ItemCount { get; private set; }

    public const int __ID__ = -353952596;
    public override int GetTypeId() => __ID__;

    public  void Resolve(Dictionary<string, object> _tables)
    {
        PostResolve();
    }

    public  void TranslateText(System.Func<string, string, string> translator)
    {
    }

    public override string ToString()
    {
        return "{ "
        + "ItemConfigId:" + ItemConfigId + ","
        + "ItemCount:" + ItemCount + ","
        + "}";
    }
    
    partial void PostInit();
    partial void PostResolve();
}

}