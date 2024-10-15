using ModelEngine;
using System;
using System.IO;

namespace RusselColumnTest
{
    public partial class RusselSolverTest
    {
    }

    public class WriteOutMatrices
    {
        public WriteOutMatrices()
        {
        }

        public void DumpData(BaseComp pc, MatrixAlgebra.Matrix mat, MatrixAlgebra.Matrix tdi, int loop, int NoTrays)
        {
            StreamWriter sw = new StreamWriter("ColumnDebug.txt", true);

            // Add some text to the file.
            sw.Write("This is the ");
            sw.WriteLine("header for the file.");
            sw.WriteLine("-------------------");
            // Arbitrary object s can also be written to the file.
            sw.Write("The date is: ");
            sw.WriteLine(DateTime.Now);
            sw.WriteLine();
            sw.WriteLine("Component " + pc.Name);
            sw.WriteLine("iter " + loop);
            sw.WriteLine();
            sw.WriteLine("MAT");
            for (int n = 0; n < NoTrays; n++)
            {
                for (int m = 0; m < NoTrays; m++)
                {
                    sw.Write(mat[n, m] + ",");
                }
                sw.WriteLine();
            }
            sw.WriteLine();
            sw.WriteLine("TDI");
            for (int n = 0; n < NoTrays; n++)
            {
                for (int m = 0; m < NoTrays; m++)
                {
                    sw.Write(tdi[n, m] + ",");
                }
                sw.WriteLine();
            }
            sw.WriteLine();

            sw.WriteLine();
            sw.Close();
        }

        public void DumpData1(double[,] X, int loop)
        {
            StreamWriter sw = new StreamWriter("ColumnDebug.txt", true);

            sw.WriteLine("X");
            for (int n = 0; n <= X.GetUpperBound(0); n++)
            {
                for (int m = 0; m <= X.GetUpperBound(1); m++)
                {
                    sw.Write(X[n, m] + ",");
                }
                sw.WriteLine();
            }
            sw.WriteLine();
            sw.Close();
        }

        public void DumpData2(double[,] Y, int loop)
        {
            StreamWriter sw = new StreamWriter("ColumnDebug.txt", true);

            sw.WriteLine("Y");
            for (int n = 0; n <= Y.GetUpperBound(0); n++)
            {
                for (int m = 0; m <= Y.GetUpperBound(1); m++)
                {
                    sw.Write(Y[n, m] + ",");
                }
                sw.WriteLine();
            }
            sw.WriteLine();
            sw.Close();
        }

        public void DumpData3(double[] Hbal, int loop)
        {
            StreamWriter sw = new StreamWriter("ColumnDebug.txt", true);

            sw.WriteLine("Hbal");

            for (int m = 0; m <= Hbal.GetUpperBound(0); m++)
            {
                sw.Write(Hbal[m] + ",");
            }

            sw.WriteLine();
            sw.WriteLine();
            sw.Close();
        }

        public void DumpData4(double[] T, int loop)
        {
            StreamWriter sw = new StreamWriter("ColumnDebug.txt", true);

            sw.WriteLine("T - tray");

            for (int m = 0; m <= T.GetUpperBound(0); m++)
            {
                sw.Write(T[m] + ",");
            }

            sw.WriteLine();
            sw.WriteLine();
            sw.Close();
        }

        public void DumpData5(double[] V, int loop)
        {
            StreamWriter sw = new StreamWriter("ColumnDebug.txt", true);

            sw.WriteLine("V - tray");

            for (int m = 0; m <= V.GetUpperBound(0); m++)
            {
                sw.Write(V[m] + ",");
            }

            sw.WriteLine();
            sw.WriteLine();
            sw.Close();
        }

        public void DumpData6(double[] L, int loop)
        {
            StreamWriter sw = new StreamWriter("ColumnDebug.txt", true);

            sw.WriteLine("L - tray");

            for (int m = 0; m <= L.GetUpperBound(0); m++)
            {
                sw.Write(L[m] + ",");
            }

            sw.WriteLine();
            sw.WriteLine();
            sw.Close();
        }

        public void DumpData7(double[] Lenth, int loop)
        {
            StreamWriter sw = new StreamWriter("ColumnDebug.txt", true);

            sw.WriteLine("L Enth - tray");

            for (int m = 0; m <= Lenth.GetUpperBound(0); m++)
            {
                sw.Write(Lenth[m] + ",");
            }

            sw.WriteLine();
            sw.WriteLine();
            sw.Close();
        }

        public void DumpData8(double[] Venth, int loop)
        {
            StreamWriter sw = new StreamWriter("ColumnDebug.txt", true);

            sw.WriteLine("V Enth - tray");

            for (int m = 0; m <= Venth.GetUpperBound(0); m++)
            {
                sw.Write(Venth[m] + ",");
            }
            sw.WriteLine();
            sw.WriteLine();
            sw.Close();
        }
    }

    public class IterationEventArgsTest : EventArgs
    {
        public string data;

        public IterationEventArgsTest(string newline)
        {
            data = newline;
        }
    }

    public delegate void IterationEventHAndlerTest(object sender, IterationEventArgsTest data);
}