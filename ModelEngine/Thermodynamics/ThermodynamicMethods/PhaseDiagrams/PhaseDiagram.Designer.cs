namespace   ModelEngine
{
    partial class  PhaseDiagram
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private  System.ComponentModel.IContainer Components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected  override  void   Dispose(bool  disposing)
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
        private  void   InitializeComponent()
        {
            this.dataGridView1 = new  System.Windows.Forms.DataGridView();
            this.Component = new  System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MW = new  System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TCritK = new  System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Pcrit = new  System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Omega = new  System.Windows.Forms.DataGridViewTextBoxColumn();
            this.button1 = new  System.Windows.Forms.Button();
            this.dataGridView2 = new  System.Windows.Forms.DataGridView();
            this.Column1 = new  System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new  System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new  System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new  System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new  System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column6 = new  System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column7 = new  System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column8 = new  System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column9 = new  System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column10 = new  System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column11 = new  System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column12 = new  System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column13 = new  System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column14 = new  System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column15 = new  System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column16 = new  System.Windows.Forms.DataGridViewTextBoxColumn();
            this.textBoxTemperature  = new  System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new  System.Windows.Forms.DataGridViewColumn[] {
            this.Component,
            this.MW,
            this.TCritK,
            this.Pcrit,
            this.Omega});
            this.dataGridView1.Location = new  System.Drawing.Point (52, 39);
            this.dataGridView1.Margin = new  System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new  System.Drawing.Size(721, 186);
            this.dataGridView1.TabIndex = 1;
            // 
            // Component
            // 
            this.Component.HeaderText = "Component";
            this.Component.Name = "Component";
            this.Component.ReadOnly  = true;
            // 
            // MW
            // 
            this.MW.HeaderText = "MW";
            this.MW.Name = "MW";
            this.MW.ReadOnly  = true;
            // 
            // TCritK
            // 
            this.TCritK.HeaderText = "TCrit K";
            this.TCritK.Name = "TCritK";
            this.TCritK.ReadOnly  = true;
            // 
            // Pcrit
            // 
            this.Pcrit.HeaderText = "PCrit (MPa)";
            this.Pcrit.Name = "Pcrit";
            this.Pcrit.ReadOnly  = true;
            // 
            // Omega
            // 
            this.Omega.HeaderText = "Acentric Factor";
            this.Omega.Name = "Omega";
            this.Omega.ReadOnly  = true;
            // 
            // button1
            // 
            this.button1.Location = new  System.Drawing.Point (1448, 39);
            this.button1.Margin = new  System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button1.Name = "button1";
            this.button1.Size = new  System.Drawing.Size(196, 78);
            this.button1.TabIndex = 2;
            this.button1.Text = "Run";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new  System.EventHandler(this.button1_Click);
            // 
            // dataGridView2
            // 
            this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView2.Columns.AddRange(new  System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column3,
            this.Column4,
            this.Column5,
            this.Column6,
            this.Column7,
            this.Column8,
            this.Column9,
            this.Column10,
            this.Column11,
            this.Column12,
            this.Column13,
            this.Column14,
            this.Column15,
            this.Column16});
            this.dataGridView2.Location = new  System.Drawing.Point (34, 299);
            this.dataGridView2.Margin = new  System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dataGridView2.Name = "dataGridView2";
            this.dataGridView2.Size = new  System.Drawing.Size(1575, 494);
            this.dataGridView2.TabIndex = 3;
            // 
            // Column1
            // 
            this.Column1.HeaderText = "MPa";
            this.Column1.Name = "Column1";
            // 
            // Column2
            // 
            this.Column2.HeaderText = "y(1) [-]";
            this.Column2.Name = "Column2";
            // 
            // Column3
            // 
            this.Column3.HeaderText = "y(2) [-]";
            this.Column3.Name = "Column3";
            // 
            // Column4
            // 
            this.Column4.HeaderText = "fv(1) [Pa]";
            this.Column4.Name = "Column4";
            // 
            // Column5
            // 
            this.Column5.HeaderText = "fv(2) [Pa]";
            this.Column5.Name = "Column5";
            // 
            // Column6
            // 
            this.Column6.HeaderText = "rho  [kg/m3]";
            this.Column6.Name = "Column6";
            // 
            // Column7
            // 
            this.Column7.HeaderText = "x(1) [-]";
            this.Column7.Name = "Column7";
            // 
            // Column8
            // 
            this.Column8.HeaderText = "x(2) [-]";
            this.Column8.Name = "Column8";
            // 
            // Column9
            // 
            this.Column9.HeaderText = "fL(1) [Pa]";
            this.Column9.Name = "Column9";
            // 
            // Column10
            // 
            this.Column10.HeaderText = "fL(2) [Pa]";
            this.Column10.Name = "Column10";
            // 
            // Column11
            // 
            this.Column11.HeaderText = "rho  [kg/m3]";
            this.Column11.Name = "Column11";
            // 
            // Column12
            // 
            this.Column12.HeaderText = "K(1) [-]";
            this.Column12.Name = "Column12";
            // 
            // Column13
            // 
            this.Column13.HeaderText = "K(2) [-]";
            this.Column13.Name = "Column13";
            // 
            // Column14
            // 
            this.Column14.HeaderText = "K(2)/K(1) [-]";
            this.Column14.Name = "Column14";
            // 
            // Column15
            // 
            this.Column15.HeaderText = "N Iter";
            this.Column15.Name = "Column15";
            // 
            // Column16
            // 
            this.Column16.HeaderText = "Notes";
            this.Column16.Name = "Column16";
            // 
            // textBoxTemperature 
            // 
            this.textBoxTemperature .Location = new  System.Drawing.Point (1448, 124);
            this.textBoxTemperature .Margin = new  System.Windows.Forms.Padding(4, 4, 4, 4);
            this.textBoxTemperature .Name = "textBoxTemperature ";
            this.textBoxTemperature .Size = new  System.Drawing.Size(132, 22);
            this.textBoxTemperature .TabIndex = 4;
            this.textBoxTemperature .Text = "150";
            // 
            // PhaseDiagram
            // 
            this.AutoScaleDimensions = new  System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new  System.Drawing.Size(1677, 818);
            this.Controls.Add(this.textBoxTemperature );
            this.Controls.Add(this.dataGridView2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.dataGridView1);
            this.Margin = new  System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "PhaseDiagram";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private  System.Windows.Forms.DataGridView dataGridView1;
        private  System.Windows.Forms.DataGridViewTextBoxColumn Component;
        private  System.Windows.Forms.DataGridViewTextBoxColumn MW;
        private  System.Windows.Forms.DataGridViewTextBoxColumn TCritK;
        private  System.Windows.Forms.DataGridViewTextBoxColumn Pcrit;
        private  System.Windows.Forms.DataGridViewTextBoxColumn Omega;
        private  System.Windows.Forms.Button button1;
        private  System.Windows.Forms.DataGridView dataGridView2;
        private  System.Windows.Forms.TextBox textBoxTemperature ;
        private  System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private  System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private  System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private  System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private  System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        private  System.Windows.Forms.DataGridViewTextBoxColumn Column6;
        private  System.Windows.Forms.DataGridViewTextBoxColumn Column7;
        private  System.Windows.Forms.DataGridViewTextBoxColumn Column8;
        private  System.Windows.Forms.DataGridViewTextBoxColumn Column9;
        private  System.Windows.Forms.DataGridViewTextBoxColumn Column10;
        private  System.Windows.Forms.DataGridViewTextBoxColumn Column11;
        private  System.Windows.Forms.DataGridViewTextBoxColumn Column12;
        private  System.Windows.Forms.DataGridViewTextBoxColumn Column13;
        private  System.Windows.Forms.DataGridViewTextBoxColumn Column14;
        private  System.Windows.Forms.DataGridViewTextBoxColumn Column15;
        private  System.Windows.Forms.DataGridViewTextBoxColumn Column16;
    }
}

