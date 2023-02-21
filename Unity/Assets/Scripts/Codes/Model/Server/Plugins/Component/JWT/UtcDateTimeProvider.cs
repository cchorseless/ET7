using System;

namespace ET.JWT
{
    /// <summary>
    /// Provider for UTC DateTime.
    /// </summary>
    public sealed class UtcDateTimeProvider : IDateTimeProvider
    {
        /// <summary>
        /// Retuns the current time (UTC).
        /// </summary>
        /// <returns></returns>
        public DateTimeOffset GetNow()
        {
            return DateTimeOffset.UtcNow;
        }

        private static UtcDateTimeProvider Instance;
        public static UtcDateTimeProvider GetInstance()
        {
            if (Instance == null)
            {
                Instance = new UtcDateTimeProvider();
            }
            return Instance;
        }
    }
}