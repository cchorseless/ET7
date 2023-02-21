
using System.Text.RegularExpressions;

namespace ET.Server
{
    public static class AccountHelper
    {
        public static (int, string) IsGoodAccountKey(string key)
        {
            int code = ErrorCode.ERR_Error;
            if (key == null)
                return (code, "Is Null");
            int strLen = key.Length;
            string trim = Regex.Replace(key, @"\s", "");
            int strLen2 = trim.Length;
            if (strLen != strLen2)
                return (code, "HAS_BLANK");

            if (strLen <= 5 || strLen > 16)
                return (code, "WRONG_LENGTH");

            return (ErrorCode.ERR_Success, "GOOD_KEY");

        }

        public static (int, string) IsGoodPasswordKey(string key)
        {
            int strLen = key.Length;
            int code = ErrorCode.ERR_Error;
            string trim = Regex.Replace(key, @"\s", "");
            int strLen2 = trim.Length;
            if (strLen != strLen2)
                return (code, "HAS_BLANK");

            if (strLen <= 5 || strLen > 16)
                return (code, "WRONG_LENGTH");

            Regex r_num = new Regex(@"\d+");
            if (!r_num.IsMatch(key))
                return (code, "NO_NUM");

            Regex r_let = new Regex(@"[a-z]+");
            if (!r_let.IsMatch(key))
                return (code, "NO_LET");

            //Regex r_cap = new Regex(@"[A-Z]+");
            //if (!r_cap.IsMatch(key))
            //    return (code, "NO_CAP");

            //Regex r_spe = new Regex(@"(?=([\x21-\x7e]+)[^a-zA-Z0-9])+");
            //if (!r_spe.IsMatch(key))
            //    return (code, "NO_SPE");

            return (ErrorCode.ERR_Success, "GOOD_KEY");

        }



    }
}
