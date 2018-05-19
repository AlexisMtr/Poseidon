using System;

namespace Poseidon.Helpers
{
    public static class DateTimeHelper
    {
        private static DateTime januaryFirst = new DateTime(1970, 1, 1, 0, 0, 0);

        public static DateTime ToDateTime(this long timestamp)
        {
            return januaryFirst.AddSeconds(timestamp);
        }

        public static long ToTimestamp(this DateTime date)
        {
            return Convert.ToInt64(date.Subtract(januaryFirst).TotalSeconds);
        }
    }
}
