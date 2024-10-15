using ModelEngine;
using System.Windows.Forms;
using ModelEngine;
using Units.PortForm;

namespace Units
{
    partial class AdjustDialog
    {
        ///<summary>
        ///Requireddesignervariable.
        ///</summary>
        private System.ComponentModel.IContainer Components = null;

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AdjustDialog));
            treeView1 = new TreeView();
            groupBox1 = new GroupBox();
            uomTXTMVValue = new UOMTextBox();
            Value = new GroupBox();
            label5 = new Label();
            txtTargetValue = new TextBox();
            uom2 = new UOMTextBox();
            txtBoxMult = new TextBox();
            txDelta = new TextBox();
            label2 = new Label();
            label1 = new Label();
            groupBox2 = new GroupBox();
            uomTXTCVValue = new UOMTextBox();
            treeView2 = new TreeView();
            groupBox3 = new GroupBox();
            uomTextBox4 = new UOMTextBox();
            uomTextStepSize = new UOMTextBox();
            textMax = new TextBox();
            textMin = new TextBox();
            label3 = new Label();
            label4 = new Label();
            groupBox1.SuspendLayout();
            Value.SuspendLayout();
            groupBox2.SuspendLayout();
            groupBox3.SuspendLayout();
            SuspendLayout();
            // 
            // treeView1
            // 
            treeView1.Location = new System.Drawing.Point(6, 22);
            treeView1.Name = "treeView1";
            treeView1.Size = new System.Drawing.Size(233, 290);
            treeView1.TabIndex = 0;
            treeView1.AfterSelect += treeView1_AfterSelect;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(treeView1);
            groupBox1.Controls.Add(uomTXTMVValue);
            groupBox1.Location = new System.Drawing.Point(25, 27);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new System.Drawing.Size(259, 353);
            groupBox1.TabIndex = 2;
            groupBox1.TabStop = false;
            groupBox1.Text = "AdjustProperty";
            // 
            // uomTXTMVValue
            // 
            uomTXTMVValue.BackColor = System.Drawing.SystemColors.Control;
            uomTXTMVValue.ComboBoxHeight = 23;
            uomTXTMVValue.ComboBoxVisible = false;
            uomTXTMVValue.ComboBoxWidth = 60;
            uomTXTMVValue.DefaultUnits = "Quality";
            uomTXTMVValue.DefaultValue = "0E000";
            uomTXTMVValue.DescriptionWidth = 140;
            uomTXTMVValue.DisplayUnitArray = (new string[] { "Quality" });
            uomTXTMVValue.DisplayUnits = "Quality";
            uomTXTMVValue.Label = "AdjustedValue";
            uomTXTMVValue.LabelSize = 140;
            uomTXTMVValue.Location = new System.Drawing.Point(6, 318);
            uomTXTMVValue.Name = "uomTXTMVValue";
            uomTXTMVValue.Precision = 4;
            uomTXTMVValue.ReadOnly = false;
            uomTXTMVValue.Size = new System.Drawing.Size(233, 22);
            uomTXTMVValue.Source = SourceEnum.Input;
            uomTXTMVValue.TabIndex = 2;
            uomTXTMVValue.TextBoxFont = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            uomTXTMVValue.TextBoxHeight = 21;
            uomTXTMVValue.TextBoxLeft = 140;
            uomTXTMVValue.TextBoxSize = 93;
            uomTXTMVValue.UOMprop = (UOMProperty)resources.GetObject("uomTXTMVValue.UOMprop");
            uomTXTMVValue.UOMType = ePropID.NullUnits;
            uomTXTMVValue.Value = 0D;
            // 
            // Value
            // 
            Value.Controls.Add(label5);
            Value.Controls.Add(txtTargetValue);
            Value.Controls.Add(uom2);
            Value.Controls.Add(txtBoxMult);
            Value.Controls.Add(txDelta);
            Value.Controls.Add(label2);
            Value.Controls.Add(label1);
            Value.Location = new System.Drawing.Point(581, 27);
            Value.Name = "Value";
            Value.Size = new System.Drawing.Size(253, 150);
            Value.TabIndex = 3;
            Value.TabStop = false;
            Value.Text = "Value";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new System.Drawing.Point(16, 25);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(67, 15);
            label5.TabIndex = 9;
            label5.Text = "TargetValue";
            // 
            // txtTargetValue
            // 
            txtTargetValue.Location = new System.Drawing.Point(137, 19);
            txtTargetValue.Name = "txtTargetValue";
            txtTargetValue.Size = new System.Drawing.Size(84, 23);
            txtTargetValue.TabIndex = 8;
            txtTargetValue.Text = "0";
            txtTargetValue.TextChanged += txtTargetValue_TextChanged;
            // 
            // uom2
            // 
            uom2.BackColor = System.Drawing.SystemColors.Control;
            uom2.ComboBoxHeight = 23;
            uom2.ComboBoxVisible = false;
            uom2.ComboBoxWidth = 60;
            uom2.DefaultUnits = "Quality";
            uom2.DefaultValue = "5";
            uom2.DescriptionWidth = 120;
            uom2.DisplayUnitArray = (new string[] { "Quality" });
            uom2.DisplayUnits = "Quality";
            uom2.Label = "FinalValue";
            uom2.LabelSize = 120;
            uom2.Location = new System.Drawing.Point(17, 111);
            uom2.Name = "uom2";
            uom2.Precision = 4;
            uom2.ReadOnly = false;
            uom2.Size = new System.Drawing.Size(204, 22);
            uom2.Source = SourceEnum.Input;
            uom2.TabIndex = 7;
            uom2.TextBoxFont = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            uom2.TextBoxHeight = 21;
            uom2.TextBoxLeft = 120;
            uom2.TextBoxSize = 84;
            uom2.UOMprop = (UOMProperty)resources.GetObject("uom2.UOMprop");
            uom2.UOMType = ePropID.NullUnits;
            uom2.Value = 5D;
            // 
            // txtBoxMult
            // 
            txtBoxMult.Location = new System.Drawing.Point(137, 80);
            txtBoxMult.Name = "txtBoxMult";
            txtBoxMult.Size = new System.Drawing.Size(84, 23);
            txtBoxMult.TabIndex = 6;
            txtBoxMult.Text = "1";
            txtBoxMult.TextChanged += txtBoxMult_TextChanged;
            // 
            // txDelta
            // 
            txDelta.Location = new System.Drawing.Point(137, 50);
            txDelta.Name = "txDelta";
            txDelta.Size = new System.Drawing.Size(84, 23);
            txDelta.TabIndex = 5;
            txDelta.Text = "0";
            txDelta.TextChanged += txDelta_TextChanged;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(17, 80);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(58, 15);
            label2.TabIndex = 4;
            label2.Text = "Multiplier";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(17, 52);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(34, 15);
            label1.TabIndex = 3;
            label1.Text = "Delta";
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(uomTXTCVValue);
            groupBox2.Controls.Add(treeView2);
            groupBox2.Location = new System.Drawing.Point(301, 27);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new System.Drawing.Size(259, 353);
            groupBox2.TabIndex = 3;
            groupBox2.TabStop = false;
            groupBox2.Text = "TargetProperty";
            // 
            // uomTXTCVValue
            // 
            uomTXTCVValue.BackColor = System.Drawing.SystemColors.Control;
            uomTXTCVValue.ComboBoxHeight = 23;
            uomTXTCVValue.ComboBoxVisible = false;
            uomTXTCVValue.ComboBoxWidth = 60;
            uomTXTCVValue.DefaultUnits = "Quality";
            uomTXTCVValue.DefaultValue = "0E000";
            uomTXTCVValue.DescriptionWidth = 140;
            uomTXTCVValue.DisplayUnitArray = (new string[] { "Quality" });
            uomTXTCVValue.DisplayUnits = "Quality";
            uomTXTCVValue.Label = "CurrentValue";
            uomTXTCVValue.LabelSize = 140;
            uomTXTCVValue.Location = new System.Drawing.Point(6, 318);
            uomTXTCVValue.Name = "uomTXTCVValue";
            uomTXTCVValue.Precision = 4;
            uomTXTCVValue.ReadOnly = false;
            uomTXTCVValue.Size = new System.Drawing.Size(233, 22);
            uomTXTCVValue.Source = SourceEnum.Input;
            uomTXTCVValue.TabIndex = 3;
            uomTXTCVValue.TextBoxFont = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            uomTXTCVValue.TextBoxHeight = 21;
            uomTXTCVValue.TextBoxLeft = 140;
            uomTXTCVValue.TextBoxSize = 93;
            uomTXTCVValue.UOMprop = (UOMProperty)resources.GetObject("uomTXTCVValue.UOMprop");
            uomTXTCVValue.UOMType = ePropID.NullUnits;
            uomTXTCVValue.Value = 0D;
            // 
            // treeView2
            // 
            treeView2.Location = new System.Drawing.Point(6, 22);
            treeView2.Name = "treeView2";
            treeView2.Size = new System.Drawing.Size(233, 290);
            treeView2.TabIndex = 0;
            treeView2.AfterSelect += treeView2_AfterSelect;
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(uomTextBox4);
            groupBox3.Controls.Add(uomTextStepSize);
            groupBox3.Controls.Add(textMax);
            groupBox3.Controls.Add(textMin);
            groupBox3.Controls.Add(label3);
            groupBox3.Controls.Add(label4);
            groupBox3.Location = new System.Drawing.Point(581, 207);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new System.Drawing.Size(253, 160);
            groupBox3.TabIndex = 8;
            groupBox3.TabStop = false;
            groupBox3.Text = "Parameters";
            // 
            // uomTextBox4
            // 
            uomTextBox4.BackColor = System.Drawing.SystemColors.Control;
            uomTextBox4.ComboBoxHeight = 23;
            uomTextBox4.ComboBoxVisible = false;
            uomTextBox4.ComboBoxWidth = 60;
            uomTextBox4.DefaultUnits = "Quality";
            uomTextBox4.DefaultValue = "0E000";
            uomTextBox4.DescriptionWidth = 120;
            uomTextBox4.DisplayUnitArray = (new string[] { "Quality" });
            uomTextBox4.DisplayUnits = "Quality";
            uomTextBox4.Label = "SpecifiedValue";
            uomTextBox4.LabelSize = 120;
            uomTextBox4.Location = new System.Drawing.Point(17, 117);
            uomTextBox4.Name = "uomTextBox4";
            uomTextBox4.Precision = 4;
            uomTextBox4.ReadOnly = false;
            uomTextBox4.Size = new System.Drawing.Size(204, 22);
            uomTextBox4.Source = SourceEnum.Input;
            uomTextBox4.TabIndex = 8;
            uomTextBox4.TextBoxFont = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            uomTextBox4.TextBoxHeight = 21;
            uomTextBox4.TextBoxLeft = 120;
            uomTextBox4.TextBoxSize = 84;
            uomTextBox4.UOMprop = (UOMProperty)resources.GetObject("uomTextBox4.UOMprop");
            uomTextBox4.UOMType = ePropID.NullUnits;
            uomTextBox4.Value = 0D;
            // 
            // uomTextStepSize
            // 
            uomTextStepSize.BackColor = System.Drawing.SystemColors.Control;
            uomTextStepSize.ComboBoxHeight = 23;
            uomTextStepSize.ComboBoxVisible = false;
            uomTextStepSize.ComboBoxWidth = 60;
            uomTextStepSize.DefaultUnits = "Quality";
            uomTextStepSize.DefaultValue = "0E000";
            uomTextStepSize.DescriptionWidth = 120;
            uomTextStepSize.DisplayUnitArray = (new string[] { "Quality" });
            uomTextStepSize.DisplayUnits = "Quality";
            uomTextStepSize.Label = "Stepsize";
            uomTextStepSize.LabelSize = 120;
            uomTextStepSize.Location = new System.Drawing.Point(17, 89);
            uomTextStepSize.Name = "uomTextStepSize";
            uomTextStepSize.Precision = 4;
            uomTextStepSize.ReadOnly = false;
            uomTextStepSize.Size = new System.Drawing.Size(204, 22);
            uomTextStepSize.Source = SourceEnum.Input;
            uomTextStepSize.TabIndex = 7;
            uomTextStepSize.TextBoxFont = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            uomTextStepSize.TextBoxHeight = 21;
            uomTextStepSize.TextBoxLeft = 120;
            uomTextStepSize.TextBoxSize = 84;
            uomTextStepSize.UOMprop = (UOMProperty)resources.GetObject("uomTextStepSize.UOMprop");
            uomTextStepSize.UOMType = ePropID.NullUnits;
            uomTextStepSize.Value = 0D;
            // 
            // textMax
            // 
            textMax.Location = new System.Drawing.Point(137, 58);
            textMax.Name = "textMax";
            textMax.Size = new System.Drawing.Size(84, 23);
            textMax.TabIndex = 6;
            textMax.Text = "1";
            // 
            // textMin
            // 
            textMin.Location = new System.Drawing.Point(137, 28);
            textMin.Name = "textMin";
            textMin.Size = new System.Drawing.Size(84, 23);
            textMin.TabIndex = 5;
            textMin.Text = "0";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(17, 58);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(30, 15);
            label3.TabIndex = 4;
            label3.Text = "Max";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new System.Drawing.Point(17, 31);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(28, 15);
            label4.TabIndex = 3;
            label4.Text = "Min";
            // 
            // AdjustDialog
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(846, 429);
            Controls.Add(groupBox3);
            Controls.Add(groupBox2);
            Controls.Add(Value);
            Controls.Add(groupBox1);
            Margin = new Padding(4, 2, 4, 2);
            Name = "AdjustDialog";
            Text = "AdjustDialog";
            Load += AdjustDialog_Load;
            groupBox1.ResumeLayout(false);
            Value.ResumeLayout(false);
            Value.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            ResumeLayout(false);
        }

        private TreeView treeView1;
        private GroupBox groupBox1;
        private GroupBox Value;
        private TextBox txtBoxMult;
        private TextBox txDelta;
        private Label label2;
        private Label label1;
        private UOMTextBox uomTXTMVValue;
        private UOMTextBox uom2;
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

        #region WindowsFormDesignergeneratedcode

        ///<summary>
        ///RequiredmethodforDesignersupport-donotmodify
        ///thecontentsofthismethodwiththecodeeditor.
        ///</summary>

        #endregion
        private TabControl tabControl1;
        private TabPage Summary;
        private TabPage Streams;
        private PortsPropertyWorksheet Worksheet;
        private FormControls.UserPropGrid Results;
        private FormControls.UserPropGrid Parameters;
        private GroupBox groupBox2;
        private TreeView treeView2;
        private UOMTextBox uomTXTCVValue;
        private GroupBox groupBox3;
        private UOMTextBox uomTextStepSize;
        private TextBox textMax;
        private TextBox textMin;
        private Label label3;
        private Label label4;
        private UOMTextBox uomTextBox4;
        private Label label5;
        private TextBox txtTargetValue;
        //private FlexTextBoxflexTextBoxDT;
    }
}