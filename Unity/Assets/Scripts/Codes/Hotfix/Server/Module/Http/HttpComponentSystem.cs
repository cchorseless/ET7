using System;
using System.Collections.Generic;
using System.Net;

namespace ET.Server
{
    [FriendOf(typeof (HttpComponent))]
    public static partial class HttpComponentSystem
    {
        public class HttpComponentAwakeSystem: AwakeSystem<HttpComponent, string>
        {
            protected override void Awake(HttpComponent self, string address)
            {
                try
                {
                    self.Load();

                    self.Listener = new HttpListener();

                    foreach (string s in address.Split(';'))
                    {
                        if (s.Trim() == "")
                        {
                            continue;
                        }

                        self.Listener.Prefixes.Add(s);
                    }

                    self.Listener.Start();

                    self.Accept().Coroutine();
                }
                catch (HttpListenerException e)
                {
                    throw new Exception($"请先在cmd中运行: netsh http add urlacl url=http://*:你的address中的端口/ user=Everyone, address: {address}", e);
                }
            }
        }

        [ObjectSystem]
        public class HttpComponentLoadSystem: LoadSystem<HttpComponent>
        {
            protected override void Load(HttpComponent self)
            {
                self.Load();
            }
        }

        [ObjectSystem]
        public class HttpComponentDestroySystem: DestroySystem<HttpComponent>
        {
            protected override void Destroy(HttpComponent self)
            {
                self.Listener.Stop();
                self.Listener.Close();
                self.httpContextQueue.Clear();
                self.handleContext = null;
            }
        }

        [ObjectSystem]
        public class HttpComponentUpdateSystem: UpdateSystem<HttpComponent>
        {
            protected override void Update(HttpComponent self)
            {
                while (true)
                {
                    if (!self.httpContextQueue.TryDequeue(out self.handleContext))
                    {
                        return;
                    }

                    try
                    {
                        self.Handle(self.handleContext).Coroutine();
                    }
                    catch (ObjectDisposedException)
                    {
                    }
                    catch (Exception e)
                    {
                        Log.Error(e);
                    }
                }
            }
        }

        public static void Load(this HttpComponent self)
        {
            self.dispatcher.Clear();
            self.handlerAttr.Clear();

            HashSet<Type> types = EventSystem.Instance.GetTypes(typeof (HttpHandlerAttribute));

            SceneType sceneType = self.GetParent<Scene>().SceneType;

            foreach (Type type in types)
            {
                object[] attrs = type.GetCustomAttributes(typeof (HttpHandlerAttribute), false);
                if (attrs.Length == 0)
                {
                    continue;
                }

                HttpHandlerAttribute httpHandlerAttribute = (HttpHandlerAttribute)attrs[0];
                // 改过
                self.handlerAttr.Add(httpHandlerAttribute.Path, httpHandlerAttribute);
                if (httpHandlerAttribute.SceneType != sceneType)
                {
                    continue;
                }

                object obj = Activator.CreateInstance(type);

                IHttpHandler ihttpHandler = obj as IHttpHandler;
                if (ihttpHandler == null)
                {
                    throw new Exception($"HttpHandler handler not inherit IHttpHandler class: {obj.GetType().FullName}");
                }

                self.dispatcher.Add(httpHandlerAttribute.Path, ihttpHandler);
            }
        }

        public static async ETTask Accept(this HttpComponent self)
        {
            long instanceId = self.InstanceId;
            while (self.InstanceId == instanceId)
            {
                try
                {
                    HttpListenerContext context = await self.Listener.GetContextAsync();
                    self.httpContextQueue.Enqueue(context);
                }
                catch (Exception e)
                {
                    Log.Error(e);
                }
            }
        }

        public static async ETTask Handle(this HttpComponent self, HttpListenerContext context)
        {
            // 跨域設置
            if (context.AllowOrigin())
            {
                context.Response.StatusCode = (int)HttpStatusCode.OK;
                context.FinishHander();
                return;
            }

            if (!self.VerifyAuth(context))
            {
                // context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                context.FinishHander();
                return;
            }

            IHttpHandler handler;
            if (!self.dispatcher.TryGetValue(context.Request.Url.AbsolutePath, out handler))
            {
                context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                context.FinishHander();
                return;
            }

            try
            {
                await handler.Handle(self.Domain, context);
            }
            catch (HttpListenerException e)
            {
                //System.Net.HttpListenerException(1229): 企图在不存在的网络连接上进行操作。
                if (e.ErrorCode != 1229)
                {
                    Log.Error(e);
                }
            }
            catch (Exception e)
            {
                Log.Error(e);
            }

            context.FinishHander();
        }
    }
}