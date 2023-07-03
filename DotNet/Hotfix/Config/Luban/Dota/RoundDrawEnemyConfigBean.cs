//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using Bright.Serialization;
using System.Collections.Generic;


namespace ET.Conf.Dota
{
public sealed partial class RoundDrawEnemyConfigBean :  Bright.Config.BeanBase 
{
    public RoundDrawEnemyConfigBean(ByteBuf _buf) 
    {
        Unitname = _buf.ReadString();
        Star = _buf.ReadInt();
        Level = _buf.ReadInt();
        PositionX = _buf.ReadFloat();
        PositionY = _buf.ReadFloat();
        Itemslot1 = _buf.ReadString();
        Itemslot2 = _buf.ReadString();
        Itemslot3 = _buf.ReadString();
        Itemslot4 = _buf.ReadString();
        Itemslot5 = _buf.ReadString();
        Itemslot6 = _buf.ReadString();
        WearBundleId = _buf.ReadString();
        SpawnBuff = _buf.ReadString();
        PostInit();
    }

    public static RoundDrawEnemyConfigBean DeserializeRoundDrawEnemyConfigBean(ByteBuf _buf)
    {
        return new Dota.RoundDrawEnemyConfigBean(_buf);
    }

    /// <summary>
    /// 回合单位名
    /// </summary>
    public string Unitname { get; private set; }
    /// <summary>
    /// 星级
    /// </summary>
    public int Star { get; private set; }
    /// <summary>
    /// 等级
    /// </summary>
    public int Level { get; private set; }
    /// <summary>
    /// 位置x[0-7]
    /// </summary>
    public float PositionX { get; private set; }
    /// <summary>
    /// 位置y[1-9]
    /// </summary>
    public float PositionY { get; private set; }
    /// <summary>
    /// 物品栏1
    /// </summary>
    public string Itemslot1 { get; private set; }
    /// <summary>
    /// 物品栏2
    /// </summary>
    public string Itemslot2 { get; private set; }
    /// <summary>
    /// 物品栏3
    /// </summary>
    public string Itemslot3 { get; private set; }
    /// <summary>
    /// 物品栏4
    /// </summary>
    public string Itemslot4 { get; private set; }
    /// <summary>
    /// 物品栏5
    /// </summary>
    public string Itemslot5 { get; private set; }
    /// <summary>
    /// 物品栏6
    /// </summary>
    public string Itemslot6 { get; private set; }
    /// <summary>
    /// 套装Id
    /// </summary>
    public string WearBundleId { get; private set; }
    /// <summary>
    /// 单位自带buff
    /// </summary>
    public string SpawnBuff { get; private set; }

    public const int __ID__ = 1481157214;
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
        + "Unitname:" + Unitname + ","
        + "Star:" + Star + ","
        + "Level:" + Level + ","
        + "PositionX:" + PositionX + ","
        + "PositionY:" + PositionY + ","
        + "Itemslot1:" + Itemslot1 + ","
        + "Itemslot2:" + Itemslot2 + ","
        + "Itemslot3:" + Itemslot3 + ","
        + "Itemslot4:" + Itemslot4 + ","
        + "Itemslot5:" + Itemslot5 + ","
        + "Itemslot6:" + Itemslot6 + ","
        + "WearBundleId:" + WearBundleId + ","
        + "SpawnBuff:" + SpawnBuff + ","
        + "}";
    }
    
    partial void PostInit();
    partial void PostResolve();
}

}