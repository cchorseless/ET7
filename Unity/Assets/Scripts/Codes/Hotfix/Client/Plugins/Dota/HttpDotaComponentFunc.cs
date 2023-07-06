using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ET.Client
{
    public static class HttpProtocol
    {
        public const string AccountLoginKey = "/AccountLoginKey";
        public const string LoginRealm = "/LoginRealm";
        public const string LoginGate = "/LoginGate";
        public const string CreateGameRecord = "/CreateGameRecord";
        public const string UploadGameRecord = "/UploadGameRecord";
        public const string RefreshToken = "/RefreshToken";
        public const string Ping = "/Ping";
        public const string LoginOut = "/LoginOut";
        public const string SetServerKey = "/SetServerKey";
        public const string Buy_ShopItem = "/Buy_ShopItem";
        public const string UploadCharacterGameRecord = "/UploadCharacterGameRecord";
    }

    public static class HttpDotaComponentFunc
    {
        [ObjectSystem]
        public class HttpDotaComponentSystemAwakeSystem: AwakeSystem<HttpDotaComponent>
        {
            protected override void Awake(HttpDotaComponent self)
            {
                self.Client = new HttpClient();
            }
        }

        [ObjectSystem]
        public class HttpDotaComponentSystemDestroySystem: DestroySystem<HttpDotaComponent>
        {
            protected override void Destroy(HttpDotaComponent self)
            {
                self.Client.Dispose();
                self.Client = null;
            }
        }

        public static void InitHeader(this HttpDotaComponent self)
        {
            self.Client.DefaultRequestHeaders.Clear();
            self.Client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json;charset=uft-8");
            self.Client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", self.TOKEN);
        }

        public static async ETTask<Response> GetAsync<Response>(this HttpDotaComponent self, string url) where Response : class, IResponse
        {
            self.InitHeader();
            Response requestData = null;
            // Log.Console(self.Account + " send :" + url);
            try
            {
                var msg = await self.Client.GetStringAsync(url);
                requestData = JsonHelper.FromJson<Response>(msg);
                if (requestData == null)
                {
                    throw new Exception($"消息类型转换错误:  {url}");
                }
            }
            catch (Exception e)
            {
                Log.Error(e);
                requestData = Activator.CreateInstance<Response>();
            }

            return requestData;
        }

        public static async ETTask<Response> PostAsync<Request, Response>(this HttpDotaComponent self, string url, Request sendData)
                where Request : class, IRequest where Response : class, IResponse
        {
            Response requestData = null;
            self.InitHeader();
            // Log.Console(self.Account + " send :" + url);
            try
            {
                var msg = await self.Client.PostAsync(url, new StringContent(sendData + ""));
                msg.EnsureSuccessStatusCode();
                requestData = await msg.Content.ReadFromJsonAsync<Response>();
                if (requestData == null)
                {
                    throw new Exception($"消息类型转换错误:  {url}");
                }
            }
            catch (Exception e)
            {
                Log.Error(e);
                requestData = Activator.CreateInstance<Response>();
            }

            return requestData;
        }

        public static string GetAdressPort(this HttpDotaComponent self, string actionstr)
        {
            if (string.IsNullOrEmpty(self.Address))
            {
                return "http://127.0.0.1:10101" + actionstr;
            }

            return self.Address + ":" + self.Port + actionstr;
        }

        public static async ETTask LoginHttp(this HttpDotaComponent self, string account)
        {
            self.Account = account;
            var cbmsg = await self.PostAsync<C2H_GetAccountLoginKey, H2C_GetAccountLoginKey>(self.GetAdressPort(HttpProtocol.AccountLoginKey),
                new() { Account = account });
            var password = "";
            if (cbmsg.Error == 0)
            {
                password = MD5Helper.GetMD5(cbmsg.Key + ConstValue.DotaDedicatedServerKeyV2 + account);
            }
            else
            {
                password = MD5Helper.GetMD5(cbmsg.Key + ConstValue.DotaDedicatedServerKeyV2) + account;
            }

            var cbmsg1 = await self.PostAsync<C2R_Login, R2C_Login>(self.GetAdressPort(HttpProtocol.LoginRealm),
                new() { Account = account, Password = password, ServerId = 1, GateId = self.GateId });
            if (cbmsg1.Error == 0)
            {
                self.GateId = (int)cbmsg1.GateId;
                var _adress = cbmsg1.Address.Split(":");
                self.Address = "http://" + _adress[0];
                self.Port = _adress[1];
                self.ServerPlayerID = cbmsg1.UserId.ToString();
                var cbmsg2 = await self.PostAsync<C2G_LoginGate, H2C_CommonResponse>(self.GetAdressPort(HttpProtocol.LoginGate),
                    new() { UserId = cbmsg1.UserId, Key = cbmsg1.Key, ServerId = 1 });
                if (cbmsg2.Error == 0)
                {
                    self.TOKEN = "Bearer " + cbmsg2.Message;
                    self.IsOnline = true;
                    self.Ping().Coroutine();
                    self.CreateGameRecord(new() { self.ServerPlayerID }).Coroutine();
                    return;
                }
            }
        }

        public static async ETTask Ping(this HttpDotaComponent self)
        {
            var instance = self.InstanceId;
            while (self.InstanceId == instance && self.IsOnline)
            {
                try
                {
                    var cbmsg = await self.GetAsync<G2C_Ping>(self.GetAdressPort(HttpProtocol.Ping));
                    if (cbmsg.Error == 0)
                    {
                        var Message = cbmsg.Message ?? "";
                        // Log.Console($" {self.Account} Ping recove msg :length {Message.Length}");
                    }
                    else
                    {
                        self.IsOnline = false;
                        throw new Exception($" {self.Account} Ping Error : {cbmsg.Error}");
                    }
                }
                catch (Exception e)
                {
                    Log.Error(e);
                    self.IsOnline = false;
                    return;
                }
            }
        }

        public static async ETTask CreateGameRecord(this HttpDotaComponent self, List<string> serverplayerIds)
        {
            if (self.IsDisposed || !self.IsOnline)
            {
                return;
            }

            var cbmsg1 = await self.PostAsync<C2H_CreateGameRecord, H2C_CommonResponse>(self.GetAdressPort(HttpProtocol.CreateGameRecord),
                new() { Players = serverplayerIds });
            if (cbmsg1.Error == 0)
            {
                return;
            }
        }
    }
}