using Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Extensions
{
    public static partial class Ext
    {
        public static IList<T> Clone<T>(this IList<T> listToClone) where T : ICloneable
        {
            return listToClone.Select(item => (T)item.Clone()).ToList();
        }

        public static List<double> MassToMole(this List<double> mass, List<double> MW)
        {
            double sum = 0;
            List<double> Mole = new List<double>(new double[mass.Count]);

            for (int i = 0; i < mass.Count; i++)
            {
                Mole[i] = mass[i] / MW[i];
                sum += Mole[i];
            }

            for (int i = 0; i < Mole.Count; i++)
                Mole[i] = Mole[i] / sum;

            return Mole;
        }

        public static List<double> VolumeToMass(this List<double> vol, List<double> Density)
        {
            double sum = 0;
            List<double> mass = new List<double>(new double[vol.Count]);
            if (Density is null)
                return mass;

            for (int i = 0; i < vol.Count; i++)
            {
                mass[i] = vol[i] * Density[i];
                sum += mass[i];
            }

            for (int i = 0; i < vol.Count; i++)
                mass[i] = mass[i] / sum;

            return mass;
        }

        public static List<double> MassToVolume(this List<double> mass, List<double> Density)
        {
            double sum = 0;
            List<double> vol = new List<double>(new double[mass.Count]);
            for (int i = 0; i < vol.Count; i++)
            {
                vol[i] = vol[i] / Density[i];
                sum += vol[i];
            }

            for (int i = 0; i < vol.Count; i++)
                vol[i] = vol[i] / sum;

            return vol;
        }

        public static double Sqr(this double d)
        {
            return d * d;
        }

        public static double Sqrt(this double d)
        {
            return Math.Sqrt(d);
        }
    }
}