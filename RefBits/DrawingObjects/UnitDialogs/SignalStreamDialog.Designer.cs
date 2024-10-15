namespace   Units.DrawingObjects.UnitDialogs
{
    partial class  SignalStreamDialog
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
            System.Windows.Forms.TreeNode treeNode1 = new  System.Windows.Forms.TreeNode("UnitOpProperty");
            System.Windows.Forms.TreeNode treeNode2 = new  System.Windows.Forms.TreeNode("StreamProperty");
            this.treeviewProptype = new  System.Windows.Forms.TreeView();
            this.treeViewProperty = new  System.Windows.Forms.TreeView();
            this.SuspendLayout();
            // 
            // treeviewProptype
            // 
            this.treeviewProptype.Location = new  System.Drawing.Point (59, 12);
            this.treeviewProptype.Name = "treeviewProptype";
            treeNode1.Name = "UnitOpProperty";
            treeNode1.Text = "UnitOpProperty";
            treeNode2.Name = "Stream Property";
            treeNode2.Text = "StreamProperty";
            this.treeviewProptype.Nodes.AddRange(new  System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode2});
            this.treeviewProptype.Size = new  System.Drawing.Size(280, 410);
            this.treeviewProptype.TabIndex = 0;
            this.treeviewProptype.AfterSelect += new  System.Windows.Forms.TreeViewEventHandler(this.treeviewProptype_AfterSelect);
            // 
            // treeViewProperty
            // 
            this.treeViewProperty.Location = new  System.Drawing.Point (59, 454);
            this.treeViewProperty.Name = "treeViewProperty";
            this.treeViewProperty.Size = new  System.Drawing.Size(280, 92);
            this.treeViewProperty.TabIndex = 1;
            this.treeViewProperty.AfterSelect += new  System.Windows.Forms.TreeViewEventHandler(this.treeView2_AfterSelect);
            // 
            // SignalStreamDialog
            // 
            this.AutoScaleDimensions = new  System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new  System.Drawing.Size(626, 629);
            this.Controls.Add(this.treeViewProperty);
            this.Controls.Add(this.treeviewProptype);
            this.Margin = new  System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "SignalStreamDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Signal Dialog";
            this.ResumeLayout(false);

        }

        #endregion

        private  System.Windows.Forms.TreeView treeviewProptype;
        private  System.Windows.Forms.TreeView treeViewProperty;
    }
}