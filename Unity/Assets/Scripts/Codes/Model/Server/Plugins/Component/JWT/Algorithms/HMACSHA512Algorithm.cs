﻿using System;
using System.Security.Cryptography;

namespace ET.JWT.Algorithms
{
    /// <summary>
    /// HMAC using SHA-512
    /// </summary>
    //[Obsolete(ObsoleteMessage,  false)]
    public sealed class HMACSHA512Algorithm : HMACSHAAlgorithm
    {
        /// <inheritdoc />
        public override string Name => nameof(JwtAlgorithmName.HS512);

        /// <inheritdoc />
        public override HashAlgorithmName HashAlgorithmName => HashAlgorithmName.SHA512;

        protected override HMAC CreateAlgorithm(byte[] key) => new HMACSHA512(key);
    }
}