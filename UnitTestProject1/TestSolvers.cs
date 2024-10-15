using Extensions;
using Math2;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.Optimization;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
    [TestClass]
    public class TestSolvers
    {
        public double Eq(double x, double y)
        {
            return (x + 3) * (x - 1).Pow(2) - y;
        }


        [TestMethod]
        public void TestBrent()
        {
            var res = BrentSolver.Solve(-4, 4D / 3D, 0, Eq);
        }


        [TestMethod]
        public void TestBFGS()
        {
            double TestFunc(Vector<double> aa)
            {
                return aa[0].Pow(2) + 4 + aa[1].Pow(2) * 3;
            }

            Vector<double> TestFuncGrad(Vector<double> aa)
            {
                double[] resvec = new double[2];

                var bb = aa.Clone();
                bb[0] += 0.00001;
                resvec[0] = (TestFunc(bb) - TestFunc(aa)) / 0.00001;

                bb = aa.Clone();
                bb[1] += 0.00001;
                resvec[1] = (TestFunc(bb) - TestFunc(aa)) / 0.00001;

                var vec = Vector<double>.Build.DenseOfArray(resvec);

                return vec;
            }

            double[,] test = new double[2, 2] { { 0, 0 }, { 0, 0 } };
            var a = Matrix<double>.Build.DenseOfArray(test);

            MathNet.Numerics.LinearAlgebra.Double.Vector guess = (MathNet.Numerics.LinearAlgebra.Double.Vector)Vector<double>.Build.DenseOfArray(new double[2] { 100, 100 });

            var res = BfgsSolver.Solve(guess, TestFunc, TestFuncGrad);

            var min = TestFunc(res);
        }


        [TestMethod]
        public void TestShellandTube()
        {
            //ExchangerProps mech = new ExchangerProps(500, new CM(2.64), new CM(0.001), new Metres(0.0254), new Metres(4), new Metres(0.6096), new Metres(0.033), TubePattern.Triangular, new Metres(0.335));
            //ExTubeShellProps tubeside = new ExTubeShellProps(new BasicProperty(20,PropertyEnum.Temperature), new BasicProperty(30, PropertyEnum.Temperature), new BasicProperty(5, PropertyEnum.Pressure), new BasicProperty(5, PropertyEnum.Pressure), 0.158, 850, 0.0376, 0.0004, new kg_hr(20), 0, 4);
            //ExTubeShellProps shellside = new ExTubeShellProps(new BasicProperty(82, PropertyEnum.Temperature), new BasicProperty( 80, PropertyEnum.Temperature), new BasicProperty(5, PropertyEnum.Pressure), new BasicProperty(5, PropertyEnum.Pressure), 0.158, 850, 0.0376, 0.0004, new kg_hr(200), 0, 2);

            //Exchanger exchanger = new Exchanger(null, null, null, null, tubeside, shellside, mech, HeExCalcOptions.TwoFixInletsFixedDesign, Guid.Empty);

            //var res = exchanger.OverallU();
        }
    }
}
