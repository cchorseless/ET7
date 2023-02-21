
namespace ET
{
    public static partial class MongoHelper
    {
        public static string ToClientJson(object obj)
        {
            var str = ToJson(obj, null);
            str = str.Replace(" ", "").
                Replace("\r\n", "").
                Replace("NumberLong", "").
                Replace("(", "").
                Replace(")", "");
            return str;
        }
    }
}