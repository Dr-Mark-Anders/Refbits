namespace FCC_COM
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            tabControl1 = new TabControl();
            Design = new TabPage();
            CatFactors = new FormControls.UserPropGrid();
            Dimensions = new FormControls.UserPropGrid();
            Feed = new TabPage();
            userPropGrid3 = new FormControls.UserPropGrid();
            userPropGrid2 = new FormControls.UserPropGrid();
            OperatingConditions = new TabPage();
            Riser = new TabPage();
            Worksheet = new TabPage();
            Regenerator = new TabPage();
            Results = new TabPage();
            tabControl1.SuspendLayout();
            Design.SuspendLayout();
            Feed.SuspendLayout();
            SuspendLayout();
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(Design);
            tabControl1.Controls.Add(Feed);
            tabControl1.Controls.Add(OperatingConditions);
            tabControl1.Controls.Add(Riser);
            tabControl1.Controls.Add(Worksheet);
            tabControl1.Controls.Add(Regenerator);
            tabControl1.Controls.Add(Results);
            tabControl1.Dock = DockStyle.Fill;
            tabControl1.Location = new Point(0, 0);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(800, 450);
            tabControl1.TabIndex = 7;
            // 
            // Design
            // 
            Design.Controls.Add(CatFactors);
            Design.Controls.Add(Dimensions);
            Design.Location = new Point(4, 24);
            Design.Name = "Design";
            Design.Padding = new Padding(3);
            Design.Size = new Size(792, 422);
            Design.TabIndex = 1;
            Design.Text = "Design";
            Design.UseVisualStyleBackColor = true;
            // 
            // CatFactors
            // 
            CatFactors.AllowChangeEvent = true;
            CatFactors.AllowUserToAddRows = false;
            CatFactors.AllowUserToDeleteRows = false;
            CatFactors.ColumnNames = (List<string>)resources.GetObject("CatFactors.ColumnNames");
            CatFactors.DisplayTitles = true;
            CatFactors.FirstColumnWidth = 64;
            CatFactors.Location = new Point(290, 17);
            CatFactors.Margin = new Padding(4, 3, 4, 3);
            CatFactors.Name = "CatFactors";
            CatFactors.ReadOnly = false;
            CatFactors.RowHeadersVisible = false;
            CatFactors.RowNames = (List<string>)resources.GetObject("CatFactors.RowNames");
            CatFactors.Size = new Size(460, 307);
            CatFactors.TabIndex = 11;
            CatFactors.TopText = "Dimensions";
            // 
            // Dimensions
            // 
            Dimensions.AllowChangeEvent = true;
            Dimensions.AllowUserToAddRows = false;
            Dimensions.AllowUserToDeleteRows = false;
            Dimensions.ColumnNames = (List<string>)resources.GetObject("Dimensions.ColumnNames");
            Dimensions.DisplayTitles = true;
            Dimensions.FirstColumnWidth = 64;
            Dimensions.Location = new Point(24, 17);
            Dimensions.Margin = new Padding(4, 3, 4, 3);
            Dimensions.Name = "Dimensions";
            Dimensions.ReadOnly = false;
            Dimensions.RowHeadersVisible = false;
            Dimensions.RowNames = (List<string>)resources.GetObject("Dimensions.RowNames");
            Dimensions.Size = new Size(216, 307);
            Dimensions.TabIndex = 10;
            Dimensions.TopText = "Dimensions";
            // 
            // Feed
            // 
            Feed.Controls.Add(userPropGrid3);
            Feed.Controls.Add(userPropGrid2);
            Feed.Location = new Point(4, 24);
            Feed.Name = "Feed";
            Feed.Padding = new Padding(3);
            Feed.Size = new Size(792, 422);
            Feed.TabIndex = 0;
            Feed.Text = "Feed";
            Feed.UseVisualStyleBackColor = true;
            // 
            // userPropGrid3
            // 
            userPropGrid3.AllowChangeEvent = true;
            userPropGrid3.AllowUserToAddRows = false;
            userPropGrid3.AllowUserToDeleteRows = false;
            userPropGrid3.ColumnNames = (List<string>)resources.GetObject("userPropGrid3.ColumnNames");
            userPropGrid3.DisplayTitles = true;
            userPropGrid3.FirstColumnWidth = 64;
            userPropGrid3.Location = new Point(19, 16);
            userPropGrid3.Margin = new Padding(4, 3, 4, 3);
            userPropGrid3.Name = "userPropGrid3";
            userPropGrid3.ReadOnly = false;
            userPropGrid3.RowHeadersVisible = false;
            userPropGrid3.RowNames = (List<string>)resources.GetObject("userPropGrid3.RowNames");
            userPropGrid3.Size = new Size(216, 307);
            userPropGrid3.TabIndex = 9;
            userPropGrid3.TopText = "Feed";
            // 
            // userPropGrid2
            // 
            userPropGrid2.AllowChangeEvent = true;
            userPropGrid2.AllowUserToAddRows = false;
            userPropGrid2.AllowUserToDeleteRows = false;
            userPropGrid2.ColumnNames = (List<string>)resources.GetObject("userPropGrid2.ColumnNames");
            userPropGrid2.DisplayTitles = true;
            userPropGrid2.FirstColumnWidth = 64;
            userPropGrid2.Location = new Point(243, 16);
            userPropGrid2.Margin = new Padding(4, 3, 4, 3);
            userPropGrid2.Name = "userPropGrid2";
            userPropGrid2.ReadOnly = false;
            userPropGrid2.RowHeadersVisible = false;
            userPropGrid2.RowNames = (List<string>)resources.GetObject("userPropGrid2.RowNames");
            userPropGrid2.Size = new Size(216, 307);
            userPropGrid2.TabIndex = 8;
            userPropGrid2.TopText = "Feed Definition";
            // 
            // OperatingConditions
            // 
            OperatingConditions.Location = new Point(4, 24);
            OperatingConditions.Name = "OperatingConditions";
            OperatingConditions.Size = new Size(792, 422);
            OperatingConditions.TabIndex = 5;
            OperatingConditions.Text = "Operating Conditions";
            OperatingConditions.UseVisualStyleBackColor = true;
            // 
            // Riser
            // 
            Riser.Location = new Point(4, 24);
            Riser.Name = "Riser";
            Riser.Size = new Size(792, 422);
            Riser.TabIndex = 3;
            Riser.Text = "Riser";
            Riser.UseVisualStyleBackColor = true;
            // 
            // Worksheet
            // 
            Worksheet.Location = new Point(4, 24);
            Worksheet.Name = "Worksheet";
            Worksheet.Size = new Size(792, 422);
            Worksheet.TabIndex = 2;
            Worksheet.Text = "Worksheet";
            Worksheet.UseVisualStyleBackColor = true;
            // 
            // Regenerator
            // 
            Regenerator.Location = new Point(4, 24);
            Regenerator.Name = "Regenerator";
            Regenerator.Size = new Size(792, 422);
            Regenerator.TabIndex = 4;
            Regenerator.Text = "Regenerator";
            Regenerator.UseVisualStyleBackColor = true;
            Regenerator.UseWaitCursor = true;
            // 
            // Results
            // 
            Results.Location = new Point(4, 24);
            Results.Name = "Results";
            Results.Size = new Size(792, 422);
            Results.TabIndex = 6;
            Results.Text = "Results";
            Results.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(tabControl1);
            Name = "Form1";
            Text = "Design";
            tabControl1.ResumeLayout(false);
            Design.ResumeLayout(false);
            Feed.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TabControl tabControl1;
        private TabPage Feed;
        private FormControls.UserPropGrid userPropGrid3;
        private FormControls.UserPropGrid userPropGrid2;
        private TabPage Design;
        private TabPage Worksheet;
        private TabPage Riser;
        private TabPage Regenerator;
        private FormControls.UserPropGrid Dimensions;
        private TabPage OperatingConditions;
        private TabPage Results;
        private FormControls.UserPropGrid CatFactors;
    }
}