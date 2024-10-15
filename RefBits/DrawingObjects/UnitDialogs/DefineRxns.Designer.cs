namespace   Units
{
    partial class  DefineRxns
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
            System.ComponentModel.ComponentResourceManager resources = new  System.ComponentModel.ComponentResourceManager(typeof(DefineRxns));
            this.button1 = new  System.Windows.Forms.Button();
            this.button2 = new  System.Windows.Forms.Button();
            this.groupBox1 = new  System.Windows.Forms.GroupBox();
            this.Products = new  FormControls.RXStoichiometryGrid();
            this.Feeds = new  FormControls.RXStoichiometryGrid();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new  System.Drawing.Point (11, 40);
            this.button1.Name = "button1";
            this.button1.Size = new  System.Drawing.Size(91, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "Add Feeds";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new  System.EventHandler(this.Feeds_Click);
            // 
            // button2
            // 
            this.button2.Location = new  System.Drawing.Point (200, 40);
            this.button2.Name = "button2";
            this.button2.Size = new  System.Drawing.Size(86, 23);
            this.button2.TabIndex = 3;
            this.button2.Text = "Add Products";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new  System.EventHandler(this.Products_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.Products);
            this.groupBox1.Controls.Add(this.button2);
            this.groupBox1.Controls.Add(this.Feeds);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Location = new  System.Drawing.Point (12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new  System.Drawing.Size(388, 344);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Components";
            // 
            // Products
            // 
            this.Products.AllowUserToAddRows = false;
            this.Products.AllowUserToDeleteRows = false;
            this.Products.ColumnNames = ((System.Collections.Generic.List<string>)(resources.GetObject("Products.ColumnNames")));
            this.Products.DisplayTitles = true;
            this.Products.FirstColumnWidth = 64;
            this.Products.Location = new  System.Drawing.Point (200, 69);
            this.Products.Margin = new  System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Products.Name = "Products";
            this.Products.ReadOnly  = false;
            this.Products.RowHeadersVisible = false;
            this.Products.RowNames = ((System.Collections.Generic.List<string>)(resources.GetObject("Products.RowNames")));
            this.Products.Size = new  System.Drawing.Size(181, 269);
            this.Products.TabIndex = 7;
            this.Products.TopText = "Products";
            // 
            // Feeds
            // 
            this.Feeds.AllowUserToAddRows = false;
            this.Feeds.AllowUserToDeleteRows = false;
            this.Feeds.ColumnNames = ((System.Collections.Generic.List<string>)(resources.GetObject("Feeds.ColumnNames")));
            this.Feeds.DisplayTitles = true;
            this.Feeds.FirstColumnWidth = 64;
            this.Feeds.Location = new  System.Drawing.Point (11, 69);
            this.Feeds.Margin = new  System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Feeds.Name = "Feeds";
            this.Feeds.ReadOnly  = false;
            this.Feeds.RowHeadersVisible = false;
            this.Feeds.RowNames = ((System.Collections.Generic.List<string>)(resources.GetObject("Feeds.RowNames")));
            this.Feeds.Size = new  System.Drawing.Size(181, 269);
            this.Feeds.TabIndex = 6;
            this.Feeds.TopText = "Feeds";
            this.Feeds.Load += new  System.EventHandler(this.Feeds_Load);
            // 
            // DefineRxns
            // 
            this.AutoScaleDimensions = new  System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new  System.Drawing.Size(413, 372);
            this.Controls.Add(this.groupBox1);
            this.Name = "DefineRxns";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Select Components";
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private  System.Windows.Forms.Button button1;
        private  System.Windows.Forms.Button button2;
        private  System.Windows.Forms.GroupBox groupBox1;
        private  FormControls.RXStoichiometryGrid Products;
        private  FormControls.RXStoichiometryGrid Feeds;
    }
}