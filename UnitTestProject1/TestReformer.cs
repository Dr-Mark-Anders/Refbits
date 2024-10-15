using EngineThermo;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NaphthaReformerSI;
using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace UnitTests
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class TestReformer
    {
        public TestReformer()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void TestReformer1()
        {
            //NapReformer NapRef = new NapReformer();
            NapReformerSI NapRef= new NapReformerSI();
            AssayBasis assayBasis = AssayBasis.Volume;
            UOMProperty Psep;
            UOMProperty Tsep;

            Psep = new UOMProperty(new Pressure(2.5, PressureUnit.Kg_cm2_g), SourceEnum.Input, "Separator Pressure");
            Tsep = new UOMProperty(new Temperature(38, TemperatureUnit.Celsius), SourceEnum.Input, "Separator Temperature");

            var watch = Stopwatch.StartNew();

            DistPoints Feed = new DistPoints();
            Tuple<double, double, double, double> PNAO;
            double[] FeedComponents = new double[31];
            Pressure[] RxP = new Pressure[4];
            RxP[0].kg_cm2_g = 4.71;
            RxP[1].kg_cm2_g = 4.21;
            RxP[2].kg_cm2_g = 3.71;
            RxP[3].kg_cm2_g = double.NaN;

            DeltaPressure[] RxDp = new DeltaPressure[4];
            RxDp[0].kg_cm2 = 0.5;
            RxDp[1].kg_cm2 = 0.5;
            RxDp[2].kg_cm2 = 0.5;
            RxDp[3].kg_cm2 = double.NaN;

            UOMProperty[] RxT = new UOMProperty[4];
            RxT[0] = new UOMProperty(new Temperature(510, TemperatureUnit.Celsius));
            RxT[1] = new UOMProperty(new Temperature(510, TemperatureUnit.Celsius));
            RxT[2] = new UOMProperty(new Temperature(510, TemperatureUnit.Celsius));
            RxT[3] = new UOMProperty(new Temperature(double.NaN));

            //Feed.Add(new DistPoint(0, new Temperature(0, TemperatureUnit.Celsius))); // dummy as reformer arrays have base 1
            Feed.Add(new DistPoint(1, new Temperature(102, TemperatureUnit.Celsius)));
            Feed.Add(new DistPoint(10, new Temperature(114, TemperatureUnit.Celsius)));
            Feed.Add(new DistPoint(30, new Temperature(double.NaN, TemperatureUnit.Celsius)));
            Feed.Add(new DistPoint(50, new Temperature(132, TemperatureUnit.Celsius)));
            Feed.Add(new DistPoint(70, new Temperature(double.NaN, TemperatureUnit.Celsius)));
            Feed.Add(new DistPoint(90, new Temperature(160, TemperatureUnit.Celsius)));
            Feed.Add(new DistPoint(99, new Temperature(178, TemperatureUnit.Celsius)));

            PNAO = Tuple.Create<double, double, double, double>(60.87, 28.34, 10.79, 0.0);

            Density density = 0.747 * 1000;
            MassFlow flow = new MassFlow();
            flow.te_d = 1675.6242;

            NapRef.ReadInput(flow, RxT, FeedComponents, assayBasis, Feed, density, PNAO, (Pressure)Psep.UOM,
                (Temperature)Tsep.UOM, new double[] { 20, 30, 50 }, RxP, RxDp, 1.95, 3, 2.2, InputOption.Short);

            NapRef.SolveCase();

            watch.Stop();

            
           // MessageBox.Show(watch.Elapsed.ToString()+" S");
        }
    }
}
