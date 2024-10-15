
namespace   DialogControls
{
    partial class  DropDownDataEntry
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
            this.gb = new  System.Windows.Forms.GroupBox();
            this.cb1 = new  System.Windows.Forms.ComboBox();
            this.gb.SuspendLayout();
            this.SuspendLayout();
            // 
            // gb
            // 
            this.gb.Controls.Add(this.cb1);
            this.gb.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gb.Location = new  System.Drawing.Point (0, 0);
            this.gb.Margin = new  System.Windows.Forms.Padding(4, 3, 4, 3);
            this.gb.Name = "gb";
            this.gb.Padding = new  System.Windows.Forms.Padding(4, 3, 4, 3);
            this.gb.Size = new  System.Drawing.Size(236, 59);
            this.gb.TabIndex = 4;
            this.gb.TabStop = false;
            this.gb.Text = "Flow Type";
            // 
            // cb1
            // 
            this.cb1.FormattingEnabled = true;
            this.cb1.Location = new  System.Drawing.Point (26, 22);
            this.cb1.Name = "cb1";
            this.cb1.Size = new  System.Drawing.Size(121, 23);
            this.cb1.TabIndex = 0;
            // 
            // DropDownDataEntry
            // 
            this.AutoScaleDimensions = new  System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gb);
            this.Margin = new  System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "DropDownDataEntry";
            this.Size = new  System.Drawing.Size(236, 59);
            this.MouseClick += new  System.Windows.Forms.MouseEventHandler(this.MassMolarVol_MouseClick);
            this.Resize += new  System.EventHandler(this.SingleProperty_Resize);
            this.gb.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private  System.Windows.Forms.GroupBox gb;
        private  System.Windows.Forms.ComboBox cb1;
    }
}
