namespace   Units
{
    partial class  SetupSimulationForm
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
            this.btn_CaseStudies = new  System.Windows.Forms.Button();
            this.btn_Worksheet = new  System.Windows.Forms.Button();
            this.btn_Compositions = new  System.Windows.Forms.Button();
            this.btn_Thermo = new  System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btn_CaseStudies
            // 
            this.btn_CaseStudies.Location = new  System.Drawing.Point (5, 92);
            this.btn_CaseStudies.Margin = new  System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btn_CaseStudies.Name = "btn_CaseStudies";
            this.btn_CaseStudies.Size = new  System.Drawing.Size(131, 29);
            this.btn_CaseStudies.TabIndex = 14;
            this.btn_CaseStudies.Text = "Case studies";
            this.btn_CaseStudies.UseVisualStyleBackColor = true;
            this.btn_CaseStudies.Click += new  System.EventHandler(this.btn_CaseStudies_Click);
            // 
            // btn_Worksheet
            // 
            this.btn_Worksheet.Location = new  System.Drawing.Point (5, 57);
            this.btn_Worksheet.Margin = new  System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btn_Worksheet.Name = "btn_Worksheet";
            this.btn_Worksheet.Size = new  System.Drawing.Size(131, 29);
            this.btn_Worksheet.TabIndex = 13;
            this.btn_Worksheet.Text = "WorkSheet";
            this.btn_Worksheet.UseVisualStyleBackColor = true;
            this.btn_Worksheet.Click += new  System.EventHandler(this.btn_Worksheet_Click);
            // 
            // btn_Compositions
            // 
            this.btn_Compositions.Location = new  System.Drawing.Point (5, 6);
            this.btn_Compositions.Margin = new  System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btn_Compositions.Name = "btn_Compositions";
            this.btn_Compositions.Size = new  System.Drawing.Size(131, 29);
            this.btn_Compositions.TabIndex = 11;
            this.btn_Compositions.Text = "Components";
            this.btn_Compositions.UseVisualStyleBackColor = true;
            this.btn_Compositions.Click += new  System.EventHandler(this.Compositions_Click);
            // 
            // btn_Thermo
            // 
            this.btn_Thermo.Location = new  System.Drawing.Point (5, 30);
            this.btn_Thermo.Margin = new  System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btn_Thermo.Name = "btn_Thermo";
            this.btn_Thermo.Size = new  System.Drawing.Size(131, 29);
            this.btn_Thermo.TabIndex = 12;
            this.btn_Thermo.Text = "ThermoDynamics";
            this.btn_Thermo.UseVisualStyleBackColor = true;
            this.btn_Thermo.Click += new  System.EventHandler(this.Thermo_Click);
            // 
            // SetupSimulationForm
            // 
            this.AutoScaleDimensions = new  System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.ClientSize = new  System.Drawing.Size(147, 444);
            this.Controls.Add(this.btn_CaseStudies);
            this.Controls.Add(this.btn_Worksheet);
            this.Controls.Add(this.btn_Compositions);
            this.Controls.Add(this.btn_Thermo);
            this.Name = "SetupSimulationForm";
            this.Text = "Setup";
            this.ResumeLayout(false);

        }

        #endregion

        private  System.Windows.Forms.Button btn_CaseStudies;
        private  System.Windows.Forms.Button btn_Worksheet;
        private  System.Windows.Forms.Button btn_Compositions;
        private  System.Windows.Forms.Button btn_Thermo;
    }
}