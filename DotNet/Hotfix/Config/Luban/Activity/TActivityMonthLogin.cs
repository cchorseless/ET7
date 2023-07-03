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
   
public partial class TActivityMonthLogin
{
    private readonly Dictionary<int, Activity.TActivityMonthLoginRecord> _dataMap;
    private readonly List<Activity.TActivityMonthLoginRecord> _dataList;
    
    public TActivityMonthLogin(ByteBuf _buf)
    {
        _dataMap = new Dictionary<int, Activity.TActivityMonthLoginRecord>();
        _dataList = new List<Activity.TActivityMonthLoginRecord>();
        
        for(int n = _buf.ReadSize() ; n > 0 ; --n)
        {
            Activity.TActivityMonthLoginRecord _v;
            _v = Activity.TActivityMonthLoginRecord.DeserializeTActivityMonthLoginRecord(_buf);
            _dataList.Add(_v);
            _dataMap.Add(_v.Id, _v);
        }
        PostInit();
    }

    public Dictionary<int, Activity.TActivityMonthLoginRecord> DataMap => _dataMap;
    public List<Activity.TActivityMonthLoginRecord> DataList => _dataList;

    public Activity.TActivityMonthLoginRecord GetOrDefault(int key) => _dataMap.TryGetValue(key, out var v) ? v : null;
    public Activity.TActivityMonthLoginRecord Get(int key) => _dataMap[key];
    public Activity.TActivityMonthLoginRecord this[int key] => _dataMap[key];

    public void Resolve(Dictionary<string, object> _tables)
    {
        foreach(var v in _dataList)
        {
            v.Resolve(_tables);
        }
        PostResolve();
    }

    public void TranslateText(System.Func<string, string, string> translator)
    {
        foreach(var v in _dataList)
        {
            v.TranslateText(translator);
        }
    }
    
    partial void PostInit();
    partial void PostResolve();
}

}