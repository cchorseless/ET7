using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bright.Serialization
{
    public static class ByteBufExtensions
    {
        public static void WriteUnityVector2(this ByteBuf buf, Unity.Mathematics.float2 v)
        {
            buf.WriteFloat(v.x);
            buf.WriteFloat(v.y);
        }

        public static Unity.Mathematics.float2 ReadUnityVector2(this ByteBuf buf)
        {
            return new Unity.Mathematics.float2(buf.ReadFloat(), buf.ReadFloat());
        }

        public static void WriteUnityVector3(this ByteBuf buf, Unity.Mathematics.float3 v)
        {
            buf.WriteFloat(v.x);
            buf.WriteFloat(v.y);
            buf.WriteFloat(v.z);
        }

        public static Unity.Mathematics.float3 ReadUnityVector3(this ByteBuf buf)
        {
            return new Unity.Mathematics.float3(buf.ReadFloat(), buf.ReadFloat(), buf.ReadFloat());
        }

        public static void WriteUnityVector4(this ByteBuf buf, Unity.Mathematics.float4 v)
        {
            buf.WriteFloat(v.x);
            buf.WriteFloat(v.y);
            buf.WriteFloat(v.z);
            buf.WriteFloat(v.w);
        }

        public static Unity.Mathematics.float4 ReadUnityVector4(this ByteBuf buf)
        {
            return new Unity.Mathematics.float4(buf.ReadFloat(), buf.ReadFloat(), buf.ReadFloat(), buf.ReadFloat());
        }
    }
}
