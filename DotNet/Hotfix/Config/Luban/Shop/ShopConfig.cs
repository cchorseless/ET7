//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using Bright.Serialization;
using System.Collections.Generic;


namespace cfg.Shop
{
   
public partial class ShopConfig
{
    private readonly Dictionary<int, Shop.ShopConfigRecord> _dataMap;
    private readonly List<Shop.ShopConfigRecord> _dataList;
    
    public ShopConfig(ByteBuf _buf)
    {
        _dataMap = new Dictionary<int, Shop.ShopConfigRecord>();
        _dataList = new List<Shop.ShopConfigRecord>();
        
        for(int n = _buf.ReadSize() ; n > 0 ; --n)
        {
            Shop.ShopConfigRecord _v;
            _v = Shop.ShopConfigRecord.DeserializeShopConfigRecord(_buf);
            _dataList.Add(_v);
            _dataMap.Add(_v.Id, _v);
        }
        PostInit();
    }

    public Dictionary<int, Shop.ShopConfigRecord> DataMap => _dataMap;
    public List<Shop.ShopConfigRecord> DataList => _dataList;

    public Shop.ShopConfigRecord GetOrDefault(int key) => _dataMap.TryGetValue(key, out var v) ? v : null;
    public Shop.ShopConfigRecord Get(int key) => _dataMap[key];
    public Shop.ShopConfigRecord this[int key] => _dataMap[key];

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