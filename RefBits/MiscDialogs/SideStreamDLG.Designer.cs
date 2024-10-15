using   System.Windows.Forms;

namespace   Units
{
    partial class  SideStreamFrm
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
            this.bindingSource1 = new  BindingSource(this.Components);
            this.button1 = new  Button();
            this.button2 = new  Button();
            this.Column2 = new  DataGridViewCheckBoxColumn();
            this.Column1 = new  DataGridViewCheckBoxColumn();
            this.SideStream_Name = new  DataGridViewTextBoxColumn();
            this.DrawTray = new  DataGridViewTextBoxColumn();
            this.DrawRate = new  DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AutoGenerateColumns = false;
            this.dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new  DataGridViewColumn[] {
            this.Column1,
            this.SideStream_Name,
            this.DrawTray,
            this.DrawRate});
            this.dataGridView1.DataSource = this.bindingSource1;
            this.dataGridView1.Location = new  System.Drawing.Point (152, 38);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.Size = new  System.Drawing.Size(411, 226);
            this.dataGridView1.TabIndex = 45;
            this.dataGridView1.CellContentClick += new  DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // bindingSource1
            // 
            //this.bindingSource1.DataSource = typeof(Units.SideStream);
            // 
            // button1
            // 
            this.button1.Location = new  System.Drawing.Point (26, 70);
            this.button1.Name = "button1";
            this.button1.Size = new  System.Drawing.Size(90, 28);
            this.button1.TabIndex = 46;
            this.button1.Text = "Add Stripper";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new  System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new  System.Drawing.Point (26, 114);
            this.button2.Name = "button2";
            this.button2.Size = new  System.Drawing.Size(92, 28);
            this.button2.TabIndex = 47;
            this.button2.Text = "Remove Stripper";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new  System.EventHandler(this.button2_Click);
            // 
            // Column2
            // 
            this.Column2.DataPropertyName = "Active";
            this.Column2.HeaderText = "Active";
            this.Column2.Name = "Column2";
            this.Column2.Width = 92;
            // 
            // Column1
            // 
            this.Column1.DataPropertyName = "Active";
            this.Column1.HeaderText = "Active";
            this.Column1.Name = "Column1";
            // 
            // SideStream_Name
            // 
            this.SideStream_Name.DataPropertyName = "SideStream_Name";
            this.SideStream_Name.HeaderText = "SideStream_Name";
            this.SideStream_Name.Name = "SideStream_Name";
            // 
            // DrawTray
            // 
            this.DrawTray.DataPropertyName = "DrawTray";
            this.DrawTray.HeaderText = "DrawTray";
            this.DrawTray.Name = "DrawTray";
            // 
            // DrawRate
            // 
            this.DrawRate.DataPropertyName = "DrawRate";
            this.DrawRate.HeaderText = "DrawRate";
            this.DrawRate.Name = "DrawRate";
            // 
            // SideStreamFrm
            // 
            this.AutoScaleDimensions = new  System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new  System.Drawing.Size(830, 376);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.dataGridView1);
            this.Name = "SideStreamFrm";
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Side Strippers";
            this.FormClosing += new  FormClosingEventHandler(this.PumpAroundsFrm_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private  DataGridView dataGridView1;
        private  BindingSource bindingSource1;
        private  Button button1;
        private  Button button2;
        private  DataGridViewCheckBoxColumn Column2;
        private  DataGridViewCheckBoxColumn Column1;
        private  DataGridViewTextBoxColumn SideStream_Name;
        private  DataGridViewTextBoxColumn DrawTray;
        private  DataGridViewTextBoxColumn DrawRate;
    }
}