﻿using Units;

namespace Test
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer Components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.columnDesignerControl1 = new Units.ColumnDesignerControl();
            this.SuspendLayout();
            // 
            // columnDesignerControl1
            // 
            this.columnDesignerControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.columnDesignerControl1.Location = new System.Drawing.Point(0, 0);
            this.columnDesignerControl1.Name = "columnDesignerControl1";
            this.columnDesignerControl1.Size = new System.Drawing.Size(800, 450);
            this.columnDesignerControl1.TabIndex = 1;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.columnDesignerControl1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private ColumnDesignerControl columnDesignerControl1;
    }
}