namespace   Units
{
    partial class  PropertyGridForm
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
            this.propertyGrid = new  System.Windows.Forms.PropertyGrid();
            this.SuspendLayout();
            // 
            // propertyGrid
            // 
            this.propertyGrid.AllowDrop = true;
            this.propertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGrid.Location = new  System.Drawing.Point (0, 0);
            this.propertyGrid.Margin = new  System.Windows.Forms.Padding(4, 3, 4, 3);
            this.propertyGrid.MinimumSize = new  System.Drawing.Size(292, 0);
            this.propertyGrid.Name = "propertyGrid";
            this.propertyGrid.PropertySort = System.Windows.Forms.PropertySort.Categorized;
            this.propertyGrid.Size = new  System.Drawing.Size(295, 450);
            this.propertyGrid.TabIndex = 18;
            // 
            // PropertyGridForm
            // 
            this.AutoScaleDimensions = new  System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new  System.Drawing.Size(295, 450);
            this.Controls.Add(this.propertyGrid);
            this.Name = "PropertyGridForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Object Properties";
            this.ResumeLayout(false);

        }

        #endregion

        public  System.Windows.Forms.PropertyGrid propertyGrid;
    }
}