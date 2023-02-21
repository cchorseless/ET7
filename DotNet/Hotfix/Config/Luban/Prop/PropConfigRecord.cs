//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using Bright.Serialization;
using System.Collections.Generic;


namespace cfg.Prop
{
public sealed partial class PropConfigRecord :  Bright.Config.BeanBase 
{
    public PropConfigRecord(ByteBuf _buf) 
    {
        Id = _buf.ReadInt();
        PropName = _buf.ReadString();
        Des = _buf.ReadString();
        PostInit();
    }

    public static PropConfigRecord DeserializePropConfigRecord(ByteBuf _buf)
    {
        return new Prop.PropConfigRecord(_buf);
    }

    /// <summary>
    /// 主键
    /// </summary>
    public int Id { get; private set; }
    /// <summary>
    /// 属性名称
    /// </summary>
    public string PropName { get; private set; }
    /// <summary>
    /// 属性描述
    /// </summary>
    public string Des { get; private set; }

    public const int __ID__ = -1644002207;
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
        + "Id:" + Id + ","
        + "PropName:" + PropName + ","
        + "Des:" + Des + ","
        + "}";
    }
    
    partial void PostInit();
    partial void PostResolve();
}

}