namespace   ModelEngine
{
    partial class  Equilibrium
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
            this.button1 = new  System.Windows.Forms.Button();
            this.dataGridView1 = new  System.Windows.Forms.DataGridView();
            this.Colu = new  System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column1 = new  System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new  System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new  System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new  System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new  System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column6 = new  System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn17 = new  System.Windows.Forms.DataGridViewTextBoxColumn();
            this.txtStartT = new  System.Windows.Forms.TextBox();
            this.txtEndT = new  System.Windows.Forms.TextBox();
            this.button2 = new  System.Windows.Forms.Button();
            this.button3 = new  System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new  System.Drawing.Point (45, 31);
            this.button1.Name = "button1";
            this.button1.Size = new  System.Drawing.Size(91, 58);
            this.button1.TabIndex = 0;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new  System.EventHandler(this.button1_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new  System.Windows.Forms.DataGridViewColumn[] {
            this.Colu,
            this.Column1,
            this.Column2,
            this.Column3,
            this.Column4,
            this.Column5,
            this.Column6});
            this.dataGridView1.Location = new  System.Drawing.Point (172, 31);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new  System.Drawing.Size(837, 350);
            this.dataGridView1.TabIndex = 1;
            // 
            // Colu
            // 
            this.Colu.HeaderText = "Column1";
            this.Colu.Name = "Colu";
            // 
            // Column1
            // 
            this.Column1.HeaderText = "Column1";
            this.Column1.Name = "Column1";
            // 
            // Column2
            // 
            this.Column2.HeaderText = "Column2";
            this.Column2.Name = "Column2";
            // 
            // Column3
            // 
            this.Column3.HeaderText = "Column3";
            this.Column3.Name = "Column3";
            // 
            // Column4
            // 
            this.Column4.HeaderText = "Column4";
            this.Column4.Name = "Column4";
            // 
            // Column5
            // 
            this.Column5.HeaderText = "Column5";
            this.Column5.Name = "Column5";
            // 
            // Column6
            // 
            this.Column6.HeaderText = "Column6";
            this.Column6.Name = "Column6";
            // 
            // dataGridViewTextBoxColumn17
            // 
            this.dataGridViewTextBoxColumn17.HeaderText = "Column1";
            this.dataGridViewTextBoxColumn17.Name = "dataGridViewTextBoxColumn17";
            // 
            // txtStartT
            // 
            this.txtStartT.Location = new  System.Drawing.Point (34, 127);
            this.txtStartT.Name = "txtStartT";
            this.txtStartT.Size = new  System.Drawing.Size(100, 20);
            this.txtStartT.TabIndex = 2;
            this.txtStartT.Text = "0";
            // 
            // txtEndT
            // 
            this.txtEndT.Location = new  System.Drawing.Point (36, 164);
            this.txtEndT.Name = "txtEndT";
            this.txtEndT.Size = new  System.Drawing.Size(100, 20);
            this.txtEndT.TabIndex = 3;
            this.txtEndT.Text = "100";
            // 
            // button2
            // 
            this.button2.Location = new  System.Drawing.Point (45, 252);
            this.button2.Name = "button2";
            this.button2.Size = new  System.Drawing.Size(91, 58);
            this.button2.TabIndex = 4;
            this.button2.Text = "button2";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new  System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new  System.Drawing.Point (43, 323);
            this.button3.Name = "button3";
            this.button3.Size = new  System.Drawing.Size(91, 58);
            this.button3.TabIndex = 5;
            this.button3.Text = "button3";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new  System.EventHandler(this.button3_Click);
            // 
            // Equilibrium
            // 
            this.ClientSize = new  System.Drawing.Size(1068, 444);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.txtEndT);
            this.Controls.Add(this.txtStartT);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.button1);
            this.Name = "Equilibrium";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private  System.Windows.Forms.DataGridView dataGridView1;
        private  System.Windows.Forms.Button button1;
        private  System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn17;
        private  System.Windows.Forms.DataGridViewTextBoxColumn Colu;
        private  System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private  System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private  System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private  System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private  System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        private  System.Windows.Forms.TextBox txtStartT;
        private  System.Windows.Forms.TextBox txtEndT;
        private  System.Windows.Forms.DataGridViewTextBoxColumn Column6;
        private  System.Windows.Forms.Button button2;
        private  System.Windows.Forms.Button button3;
    }
}

