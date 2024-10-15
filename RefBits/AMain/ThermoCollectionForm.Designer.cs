using   System.Windows.Forms;

namespace   Units
{
    public  partial class  ThermoCollectionForm
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
            System.ComponentModel.ComponentResourceManager resources = new  System.ComponentModel.ComponentResourceManager(typeof(ThermoCollectionForm));
            this.enumerationGrid1 = new  FormControls.EnumerationGrid();
            this.SuspendLayout();
            // 
            // enumerationGrid1
            // 
            this.enumerationGrid1.AllowUserToAddRows = false;
            this.enumerationGrid1.AllowUserToDeleteRows = false;
            this.enumerationGrid1.ColumnNames = ((System.Collections.Generic.List<string>)(resources.GetObject("enumerationGrid1.ColumnNames")));
            this.enumerationGrid1.DisplayTitles = true;
            this.enumerationGrid1.FirstColumnWidth = 5;
            this.enumerationGrid1.Location = new  System.Drawing.Point (13, 12);
            this.enumerationGrid1.Margin = new  System.Windows.Forms.Padding(4, 3, 4, 3);
            this.enumerationGrid1.Name = "enumerationGrid1";
            this.enumerationGrid1.RowHeadersVisible = false;
            this.enumerationGrid1.Size = new  System.Drawing.Size(483, 379);
            this.enumerationGrid1.TabIndex = 1;
            this.enumerationGrid1.TopText = "Options";
            // 
            // ThermoCollectionForm
            // 
            this.AutoScaleDimensions = new  System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new  System.Drawing.Size(520, 414);
            this.Controls.Add(this.enumerationGrid1);
            this.Name = "ThermoCollectionForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ThermoCollectionForm";
            this.ResumeLayout(false);

        }

        #endregion
        private  FormControls.EnumerationGrid enumerationGrid1;
    }
}