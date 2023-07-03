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
public sealed partial class ItemPrizePoolBean :  Bright.Config.BeanBase 
{
    public ItemPrizePoolBean(ByteBuf _buf) 
    {
        ItemConfigId = _buf.ReadInt();
        ItemName = _buf.ReadString();
        ItemCount = _buf.ReadInt();
        ItemWeight = _buf.ReadInt();
        IsVaild = _buf.ReadBool();
        PostInit();
    }

    public static ItemPrizePoolBean DeserializeItemPrizePoolBean(ByteBuf _buf)
    {
        return new Item.ItemPrizePoolBean(_buf);
    }

    /// <summary>
    /// 道具索引
    /// </summary>
    public int ItemConfigId { get; private set; }
    /// <summary>
    /// 道具名称
    /// </summary>
    public string ItemName { get; private set; }
    /// <summary>
    /// 道具数量
    /// </summary>
    public int ItemCount { get; private set; }
    /// <summary>
    /// 道具权重
    /// </summary>
    public int ItemWeight { get; private set; }
    /// <summary>
    /// 是否启用
    /// </summary>
    public bool IsVaild { get; private set; }

    public const int __ID__ = 1226681776;
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
        + "ItemName:" + ItemName + ","
        + "ItemCount:" + ItemCount + ","
        + "ItemWeight:" + ItemWeight + ","
        + "IsVaild:" + IsVaild + ","
        + "}";
    }
    
    partial void PostInit();
    partial void PostResolve();
}

}