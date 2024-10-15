using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Units.MiscDialogs
{
    public partial class Simple2ColumnDatGridView : DataGridView
    {
        private Dictionary<string, int> data;
        private Boolean dataloaded = false;

        public Simple2ColumnDatGridView()
        {
            InitializeComponent();
        }

        private void DatGridViewSimpleEntry_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (dataloaded)
            {
                for (int i = 0; i < data.Count; i++)
                {
                    //data[i].Item1=this[0, i].Value =;
                    if (int.TryParse(this[1, i].Value.ToString(), out int res))
                        if (data.ContainsKey(this[0, i].Value.ToString()))
                            data[this[0, i].Value.ToString()] = res;
                        else
                            this[1, i].Value = data[this[0, i].Value.ToString()];
                }
            }
        }

        public void LoadData(Dictionary<string, int> data)
        {
            this.data = data;
            this.Rows.Add(data.Count);
            int Count = 0;

            foreach (KeyValuePair<string, int> item in data)
            {
                this[0, Count].Value = item.Key;
                this[1, Count].Value = item.Value;
                Count++;
            }
            dataloaded = true;
        }
    }
}