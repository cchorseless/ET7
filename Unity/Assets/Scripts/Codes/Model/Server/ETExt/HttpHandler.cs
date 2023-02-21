using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace ET.Server

{
    public static class HttpHandlerFunc
    {
        public static void Reply<Response>(this HttpListenerContext context, Response msg) where Response : class, IResponse
        {
            if (context.Response.OutputStream.CanWrite)
            {
                var bytes = MongoHelper.ToClientJson(msg).ToByteArray();
                context.Response.ContentEncoding = Encoding.UTF8;
                context.Response.ContentLength64 = bytes.Length;
                context.Response.OutputStream.Write(bytes, 0, bytes.Length);
            }
        }

        public static long ParsePlayerId(this HttpListenerContext context)
        {
            var playerid = context.Request.Headers.Get(HttpConfig.UUID);
            if (!string.IsNullOrEmpty(playerid) && long.TryParse(playerid, out var id))
            {
                return id;
            }

            return 0;
        }

        public static bool AllowOrigin(this HttpListenerContext context)
        {
            //context.Request.Headers.Add("Access-Control-Allow-Origin", "*");
            context.Response.AddHeader("Access-Control-Allow-Origin", "*");
            if (context.Request.HttpMethod.ToUpper() == "OPTIONS")
            {
                context.Response.AddHeader("Access-Control-Allow-Headers",
                    "Content-Type, Access-Control-Allow-Headers, Authorization, X-Requested-With, Access-Control-Allow-Methods, Access-Control-Allow-Origin");
                context.Response.AddHeader("Access-Control-Max-Age", "1728000");
            }

            //context.Response.AddHeader("Cache-Control", "no-cache");
            context.Response.AddHeader("Access-Control-Allow-Methods", "GET, POST, OPTIONS");
            return context.Request.HttpMethod.ToUpper() == "OPTIONS";
            //return false;
        }
    }

    public abstract class HttpGetHandler<Response>: IHttpHandler where Response : class, IResponse
    {
        protected abstract ETTask Run(Entity domain, Response response, long playerid);

        public async ETTask Handle(Entity domain, HttpListenerContext context)
        {
            context.Response.ContentEncoding = Encoding.UTF8;
            if (context.Request.HttpMethod.ToLower() != "get")
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return;
            }
            context.Response.StatusCode = (int)HttpStatusCode.OK;
            Response responseData = Activator.CreateInstance<Response>();
            await Run(domain, responseData, context.ParsePlayerId());
            context.Reply(responseData);
        }
    }

    public abstract class HttpPostHandler<Request, Response>: IHttpHandler where Request : class, IRequest where Response : class, IResponse
    {
        protected abstract ETTask Run(Entity domain, Request request, Response response, long playerid);

        public async ETTask Handle(Entity domain, HttpListenerContext context)
        {
            context.Response.ContentEncoding = Encoding.UTF8;
            if (context.Request.HttpMethod.ToLower() != "post" || context.Request.ContentLength64 > HttpConfig.PostMaxDataLength)
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return;
            }

            string requestmsg = await context.Request.ReadStringAsync();

            Request requestData = JsonHelper.FromJson<Request>(requestmsg);
            if (requestData == null)
            {
                throw new Exception($"消息类型转换错误:  {typeof (Request).Name}");
            }

            Response responseData = Activator.CreateInstance<Response>();
            await Run(domain, requestData, responseData, context.ParsePlayerId());
            context.Reply(responseData);
        }
    }

    public abstract class HttpPostWithContextHandler<Request, Response>: IHttpHandler where Request : class, IRequest
            where Response : class, IResponse
    {
        protected abstract ETTask Run(Entity domain, Request request, Response response, HttpListenerContext context);

        public async ETTask Handle(Entity domain, HttpListenerContext context)
        {
            context.Response.ContentEncoding = Encoding.UTF8;
            if (context.Request.HttpMethod.ToLower() != "post" || context.Request.ContentLength64 > HttpConfig.PostMaxDataLength)
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return;
            }

            string requestmsg = await context.Request.ReadStringAsync();
            Request requestData = JsonHelper.FromJson<Request>(requestmsg);
            if (requestData == null)
            {
                throw new Exception($"消息类型转换错误:  {typeof (Request).Name}");
            }

            Response responseData = Activator.CreateInstance<Response>();
            await Run(domain, requestData, responseData, context);
            context.Reply(responseData);
        }
    }

    public abstract class HttpBasePostHandler: IHttpHandler
    {
        protected abstract ETTask Run(Entity domain, HttpListenerContext context);

        public async ETTask Handle(Entity domain, HttpListenerContext context)
        {
            context.Response.ContentEncoding = Encoding.UTF8;
            if (context.Request.HttpMethod.ToLower() != "post" ||
                context.Request.ContentLength64 > HttpConfig.PostMaxDataLength)
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return;
            }

            context.Response.StatusCode = (int)HttpStatusCode.OK;
            await Run(domain, context);
        }
    }
}