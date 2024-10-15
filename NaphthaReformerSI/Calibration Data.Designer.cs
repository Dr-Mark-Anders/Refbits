namespace NaphthaReformerSI
{
    partial class Calibration_Data
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Calibration_Data));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabDesign = new System.Windows.Forms.TabPage();
            this.RxData = new FormControls.UserPropGrid();
            this.CatData = new FormControls.UserPropGrid();
            this.FurnEff = new FormControls.UserPropGrid();
            this.NoRx = new FormControls.UserPropGrid();
            this.tabProducts = new System.Windows.Forms.TabPage();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.shortMediumFull2 = new DialogControls.StreamAnalysysType();
            this.button5 = new System.Windows.Forms.Button();
            this.productNames = new FormControls.UserPropGrid();
            this.MassBalance = new FormControls.UserPropGrid();
            this.Products = new DialogControls.StreamCompositionEntry();
            this.tabOperating = new System.Windows.Forms.TabPage();
            this.CalibFactors = new FormControls.UserPropGrid();
            this.RxOPData = new FormControls.UserPropGrid();
            this.RecData = new FormControls.UserPropGrid();
            this.SepConditions = new FormControls.UserPropGrid();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.btnRemove = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.btnCopy = new System.Windows.Forms.Button();
            this.btnAddCase = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabDesign.SuspendLayout();
            this.tabProducts.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tabOperating.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabDesign);
            this.tabControl1.Controls.Add(this.tabProducts);
            this.tabControl1.Controls.Add(this.tabOperating);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Multiline = true;
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1212, 524);
            this.tabControl1.TabIndex = 51;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            this.tabControl1.TabIndexChanged += new System.EventHandler(this.tabControl1_TabIndexChanged);
            // 
            // tabDesign
            // 
            this.tabDesign.BackColor = System.Drawing.SystemColors.Control;
            this.tabDesign.Controls.Add(this.RxData);
            this.tabDesign.Controls.Add(this.CatData);
            this.tabDesign.Controls.Add(this.FurnEff);
            this.tabDesign.Controls.Add(this.NoRx);
            this.tabDesign.Location = new System.Drawing.Point(4, 24);
            this.tabDesign.Name = "tabDesign";
            this.tabDesign.Padding = new System.Windows.Forms.Padding(3);
            this.tabDesign.Size = new System.Drawing.Size(1204, 496);
            this.tabDesign.TabIndex = 0;
            this.tabDesign.Text = "Design Data";
            // 
            // RxData
            // 
            this.RxData.AllowUserToAddRows = false;
            this.RxData.AllowUserToDeleteRows = false;
            this.RxData.ColumnNames = ((System.Collections.Generic.List<string>)(resources.GetObject("RxData.ColumnNames")));
            this.RxData.DisplayTitles = true;
            this.RxData.FirstColumnWidth = 64;
            this.RxData.Location = new System.Drawing.Point(319, 139);
            this.RxData.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.RxData.Name = "RxData";
            this.RxData.RowHeadersVisible = false;
            this.RxData.RowNames = ((System.Collections.Generic.List<string>)(resources.GetObject("RxData.RowNames")));
            this.RxData.Size = new System.Drawing.Size(506, 176);
            this.RxData.TabIndex = 11;
            this.RxData.TopText = "Reactor Data";
            // 
            // CatData
            // 
            this.CatData.AllowUserToAddRows = false;
            this.CatData.AllowUserToDeleteRows = false;
            this.CatData.ColumnNames = ((System.Collections.Generic.List<string>)(resources.GetObject("CatData.ColumnNames")));
            this.CatData.DisplayTitles = true;
            this.CatData.FirstColumnWidth = 64;
            this.CatData.Location = new System.Drawing.Point(319, 321);
            this.CatData.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.CatData.Name = "CatData";
            this.CatData.RowHeadersVisible = false;
            this.CatData.RowNames = ((System.Collections.Generic.List<string>)(resources.GetObject("CatData.RowNames")));
            this.CatData.Size = new System.Drawing.Size(325, 116);
            this.CatData.TabIndex = 10;
            this.CatData.TopText = "Catalyst Data";
            // 
            // FurnEff
            // 
            this.FurnEff.AllowUserToAddRows = false;
            this.FurnEff.AllowUserToDeleteRows = false;
            this.FurnEff.ColumnNames = ((System.Collections.Generic.List<string>)(resources.GetObject("FurnEff.ColumnNames")));
            this.FurnEff.DisplayTitles = true;
            this.FurnEff.FirstColumnWidth = 64;
            this.FurnEff.Location = new System.Drawing.Point(571, 50);
            this.FurnEff.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.FurnEff.Name = "FurnEff";
            this.FurnEff.RowHeadersVisible = false;
            this.FurnEff.RowNames = ((System.Collections.Generic.List<string>)(resources.GetObject("FurnEff.RowNames")));
            this.FurnEff.Size = new System.Drawing.Size(165, 83);
            this.FurnEff.TabIndex = 9;
            this.FurnEff.TopText = "Furnace Efficiencies";
            // 
            // NoRx
            // 
            this.NoRx.AllowUserToAddRows = false;
            this.NoRx.AllowUserToDeleteRows = false;
            this.NoRx.ColumnNames = ((System.Collections.Generic.List<string>)(resources.GetObject("NoRx.ColumnNames")));
            this.NoRx.DisplayTitles = true;
            this.NoRx.FirstColumnWidth = 64;
            this.NoRx.Location = new System.Drawing.Point(314, 50);
            this.NoRx.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.NoRx.Name = "NoRx";
            this.NoRx.RowHeadersVisible = false;
            this.NoRx.RowNames = ((System.Collections.Generic.List<string>)(resources.GetObject("NoRx.RowNames")));
            this.NoRx.Size = new System.Drawing.Size(237, 83);
            this.NoRx.TabIndex = 8;
            this.NoRx.TopText = "Reactors";
            // 
            // tabProducts
            // 
            this.tabProducts.Controls.Add(this.splitContainer1);
            this.tabProducts.Location = new System.Drawing.Point(4, 24);
            this.tabProducts.Name = "tabProducts";
            this.tabProducts.Padding = new System.Windows.Forms.Padding(3);
            this.tabProducts.Size = new System.Drawing.Size(1204, 496);
            this.tabProducts.TabIndex = 1;
            this.tabProducts.Text = "Streams";
            this.tabProducts.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Cursor = System.Windows.Forms.Cursors.VSplit;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(3, 3);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainer1.Panel1.Controls.Add(this.shortMediumFull2);
            this.splitContainer1.Panel1.Controls.Add(this.button5);
            this.splitContainer1.Panel1.Controls.Add(this.productNames);
            this.splitContainer1.Panel1.Controls.Add(this.MassBalance);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainer1.Panel2.Controls.Add(this.Products);
            this.splitContainer1.Size = new System.Drawing.Size(1198, 490);
            this.splitContainer1.SplitterDistance = 650;
            this.splitContainer1.TabIndex = 69;
            // 
            // shortMediumFull2
            // 
            this.shortMediumFull2.Location = new System.Drawing.Point(90, 55);
            this.shortMediumFull2.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.shortMediumFull2.Name = "shortMediumFull2";
            this.shortMediumFull2.Size = new System.Drawing.Size(163, 104);
            this.shortMediumFull2.TabIndex = 68;
            this.shortMediumFull2.Value = enumShortMediumFull.Short;
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(90, 177);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(128, 33);
            this.button5.TabIndex = 67;
            this.button5.Text = "Feed Definition";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // productNames
            // 
            this.productNames.AllowUserToAddRows = false;
            this.productNames.AllowUserToDeleteRows = false;
            this.productNames.ColumnNames = ((System.Collections.Generic.List<string>)(resources.GetObject("productNames.ColumnNames")));
            this.productNames.DisplayTitles = true;
            this.productNames.FirstColumnWidth = 64;
            this.productNames.Location = new System.Drawing.Point(261, 55);
            this.productNames.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.productNames.Name = "productNames";
            this.productNames.RowHeadersVisible = false;
            this.productNames.RowNames = ((System.Collections.Generic.List<string>)(resources.GetObject("productNames.RowNames")));
            this.productNames.Size = new System.Drawing.Size(217, 216);
            this.productNames.TabIndex = 66;
            this.productNames.TopText = "Product Names";
            // 
            // MassBalance
            // 
            this.MassBalance.AllowUserToAddRows = false;
            this.MassBalance.AllowUserToDeleteRows = false;
            this.MassBalance.ColumnNames = ((System.Collections.Generic.List<string>)(resources.GetObject("MassBalance.ColumnNames")));
            this.MassBalance.DisplayTitles = true;
            this.MassBalance.FirstColumnWidth = 64;
            this.MassBalance.Location = new System.Drawing.Point(78, 277);
            this.MassBalance.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.MassBalance.Name = "MassBalance";
            this.MassBalance.RowHeadersVisible = false;
            this.MassBalance.RowNames = ((System.Collections.Generic.List<string>)(resources.GetObject("MassBalance.RowNames")));
            this.MassBalance.Size = new System.Drawing.Size(457, 140);
            this.MassBalance.TabIndex = 65;
            this.MassBalance.TopText = "Mass Balance";
            // 
            // Products
            // 
            this.Products.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Products.Location = new System.Drawing.Point(0, 0);
            this.Products.Name = "Products";
            this.Products.Size = new System.Drawing.Size(544, 490);
            this.Products.TabIndex = 0;
            // 
            // tabOperating
            // 
            this.tabOperating.BackColor = System.Drawing.SystemColors.Control;
            this.tabOperating.Controls.Add(this.CalibFactors);
            this.tabOperating.Controls.Add(this.RxOPData);
            this.tabOperating.Controls.Add(this.RecData);
            this.tabOperating.Controls.Add(this.SepConditions);
            this.tabOperating.Location = new System.Drawing.Point(4, 24);
            this.tabOperating.Name = "tabOperating";
            this.tabOperating.Size = new System.Drawing.Size(1204, 496);
            this.tabOperating.TabIndex = 3;
            this.tabOperating.Text = "Operating Conditions";
            // 
            // CalibFactors
            // 
            this.CalibFactors.AllowUserToAddRows = false;
            this.CalibFactors.AllowUserToDeleteRows = false;
            this.CalibFactors.ColumnNames = ((System.Collections.Generic.List<string>)(resources.GetObject("CalibFactors.ColumnNames")));
            this.CalibFactors.DisplayTitles = true;
            this.CalibFactors.FirstColumnWidth = 64;
            this.CalibFactors.Location = new System.Drawing.Point(735, 54);
            this.CalibFactors.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.CalibFactors.Name = "CalibFactors";
            this.CalibFactors.RowHeadersVisible = false;
            this.CalibFactors.RowNames = ((System.Collections.Generic.List<string>)(resources.GetObject("CalibFactors.RowNames")));
            this.CalibFactors.Size = new System.Drawing.Size(216, 419);
            this.CalibFactors.TabIndex = 14;
            this.CalibFactors.TopText = "Calibration Factors";
            // 
            // RxOPData
            // 
            this.RxOPData.AllowUserToAddRows = false;
            this.RxOPData.AllowUserToDeleteRows = false;
            this.RxOPData.ColumnNames = ((System.Collections.Generic.List<string>)(resources.GetObject("RxOPData.ColumnNames")));
            this.RxOPData.DisplayTitles = true;
            this.RxOPData.FirstColumnWidth = 64;
            this.RxOPData.Location = new System.Drawing.Point(194, 54);
            this.RxOPData.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.RxOPData.Name = "RxOPData";
            this.RxOPData.RowHeadersVisible = false;
            this.RxOPData.RowNames = ((System.Collections.Generic.List<string>)(resources.GetObject("RxOPData.RowNames")));
            this.RxOPData.Size = new System.Drawing.Size(506, 139);
            this.RxOPData.TabIndex = 12;
            this.RxOPData.TopText = "Reactor Data";
            // 
            // RecData
            // 
            this.RecData.AllowUserToAddRows = false;
            this.RecData.AllowUserToDeleteRows = false;
            this.RecData.ColumnNames = ((System.Collections.Generic.List<string>)(resources.GetObject("RecData.ColumnNames")));
            this.RecData.DisplayTitles = true;
            this.RecData.FirstColumnWidth = 64;
            this.RecData.Location = new System.Drawing.Point(194, 337);
            this.RecData.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.RecData.Name = "RecData";
            this.RecData.RowHeadersVisible = false;
            this.RecData.RowNames = ((System.Collections.Generic.List<string>)(resources.GetObject("RecData.RowNames")));
            this.RecData.Size = new System.Drawing.Size(282, 136);
            this.RecData.TabIndex = 3;
            this.RecData.TopText = "Recycle Data";
            // 
            // SepConditions
            // 
            this.SepConditions.AllowUserToAddRows = false;
            this.SepConditions.AllowUserToDeleteRows = false;
            this.SepConditions.ColumnNames = ((System.Collections.Generic.List<string>)(resources.GetObject("SepConditions.ColumnNames")));
            this.SepConditions.DisplayTitles = true;
            this.SepConditions.FirstColumnWidth = 64;
            this.SepConditions.Location = new System.Drawing.Point(194, 199);
            this.SepConditions.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.SepConditions.Name = "SepConditions";
            this.SepConditions.RowHeadersVisible = false;
            this.SepConditions.RowNames = ((System.Collections.Generic.List<string>)(resources.GetObject("SepConditions.RowNames")));
            this.SepConditions.Size = new System.Drawing.Size(282, 136);
            this.SepConditions.TabIndex = 0;
            this.SepConditions.TopText = "SepConditions";
            // 
            // splitContainer2
            // 
            this.splitContainer2.Cursor = System.Windows.Forms.Cursors.HSplit;
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.tabControl1);
            this.splitContainer2.Panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.splitContainer2_Panel1_Paint);
            this.splitContainer2.Panel1MinSize = 100;
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.btnRemove);
            this.splitContainer2.Panel2.Controls.Add(this.button2);
            this.splitContainer2.Panel2.Controls.Add(this.button1);
            this.splitContainer2.Panel2.Controls.Add(this.btnCopy);
            this.splitContainer2.Panel2.Controls.Add(this.btnAddCase);
            this.splitContainer2.Size = new System.Drawing.Size(1212, 609);
            this.splitContainer2.SplitterDistance = 524;
            this.splitContainer2.TabIndex = 51;
            // 
            // btnRemove
            // 
            this.btnRemove.Location = new System.Drawing.Point(318, 21);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(87, 48);
            this.btnRemove.TabIndex = 62;
            this.btnRemove.Text = "Remove Case";
            this.btnRemove.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(616, 21);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(106, 48);
            this.button2.TabIndex = 65;
            this.button2.Text = "Load Calibration Data";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(504, 21);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(106, 48);
            this.button1.TabIndex = 64;
            this.button1.Text = "Save Calibration Data";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // btnCopy
            // 
            this.btnCopy.Location = new System.Drawing.Point(411, 21);
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.Size = new System.Drawing.Size(87, 48);
            this.btnCopy.TabIndex = 63;
            this.btnCopy.Text = "Copy Case";
            this.btnCopy.UseVisualStyleBackColor = true;
            // 
            // btnAddCase
            // 
            this.btnAddCase.Location = new System.Drawing.Point(225, 21);
            this.btnAddCase.Name = "btnAddCase";
            this.btnAddCase.Size = new System.Drawing.Size(87, 48);
            this.btnAddCase.TabIndex = 61;
            this.btnAddCase.Text = "Add Case";
            this.btnAddCase.UseVisualStyleBackColor = true;
            // 
            // Calibration_Data
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1212, 609);
            this.Controls.Add(this.splitContainer2);
            this.Name = "Calibration_Data";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Calibration_Data";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Calibration_Data_FormClosing);
            this.Load += new System.EventHandler(this.Calibration_Data_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabDesign.ResumeLayout(false);
            this.tabProducts.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tabOperating.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabDesign;
        private DialogControls.DropDownDataEntry dropDownDataEntry1;
        private System.Windows.Forms.TabPage tabProducts;
        private System.Windows.Forms.TabPage tabOperating;
        private FormControls.UserPropGrid SepConditions;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btnCopy;
        private System.Windows.Forms.Button btnAddCase;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private FormControls.UserPropGrid RecData;
        private DialogControls.StreamCompositionEntry Products;
        private FormControls.UserPropGrid CatData;
        private FormControls.UserPropGrid FurnEff;
        private FormControls.UserPropGrid NoRx;
        private FormControls.UserPropGrid CalibFactors;
        private FormControls.UserPropGrid RxData;
        private FormControls.UserPropGrid RxOPData;
        private DialogControls.StreamAnalysysType shortMediumFull2;
        private System.Windows.Forms.Button button5;
        private FormControls.UserPropGrid productNames;
        private FormControls.UserPropGrid MassBalance;
    }
}