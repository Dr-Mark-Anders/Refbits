using System.Windows.Forms;

namespace Units
{
    partial class ComponenetSelection
    {
        ///<summary>
        ///Requireddesignervariable.
        ///</summary>
        private System.ComponentModel.IContainer Components = null;

        ///<summary>
        ///Cleanupanyresourcesbeingused.
        ///</summary>
        ///<paramname="disposing">trueifmanagedresourcesshouldbedisposed;otherwise,false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (Components != null))
            {
                Components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region ComponentDesignergeneratedcode

        ///<summary>
        ///RequiredmethodforDesignersupport-donotmodify
        ///thecontentsofthismethodwiththecodeeditor.
        ///</summary>
        private void InitializeComponent()
        {
            this.Composition = new System.Windows.Forms.TabControl();
            this.tabPageComponents = new System.Windows.Forms.TabPage();
            this.button5 = new System.Windows.Forms.Button();
            this.FilterBP = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button4 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.FilterMW = new System.Windows.Forms.TextBox();
            this.dgv2 = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FilterType = new System.Windows.Forms.ComboBox();
            this.FilterFormula = new System.Windows.Forms.TextBox();
            this.FilterName = new System.Windows.Forms.TextBox();
            this.dgv = new System.Windows.Forms.DataGridView();
            this.CAS = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Formula = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MW = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Bp = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.button2 = new System.Windows.Forms.Button();
            this.textBoxUBP = new System.Windows.Forms.TextBox();
            this.textBoxSG = new System.Windows.Forms.TextBox();
            this.textBoxLBP = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.FilterCAS = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.button6 = new System.Windows.Forms.Button();
            this.Composition.SuspendLayout();
            this.tabPageComponents.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).BeginInit();
            this.SuspendLayout();
            //
            //Composition
            //
            this.Composition.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
            this.Composition.Controls.Add(this.tabPageComponents);
            this.Composition.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Composition.Location = new System.Drawing.Point(0, 0);
            this.Composition.Margin = new System.Windows.Forms.Padding(2);
            this.Composition.Name = "Composition";
            this.Composition.SelectedIndex = 0;
            this.Composition.Size = new System.Drawing.Size(1118, 569);
            this.Composition.TabIndex = 4;
            //
            //tabPageComponents
            //
            this.tabPageComponents.Controls.Add(this.button6);
            this.tabPageComponents.Controls.Add(this.button5);
            this.tabPageComponents.Controls.Add(this.FilterBP);
            this.tabPageComponents.Controls.Add(this.label1);
            this.tabPageComponents.Controls.Add(this.button4);
            this.tabPageComponents.Controls.Add(this.button3);
            this.tabPageComponents.Controls.Add(this.FilterMW);
            this.tabPageComponents.Controls.Add(this.dgv2);
            this.tabPageComponents.Controls.Add(this.FilterType);
            this.tabPageComponents.Controls.Add(this.FilterFormula);
            this.tabPageComponents.Controls.Add(this.FilterName);
            this.tabPageComponents.Controls.Add(this.dgv);
            this.tabPageComponents.Controls.Add(this.button2);
            this.tabPageComponents.Controls.Add(this.textBoxUBP);
            this.tabPageComponents.Controls.Add(this.textBoxSG);
            this.tabPageComponents.Controls.Add(this.textBoxLBP);
            this.tabPageComponents.Controls.Add(this.button1);
            this.tabPageComponents.Controls.Add(this.label10);
            this.tabPageComponents.Controls.Add(this.label9);
            this.tabPageComponents.Controls.Add(this.FilterCAS);
            this.tabPageComponents.Controls.Add(this.label6);
            this.tabPageComponents.Location = new System.Drawing.Point(4, 27);
            this.tabPageComponents.Margin = new System.Windows.Forms.Padding(2);
            this.tabPageComponents.Name = "tabPageComponents";
            this.tabPageComponents.Size = new System.Drawing.Size(1110, 538);
            this.tabPageComponents.TabIndex = 2;
            this.tabPageComponents.Text = "Components";
            this.tabPageComponents.UseVisualStyleBackColor = true;
            //
            //button5
            //
            this.button5.Location = new System.Drawing.Point(940, 157);
            this.button5.Margin = new System.Windows.Forms.Padding(2);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(152, 35);
            this.button5.TabIndex = 25;
            this.button5.Text = "AddFullCrudeList";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.CreateAssay_Click);
            //
            //FilterBP
            //
            this.FilterBP.Location = new System.Drawing.Point(354, 38);
            this.FilterBP.Margin = new System.Windows.Forms.Padding(2);
            this.FilterBP.Name = "FilterBP";
            this.FilterBP.Size = new System.Drawing.Size(75, 23);
            this.FilterBP.TabIndex = 24;
            this.FilterBP.TextChanged += new System.EventHandler(this.FilterBP_TextChanged);
            //
            //label1
            //
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(446, 18);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 15);
            this.label1.TabIndex = 23;
            this.label1.Text = "SelectGroup";
            //
            //button4
            //
            this.button4.Location = new System.Drawing.Point(440, 264);
            this.button4.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(78, 27);
            this.button4.TabIndex = 22;
            this.button4.Text = "<--Remove";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.RemoveComponent_Click);
            //
            //button3
            //
            this.button3.Location = new System.Drawing.Point(440, 231);
            this.button3.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(78, 27);
            this.button3.TabIndex = 21;
            this.button3.Text = "Add-->";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.AddComponent_Click);
            //
            //FilterMW
            //
            this.FilterMW.Location = new System.Drawing.Point(273, 38);
            this.FilterMW.Margin = new System.Windows.Forms.Padding(2);
            this.FilterMW.Name = "FilterMW";
            this.FilterMW.Size = new System.Drawing.Size(75, 23);
            this.FilterMW.TabIndex = 20;
            this.FilterMW.TextChanged += new System.EventHandler(this.FilterMW_TextChanged);
            //
            //dgv2
            //
            this.dgv2.AllowDrop = true;
            this.dgv2.AllowUserToAddRows = false;
            this.dgv2.AllowUserToOrderColumns = true;
            this.dgv2.AllowUserToResizeColumns = false;
            this.dgv2.AllowUserToResizeRows = false;
            this.dgv2.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgv2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv2.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[]{
this.Column1,
this.Column2,
this.Column3});
            this.dgv2.Location = new System.Drawing.Point(526, 112);
            this.dgv2.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.dgv2.Name = "dgv2";
            this.dgv2.RowHeadersVisible = false;
            this.dgv2.Size = new System.Drawing.Size(393, 369);
            this.dgv2.TabIndex = 19;
            this.dgv2.DragDrop += new System.Windows.Forms.DragEventHandler(this.dataGridView1_DragDrop);
            this.dgv2.DragOver += new System.Windows.Forms.DragEventHandler(this.dataGridView1_DragOver);
            //
            //Column1
            //
            this.Column1.HeaderText = "CasNo.";
            this.Column1.Name = "Column1";
            //
            //Column2
            //
            this.Column2.HeaderText = "Name";
            this.Column2.Name = "Column2";
            //
            //Column3
            //
            this.Column3.HeaderText = "Formula";
            this.Column3.Name = "Column3";
            //
            //FilterType
            //
            this.FilterType.FormattingEnabled = true;
            this.FilterType.Location = new System.Drawing.Point(449, 37);
            this.FilterType.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.FilterType.Name = "FilterType";
            this.FilterType.Size = new System.Drawing.Size(132, 23);
            this.FilterType.TabIndex = 18;
            this.FilterType.SelectedIndexChanged += new System.EventHandler(this.FilterType_SelectedIndexChanged);
            //
            //FilterFormula
            //
            this.FilterFormula.Location = new System.Drawing.Point(192, 38);
            this.FilterFormula.Margin = new System.Windows.Forms.Padding(2);
            this.FilterFormula.Name = "FilterFormula";
            this.FilterFormula.Size = new System.Drawing.Size(75, 23);
            this.FilterFormula.TabIndex = 17;
            this.FilterFormula.TextChanged += new System.EventHandler(this.FilterFormula_TextChanged);
            //
            //FilterName
            //
            this.FilterName.Location = new System.Drawing.Point(112, 38);
            this.FilterName.Margin = new System.Windows.Forms.Padding(2);
            this.FilterName.Name = "FilterName";
            this.FilterName.Size = new System.Drawing.Size(75, 23);
            this.FilterName.TabIndex = 16;
            this.FilterName.TextChanged += new System.EventHandler(this.FilterName_TextChanged);
            //
            //dgv
            //
            this.dgv.AllowDrop = true;
            this.dgv.AllowUserToAddRows = false;
            this.dgv.AllowUserToDeleteRows = false;
            this.dgv.AllowUserToOrderColumns = true;
            this.dgv.AllowUserToResizeColumns = false;
            this.dgv.AllowUserToResizeRows = false;
            this.dgv.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[]{
this.CAS,
this.NameColumn,
this.Formula,
this.MW,
this.Bp});
            this.dgv.Location = new System.Drawing.Point(27, 112);
            this.dgv.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.dgv.Name = "dgv";
            this.dgv.RowHeadersVisible = false;
            this.dgv.Size = new System.Drawing.Size(406, 369);
            this.dgv.TabIndex = 15;
            this.dgv.MouseDown += new System.Windows.Forms.MouseEventHandler(this.DataGridView1_MouseDown);
            this.dgv.MouseMove += new System.Windows.Forms.MouseEventHandler(this.DataGridView1_MouseMove);
            //
            //CAS
            //
            this.CAS.DataPropertyName = "CAS";
            this.CAS.HeaderText = "CASNo.";
            this.CAS.Name = "CAS";
            //
            //NameColumn
            //
            this.NameColumn.DataPropertyName = "NAME";
            this.NameColumn.HeaderText = "Name";
            this.NameColumn.Name = "NameColumn";
            //
            //Formula
            //
            this.Formula.DataPropertyName = "FORMULA";
            this.Formula.HeaderText = "Formula";
            this.Formula.Name = "Formula";
            //
            //MW
            //
            this.MW.DataPropertyName = "MW";
            this.MW.HeaderText = "MW";
            this.MW.Name = "MW";
            //
            //Bp
            //
            this.Bp.DataPropertyName = "TB";
            this.Bp.HeaderText = "BP";
            this.Bp.Name = "Bp";
            //
            //button2
            //
            this.button2.Location = new System.Drawing.Point(526, 488);
            this.button2.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(88, 27);
            this.button2.TabIndex = 14;
            this.button2.Text = "Clear";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            //
            //textBoxUBP
            //
            this.textBoxUBP.Location = new System.Drawing.Point(940, 308);
            this.textBoxUBP.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxUBP.Name = "textBoxUBP";
            this.textBoxUBP.Size = new System.Drawing.Size(152, 23);
            this.textBoxUBP.TabIndex = 13;
            this.textBoxUBP.Text = "110";
            //
            //textBoxSG
            //
            this.textBoxSG.Location = new System.Drawing.Point(940, 254);
            this.textBoxSG.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxSG.Name = "textBoxSG";
            this.textBoxSG.Size = new System.Drawing.Size(152, 23);
            this.textBoxSG.TabIndex = 12;
            this.textBoxSG.Text = "0.7179";
            //
            //textBoxLBP
            //
            this.textBoxLBP.Location = new System.Drawing.Point(940, 281);
            this.textBoxLBP.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxLBP.Name = "textBoxLBP";
            this.textBoxLBP.Size = new System.Drawing.Size(152, 23);
            this.textBoxLBP.TabIndex = 11;
            this.textBoxLBP.Text = "90";
            //
            //button1
            //
            this.button1.Location = new System.Drawing.Point(940, 204);
            this.button1.Margin = new System.Windows.Forms.Padding(2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(152, 35);
            this.button1.TabIndex = 10;
            this.button1.Text = "AddPseudocomponent";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.AddQuasi_Click);
            //
            //label10
            //
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(28, 83);
            this.label10.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(127, 15);
            this.label10.TabIndex = 8;
            this.label10.Text = "DataBaseComponents";
            //
            //label9
            //
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(28, 17);
            this.label9.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(33, 15);
            this.label9.TabIndex = 7;
            this.label9.Text = "Filter";
            //
            //FilterCAS
            //
            this.FilterCAS.Location = new System.Drawing.Point(31, 38);
            this.FilterCAS.Margin = new System.Windows.Forms.Padding(2);
            this.FilterCAS.Name = "FilterCAS";
            this.FilterCAS.Size = new System.Drawing.Size(75, 23);
            this.FilterCAS.TabIndex = 6;
            this.FilterCAS.TextChanged += new System.EventHandler(this.FilterTxt_TextChanged);
            //
            //label6
            //
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(289, 83);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(134, 15);
            this.label6.TabIndex = 2;
            this.label6.Text = "DragAndDroptoSelect";
            //
            //dataGridViewTextBoxColumn1
            //
            this.dataGridViewTextBoxColumn1.DataPropertyName = "CAS";
            this.dataGridViewTextBoxColumn1.HeaderText = "CASNo.";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.Width = 115;
            //
            //dataGridViewTextBoxColumn2
            //
            this.dataGridViewTextBoxColumn2.DataPropertyName = "NAME";
            this.dataGridViewTextBoxColumn2.HeaderText = "Name";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.Width = 115;
            //
            //dataGridViewTextBoxColumn3
            //
            this.dataGridViewTextBoxColumn3.DataPropertyName = "FORMULA";
            this.dataGridViewTextBoxColumn3.HeaderText = "Formula";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.Width = 115;
            //
            //dataGridViewTextBoxColumn4
            //
            this.dataGridViewTextBoxColumn4.DataPropertyName = "CAS";
            this.dataGridViewTextBoxColumn4.HeaderText = "CASNo.";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.Width = 86;
            //
            //dataGridViewTextBoxColumn5
            //
            this.dataGridViewTextBoxColumn5.DataPropertyName = "NAME";
            this.dataGridViewTextBoxColumn5.HeaderText = "Name";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.Width = 87;
            //
            //dataGridViewTextBoxColumn6
            //
            this.dataGridViewTextBoxColumn6.DataPropertyName = "FORMULA";
            this.dataGridViewTextBoxColumn6.HeaderText = "Formula";
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            this.dataGridViewTextBoxColumn6.Width = 86;
            //
            //dataGridViewTextBoxColumn7
            //
            this.dataGridViewTextBoxColumn7.DataPropertyName = "MW";
            this.dataGridViewTextBoxColumn7.HeaderText = "MW";
            this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            this.dataGridViewTextBoxColumn7.Width = 86;
            //
            //dataGridViewTextBoxColumn8
            //
            this.dataGridViewTextBoxColumn8.DataPropertyName = "TB";
            this.dataGridViewTextBoxColumn8.HeaderText = "BP";
            this.dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
            this.dataGridViewTextBoxColumn8.Width = 69;
            //
            //button6
            //
            this.button6.Location = new System.Drawing.Point(940, 112);
            this.button6.Margin = new System.Windows.Forms.Padding(2);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(152, 35);
            this.button6.TabIndex = 26;
            this.button6.Text = "LoadSavedStream";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.btnLoadData_Click);
            //
            //ComponenetSelection
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1118, 569);
            this.Controls.Add(this.Composition);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "ComponenetSelection";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Componentselection_FormClosing);
            this.Load += new System.EventHandler(this.StreamDataEntry_Load);
            this.Composition.ResumeLayout(false);
            this.tabPageComponents.ResumeLayout(false);
            this.tabPageComponents.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private TabControl Composition;
        private TabPage tabPageComponents;
        private Label label6;
        private TextBox FilterCAS;
        private Label label9;
        private Label label10;
        private TextBox textBoxUBP;
        private TextBox textBoxSG;
        private TextBox textBoxLBP;
        private Button button1;
        private Button button2;
        private TextBox FilterFormula;
        private TextBox FilterName;
        private DataGridView dgv;
        private ComboBox FilterType;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private DataGridView dgv2;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private TextBox FilterMW;
        private DataGridViewTextBoxColumn CAS;
        private DataGridViewTextBoxColumn NameColumn;
        private DataGridViewTextBoxColumn Formula;
        private DataGridViewTextBoxColumn MW;
        private DataGridViewTextBoxColumn Bp;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
        private Button button3;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;
        private Button button4;
        private DataGridViewTextBoxColumn Column1;
        private DataGridViewTextBoxColumn Column2;
        private DataGridViewTextBoxColumn Column3;
        private Label label1;
        private TextBox FilterBP;
        private Button button5;
        private Button button6;
    }
}
