//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using Bright.Serialization;
using System.Collections.Generic;


namespace ET.Conf.Glob
{
public sealed partial class GlobalSettingRecord :  Bright.Config.BeanBase 
{
    public GlobalSettingRecord(ByteBuf _buf) 
    {
        GuildOpenLevel = _buf.ReadInt();
        BagInitCapacity = _buf.ReadInt();
        BagMaxCapacity = _buf.ReadInt();
        {int n0 = System.Math.Min(_buf.ReadSize(), _buf.Size);NewbieTasks = new System.Collections.Generic.List<int>(n0);for(var i0 = 0 ; i0 < n0 ; i0++) { int _e0;  _e0 = _buf.ReadInt(); NewbieTasks.Add(_e0);}}
        PostInit();
    }

    public static GlobalSettingRecord DeserializeGlobalSettingRecord(ByteBuf _buf)
    {
        return new Glob.GlobalSettingRecord(_buf);
    }

    /// <summary>
    /// desc1
    /// </summary>
    public int GuildOpenLevel { get; private set; }
    /// <summary>
    /// desc2
    /// </summary>
    public int BagInitCapacity { get; private set; }
    /// <summary>
    /// desc3
    /// </summary>
    public int BagMaxCapacity { get; private set; }
    /// <summary>
    /// desc4
    /// </summary>
    public System.Collections.Generic.List<int> NewbieTasks { get; private set; }

    public const int __ID__ = 1804751464;
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
        + "GuildOpenLevel:" + GuildOpenLevel + ","
        + "BagInitCapacity:" + BagInitCapacity + ","
        + "BagMaxCapacity:" + BagMaxCapacity + ","
        + "NewbieTasks:" + Bright.Common.StringUtil.CollectionToString(NewbieTasks) + ","
        + "}";
    }
    
    partial void PostInit();
    partial void PostResolve();
}

}