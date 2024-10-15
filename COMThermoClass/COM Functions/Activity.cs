using ModelEngine;
using ModelEngine.ThermodynamicMethods.Activity_Models;

namespace COMColumnNS
{
    public partial class COMThermo
    {
        public object UNIQUAC(object X)
        {
            double[] x = (double[])X;
            UNIQUAC uq = new();
            Components cc = new();

            BaseComp H2O = Thermodata.GetComponent("H2O");
            BaseComp MEK = Thermodata.GetRealComponentCAS("78-93-3");
            BaseComp PropionicAcid = Thermodata.GetRealComponentCAS("79-09-4");

            H2O.MoleFraction = 0.0999;
            MEK.MoleFraction = 0.899101;
            PropionicAcid.MoleFraction = 0.000999;

            cc.Add(H2O);
            cc.Add(MEK);
            cc.Add(PropionicAcid);

            cc.SetMolFractions(x);

            double[][] Params = new double[3][];

            Params[0] = new double[] { 1, 10.75, -142.30 };
            Params[1] = new double[] { 1187.00, 1, 579.08 };
            Params[2] = new double[] { 570.61, -327.84, 1 };

            double[] R = new double[] { 0.9200, 3.2479, 2.8768 };
            double[] Q = new double[] { 1.3997, 2.8759, 2.612 };

            uq.Init(cc, 273.15 + 25, Params, R, Q);

            cc.SetMolFractions(new double[] { 1, 0, 0 });
            var activity = uq.SolveGamma();

            return activity;
        }
    }
}