using ModelEngine;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace DialogControls
{
    public partial class StreamCompositionEntry : UserControl
    {
        public List<string> ComponentNames = new List<string> { };

        private List<string> StreamNames = new List<string> { "Net H2", "FG", "LPG", "Reformate" };

        public StreamCompositionEntry()
        {
            InitializeComponent();
        }

        public void SetData(List<RefomerFullCompList> products)
        {
            for (int product = 0; product < products.Count; product++)
            {
                DGV[product, 0].Value = products[product].Name;

                for (int comp = 0; comp < products[product].Components.Count; comp++)
                {
                    DGV[product, comp].Value = products[product].Components[comp].MoleFraction;
                }
            }
        }

        public void GetData(List<RefomerFullCompList> products)
        {
            for (int product = 0; product < products.Count; product++)
            {
                for (int comp = 0; comp < products[product].Components.Count; comp++)
                {
                    if (DGV.RowCount > comp)
                        if (double.TryParse(DGV[product, comp].Value.ToString(), out double res))
                            products[product].Components[comp].MoleFraction = res;
                }
            }

            for (int i = 0; i < products.Count; i++)
            {
                products[i].Normalise();
            }
        }

        private void UserControl1_Load(object sender, EventArgs e)
        {
            DGV.Columns.Add("CompName", "Name");
            DGV.Columns[0].Width = 200;
            int RowNo;

            for (int i = 0; i < ComponentNames.Count; i++)
            {
                RowNo = DGV.Rows.Add();
                DGV[0, RowNo].Value = ComponentNames[i];
            }

            for (int i = 0; i < StreamNames.Count; i++)
            {
                DGV.Columns.Add(StreamNames[i], StreamNames[i]);
            }
        }

        public void setColumns(List<string> streamnames)
        {
            DGV.Columns.Clear();

            StreamNames = streamnames;
            DGV.Columns.Add("CompName", "Name");
            DGV.Columns[0].Width = 200;

            int RowNo;
            for (int i = 0; i < ComponentNames.Count; i++)
            {
                RowNo = DGV.Rows.Add();
                DGV[0, RowNo].Value = ComponentNames[i];
            }

            for (int i = 0; i < StreamNames.Count; i++)
            {
                DGV.Columns.Add(StreamNames[i], StreamNames[i]);
            }

            for (int i = 0; i < ComponentNames.Count; i++)
            {
                RowNo = DGV.Rows.Add();
                DGV[0, RowNo].Value = ComponentNames[i];
            }
        }
    }
}