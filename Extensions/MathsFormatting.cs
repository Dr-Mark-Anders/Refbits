using System;
using System.Globalization;

namespace Units
{
    public static class Precision
    {
        // 2^-24
        public const float FLOAT_EPSILON = 0.0000000596046448f;

        // 2^-53
        public const double DOUBLE_EPSILON = 0.00000000000000011102230246251565d;

        public static bool AlmostEquals(this double a, double b, double epsilon)
        {
            // ReSharper disable CompareOfFloatsByEqualityOperator
            if (a == b)
            {
                return true;
            }
            // ReSharper restore CompareOfFloatsByEqualityOperator

            if (double.IsNaN(a) && double.IsNaN(b))
                return true;

            return Math.Abs(a - b) < epsilon;
        }

        public static bool AlmostEquals(this float a, float b, float epsilon)
        {
            // ReSharper disable CompareOfFloatsByEqualityOperator
            if (a == b)
            {
                return true;
            }
            // ReSharper restore CompareOfFloatsByEqualityOperator

            return (System.Math.Abs(a - b) < epsilon);
        }
    }

    public static class SignificantDigits
    {
        public static double Round(this double value, int significantDigits)
        {
            int unneededRoundingPosition;
            return RoundSignificantDigits(value, significantDigits, out unneededRoundingPosition);
        }

        public static int CountSignificantDigits(double val)
        {
            string s = Math.Abs(val).ToString();
            int len = s.Length;
            if (s[0] != '0' && s.Contains("."))
                len -= 1;
            int start = 0;

            for (int i = 0; i < len; i++)
            {
                switch (s[i])
                {
                    case '0':
                    case '.':
                        break;

                    case '1':
                    case '2':
                    case '3':
                    case '4':
                    case '5':
                    case '6':
                    case '7':
                    case '8':
                    case '9':
                        start = i;
                        i = len;
                        break;
                }
            }
            return len - start;
        }

        public static string ToString(this double value, int significantDigits)
        {
            // this method will round and then append zeros if needed.
            // i.e. if you round .002 to two significant figures, the resulting number should be .0020.

            var currentInfo = CultureInfo.CurrentCulture.NumberFormat;

            if (double.IsNaN(value))
            {
                return currentInfo.NaNSymbol;
            }

            if (double.IsPositiveInfinity(value))
            {
                return currentInfo.PositiveInfinitySymbol;
            }

            if (double.IsNegativeInfinity(value))
            {
                return currentInfo.NegativeInfinitySymbol;
            }

            int roundingPosition;
            var roundedValue = RoundSignificantDigits(value, significantDigits, out roundingPosition);

            // when rounding causes a cascading round affecting digits of greater significance,
            // need to re-round to get a correct rounding position afterwards
            // this fixes a bug where rounding 9.96 to 2 figures yeilds 10.0 instead of 10
            RoundSignificantDigits(roundedValue, significantDigits, out roundingPosition);

            if (Math.Abs(roundingPosition) > 9)
            {
                // use exponential notation format
                // ReSharper disable FormatStringProblem
                return string.Format(currentInfo, "{0:E" + (significantDigits - 1) + "}", roundedValue);
                // ReSharper restore FormatStringProblem
            }

            // string.format is only needed with decimal numbers (whole numbers won't need to be padded with zeros to the right.)
            // ReSharper disable FormatStringProblem
            return roundingPosition > 0 ? string.Format(currentInfo, "{0:F" + roundingPosition + "}", roundedValue) : roundedValue.ToString(currentInfo);
            // ReSharper restore FormatStringProblem
        }

        private static double RoundSignificantDigits(double value, int significantDigits, out int roundingPosition)
        {
            // this method will return a rounded double value at a number of signifigant figures.
            // the sigFigures parameter must be between 0 and 15, exclusive.

            roundingPosition = 0;

            /*if (value.AlmostEquals(0d))
            {
                roundingPosition = significantDigits - 1;
                return 0d;
            }*/

            if (double.IsNaN(value))
            {
                return double.NaN;
            }

            if (value == 0)
            {
                return 0;
            }

            if (double.IsPositiveInfinity(value))
            {
                return double.PositiveInfinity;
            }

            if (double.IsNegativeInfinity(value))
            {
                return double.NegativeInfinity;
            }

            if (significantDigits < 1 || significantDigits > 15)
            {
                throw new ArgumentOutOfRangeException("significantDigits", value, "The significantDigits argument must be between 1 and 15.");
            }

            // The resulting rounding position will be negative for rounding at whole numbers, and positive for decimal places.
            roundingPosition = significantDigits - 1 - (int)(Math.Floor(Math.Log10(Math.Abs(value))));

            // try to use a rounding position directly, if no scale is needed.
            // this is because the scale mutliplication after the rounding can introduce error, although
            // this only happens when you're dealing with really tiny numbers, i.e 9.9e-14.
            if (roundingPosition > 0 && roundingPosition < 16)
            {
                return Math.Round(value, roundingPosition, MidpointRounding.AwayFromZero);
            }

            // Shouldn't get here unless we need to scale it.
            // Set the scaling value, for rounding whole numbers or decimals past 15 places
            var scale = Math.Pow(10, Math.Ceiling(Math.Log10(Math.Abs(value))));

            int decpoints = significantDigits - (int)Math.Log10(scale);
            if (decpoints < 0)
            {
                value = value * Math.Pow(10, decpoints);
                value = Math.Truncate(value);
                value = value / Math.Pow(10, decpoints);
                decpoints = 0;
            }
            else
            {
                value = value * Math.Pow(10, decpoints);
                value = Math.Truncate(value);
                value = value / Math.Pow(10, decpoints);
            }
            return value;

            //return Math.Round(value / scale, significantDigits, MidpointRounding.AwayFromZero) * scale;
        }
    }
}