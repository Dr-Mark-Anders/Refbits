namespace Extensions
{
    public static partial class Ext
    {
        public static double[] MassToMole(this double[] mass, double[] MW)
        {
            double sum = 0;
            double[] Mole = new double[mass.Length];

            for (int i = 0; i < mass.Length; i++)
            {
                Mole[i] = mass[i] / MW[i];
                sum += Mole[i];
            }

            for (int i = 0; i < Mole.Length; i++)
                Mole[i] = Mole[i] / sum;

            return Mole;
        }

        public static double[] MoleToMass(this double[] Mole, double[] MW)
        {
            double sum = 0;
            double[] Mass = new double[Mole.Length];

            for (int i = 0; i < Mole.Length; i++)
            {
                Mass[i] = Mole[i] * MW[i];
                sum += Mass[i];
            }

            for (int i = 0; i < Mole.Length; i++)
                Mass[i] = Mass[i] / sum;

            return Mass;
        }

        public static double[] VolumeToMass(this double[] vol, double[] Density)
        {
            double sum = 0;
            double[] mass = new double[vol.Length];

            for (int i = 0; i < vol.Length; i++)
            {
                mass[i] = vol[i] * Density[i];
                sum += mass[i];
            }

            for (int i = 0; i < vol.Length; i++)
                mass[i] = mass[i] / sum;

            return mass;
        }

        public static double[] MassToVolume(this double[] mass, double[] Density)
        {
            double sum = 0;
            double[] vol = new double[mass.Length];
            for (int i = 0; i < vol.Length; i++)
            {
                vol[i] = vol[i] / Density[i];
                sum += vol[i];
            }

            for (int i = 0; i < vol.Length; i++)
                vol[i] = vol[i] / sum;

            return vol;
        }
    }
}