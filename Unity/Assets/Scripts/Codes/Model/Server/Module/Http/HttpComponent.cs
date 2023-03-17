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
    [ComponentOf(typeof (Scene))]
    public class HttpComponent: Entity, IAwake<string>, IDestroy, ILoad, IUpdate
    {
        public HttpListener Listener;
        public HttpListenerContext handleContext;
        public readonly Dictionary<string, IHttpHandler> dispatcher = new Dictionary<string, IHttpHandler>();
        public readonly Dictionary<string, HttpHandlerAttribute> handlerAttr = new Dictionary<string, HttpHandlerAttribute>();
        public readonly Queue<HttpListenerContext> httpContextQueue = new Queue<HttpListenerContext>();
        public readonly IJwtAlgorithm HMACSHA256Algorithm = new HMACSHA256Algorithm();

        //protected static readonly IJwtAlgorithm RS256Algorithm = new RS256Algorithm(
        //    new X509Certificate2(Convert.FromBase64String(ServerRsaPublicKey2)));
    }
}