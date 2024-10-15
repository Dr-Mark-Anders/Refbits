using ModelEngine;
using System;
using System.Windows.Forms;

namespace Units
{
    public class DataGridViewSpecTypeColumn : DataGridViewComboBoxColumn
    {
        /*
         * DataGridViewComboBoxColumn col = new  DataGridViewComboBoxColumn();
        col.Name = "My Enum Column";
        col.DataSource = Enum.GetValues(typeof(MyEnum));
        col.ValueType = typeof(MyEnum);
        dataGridView1.Columns.Add(col);*/

        public DataGridViewSpecTypeColumn()
        {
            this.CellTemplate = new DataGridViewSpecTypeCell();
            this.ReadOnly = false;
            this.DataSource = Enum.GetNames(typeof(eSpecType));
            //this.ValueType = typeof(eSpecType);
        }

        /* DataGridViewComboBoxColumn CreateComboBoxWithEnums()
         {
             DataGridViewComboBoxColumn combo = new  DataGridViewComboBoxColumn();
             combo.DataSource = Enum.GetValues(typeof(Title));
             combo.DataPropertyName = "Title";
             combo.Name = "Title";
             return   combo;
         }*/
    }
}