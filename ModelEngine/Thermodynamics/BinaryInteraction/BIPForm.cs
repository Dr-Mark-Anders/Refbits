using System;
using System.Windows.Forms;
using Units.UOM;

namespace ModelEngine.BinaryInteraction
{
    public partial class BIPForm : Form
    {
        private Components cc;
        Port_Material port;

        public BIPForm(Port_Material port)
        {
            InitializeComponent();
            this.cc = port.cc;
            this.port = port;
            //BipMethod.SelectedIndex = (int)cc.ThermoBasis.BIPMethod;
        }

        private void BIPForm_Load(object sender, System.EventArgs e)
        {
            for (int x = 0; x < cc.ComponentList.Count; x++)
                DGV.Columns.Add(cc[x].name, cc[x].name);

            DGV.Rows.Add(cc.ComponentList.Count);

            double[,] Kij = InteractionParameters.Kij(cc,port.T);

            for (int x = 0; x < cc.ComponentList.Count; x++)
            {
                DGV.Rows[x].HeaderCell.Value = cc[x].Name;
                for (int y = 0; y < cc.ComponentList.Count; y++)
                    DGV[x, y].Value = Kij[x, y].ToString("0.####");
            }

            BipMethod.Items.AddRange(Enum.GetNames(typeof(enumBIPPredMethod)));
            BipMethod.SelectedIndex = (int)cc.Thermo.BIPMethod;
        }

        public void UpdateValues(Temperature T)
        {
            double[,] Kij = InteractionParameters.Kij(cc,T);

            for (int x = 0; x < cc.ComponentList.Count; x++)
            {
                for (int y = 0; y < cc.ComponentList.Count; y++)
                {
                    DGV[x, y].Value = Kij[x, y].ToString("0.####");
                }
            }
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            InteractionParameters.Update(cc, port.T, cc.Thermo.BIPMethod);
            UpdateValues(port.T);
            DGV.Refresh();
        }

        private void DGV_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            double[,] Kij = InteractionParameters.Kij(cc,port.T);

            string cellvalue = DGV[e.ColumnIndex, e.RowIndex].Value.ToString();
            if (double.TryParse(cellvalue, out double res))
            {
                Kij[e.RowIndex, e.ColumnIndex] = res;
                Kij[e.ColumnIndex, e.RowIndex] = res;
            }
        }

        private void BipMethod_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (BipMethod.SelectedValue != null && Enum.TryParse(BipMethod.SelectedValue.ToString(), out enumBIPPredMethod espec))
                cc.Thermo.BIPMethod = espec;
        }
    }
}