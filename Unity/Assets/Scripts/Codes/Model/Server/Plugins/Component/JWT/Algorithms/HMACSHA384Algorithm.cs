using System;
using System.Security.Cryptography;

namespace ET.JWT.Algorithms
{
    /// <summary>
    /// HMAC using SHA-384
    /// </summary>
    //[Obsolete(ObsoleteMessage,  false)]
    public sealed class HMACSHA384Algorithm : HMACSHAAlgorithm
    {
        /// <inheritdoc />
        public override string Name => nameof(JwtAlgorithmName.HS384);

        /// <inheritdoc />
        public override HashAlgorithmName HashAlgorithmName => HashAlgorithmName.SHA384;

        protected override HMAC CreateAlgorithm(byte[] key) => new HMACSHA384(key);
    }
}