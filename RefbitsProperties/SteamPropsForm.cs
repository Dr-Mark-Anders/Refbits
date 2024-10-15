using ModelEngine;
using ModelEngine;
using Steam97;
using Units;
using Units.UOM;

namespace RefbitsProperties
{
    public partial class SteamPropsForm : Form
    {
        private WaterSteam bc = new WaterSteam(Thermodata.GetComponent("H2O"));
        private Port_Material water = new Port_Material();

        public SteamPropsForm()
        {
            InitializeComponent();
        }

        private void SteamPropsForm_Load(object sender, EventArgs e)
        {
            UOMDisplayList units = new UOMDisplayList();
            Pressure p = new Pressure(3, PressureUnit.MPa);
            water.MF_ = new StreamProperty(ePropID.MF, 1, SourceEnum.Input);
            water.P_ = new StreamProperty(ePropID.P, new Pressure(1, PressureUnit.atma), SourceEnum.Input);
            water.T_ = new StreamProperty(ePropID.T, new Temperature(60, TemperatureUnit.Fahrenheit), SourceEnum.Input);
            bc.MoleFraction = 1;
            water.cc.Add(bc);
            water.cc.Origin = SourceEnum.Input;
            water.Flash(forceflash: false, calcderivatives: true);
            ThermoDynamicOptions thermo = new();
            water.Thermo.UseSteamTables = true;
            portProperty.PortsPropertyWorksheetInitialise(water, units);
        }

        private void steamPropsdlg1_SteamPropsChanged(object sender, EventArgs e)
        {
            recalculate();
        }

        private void recalculate()
        {
            if (rb1997.Checked == true)
            {
                water.Thermo.UseSteamTables = true;
                water.Thermo.SteamMethod = enumSteamMethod._1997;
                StmPropIAPWS97 steam = new();
            }
            else
            {
            }
        }

        private void steamPropsdlg1_Load(object sender, EventArgs e)
        {
        }

        private void portPropertyWorksheet1_Load(object sender, EventArgs e)
        {
        }
    }
}