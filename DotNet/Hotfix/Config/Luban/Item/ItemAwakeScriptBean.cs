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
public sealed partial class ItemAwakeScriptBean :  Bright.Config.BeanBase 
{
    public ItemAwakeScriptBean(ByteBuf _buf) 
    {
        ScriptName = (EEnum.EItemAwakeScript)_buf.ReadInt();
        {int n0 = System.Math.Min(_buf.ReadSize(), _buf.Size);ScriptValue = new System.Collections.Generic.List<int>(n0);for(var i0 = 0 ; i0 < n0 ; i0++) { int _e0;  _e0 = _buf.ReadInt(); ScriptValue.Add(_e0);}}
        PostInit();
    }

    public static ItemAwakeScriptBean DeserializeItemAwakeScriptBean(ByteBuf _buf)
    {
        return new Item.ItemAwakeScriptBean(_buf);
    }

    /// <summary>
    /// 道具脚本
    /// </summary>
    public EEnum.EItemAwakeScript ScriptName { get; private set; }
    /// <summary>
    /// 道具脚本参数
    /// </summary>
    public System.Collections.Generic.List<int> ScriptValue { get; private set; }

    public const int __ID__ = -2009208942;
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
        + "ScriptName:" + ScriptName + ","
        + "ScriptValue:" + Bright.Common.StringUtil.CollectionToString(ScriptValue) + ","
        + "}";
    }
    
    partial void PostInit();
    partial void PostResolve();
}

}