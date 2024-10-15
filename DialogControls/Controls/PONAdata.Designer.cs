
using   System.Windows.Forms;

namespace   DialogControls
{
    partial class  PONAdata
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private  void   InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new  System.ComponentModel.ComponentResourceManager(typeof(PONAdata));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new  System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new  System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new  System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new  System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new  System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new  System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new  System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new  System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new  System.Windows.Forms.DataGridViewCellStyle();
            this.gb1 = new  System.Windows.Forms.GroupBox();
            this.gcC8A = new  FormControls.GCCompGrid();
            this.PCTType = new  DialogControls.MassMolarVol();
            this.DGV = new  System.Windows.Forms.DataGridView();
            this.CName = new  System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Paraffins = new  System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IParaffins = new  System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Olefins = new  System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CycloPentanes = new  System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CycloHexanes = new  System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Aromatics = new  System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gb1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DGV)).BeginInit();
            this.SuspendLayout();
            // 
            // gb1
            // 
            this.gb1.Controls.Add(this.gcC8A);
            this.gb1.Controls.Add(this.PCTType);
            this.gb1.Controls.Add(this.DGV);
            this.gb1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gb1.Location = new  System.Drawing.Point (0, 0);
            this.gb1.Margin = new  System.Windows.Forms.Padding(4, 3, 4, 3);
            this.gb1.Name = "gb1";
            this.gb1.Padding = new  System.Windows.Forms.Padding(4, 3, 4, 3);
            this.gb1.Size = new  System.Drawing.Size(745, 384);
            this.gb1.TabIndex = 4;
            this.gb1.TabStop = false;
            this.gb1.Text = "Flow Type";
            this.gb1.Resize += new  System.EventHandler(this.PONAdata_Resize);
            // 
            // gcC8A
            // 
            this.gcC8A.AllowUserToAddRows = false;
            this.gcC8A.AllowUserToDeleteRows = false;
            this.gcC8A.ColumnNames = ((System.Collections.Generic.List<string>)(resources.GetObject ("gcC8A.ColumnNames")));
            this.gcC8A.DisplayTitles = true;
            this.gcC8A.FirstColumnWidth = 100;
            this.gcC8A.Location = new  System.Drawing.Point (8, 185);
            this.gcC8A.Margin = new  System.Windows.Forms.Padding(4, 3, 4, 3);
            this.gcC8A.Name = "gcC8A";
            this.gcC8A.RowHeadersVisible = false;
            this.gcC8A.RowNames = ((System.Collections.Generic.List<string>)(resources.GetObject ("gcC8A.RowNames")));
            this.gcC8A.Size = new  System.Drawing.Size(159, 193);
            this.gcC8A.TabIndex = 2;
            this.gcC8A.TopText = "C8 Aromatics";
            // 
            // PCTType
            // 
            this.PCTType.Location = new  System.Drawing.Point (8, 22);
            this.PCTType.Margin = new  System.Windows.Forms.Padding(4, 3, 4, 3);
            this.PCTType.Name = "PCTType";
            this.PCTType.Size = new  System.Drawing.Size(134, 114);
            this.PCTType.TabIndex = 1;
            this.PCTType.Value = enumMassMolarOrVol.Molar;
            // 
            // DGV
            // 
            this.DGV.AllowUserToAddRows = false;
            this.DGV.AllowUserToDeleteRows = false;
            this.DGV.AllowUserToResizeColumns = false;
            this.DGV.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new  System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point );
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.DGV.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.DGV.Columns.AddRange(new  System.Windows.Forms.DataGridViewColumn[] {
            this.CName,
            this.Paraffins,
            this.IParaffins,
            this.Olefins,
            this.CycloPentanes,
            this.CycloHexanes,
            this.Aromatics});
            this.DGV.Dock = System.Windows.Forms.DockStyle.Right;
            this.DGV.Location = new  System.Drawing.Point (180, 19);
            this.DGV.Margin = new  System.Windows.Forms.Padding(4, 3, 4, 3);
            this.DGV.Name = "DGV";
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle9.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle9.Font = new  System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point );
            dataGridViewCellStyle9.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle9.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle9.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.DGV.RowHeadersDefaultCellStyle = dataGridViewCellStyle9;
            this.DGV.RowHeadersVisible = false;
            this.DGV.Size = new  System.Drawing.Size(561, 362);
            this.DGV.TabIndex = 0;
            // 
            // CName
            // 
            this.CName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.Font = new  System.Drawing.Font("Segoe UI", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point );
            this.CName.DefaultCellStyle = dataGridViewCellStyle2;
            this.CName.HeaderText = "C. No.";
            this.CName.Name = "CName";
            this.CName.Width = 63;
            // 
            // Paraffins
            // 
            this.Paraffins.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.Font = new  System.Drawing.Font("Segoe UI", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point );
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.Paraffins.DefaultCellStyle = dataGridViewCellStyle3;
            this.Paraffins.HeaderText = "Paraffins";
            this.Paraffins.Name = "Paraffins";
            // 
            // IParaffins
            // 
            this.IParaffins.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle4.Font = new  System.Drawing.Font("Segoe UI", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point );
            this.IParaffins.DefaultCellStyle = dataGridViewCellStyle4;
            this.IParaffins.HeaderText = "Iso-Paraffins";
            this.IParaffins.Name = "IParaffins";
            // 
            // Olefins
            // 
            this.Olefins.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle5.Font = new  System.Drawing.Font("Segoe UI", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point );
            this.Olefins.DefaultCellStyle = dataGridViewCellStyle5;
            this.Olefins.HeaderText = "Olefins";
            this.Olefins.Name = "Olefins";
            // 
            // CycloPentanes
            // 
            this.CycloPentanes.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle6.Font = new  System.Drawing.Font("Segoe UI", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point );
            this.CycloPentanes.DefaultCellStyle = dataGridViewCellStyle6;
            this.CycloPentanes.HeaderText = "Cyclo-Pentanes";
            this.CycloPentanes.Name = "CycloPentanes";
            // 
            // CycloHexanes
            // 
            this.CycloHexanes.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle7.Font = new  System.Drawing.Font("Segoe UI", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point );
            this.CycloHexanes.DefaultCellStyle = dataGridViewCellStyle7;
            this.CycloHexanes.HeaderText = "Cyclo-Hexanes";
            this.CycloHexanes.Name = "CycloHexanes";
            // 
            // Aromatics
            // 
            this.Aromatics.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle8.Font = new  System.Drawing.Font("Segoe UI", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point );
            this.Aromatics.DefaultCellStyle = dataGridViewCellStyle8;
            this.Aromatics.HeaderText = "Aromatics";
            this.Aromatics.Name = "Aromatics";
            // 
            // PONAdata
            // 
            this.AutoScaleDimensions = new  System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gb1);
            this.Margin = new  System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "PONAdata";
            this.Size = new  System.Drawing.Size(745, 384);
            this.MouseClick += new  System.Windows.Forms.MouseEventHandler(this.MassMolarVol_MouseClick);
            this.Resize += new  System.EventHandler(this.PONAdata_Resize);
            this.gb1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.DGV)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private  DataGridView DGV;
        private  System.Windows.Forms.GroupBox gb1;
        private  MassMolarVol PCTType;
        private  FormControls.GCCompGrid gcC8A;
        private  DataGridViewTextBoxColumn CName;
        private  DataGridViewTextBoxColumn Paraffins;
        private  DataGridViewTextBoxColumn IParaffins;
        private  DataGridViewTextBoxColumn Olefins;
        private  DataGridViewTextBoxColumn CycloPentanes;
        private  DataGridViewTextBoxColumn CycloHexanes;
        private  DataGridViewTextBoxColumn Aromatics;
    }
}
