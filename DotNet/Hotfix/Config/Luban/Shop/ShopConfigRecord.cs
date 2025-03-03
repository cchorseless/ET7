//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using Bright.Serialization;
using System.Collections.Generic;


namespace ET.Conf.Shop
{
public sealed partial class ShopConfigRecord :  Bright.Config.BeanBase 
{
    public ShopConfigRecord(ByteBuf _buf) 
    {
        Id = _buf.ReadInt();
        IsVaild = _buf.ReadBool();
        ShopType = _buf.ReadInt();
        ShopName = _buf.ReadString();
        ShopRefreshType = _buf.ReadInt();
        ShopStartTime = _buf.ReadInt();
        ShopValidTime = _buf.ReadInt();
        {int n0 = System.Math.Min(_buf.ReadSize(), _buf.Size);Sellinfo = new System.Collections.Generic.List<Shop.ShopSellItemBean>(n0);for(var i0 = 0 ; i0 < n0 ; i0++) { Shop.ShopSellItemBean _e0;  _e0 = Shop.ShopSellItemBean.DeserializeShopSellItemBean(_buf); Sellinfo.Add(_e0);}}
        PostInit();
    }

    public static ShopConfigRecord DeserializeShopConfigRecord(ByteBuf _buf)
    {
        return new Shop.ShopConfigRecord(_buf);
    }

    /// <summary>
    /// 池子id
    /// </summary>
    public int Id { get; private set; }
    /// <summary>
    /// 是否启用
    /// </summary>
    public bool IsVaild { get; private set; }
    /// <summary>
    /// 商店类型
    /// </summary>
    public int ShopType { get; private set; }
    /// <summary>
    /// 商店名称
    /// </summary>
    public string ShopName { get; private set; }
    /// <summary>
    /// 商店刷新类型
    /// </summary>
    public int ShopRefreshType { get; private set; }
    /// <summary>
    /// 商店开启时间
    /// </summary>
    public int ShopStartTime { get; private set; }
    /// <summary>
    /// 商店有效时间
    /// </summary>
    public int ShopValidTime { get; private set; }
    public System.Collections.Generic.List<Shop.ShopSellItemBean> Sellinfo { get; private set; }

    public const int __ID__ = -643925951;
    public override int GetTypeId() => __ID__;

    public  void Resolve(Dictionary<string, object> _tables)
    {
        foreach(var _e in Sellinfo) { _e?.Resolve(_tables); }
        PostResolve();
    }

    public  void TranslateText(System.Func<string, string, string> translator)
    {
        foreach(var _e in Sellinfo) { _e?.TranslateText(translator); }
    }

    public override string ToString()
    {
        return "{ "
        + "Id:" + Id + ","
        + "IsVaild:" + IsVaild + ","
        + "ShopType:" + ShopType + ","
        + "ShopName:" + ShopName + ","
        + "ShopRefreshType:" + ShopRefreshType + ","
        + "ShopStartTime:" + ShopStartTime + ","
        + "ShopValidTime:" + ShopValidTime + ","
        + "Sellinfo:" + Bright.Common.StringUtil.CollectionToString(Sellinfo) + ","
        + "}";
    }
    
    partial void PostInit();
    partial void PostResolve();
}

}