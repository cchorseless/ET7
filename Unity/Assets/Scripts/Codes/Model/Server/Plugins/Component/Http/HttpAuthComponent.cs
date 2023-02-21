//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net;
//using System.Net.Sockets;
//using System.Text;
//using System.Text.RegularExpressions;

//namespace ET
//{
//    public struct HttpCredential
//    {
//        public string Name { get; set; }
//        public string Password { get; set; }
//    }

//    public class HttpAuthComponent : Entity, IAwake, IDestroy
//    {
//        public static HttpAuthComponent Instance;

//        public Regex IgnorePathRegex;
//        public List<long> AuthStrategies;
//        public List<long> IgnoreIPAddresses;
//        public List<string> ClientIPHeaders;
//    }

//    public class HttpCredentialAuthStrategy : Entity, IAwake, IAwake<string>
//    {
//        public string Realm { get; set; }

//        public void Respond401(HttpListenerContext app, string wwwAuthenticate)
//        {
//            app.Response.StatusDescription = "401 Unauthorized";
//            app.Response.StatusCode = 401;
//            app.Response.AddHeader("WWW-Authenticate", wwwAuthenticate);
//            app.Response.Close();
//        }
//    }


//    public class HttpBasicAuthStrategy : HttpCredentialAuthStrategy
//    {
//        public string[] ValidAuthVals;

//    }

//    public class HttpDigestAuthStrategy : HttpCredentialAuthStrategy
//    {
//        public TimeSpan NonceValidDuration;
//        public string NonceSalt;
//        public Dictionary<string, string> ValidTokens;

//    }

//    public class HttpRestrictIPStrategy : Entity, IAwake<string>
//    {
//        public List<long> IpRanges;
//    }

//    public class HttpIPAddressRange : Entity, IAwake<string>
//    {
//        public AddressFamily AddressFamily;
//        public byte[] NetworkAddressBytes;
//        public byte[] SubnetMaskBytes;
//    }
//}
