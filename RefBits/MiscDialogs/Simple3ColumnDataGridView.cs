using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Units.MiscDialogs
{
    public partial class Simple3ColumnDatGridView : DataGridView
    {
        private Dictionary<int, List<double>> data;
        private Boolean dataloaded = false;

        public Simple3ColumnDatGridView()
        {
            InitializeComponent();
        }

        private void DatGridViewSimpleEntry_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            int column = e.ColumnIndex;
            if (dataloaded)
            {
                for (int i = 0; i < data.Count; i++)
                {
                    if (data.ContainsKey(i))
                    {
                        if (double.TryParse(this[column, i].Value.ToString(), out double res))
                            for (int n = 0; n < data[i].Count; n++)
                                data[i][n] = res;
                        else
                            this[column, i].Value = data[i][column].ToString();
                    }
                }
            }
        }

        public void LoadData(Dictionary<int, List<double>> data)
        {
            this.data = data;
            this.Rows.Add(data.Count);

            for (int i = 0; i < data.Count; i++)
            {
                //data[i].Item1=this[0, i].Value =;
                if (int.TryParse(this[1, i].Value.ToString(), out int res))
                    if (data.ContainsKey(i))
                        for (int n = 0; n < data[i].Count; n++)
                        {
                            this[i, n].Value = data[i][n];
                        }
            }
            dataloaded = true;
        }
    }
}