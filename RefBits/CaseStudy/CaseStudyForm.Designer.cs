
namespace Units.CaseStudy
{
    partial class CaseStudyForm
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

        #region WindowsFormDesignergeneratedcode

        ///<summary>
        ///RequiredmethodforDesignersupport-donotmodify
        ///thecontentsofthismethodwiththecodeeditor.
        ///</summary>
        private void InitializeComponent()
        {
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.button1 = new System.Windows.Forms.Button();
            this.myTextBox1 = new Units.MyTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.gridInputs = new UOMGrid.Grid();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.gridOutputs = new UOMGrid.Grid();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.gridResults = new UOMGrid.Grid();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            //
            //tabControl1
            //
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1041, 682);
            this.tabControl1.TabIndex = 0;
            //
            //tabPage1
            //
            this.tabPage1.BackColor = System.Drawing.Color.LightGray;
            this.tabPage1.Controls.Add(this.button1);
            this.tabPage1.Controls.Add(this.myTextBox1);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.gridInputs);
            this.tabPage1.Location = new System.Drawing.Point(4, 24);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tabPage1.Size = new System.Drawing.Size(1033, 654);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Setup";
            //
            //button1
            //
            this.button1.Location = new System.Drawing.Point(7, 13);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 5;
            this.button1.Text = "Run";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Run_Click);
            //
            //myTextBox1
            //
            this.myTextBox1.Location = new System.Drawing.Point(925, 21);
            this.myTextBox1.Name = "myTextBox1";
            this.myTextBox1.Size = new System.Drawing.Size(100, 23);
            this.myTextBox1.TabIndex = 4;
            this.myTextBox1.Validating += new System.ComponentModel.CancelEventHandler(this.MyTextBox1_Validating);
            this.myTextBox1.Validated += new System.EventHandler(this.myTextBox1_Validated);
            //
            //label1
            //
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(836, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 15);
            this.label1.TabIndex = 3;
            this.label1.Text = "NoCases";
            //
            //gridInputs
            //
            this.gridInputs.AllowDrop = true;
            this.gridInputs.AllowUserToAddRows = false;
            this.gridInputs.Cols = 2;
            this.gridInputs.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.gridInputs.Location = new System.Drawing.Point(4, 68);
            this.gridInputs.Name = "gridInputs";
            this.gridInputs.Rows = 5;
            this.gridInputs.Size = new System.Drawing.Size(1025, 583);
            this.gridInputs.TabIndex = 1;
            //
            //tabPage3
            //
            this.tabPage3.BackColor = System.Drawing.SystemColors.ControlLight;
            this.tabPage3.Controls.Add(this.gridOutputs);
            this.tabPage3.Location = new System.Drawing.Point(4, 24);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(1033, 654);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "OutVariables";
            //
            //gridOutputs
            //
            this.gridOutputs.AllowDrop = true;
            this.gridOutputs.AllowUserToAddRows = false;
            this.gridOutputs.BackColor = System.Drawing.SystemColors.ControlLight;
            this.gridOutputs.Cols = 2;
            this.gridOutputs.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.gridOutputs.Location = new System.Drawing.Point(3, 62);
            this.gridOutputs.Name = "gridOutputs";
            this.gridOutputs.Rows = 10;
            this.gridOutputs.Size = new System.Drawing.Size(1027, 589);
            this.gridOutputs.TabIndex = 1;
            //
            //tabPage2
            //
            this.tabPage2.BackColor = System.Drawing.Color.LightGray;
            this.tabPage2.Controls.Add(this.gridResults);
            this.tabPage2.Location = new System.Drawing.Point(4, 24);
            this.tabPage2.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tabPage2.Size = new System.Drawing.Size(1033, 654);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Results";
            //
            //gridResults
            //
            this.gridResults.AllowDrop = true;
            this.gridResults.AllowUserToAddRows = false;
            this.gridResults.Cols = 2;
            this.gridResults.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.gridResults.Location = new System.Drawing.Point(4, 80);
            this.gridResults.Name = "gridResults";
            this.gridResults.Rows = 10;
            this.gridResults.Size = new System.Drawing.Size(1025, 571);
            this.gridResults.TabIndex = 0;
            //
            //CaseStudyForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1041, 682);
            this.Controls.Add(this.tabControl1);
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "CaseStudyForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CaseStudyForm";
            this.TopMost = true;
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Label label1;
        private MyTextBox myTextBox1;
        private UOMGrid.Grid gridInputs;
        private UOMGrid.Grid gridResults;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TabPage tabPage3;
        private UOMGrid.Grid gridOutputs;
    }
}