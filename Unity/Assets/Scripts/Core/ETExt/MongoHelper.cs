namespace ET
{
    public static partial class MongoHelper
    {
        public static string ToClientJson(object obj)
        {
            if (obj == null)
            {
                return "";
            }

            var str = ToJson(obj, null);
            str = str.Replace(" ", "").Replace("\r\n", "").Replace("NumberLong", "").Replace("(", "").Replace(")", "");
            return str;
        }

        public static string ToArrayClientJson<T>(T[] obj) where T : Entity
        {
            string str = "[";
            //while (self.SyncString.Count > 0 && message.Length + self.SyncString[0].Length < 6000)
            //{
            //    message += self.SyncString[0];
            //    message += ",";
            //    self.SyncString.RemoveAt(0);
            //}
            for (var i = 0; i < obj.Length; i++)
            {
                if (obj[i] == null)
                {
                    continue;
                }

                str += ToClientJson(obj[i]) + ",";
            }

            if (str.Length > 1)
            {
                str = str.Substring(0, str.Length - 1);
            }
            str += "]";
            return str;
        }
    }
}