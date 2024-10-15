using Extensions;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace DialogControls
{
    public partial class FlowFraction : UserControl
    {
        private RadioButton rbFraction;
        private RadioButton rbFlow;
        private GroupBox groupBox1;
        private enumFlowOrFraction value = enumFlowOrFraction.Flow;

        public FlowFraction()
        {
            InitializeComponent();
        }

        [Browsable(true)]
        public override string Text
        {
            get
            {
                return groupBox1.Text;
            }
            set
            {
                groupBox1.Text = value;
            }
        }

        public enumFlowOrFraction Value
        {
            get
            {
                return value;
            }

            set
            {
                this.value = value;

                switch (value)
                {
                    case enumFlowOrFraction.Flow:
                        rbFlow.Checked = true;
                        break;

                    case enumFlowOrFraction.Fraction:
                        rbFraction.Checked = true;
                        break;

                    default:
                        break;
                }
            }
        }

        public enumFlowOrFraction Value1 { get => value; set => this.value = value; }

        private void rbMass_CheckedChanged(object sender, EventArgs e)
        {
            value = enumFlowOrFraction.Flow;
        }

        private void rbMolar_CheckedChanged(object sender, EventArgs e)
        {
            value = enumFlowOrFraction.Fraction;
        }

        public double[] Composition(double[] Mol, double[] MW)
        {
            switch (value)
            {
                case enumFlowOrFraction.Flow:
                    SelectionChanged?.Invoke(this, EventArgs.Empty);
                    return Mol.Mult(MW).Normalise();

                case enumFlowOrFraction.Fraction:
                    SelectionChanged?.Invoke(this, EventArgs.Empty);
                    return Mol;
            }
            return null;
        }

        public new event MouseEventHandler MouseClick;

        public event EventHandler SelectionChanged;

        private void MassMolarVol_MouseClick(object sender, MouseEventArgs e)
        {
            RaiseMouseEvent(MouseClick, e);
        }

        private void InitializeComponent()
        {
            this.rbFraction = new System.Windows.Forms.RadioButton();
            this.rbFlow = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            //
            // rbFraction
            //
            this.rbFraction.AutoSize = true;
            this.rbFraction.Location = new System.Drawing.Point(16, 52);
            this.rbFraction.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.rbFraction.Name = "rbFraction";
            this.rbFraction.Size = new System.Drawing.Size(68, 19);
            this.rbFraction.TabIndex = 3;
            this.rbFraction.TabStop = true;
            this.rbFraction.Text = "Fraction";
            this.rbFraction.UseVisualStyleBackColor = true;
            //
            // rbFlow
            //
            this.rbFlow.AutoSize = true;
            this.rbFlow.Location = new System.Drawing.Point(16, 25);
            this.rbFlow.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.rbFlow.Name = "rbFlow";
            this.rbFlow.Size = new System.Drawing.Size(50, 19);
            this.rbFlow.TabIndex = 2;
            this.rbFlow.TabStop = true;
            this.rbFlow.Text = "Flow";
            this.rbFlow.UseVisualStyleBackColor = true;
            //
            // groupBox1
            //
            this.groupBox1.Controls.Add(this.rbFraction);
            this.groupBox1.Controls.Add(this.rbFlow);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(115, 95);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Data Type";
            //
            // FlowFraction
            //
            this.Controls.Add(this.groupBox1);
            this.Name = "FlowFraction";
            this.Size = new System.Drawing.Size(115, 95);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
        }
    }
}