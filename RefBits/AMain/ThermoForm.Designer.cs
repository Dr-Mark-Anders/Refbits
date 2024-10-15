using   System.Windows.Forms;

namespace   Units
{
    partial class  ThermoForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private  System.ComponentModel.IContainer Components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void  Dispose(bool disposing)
        {
            if (disposing && (Components != null))
            {
                Components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private  void  InitializeComponent()
        {
            this.Components = new  System.ComponentModel.Container();
            this.dataGridView1 = new  DataGridView();
            this.thermoMethodsBindingSource = new  BindingSource(this.Components);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.thermoMethodsBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AutoGenerateColumns = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.DataSource = this.thermoMethodsBindingSource;
            this.dataGridView1.Location = new  System.Drawing.Point (104, 103);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.Size = new  System.Drawing.Size(451, 229);
            this.dataGridView1.TabIndex = 0;
            // 
            // thermoMethodsBindingSource
            // 
            this.thermoMethodsBindingSource.DataSource = typeof(ThermoDynamicOptions);
            // 
            // ThermoForm
            // 
            this.AutoScaleDimensions = new  System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new  System.Drawing.Size(759, 497);
            this.Controls.Add(this.dataGridView1);
            this.Name = "ThermoForm";
            this.Text = "ThermoForm";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.thermoMethodsBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private  DataGridView dataGridView1;
        private  BindingSource thermoMethodsBindingSource;

    }
}