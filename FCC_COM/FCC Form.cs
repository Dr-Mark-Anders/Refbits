using ModelEngine;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Threading;
using System.Windows.Forms.DataVisualization.Charting;
using Units;
using static alglib;
using Units.UOM;
using System.Text.RegularExpressions;

namespace FCC_COM
{
    public partial class Form1 : Form
    {
        Variables vars = new();
        public Form1()
        {
            InitializeComponent();
            LoadCtaFactors();
        }

        public void LoadCtaFactors()
        {
            CatFactors.Add(vars.HeatofCrackingParameter, "HeatofCrackingParameter");
            CatFactors.Add(vars.KineticCokeActivityFactor, "KineticCokeActivityFactor");
            CatFactors.Add(vars.ActivityonPathwaystoCL, "ActivityonPathwaystoCL");
            CatFactors.Add(vars.ActivityonPathwaystoGLump, "ActivityonPathwaystoGLump");
            CatFactors.Add(vars.ActivityonPathwaystoLL, "ActivityonPathwaystoLL");
            CatFactors.Add(vars.MetalsCokeActivity, "MetalsCokeActivity");
            CatFactors.Add(vars.CatalystDeactivationFactor, "CatalystDeactivationFactor");
            CatFactors.Add(vars.StripperParameter, "StripperParameter");
            CatFactors.Add(vars.EffluentperMassofCatalystintoStripper, "EffluentperMassofCatalystintoStripper");
            CatFactors.Add(vars.HtoCRatioforCoke, "HtoCRatioforCoke");
            CatFactors.Add(vars.PreexponentialFactorforGasolineCracking, "PreexponentialFactorforGasolineCracking");
            CatFactors.Add(vars.Ea_RforGasolineCracking, "Ea_RforGasolineCracking");
            CatFactors.Add(vars.FractionConcarbontoCoke, "FractionConcarbontoCoke");
            CatFactors.Add(vars.LightGasDelumpingtoEthane, "LightGasDelumpingtoEthane");
            CatFactors.Add(vars.LightGasDelumpingtoEthylene, "LightGasDelumpingtoEthylene");
            CatFactors.Add(vars.LightGasDelumpingtoPropane, "LightGasDelumpingtoPropane");
            CatFactors.Add(vars.LightGasDelumpingtoPropylene, "LightGasDelumpingtoPropylene");
            CatFactors.Add(vars.LightGasDelumpingtonButane, "LightGasDelumpingtonButane");
            CatFactors.Add(vars.LightGasDelumpingtoisoButane, "LightGasDelumpingtoisoButane");
            CatFactors.Add(vars.LightGasDelumpingtoButenes, "LightGasDelumpingtoButenes");
            CatFactors.Add(vars.LightGasDelumpingtonPentane, "LightGasDelumpingtonPentane");
            CatFactors.Add(vars.LightGasDelumpingtoPentenes, "LightGasDelumpingtoPentenes");
        }

        public void LoadData()
        {
            Dimensions.Add(vars.TotalLength);
            Dimensions.Add(vars.Diameter);
        }
    }
}