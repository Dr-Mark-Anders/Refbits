using   System.Windows.Forms;

namespace   Units.PortForm
{
    partial class  PortPropertyForm2 : Form
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private  System.ComponentModel.IContainer Components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private  void  InitializeComponent()
        {
            this.portworksheet = new  Units.PortForm.PortPropertyWorksheet();
            this.button1 = new  System.Windows.Forms.Button();
            this.button2 = new  System.Windows.Forms.Button();
            this.txtStreamName = new  System.Windows.Forms.TextBox();
            this.lblStreamName = new  System.Windows.Forms.Label();
            this.FlashButton = new  System.Windows.Forms.Button();
            this.LoadTestDataBtn = new  System.Windows.Forms.Button();
            this.ViewOutPort = new  System.Windows.Forms.Button();
            this.btnAssayCreate = new  System.Windows.Forms.Button();
            this.button3 = new  System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // portworksheet
            // 
            this.portworksheet.DrawMaterialStream = null;
            this.portworksheet.Location = new  System.Drawing.Point (11, 40);
            this.portworksheet.Margin = new  System.Windows.Forms.Padding(2);
            this.portworksheet.Name = "portworksheet";
            this.portworksheet.Simplify = false;
            this.portworksheet.Size = new  System.Drawing.Size(901, 477);
            this.portworksheet.TabIndex = 2;
            this.portworksheet.Load += new  System.EventHandler(this.portPropertyWorksheet1_Load);
            // 
            // button1
            // 
            this.button1.Location = new  System.Drawing.Point (24, 506);
            this.button1.Name = "button1";
            this.button1.Size = new  System.Drawing.Size(94, 27);
            this.button1.TabIndex = 4;
            this.button1.Text = "Save Data";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new  System.EventHandler(this.btnSave_Click);
            // 
            // button2
            // 
            this.button2.Location = new  System.Drawing.Point (124, 506);
            this.button2.Name = "button2";
            this.button2.Size = new  System.Drawing.Size(94, 27);
            this.button2.TabIndex = 5;
            this.button2.Text = "Load Data";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new  System.EventHandler(this.btnLoadData_Click);
            // 
            // txtStreamName
            // 
            this.txtStreamName.Location = new  System.Drawing.Point (116, 12);
            this.txtStreamName.Name = "txtStreamName";
            this.txtStreamName.Size = new  System.Drawing.Size(192, 23);
            this.txtStreamName.TabIndex = 6;
            this.txtStreamName.Validated += new  System.EventHandler(this.txtStreamName_Validated);
            // 
            // lblStreamName
            // 
            this.lblStreamName.AutoSize = true;
            this.lblStreamName.Location = new  System.Drawing.Point (15, 15);
            this.lblStreamName.Name = "lblStreamName";
            this.lblStreamName.Size = new  System.Drawing.Size(79, 15);
            this.lblStreamName.TabIndex = 7;
            this.lblStreamName.Text = "Stream Name";
            // 
            // FlashButton
            // 
            this.FlashButton.Location = new  System.Drawing.Point (328, 506);
            this.FlashButton.Name = "FlashButton";
            this.FlashButton.Size = new  System.Drawing.Size(94, 27);
            this.FlashButton.TabIndex = 8;
            this.FlashButton.Text = "Re Flash";
            this.FlashButton.UseVisualStyleBackColor = true;
            this.FlashButton.Click += new  System.EventHandler(this.button3_Click);
            // 
            // LoadTestDataBtn
            // 
            this.LoadTestDataBtn.Location = new  System.Drawing.Point (428, 506);
            this.LoadTestDataBtn.Name = "LoadTestDataBtn";
            this.LoadTestDataBtn.Size = new  System.Drawing.Size(94, 27);
            this.LoadTestDataBtn.TabIndex = 9;
            this.LoadTestDataBtn.Text = "Load Test Data";
            this.LoadTestDataBtn.UseVisualStyleBackColor = true;
            this.LoadTestDataBtn.Click += new  System.EventHandler(this.LoadTestData);
            // 
            // ViewOutPort
            // 
            this.ViewOutPort.Location = new  System.Drawing.Point (528, 506);
            this.ViewOutPort.Name = "ViewOutPort";
            this.ViewOutPort.Size = new  System.Drawing.Size(94, 27);
            this.ViewOutPort.TabIndex = 10;
            this.ViewOutPort.Text = "View Out Port";
            this.ViewOutPort.UseVisualStyleBackColor = true;
            this.ViewOutPort.Click += new  System.EventHandler(this.ViewOutPort_Click);
            // 
            // btnAssayCreate
            // 
            this.btnAssayCreate.Location = new  System.Drawing.Point (224, 506);
            this.btnAssayCreate.Name = "btnAssayCreate";
            this.btnAssayCreate.Size = new  System.Drawing.Size(94, 27);
            this.btnAssayCreate.TabIndex = 11;
            this.btnAssayCreate.Text = "Create Assay";
            this.btnAssayCreate.UseVisualStyleBackColor = true;
            this.btnAssayCreate.Click += new  System.EventHandler(this.btnAssayCreate_Click);
            // 
            // button3
            // 
            this.button3.Location = new  System.Drawing.Point (628, 506);
            this.button3.Name = "button3";
            this.button3.Size = new  System.Drawing.Size(94, 27);
            this.button3.TabIndex = 12;
            this.button3.Text = "Flash Outport";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new  System.EventHandler(this.button3_Click_1);
            // 
            // PortPropertyForm2
            // 
            this.AutoScaleDimensions = new  System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new  System.Drawing.Size(918, 541);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.btnAssayCreate);
            this.Controls.Add(this.ViewOutPort);
            this.Controls.Add(this.LoadTestDataBtn);
            this.Controls.Add(this.FlashButton);
            this.Controls.Add(this.lblStreamName);
            this.Controls.Add(this.txtStreamName);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.portworksheet);
            this.KeyPreview = true;
            this.Name = "PortPropertyForm2";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Refbits Properties";
            this.TopMost = true;
            this.Load += new  System.EventHandler(this.SteamPropsForm_Load);
            this.KeyDown += new  System.Windows.Forms.KeyEventHandler(this.PortPropertyForm2_KeyDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private  Units.PortForm.PortPropertyWorksheet portworksheet;
        private  Button button1;
        private  Button button2;
        private  TextBox txtStreamName;
        private  Label lblStreamName;
        private  Button FlashButton;
        private  Button LoadTestDataBtn;
        private  Button ViewOutPort;
        private  Button btnAssayCreate;
        private  Button button3;
    }
}