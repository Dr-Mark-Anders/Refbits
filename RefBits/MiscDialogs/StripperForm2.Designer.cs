using   System.Windows.Forms;

namespace   Units.Dialogs
{
    partial class  StripperFrm2
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
            DataGridViewCellStyle dataGridViewCellStyle1 = new  DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new  DataGridViewCellStyle();
            this.DeletePA = new  Button();
            this.AddPA = new  Button();
            this.dataGridView1 = new  DataGridView();
            this.Active = new  DataGridViewCheckBoxColumn();
            this.Spec1 = new  DataGridViewComboBoxColumn();
            this.Spec2 = new  DataGridViewComboBoxColumn();
            this.dataGridViewTextBoxColumn1 = new  DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new  DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new  DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new  DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new  DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn6 = new  DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn7 = new  DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn8 = new  DataGridViewTextBoxColumn();
            this.Names = new  DataGridViewTextBoxColumn();
            this.DrawTray = new  DataGridViewTextBoxColumn();
            this.returnTray = new  DataGridViewTextBoxColumn();
            this.Column1 = new  DataGridViewTextBoxColumn();
            this.Column8 = new  DataGridViewCheckBoxColumn();
            this.NoTrays = new  DataGridViewTextBoxColumn();
            this.Column2 = new  DataGridViewTextBoxColumn();
            this.Column3 = new  DataGridViewTextBoxColumn();
            this.Reboiled = new  DataGridViewComboBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // DeletePA
            // 
            this.DeletePA.Location = new  System.Drawing.Point (109, 341);
            this.DeletePA.Margin = new  Padding(3, 2, 3, 2);
            this.DeletePA.Name = "DeletePA";
            this.DeletePA.Size = new  System.Drawing.Size(67, 23);
            this.DeletePA.TabIndex = 27;
            this.DeletePA.Text = "Delete Tray";
            this.DeletePA.UseVisualStyleBackColor = true;
            this.DeletePA.Click += new  System.EventHandler(this.DeleteSt_Click);
            // 
            // AddPA
            // 
            this.AddPA.Location = new  System.Drawing.Point (17, 341);
            this.AddPA.Margin = new  Padding(3, 2, 3, 2);
            this.AddPA.Name = "AddPA";
            this.AddPA.Size = new  System.Drawing.Size(75, 23);
            this.AddPA.TabIndex = 26;
            this.AddPA.Text = "Add";
            this.AddPA.UseVisualStyleBackColor = true;
            this.AddPA.Click += new  System.EventHandler(this.AddSt_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new  System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point , ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            this.dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new  DataGridViewColumn[] {
            this.Column1,
            this.Column8,
            this.NoTrays,
            this.Column2,
            this.Column3,
            this.Reboiled});
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new  System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point , ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
            this.dataGridView1.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridView1.Location = new  System.Drawing.Point (12, 12);
            this.dataGridView1.Margin = new  Padding(3, 2, 3, 2);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.Size = new  System.Drawing.Size(765, 298);
            this.dataGridView1.TabIndex = 29;
            this.dataGridView1.CellContentClick += new  DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // Active
            // 
            this.Active.HeaderText = "Active";
            this.Active.Name = "Active";
            // 
            // Spec1
            // 
            this.Spec1.HeaderText = "Spec 1";
            this.Spec1.Name = "Spec1";
            // 
            // Spec2
            // 
            this.Spec2.HeaderText = "Spec 2";
            this.Spec2.Name = "Spec2";
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.HeaderText = "Column1";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.HeaderText = "Column2";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.HeaderText = "Column3";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.HeaderText = "Column4";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.HeaderText = "Column5";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.HeaderText = "Column6";
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            // 
            // dataGridViewTextBoxColumn7
            // 
            this.dataGridViewTextBoxColumn7.HeaderText = "Column7";
            this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            // 
            // dataGridViewTextBoxColumn8
            // 
            this.dataGridViewTextBoxColumn8.HeaderText = "Column8";
            this.dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
            // 
            // Names
            // 
            this.Names.HeaderText = "Name";
            this.Names.Name = "Names";
            // 
            // DrawTray
            // 
            this.DrawTray.HeaderText = "Draw Tray";
            this.DrawTray.Name = "DrawTray";
            // 
            // return  Tray
            // 
            this.returnTray.HeaderText = "return   Tray";
            this.returnTray.Name = "return  Tray";
            // 
            // Column1
            // 
            this.Column1.HeaderText = "Name";
            this.Column1.MinimumWidth = 50;
            this.Column1.Name = "Column1";
            this.Column1.SortMode = DataGridViewColumnSortMode.NotSortable;
            this.Column1.Width = 50;
            // 
            // Column8
            // 
            this.Column8.HeaderText = "Active";
            this.Column8.MinimumWidth = 50;
            this.Column8.Name = "Column8";
            this.Column8.Resizable = DataGridViewTriState.True;
            this.Column8.Width = 50;
            // 
            // NoTrays
            // 
            this.NoTrays.HeaderText = "NoTrays";
            this.NoTrays.Name = "NoTrays";
            this.NoTrays.Width = 72;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "DrawTray";
            this.Column2.Name = "Column2";
            this.Column2.SortMode = DataGridViewColumnSortMode.NotSortable;
            this.Column2.Width = 59;
            // 
            // Column3
            // 
            this.Column3.HeaderText = "return  Tray";
            this.Column3.Name = "Column3";
            this.Column3.SortMode = DataGridViewColumnSortMode.NotSortable;
            this.Column3.Width = 66;
            // 
            // Reboiled
            // 
            this.Reboiled.HeaderText = "Stripper Type";
            this.Reboiled.Name = "Reboiled";
            this.Reboiled.Resizable = DataGridViewTriState.True;
            this.Reboiled.SortMode = DataGridViewColumnSortMode.Automatic;
            this.Reboiled.Width = 95;
            // 
            // StripperFrm2
            // 
            this.AutoScaleDimensions = new  System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new  System.Drawing.Size(863, 391);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.DeletePA);
            this.Controls.Add(this.AddPA);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.Margin = new  Padding(3, 2, 3, 2);
            this.Name = "StripperFrm2";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Side Strippers";
            this.FormClosing += new  FormClosingEventHandler(this.StripperFrm_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private  Button DeletePA;
        private  Button AddPA;
        private  DataGridView dataGridView1;
        private  DataGridViewTextBoxColumn Names;
        private  DataGridViewCheckBoxColumn Active;
        private  DataGridViewTextBoxColumn DrawTray;
        private  DataGridViewTextBoxColumn returnTray;
        private  DataGridViewComboBoxColumn Spec1;
        private  DataGridViewComboBoxColumn Spec2;
        private  DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private  DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private  DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private  DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private  DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private  DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private  DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
        private  DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;
        private  DataGridViewTextBoxColumn Column1;
        private  DataGridViewCheckBoxColumn Column8;
        private  DataGridViewTextBoxColumn NoTrays;
        private  DataGridViewTextBoxColumn Column2;
        private  DataGridViewTextBoxColumn Column3;
        private  DataGridViewComboBoxColumn Reboiled;
    }
}