using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace ET.Client
{
    public class HttpDotaComponent : Entity, IAwake, IDestroy
    {
        public HttpClient Client;
        public string Account = "";
        public string TOKEN = "";
        public string Address = "";
        public string Port = "";
        public string ServerPlayerID = "";
        public int ReConnectTime = 10;
        public bool IsOnline = false;
        public int  GateId = 0;
    }
}