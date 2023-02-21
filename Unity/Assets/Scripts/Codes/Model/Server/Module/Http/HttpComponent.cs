using System.Collections.Generic;
using System.Net;
using ET.JWT;
using ET.JWT.Algorithms;
using ET.JWT.Builder;
using ET.JWT.Serializers;

namespace ET.Server
{
    /// <summary>
    /// http请求分发器
    /// </summary>
    [ComponentOf(typeof(Scene))]
    public class HttpComponent : Entity, IAwake<string>, IDestroy, ILoad
    {
        public HttpListener Listener;
        public Dictionary<string, IHttpHandler> dispatcher;
        public Dictionary<string, HttpHandlerAttribute> handlerAttr;

        public readonly IJwtAlgorithm HMACSHA256Algorithm = new HMACSHA256Algorithm();

        //protected static readonly IJwtAlgorithm RS256Algorithm = new RS256Algorithm(
        //    new X509Certificate2(Convert.FromBase64String(ServerRsaPublicKey2)));
    }
}