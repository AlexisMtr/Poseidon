namespace PoseidonFA.Helpers
{
    public static class NumericsExtension
    {
        public static bool IsBetween(this int value, int lowerBound, int upperBound, bool strict = false)
        {
            if(strict)
                return value > lowerBound && value < upperBound;
            else
                return value >= lowerBound && value <= upperBound;
        }

        public static bool IsBetween(this long value, long lowerBound, long upperBound, bool strict = false)
        {
            if (strict)
                return value > lowerBound && value < upperBound;
            else
                return value >= lowerBound && value <= upperBound;
        }

        public static bool IsBetween(this double value, double lowerBound, double upperBound, bool strict = false)
        {
            if (strict)
                return value > lowerBound && value < upperBound;
            else
                return value >= lowerBound && value <= upperBound;
        }

        public static bool IsBetween(this float value, float lowerBound, float upperBound, bool strict = false)
        {
            if (strict)
                return value > lowerBound && value < upperBound;
            else
                return value >= lowerBound && value <= upperBound;
        }
    }
}
