//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using Bright.Serialization;
using System.Collections.Generic;


namespace cfg.Item
{
public sealed partial class ItemConfigRecord :  Bright.Config.BeanBase 
{
    public ItemConfigRecord(ByteBuf _buf) 
    {
        Id = _buf.ReadInt();
        IsVaild = _buf.ReadBool();
        ItemName = _buf.ReadString();
        ItemIcon = _buf.ReadString();
        ItemQuality = (EEnum.ERarity)_buf.ReadInt();
        ItemType = (EEnum.EItemType)_buf.ReadInt();
        ItemDes = _buf.ReadString();
        BagSlotType = (EEnum.EBagSlotType)_buf.ReadInt();
        AutoUse = _buf.ReadBool();
        OneGameUseLimit = _buf.ReadInt();
        DecomposeStarStone = _buf.ReadInt();
        BatchUseable = _buf.ReadBool();
        BindHeroName = _buf.ReadString();
        UseScript = (EEnum.EItemUseScript)_buf.ReadInt();
        {int n0 = System.Math.Min(_buf.ReadSize(), _buf.Size);UseArgs = new System.Collections.Generic.List<int>(n0);for(var i0 = 0 ; i0 < n0 ; i0++) { int _e0;  _e0 = _buf.ReadInt(); UseArgs.Add(_e0);}}
        {int n0 = System.Math.Min(_buf.ReadSize(), _buf.Size);AwakeScript = new System.Collections.Generic.List<Item.ItemAwakeScriptBean>(n0);for(var i0 = 0 ; i0 < n0 ; i0++) { Item.ItemAwakeScriptBean _e0;  _e0 = Item.ItemAwakeScriptBean.DeserializeItemAwakeScriptBean(_buf); AwakeScript.Add(_e0);}}
        PostInit();
    }

    public static ItemConfigRecord DeserializeItemConfigRecord(ByteBuf _buf)
    {
        return new Item.ItemConfigRecord(_buf);
    }

    /// <summary>
    /// 道具id
    /// </summary>
    public int Id { get; private set; }
    /// <summary>
    /// 是否启用
    /// </summary>
    public bool IsVaild { get; private set; }
    /// <summary>
    /// 道具名称
    /// </summary>
    public string ItemName { get; private set; }
    /// <summary>
    /// 道具icon
    /// </summary>
    public string ItemIcon { get; private set; }
    /// <summary>
    /// 道具品质
    /// </summary>
    public EEnum.ERarity ItemQuality { get; private set; }
    /// <summary>
    /// 道具类型
    /// </summary>
    public EEnum.EItemType ItemType { get; private set; }
    /// <summary>
    /// 道具描述
    /// </summary>
    public string ItemDes { get; private set; }
    /// <summary>
    /// 背包占用格子类型
    /// </summary>
    public EEnum.EBagSlotType BagSlotType { get; private set; }
    /// <summary>
    /// 获得自动使用
    /// </summary>
    public bool AutoUse { get; private set; }
    /// <summary>
    /// 单局使用次数限制
    /// </summary>
    public int OneGameUseLimit { get; private set; }
    /// <summary>
    /// 自动分解获得星石数量
    /// </summary>
    public int DecomposeStarStone { get; private set; }
    /// <summary>
    /// 批量使用
    /// </summary>
    public bool BatchUseable { get; private set; }
    /// <summary>
    /// 绑定英雄
    /// </summary>
    public string BindHeroName { get; private set; }
    /// <summary>
    /// 使用脚本
    /// </summary>
    public EEnum.EItemUseScript UseScript { get; private set; }
    /// <summary>
    /// 使用参数
    /// </summary>
    public System.Collections.Generic.List<int> UseArgs { get; private set; }
    public System.Collections.Generic.List<Item.ItemAwakeScriptBean> AwakeScript { get; private set; }

    public const int __ID__ = -1455369631;
    public override int GetTypeId() => __ID__;

    public  void Resolve(Dictionary<string, object> _tables)
    {
        foreach(var _e in AwakeScript) { _e?.Resolve(_tables); }
        PostResolve();
    }

    public  void TranslateText(System.Func<string, string, string> translator)
    {
        foreach(var _e in AwakeScript) { _e?.TranslateText(translator); }
    }

    public override string ToString()
    {
        return "{ "
        + "Id:" + Id + ","
        + "IsVaild:" + IsVaild + ","
        + "ItemName:" + ItemName + ","
        + "ItemIcon:" + ItemIcon + ","
        + "ItemQuality:" + ItemQuality + ","
        + "ItemType:" + ItemType + ","
        + "ItemDes:" + ItemDes + ","
        + "BagSlotType:" + BagSlotType + ","
        + "AutoUse:" + AutoUse + ","
        + "OneGameUseLimit:" + OneGameUseLimit + ","
        + "DecomposeStarStone:" + DecomposeStarStone + ","
        + "BatchUseable:" + BatchUseable + ","
        + "BindHeroName:" + BindHeroName + ","
        + "UseScript:" + UseScript + ","
        + "UseArgs:" + Bright.Common.StringUtil.CollectionToString(UseArgs) + ","
        + "AwakeScript:" + Bright.Common.StringUtil.CollectionToString(AwakeScript) + ","
        + "}";
    }
    
    partial void PostInit();
    partial void PostResolve();
}

}