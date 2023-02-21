using System;
using System.Collections.Generic;
using System.Net;
using ET.JWT;
using ET.JWT.Builder;
using ET.JWT.Exceptions;
using ET.JWT.Serializers;

namespace ET.Server
{
    [FriendOf(typeof(HttpComponent))]
    public static partial class HttpComponentSystem {

        public static string AuthorizeToken(this HttpComponent self, long userId, long key, int hour)
        {
            using (var build = JwtBuilder.Create())
            {
                var token = build
                      .WithAlgorithm(self.HMACSHA256Algorithm) // symmetric
                      .WithSecret(HttpConfig.AuthSecret)
                      .AddClaim(HttpConfig.Exp, DateTimeOffset.UtcNow.AddHours(hour).ToUnixTimeSeconds())
                      .AddClaim(HttpConfig.UUID, userId.ToString())
                      .AddClaim(HttpConfig.Key, key.ToString())
                      .Encode();
                return token;
            }
        }

        public static bool VerifyPlayer(this HttpComponent self, string uuid, string key)
        {
            if (!string.IsNullOrEmpty(uuid) && long.TryParse(uuid, out long playerid))
            {
                Scene scene = self.DomainScene();
                PlayerComponent playerComponent = scene.GetComponent<PlayerComponent>();
                Player player = playerComponent.Get(playerid);
                if (player != null && !player.IsDisposed &&
                    player.GateSessionActorId != 0 &&
                    player.GateSessionActorId.ToString() == key)
                {
                    return true;
                }
            }
            return false;
        }



        public static bool VerifyAuth(this HttpComponent self, HttpListenerContext context)
        {
            if (self.handlerAttr.TryGetValue(context.Request.Url.AbsolutePath, out var attrInfo) && !attrInfo.NeedAuth)
            {
                return true;
            }
            var token = context.Request.Headers.Get(HttpConfig.Authorization);
            if (string.IsNullOrEmpty(token) || !token.StartsWith(HttpConfig.TokenPrefix))
            {
                return false;
            }
            token = token.Substring(HttpConfig.TokenPrefix.Length);
            using (var build = JwtBuilder.Create())
            {
                try
                {
                    var payload = build.WithAlgorithm(self.HMACSHA256Algorithm) // symmetric
                     .WithSecret(HttpConfig.AuthSecret)
                     .MustVerifySignature()
                     .Decode(token);
                    var json = JsonHelper.FromJson<Dictionary<string, object>>(payload);
                    if (json.TryGetValue(HttpConfig.UUID, out var playerId) &&
                        json.TryGetValue(HttpConfig.Key, out var key) &&
                        self.VerifyPlayer((string)playerId, (string)key))
                    {
                        context.Request.Headers[HttpConfig.UUID] = (string)playerId;
                        return true;
                    }
                    context.Response.StatusDescription = "Player Miss";
                }
                catch (TokenExpiredException)
                {
                    context.Response.StatusDescription = "Token Expired";
                }
                catch (SignatureVerificationException)
                {
                    context.Response.StatusDescription = "Signature Verification";
                }
            }
            return false;
        }
    }
}