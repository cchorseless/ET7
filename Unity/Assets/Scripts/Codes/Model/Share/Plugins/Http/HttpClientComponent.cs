﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace ET
{
    public class HttpClientComponent : Entity, IAwake, IDestroy
    {
        public HttpClient Client;
    }
}
