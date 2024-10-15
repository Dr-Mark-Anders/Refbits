using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using Units.UOM;

public static class UnitExtension
{
    public const double DOUBLE_EPSILON = 0.00000000000000011102230246251565d;

    public static string GetDescription<T>(this T e) where T : IConvertible
    {
        if (e is Enum)
        {
            Type type = e.GetType();
            Array values = System.Enum.GetValues(type);

            foreach (int val in values)
            {
                if (val == e.ToInt32(CultureInfo.InvariantCulture))
                {
                    var memInfo = type.GetMember(type.GetEnumName(val));

                    if (memInfo[0]
                        .GetCustomAttributes(typeof(DescriptionAttribute), false).FirstOrDefault() is DescriptionAttribute descriptionAttribute)
                    {
                        return descriptionAttribute.Description;
                    }
                    continue;
                }
            }
        }

        return null; // could also return string.Empty
    }

    public static double[][] TemperatureToDouble(this Temperature[][] a)
    {
        double[][] res = new double[a.Length][];

        for (int i = 0; i < a.Length; i++)
        {
            res[i] = new double[a[i].Length];

            for (int n = 0; n < a[i].Length; n++)
            {
                res[i][n] = a[i][n];
            }
        }
        return res;
    }

    public static double[] TemperatureToDouble(this Temperature[] a)
    {
        double[] res = new double[a.Length];

        for (int n = 0; n < a.Length; n++)
            res[n] = a[n];
        
        return res;
    }

    public static List<double> TemperatureToDoubleList(this Temperature[] a)
    {
        List<double> res = new List<double>();

        for (int n = 0; n < a.Length; n++)
        {
            res.Add(a[n]);
        }

        return res;
    }

    public static Temperature[][] DoubleToTemperature(this double[][] a)
    {
        Temperature[][] res = new Temperature[a.Length][];

        for (int i = 0; i < a.Length; i++)
        {
            res[i] = new Temperature[a[i].Length];

            for (int n = 0; n < a[i].Length; n++)
            {
                res[i][n] = new Temperature(a[i][n]);
            }
        }
        return res;
    }

    public static bool AlmostEquals(this double a, double b, double epsilon = 1e-5)
    {
        if (double.IsNaN(a) && double.IsNaN(b))
            return true;

        if (a == b)
        {
            return true;
        }

        return Math.Abs((a - b) / (Math.Max(a, b))) * 100 < epsilon;
    }

    public static double[][] DegKtoDouble(this Temperature[][] a)
    {
        double[][] res = new double[a.Length][];

        for (int i = 0; i < a.Length; i++)
        {
            res[i] = new double[a[i].Length];

            for (int n = 0; n < a[i].Length; n++)
            {
                res[i][n] = a[i][n];
            }
        }
        return res;
    }

    public static double[] DegKtoDouble(this Temperature[] a)
    {
        double[] res = new double[a.Length];

        for (int n = 0; n < a.Length; n++)
        {
            res[n] = a[n];
        }

        return res;
    }

    public static Temperature[][] DoubleToDegK(this double[][] a)
    {
        Temperature[][] res = new Temperature[a.Length][];

        for (int i = 0; i < a.Length; i++)
        {
            res[i] = new Temperature[a[i].Length];

            for (int n = 0; n < a[i].Length; n++)
            {
                res[i][n] = new Temperature(a[i][n]);
            }
        }
        return res;
    }

    public static Temperature[][] DegKtoDegC(this Temperature[][] a)
    {
        if (a != null)
        {
            Temperature[][] res = new Temperature[a.Length][];

            for (int i = 0; i < a.Length; i++)
            {
                res[i] = new Temperature[a[i].Length];

                for (int n = 0; n < a[i].Length; n++)
                {
                    res[i][n] = a[i][n].Celsius;
                }
            }
            return res;
        }
        return null;
    }

    public static Temperature[] DegCtoDegK(this Temperature[] a)
    {
        if (a != null)
        {
            Temperature[] res = new Temperature[a.Length];

            for (int n = 0; n < a.Length; n++)
            {
                res[n] = a[n];
            }
            return res;
        }
        return null;
    }

    public static double[][] DegKtoDoublr(this Temperature[][] a)
    {
        if (a != null)
        {
            double[][] res = new double[a.Length][];

            for (int i = 0; i < a.Length; i++)
            {
                res[i] = new double[a[i].Length];

                for (int n = 0; n < a[i].Length; n++)
                {
                    res[i][n] = a[i][n];
                }
            }
            return res;
        }
        return null;
    }

    public static Temperature[][] DegCtoDegK(this Temperature[][] a)
    {
        if (a != null)
        {
            Temperature[][] res = new Temperature[a.Length][];

            for (int i = 0; i < a.Length; i++)
            {
                res[i] = new Temperature[a[i].Length];

                for (int n = 0; n < a[i].Length; n++)
                {
                    res[i][n] = a[i][n];
                }
            }
            return res;
        }
        else
            return null;
    }

    public static double[][] DegCtoDouble(this Temperature[][] a)
    {
        double[][] res = new double[a.Length][];

        for (int i = 0; i < a.Length; i++)
        {
            res[i] = new double[a[i].Length];

            for (int n = 0; n < a[i].Length; n++)
            {
                res[i][n] = a[i][n];
            }
        }
        return res;
    }
}