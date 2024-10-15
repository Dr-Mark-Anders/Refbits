namespace DialogControls
{
    partial class Distillations
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer Components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
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
        private void InitializeComponent()
        {
            pdg = new FormControls.UserPropGrid();
            distillationBasis1 = new DistillationBasis();
            pdgPNA = new FormControls.UserPropGrid();
            PropGridDensity = new FormControls.UserPropGrid();
            btnDelete = new System.Windows.Forms.Button();
            btnCreate = new System.Windows.Forms.Button();
            CBXTempUnits = new System.Windows.Forms.ComboBox();
            groupBox1 = new System.Windows.Forms.GroupBox();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // pdg
            // 
            pdg.AllowChangeEvent = true;
            pdg.AllowUserToAddRows = false;
            pdg.AllowUserToDeleteRows = false;
            pdg.BackColor = System.Drawing.SystemColors.Control;
            pdg.ColumnNames = null;
            pdg.DisplayTitles = true;
            pdg.FirstColumnWidth = 35;
            pdg.Location = new System.Drawing.Point(198, 91);
            pdg.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            pdg.Name = "pdg";
            pdg.ReadOnly = false;
            pdg.RowHeadersVisible = false;
            pdg.RowNames = null;
            pdg.Size = new System.Drawing.Size(170, 304);
            pdg.TabIndex = 1;
            pdg.TopText = "Feed Distillations";
            pdg.ValueChanged += pdg_ValueChanged;
            pdg.Load += pdg_Load;
            // 
            // distillationBasis1
            // 
            distillationBasis1.Location = new System.Drawing.Point(13, 25);
            distillationBasis1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            distillationBasis1.Name = "distillationBasis1";
            distillationBasis1.Size = new System.Drawing.Size(161, 149);
            distillationBasis1.TabIndex = 21;
            distillationBasis1.Value = enumDistType.D86;
            // 
            // pdgPNA
            // 
            pdgPNA.AllowChangeEvent = true;
            pdgPNA.AllowUserToAddRows = false;
            pdgPNA.AllowUserToDeleteRows = false;
            pdgPNA.BackColor = System.Drawing.SystemColors.Control;
            pdgPNA.ColumnNames = null;
            pdgPNA.DisplayTitles = true;
            pdgPNA.FirstColumnWidth = 50;
            pdgPNA.Location = new System.Drawing.Point(14, 265);
            pdgPNA.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            pdgPNA.Name = "pdgPNA";
            pdgPNA.ReadOnly = false;
            pdgPNA.RowHeadersVisible = false;
            pdgPNA.RowNames = null;
            pdgPNA.Size = new System.Drawing.Size(161, 159);
            pdgPNA.TabIndex = 1;
            pdgPNA.TopText = "PNAO";
            // 
            // PropGridDensity
            // 
            PropGridDensity.AllowChangeEvent = true;
            PropGridDensity.AllowUserToAddRows = false;
            PropGridDensity.AllowUserToDeleteRows = false;
            PropGridDensity.BackColor = System.Drawing.SystemColors.Control;
            PropGridDensity.ColumnNames = null;
            PropGridDensity.DisplayTitles = true;
            PropGridDensity.FirstColumnWidth = 50;
            PropGridDensity.Location = new System.Drawing.Point(13, 180);
            PropGridDensity.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            PropGridDensity.Name = "PropGridDensity";
            PropGridDensity.ReadOnly = false;
            PropGridDensity.RowHeadersVisible = false;
            PropGridDensity.RowNames = null;
            PropGridDensity.Size = new System.Drawing.Size(161, 79);
            PropGridDensity.TabIndex = 22;
            PropGridDensity.TopText = "Density";
            // 
            // btnDelete
            // 
            btnDelete.Location = new System.Drawing.Point(285, 401);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new System.Drawing.Size(75, 23);
            btnDelete.TabIndex = 23;
            btnDelete.Text = "Delete";
            btnDelete.UseVisualStyleBackColor = true;
            btnDelete.Click += btnDelete_Click;
            // 
            // btnCreate
            // 
            btnCreate.Location = new System.Drawing.Point(198, 401);
            btnCreate.Name = "btnCreate";
            btnCreate.Size = new System.Drawing.Size(75, 23);
            btnCreate.TabIndex = 24;
            btnCreate.Text = "Create";
            btnCreate.UseVisualStyleBackColor = true;
            btnCreate.Click += btnCreate_Click;
            // 
            // CBXTempUnits
            // 
            CBXTempUnits.FormattingEnabled = true;
            CBXTempUnits.Location = new System.Drawing.Point(11, 22);
            CBXTempUnits.Name = "CBXTempUnits";
            CBXTempUnits.Size = new System.Drawing.Size(121, 23);
            CBXTempUnits.TabIndex = 25;
            CBXTempUnits.SelectedIndexChanged += CBXTempUnits_SelectedIndexChanged;
            CBXTempUnits.SelectionChangeCommitted += CBXTempUnits_SelectionChangeCommitted;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(CBXTempUnits);
            groupBox1.Location = new System.Drawing.Point(198, 25);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new System.Drawing.Size(170, 60);
            groupBox1.TabIndex = 26;
            groupBox1.TabStop = false;
            groupBox1.Text = "Temperature Units";
            // 
            // Distillations
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(392, 448);
            Controls.Add(groupBox1);
            Controls.Add(btnCreate);
            Controls.Add(btnDelete);
            Controls.Add(PropGridDensity);
            Controls.Add(pdg);
            Controls.Add(pdgPNA);
            Controls.Add(distillationBasis1);
            Name = "Distillations";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "Distillation Data";
            TopMost = true;
            FormClosing += Distillations_FormClosing;
            groupBox1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
        private FormControls.UserPropGrid pdg;
        private DistillationBasis distillationBasis1;
        private FormControls.UserPropGrid pdgPNA;
        private FormControls.UserPropGrid PropGridDensity;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnCreate;
        private System.Windows.Forms.ComboBox CBXTempUnits;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}