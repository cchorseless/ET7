//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net;
//using System.Net.Sockets;
//using System.Security.Cryptography;
//using System.Text;
//using System.Text.RegularExpressions;

//namespace ET.HttpAuth
//{
//    public interface IHttpAuthStrategy
//    {
//        bool Execute(HttpListenerContext app);
//    }

//    public static class HttpAuthConfig
//    {
//        //  [required] Http Authentication Mode.
//        //  - Basic: Basic authentication
//        //  - Digest: Digest authentication
//        //  - None: No authentication -->
//        //  <add key = "AuthMode" value="Digest"/>
//        public static readonly string AuthMode = "AuthMode";
//        //  <!-- [optional] default is "SecureZone" -->
//        //  <add key = "Realm" value="SecureZone"/>
//        public static readonly string Realm = "Realm";
//        //  <!-- [required if http auth on] user1:pass1;user2:pass2;... -->
//        //  <add key = "Credentials" value="hoge:hogepass;foo:foopass;"/>
//        public static readonly string Credentials = "Credentials";
//        //  <!-- [optional] Digest Auth Nonce Valid Duration Minutes. default is 120 -->
//        //  <add key = "DigestNonceValidDuration" value= "120" />
//        public static readonly string DigestNonceValidDuration = "DigestNonceValidDuration";
//        //  < !-- [required if digest auth on] Digest Auth Nonce Salt -->
//        //  <add key = "DigestNonceSalt" value= "uht9987bbbSAX" />
//        public static readonly string DigestNonceSalt = "DigestNonceSalt";
//        //  [optional] If set, specified IPs are only allowed: otherwize All IPs are allowed.
//        //  value is joined IP Range Combination as following.
//        //  - 10.23.0.0/24
//        //  - 127.0.0.1 (equals to 127.0.0.1/32)
//        //  - 2001:0db8:bd05:01d2:288a:1fc0:0001:0000/16
//        //  - ::1 (equals to ::1/128)
//        //  e.g) 127.0.0.1;182.249.0.0/16;182.248.112.128/26;::1 -->
//        //<add key = "RestrictIPAddresses" value="127.0.0.1;::1"/>
//        public static readonly string RestrictIPAddresses = "RestrictIPAddresses";
//        //[optional] If set, specified pattern url request skip http auth and IP Restriction.
//        //<add key = "IgnorePathRegex" value="^/Home/Ignore$|^/Ignore\.aspx$"/>
//        public static readonly string IgnorePathRegex = "IgnorePathRegex";
//        //  [optional] If set, specified IPs requests skip http auth Restriction.
//        //   value format is same as 'RestrictIPAddresses'
//        //<add key = "IgnoreIPAddresses" value= "127.0.0.1;::1" />
//        public static readonly string IgnoreIPAddresses = "IgnoreIPAddresses";
//        //  [optional] If set, specified value of Request Header is regarded as Client IP.
//        // <add key = "ClientIPHeaders" value= "CF-CONNECTING-IP;True-Client-IP" /> 
//        public static readonly string ClientIPHeaders = "ClientIPHeaders";


//        private static readonly Dictionary<string, string> Section = new Dictionary<string, string>();

//        static HttpAuthConfig()
//        {
//            Section.Add(AuthMode, "Digest");
//            Section.Add(Realm, "SecureZone");
//            Section.Add(DigestNonceValidDuration, "120");
//            Section.Add(DigestNonceSalt, "uht9987bbbSAX");
//            Section.Add(RestrictIPAddresses, "127.0.0.1;::1");
//            Section.Add(IgnorePathRegex, "^/Home/Ignore$|^/Ignore\\.aspx$");
//            Section.Add(IgnoreIPAddresses, "127.0.0.1;::1");
//        }

//        public static string Get(string key, string nullVal = "")
//        {
//            if (Section.TryGetValue(key, out var val))
//            {
//                return val;
//            }
//            return nullVal;
//        }
//    }

//    [ObjectSystem]
//    public class HttpAuthComponentAwakeSystem : AwakeSystem<HttpAuthComponent>
//    {
//        public override void Awake(HttpAuthComponent self)
//        {
//            HttpAuthComponent.Instance = self;
//            self.AuthStrategies = new List<long>();
//            self.IgnoreIPAddresses = new List<long>();
//            self.ClientIPHeaders = new List<string>();
//            self.OnAwake();
//        }
//    }

//    public static class HttpAuthComponentFunc
//    {
//        public static void OnAwake(this HttpAuthComponent self)
//        {
//            var restrictIPAddresses = HttpAuthConfig.Get(HttpAuthConfig.RestrictIPAddresses);
//            if (!string.IsNullOrEmpty(restrictIPAddresses))
//            {
//                self.AuthStrategies.Add(self.AddChild<HttpRestrictIPStrategy, string>(restrictIPAddresses).Id);
//            }
//            switch (HttpAuthConfig.Get(HttpAuthConfig.AuthMode).ToLower())
//            {
//                case "basic":
//                    self.AuthStrategies.Add(self.AddChild<HttpBasicAuthStrategy>().Id);
//                    break;
//                case "digest":
//                    self.AuthStrategies.Add(self.AddChild<HttpDigestAuthStrategy>().Id);
//                    break;
//            }
//            var ignorePathRegex = HttpAuthConfig.Get(HttpAuthConfig.IgnorePathRegex);
//            if (!string.IsNullOrEmpty(ignorePathRegex))
//            {
//                try
//                {
//                    self.IgnorePathRegex = new Regex(ignorePathRegex, RegexOptions.Compiled | RegexOptions.IgnoreCase);
//                }
//                catch (Exception ex)
//                {
//                    throw new InvalidOperationException("IgnorePathRegex is invalid.", ex);
//                }
//            }
//            var ignoreIPAddresses = HttpAuthConfig.Get(HttpAuthConfig.IgnoreIPAddresses);
//            if (!string.IsNullOrEmpty(ignoreIPAddresses))
//            {
//                var address = ignoreIPAddresses.Split(
//                         new char[] { ';' },
//                         StringSplitOptions.RemoveEmptyEntries);
//                foreach (var ipAddress in address)
//                {
//                    self.IgnoreIPAddresses.Add(self.AddChild<HttpIPAddressRange, string>(ipAddress).Id);
//                }
//            }
//            var clientIPHeaders = HttpAuthConfig.Get(HttpAuthConfig.ClientIPHeaders);
//            if (!string.IsNullOrEmpty(clientIPHeaders))
//            {
//                self.ClientIPHeaders.AddRange(clientIPHeaders.Split(
//                    new char[] { ';' },
//                    StringSplitOptions.RemoveEmptyEntries));
//            }

//        }


//        public static IEnumerable<string> GetClientIPAddresses(this HttpAuthComponent self, HttpListenerContext app)
//        {

//            var ip = app.Request.RemoteEndPoint.ToString();
//            if (!string.IsNullOrEmpty(ip))
//                yield return ip;
//            foreach (var key in self.ClientIPHeaders)
//            {
//                ip = app.Request.Headers[key];
//                if (!string.IsNullOrEmpty(ip))
//                    yield return ip;
//            }

//        }

//        public static bool AuthenticateRequest(this HttpAuthComponent self, HttpListenerContext app)
//        {
//            foreach (var ip in self.GetClientIPAddresses(app))
//            {
//                if (self.IgnoreIPAddresses.Any(a => self.GetChild<HttpIPAddressRange>(a).IsInRange(ip)))
//                    return true;
//            }
//            if (self.IgnorePathRegex.IsMatch(app.Request.RawUrl))
//                return true;

//            foreach (var s in self.AuthStrategies)
//            {
//                var auth = self.GetChild<Entity>(s) as IHttpAuthStrategy;
//                if (auth != null && !auth.Execute(app))
//                {
//                    return false;
//                }
//            }
//            return true;
//        }
//    }

//    [ObjectSystem]
//    public class HttpDigestAuthStrategyAwakeSystem : AwakeSystem<HttpDigestAuthStrategy>
//    {
//        public override void Awake(HttpDigestAuthStrategy self)
//        {
//            self.Realm = HttpAuthConfig.Get(HttpAuthConfig.Realm, "SecureZone");
//            self.OnAwake();
//        }
//    }

//    [ObjectSystem]
//    public class HttpDigestAuthStrategyAwakeSystem1 : AwakeSystem<HttpDigestAuthStrategy, string>
//    {
//        public override void Awake(HttpDigestAuthStrategy self, string realName)
//        {
//            self.Realm = HttpAuthConfig.Get(HttpAuthConfig.Realm, realName);
//            self.OnAwake();
//        }
//    }

//    public static class HttpDigestAuthStrategyFunc
//    {
//        public static readonly string Authorization = "Authorization";
//        public static readonly string Nonce = "nonce";
//        public static readonly string Name = "name";
//        public static readonly string Username = "username";
//        public static readonly string Uri = "uri";
//        public static readonly string Cnonce = "cnonce";
//        public static readonly string Qop = "qop";
//        public static readonly string Nc = "nc";
//        public static readonly string Response = "response";
//        public static readonly string Val = "val";

//        public static void OnAwake(this HttpDigestAuthStrategy self)
//        {
//            var nonceValidDuration = HttpAuthConfig.Get(HttpAuthConfig.DigestNonceValidDuration, "120");
//            if (!int.TryParse(nonceValidDuration, out var intNonceValidDuration) || intNonceValidDuration <= 0)
//            {
//                throw new InvalidOperationException("DigestNonceValidDuration is invalid.");
//            }
//            self.NonceValidDuration = new TimeSpan(0, intNonceValidDuration, 0);
//            self.NonceSalt = HttpAuthConfig.Get(HttpAuthConfig.DigestNonceSalt);
//            if (string.IsNullOrEmpty(self.NonceSalt))
//            {
//                throw new InvalidOperationException("DigestNonceSalt is required.");
//            }
//            //self.ValidTokens = self.Credentials
//            //       .ToDictionary(c => c.Name, c => GetMD5(string.Format("{0}:{1}:{2}", c.Name, self.Realm, c.Password)));
//        }

//        public static bool Execute(this HttpDigestAuthStrategy self, HttpListenerContext app)
//        {
//            var authVal = app.Request.Headers[Authorization];
//            if (string.IsNullOrEmpty(authVal))
//                return self.RespondError(app);
//            var vals = Regex.Matches(app.Request.Headers[Authorization],
//                @"(?<name>\w+)=(""(?<val>[^""]*)""|(?<val>[^"" ,\t\r\n]+))")
//                .Cast<Match>()
//                .ToDictionary(m => m.Groups[Name].Value, m => m.Groups[Val].Value);
//            vals.TryGetValue(Nonce, out var nonce);
//            if (!self.ValidateNonce(nonce))
//                return self.RespondError(app);
//            vals.TryGetValue(Username, out var username);
//            if (!self.ValidTokens.ContainsKey(username))
//                return self.RespondError(app);
//            vals.TryGetValue(Uri, out var uri);
//            vals.TryGetValue(Cnonce, out var cnonce);
//            vals.TryGetValue(Qop, out var qop);
//            vals.TryGetValue(Nc, out var nc);
//            vals.TryGetValue(Response, out var response);
//            var a1 = self.ValidTokens[username];
//            var a2 = GetMD5(app.Request.HttpMethod + ":" + uri);
//            if (response != GetMD5(string.Format("{0}:{1}:{2}:{3}:{4}:{5}", a1, nonce, nc, cnonce, qop, a2)))
//                return self.RespondError(app);
//            return true;
//        }

//        private static bool RespondError(this HttpDigestAuthStrategy self, HttpListenerContext app)
//        {
//            self.Respond401(app, string.Format(@"Digest realm=""{0}"", nonce=""{1}"", algorithm=MD5, qop=""auth""",
//                self.Realm, self.CreateNonce(DateTime.UtcNow)));
//            return false;
//        }

//        private static string CreateNonce(this HttpDigestAuthStrategy self, DateTime dt)
//        {
//            string hash = string.Format("{0}{1}", self.NonceSalt, dt.Ticks);
//            for (int i = 0; i < 3; i++) hash = GetSHA1(hash);
//            return string.Format("{0}-{1}", dt.Ticks, hash);
//        }
//        private static bool ValidateNonce(this HttpDigestAuthStrategy self, string nonce)
//        {
//            if (string.IsNullOrEmpty(nonce)) return false;

//            DateTime dt;
//            try
//            {
//                dt = new DateTime(long.Parse(nonce.Split('-')[0]), DateTimeKind.Utc);
//            }
//            catch
//            {
//                return false;
//            }
//            return dt + self.NonceValidDuration >= DateTime.UtcNow && nonce == self.CreateNonce(dt);
//        }

//        private static string GetMD5(string s)
//        {
//            var md5 = MD5.Create();
//            return string.Concat(md5.ComputeHash(Encoding.UTF8.GetBytes(s)).Select(d => d.ToString("x2"))).ToLower();
//        }

//        private static string GetSHA1(string s)
//        {
//            var sha1 = SHA1.Create();
//            return string.Concat(sha1.ComputeHash(Encoding.UTF8.GetBytes(s)).Select(d => d.ToString("x2"))).ToLower();
//        }
//    }

//    [ObjectSystem]
//    public class HttpBasicAuthStrategyAwakeSystem : AwakeSystem<HttpBasicAuthStrategy>
//    {
//        public override void Awake(HttpBasicAuthStrategy self)
//        {
//            self.Realm = HttpAuthConfig.Get(HttpAuthConfig.Realm, "SecureZone");
//            //self.ValidAuthVals = self.Credentials.Select(c => "Basic " +
//            //Convert.ToBase64String(Encoding.ASCII.GetBytes(c.Name + ":" + c.Password))).ToArray();
//        }
//    }

//    [ObjectSystem]
//    public class HttpBasicAuthStrategyAwakeSystem1 : AwakeSystem<HttpBasicAuthStrategy, string>
//    {
//        public override void Awake(HttpBasicAuthStrategy self, string realName)
//        {
//            self.Realm = HttpAuthConfig.Get(HttpAuthConfig.Realm, realName);
//            //self.ValidAuthVals = self.Credentials.Select(c => "Basic " +
//            //Convert.ToBase64String(Encoding.ASCII.GetBytes(c.Name + ":" + c.Password))).ToArray();
//        }
//    }

//    public static class HttpBasicAuthStrategyFunc
//    {
//        public static bool Execute(this HttpBasicAuthStrategy self, HttpListenerContext app)
//        {
//            var authVal = app.Request.Headers["Authorization"];
//            if (!self.ValidAuthVals.Contains(authVal))
//            {
//                self.Respond401(app, "Basic Realm=" + self.Realm);
//                return false;
//            }
//            return true;
//        }
//    }




//    [ObjectSystem]
//    public class HttpRestrictIPStrategyAwakeSystem : AwakeSystem<HttpRestrictIPStrategy, string>
//    {
//        public override void Awake(HttpRestrictIPStrategy self, string ipAddresses)
//        {
//            var ranges = ipAddresses.Split(
//                new char[] { ';' },
//                StringSplitOptions.RemoveEmptyEntries);
//            foreach (var ss in ranges)
//            {
//                self.IpRanges.Add(self.AddChild<HttpIPAddressRange, string>(ss).Id);
//            }
//        }
//    }
//    public static class HttpRestrictIPStrategyFunc
//    {
//        public static bool Execute(this HttpRestrictIPStrategy self, HttpListenerContext app)
//        {
//            foreach (var ip in HttpAuthComponent.Instance.GetClientIPAddresses(app))
//            {
//                if (self.IpRanges.Any(a => self.GetChild<HttpIPAddressRange>(a).IsInRange(ip)))
//                    return true;
//            }
//            return RespondError(app);
//        }

//        public static bool RespondError(HttpListenerContext app)
//        {
//            app.Response.StatusDescription = "403 Forbidden";
//            app.Response.StatusCode = 403;
//            app.Response.Close();
//            return false;
//        }
//    }



//    [ObjectSystem]
//    public class HttpIPAddressRangeAwakeSystem : AwakeSystem<HttpIPAddressRange, string>
//    {
//        /// <param name="ipRangeStr">
//        /// e.g)
//        /// "10.23.0.0/24",
//        /// "127.0.0.1" (equals to "127.0.0.1/32"),
//        /// "2001:0db8:bd05:01d2:288a:1fc0:0001:0000/16",
//        /// "::1" (equals to "::1/128")
//        /// </param>
//        public override void Awake(HttpIPAddressRange self, string ipRangeString)
//        {
//            if (string.IsNullOrEmpty(ipRangeString))
//                throw new InvalidOperationException("IP Address is null or empty.");

//            var vals = ipRangeString.Split('/');
//            if (!IPAddress.TryParse(vals[0], out var ipAddr))
//                throw new InvalidOperationException(string.Format("IP Address({0}) is invalid format.", ipRangeString));

//            self.AddressFamily = ipAddr.AddressFamily;
//            if (self.AddressFamily != AddressFamily.InterNetwork &&
//                self.AddressFamily != AddressFamily.InterNetworkV6)
//                throw new InvalidOperationException(string.Format("IP Address({0}) is not ip4 or ip6 address famiry.", ipRangeString));

//            var maxMaskRange = self.AddressFamily == AddressFamily.InterNetwork ? 32 : 128;
//            int maskRange;
//            if (vals.Length > 1)
//            {
//                if (!int.TryParse(vals[1], out maskRange) || maskRange < 0 || maskRange > maxMaskRange)
//                    throw new InvalidOperationException(string.Format("IP Address({0}) is invalid range.", ipRangeString));
//            }
//            else
//                maskRange = maxMaskRange;

//            self.NetworkAddressBytes = ipAddr.GetAddressBytes();
//            self.SubnetMaskBytes = Enumerable.Repeat<byte>(0xFF, self.NetworkAddressBytes.Length).ToArray();

//            for (int i = 0; i < (maxMaskRange - maskRange); i++)
//                self.SubnetMaskBytes[self.SubnetMaskBytes.Length - 1 - i / 8] -= (byte)(1 << (i % 8));
//        }
//    }


//    public static class HttpIPAddressRangeFunc
//    {
//        public static bool IsInRange(this HttpIPAddressRange self, IPAddress ipAddr)
//        {
//            if (ipAddr.AddressFamily != self.AddressFamily)
//                return false;

//            var addrBytes = ipAddr.GetAddressBytes();
//            for (int i = 0; i < addrBytes.Length; i++)
//                if ((addrBytes[i] & self.SubnetMaskBytes[i]) != self.NetworkAddressBytes[i])
//                    return false;

//            return true;
//        }
//        public static bool IsInRange(this HttpIPAddressRange self, string ipAddrString)
//        {
//            if (!IPAddress.TryParse(ipAddrString, out var ipAddr))
//                return false;
//            return self.IsInRange(ipAddr);
//        }
//    }


//}
