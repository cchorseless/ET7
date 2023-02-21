using System;
using System.Security.Cryptography;

namespace ET.JWT.Algorithms
{
    /// <summary>
    /// HMAC using SHA-256
    /// </summary>
    //[Obsolete(ObsoleteMessage,  false)]
    public sealed class HMACSHA256Algorithm : HMACSHAAlgorithm
    {
        /// <inheritdoc />
        public override string Name => nameof(JwtAlgorithmName.HS256);

        /// <inheritdoc />
        public override HashAlgorithmName HashAlgorithmName => HashAlgorithmName.SHA256;

        protected override HMAC CreateAlgorithm(byte[] key) => new HMACSHA256(key);
    }
}