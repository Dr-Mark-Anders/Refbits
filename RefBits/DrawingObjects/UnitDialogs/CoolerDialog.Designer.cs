using ModelEngine;
using System.Windows.Forms;
using ModelEngine;
using Units.PortForm;

namespace Units
{
partial class CoolerDialog
{
///<summary>
///Requireddesignervariable.
///</summary>
private System.ComponentModel.IContainer Components=null;

///<summary>
///Cleanupanyresourcesbeingused.
///</summary>
///<paramname="disposing">trueifmanagedresourcesshouldbedisposed;otherwise,false.</param>
protected override void  Dispose(bool disposing)
{
if(disposing&&(Components!=null))
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
private void  InitializeComponent()
{
System.ComponentModel.ComponentResourceManager resources=new System.ComponentModel.ComponentResourceManager(typeof(CoolerDialog));
this.tabControl1=new System.Windows.Forms.TabControl();
this.Summary=new System.Windows.Forms.TabPage();
this.Results=new FormControls.UserPropGrid();
this.Parameters=new FormControls.UserPropGrid();
this.Streams=new System.Windows.Forms.TabPage();
this.Worksheet=new Units.PortForm.PortsPropertyWorksheet();
this.tabControl1.SuspendLayout();
this.Summary.SuspendLayout();
this.Streams.SuspendLayout();
this.SuspendLayout();
//
//tabControl1
//
this.tabControl1.Controls.Add(this.Summary);
this.tabControl1.Controls.Add(this.Streams);
this.tabControl1.Dock=System.Windows.Forms.DockStyle.Fill;
this.tabControl1.Location=new System.Drawing.Point (0,0);
this.tabControl1.Margin=new System.Windows.Forms.Padding(2);
this.tabControl1.Name="tabControl1";
this.tabControl1.SelectedIndex=0;
this.tabControl1.Size=new System.Drawing.Size(910,507);
this.tabControl1.TabIndex=20;
//
//Summary
//
this.Summary.BackColor=System.Drawing.SystemColors.Control;
this.Summary.Controls.Add(this.Results);
this.Summary.Controls.Add(this.Parameters);
this.Summary.Location=new System.Drawing.Point (4,24);
this.Summary.Margin=new System.Windows.Forms.Padding(2);
this.Summary.Name="Summary";
this.Summary.Padding=new System.Windows.Forms.Padding(2);
this.Summary.Size=new System.Drawing.Size(902,479);
this.Summary.TabIndex=0;
this.Summary.Text="Summary";
//
//Results
//
this.Results.AllowUserToAddRows=false;
this.Results.AllowUserToDeleteRows=false;
this.Results.ColumnNames=((System.Collections.Generic.List<string>)(resources.GetObject("Results.ColumnNames")));
this.Results.DisplayTitles=true;
this.Results.FirstColumnWidth=64;
this.Results.Location=new System.Drawing.Point (358,41);
this.Results.Margin=new System.Windows.Forms.Padding(4,3,4,3);
this.Results.Name="Results";
this.Results.ReadOnly =false;
this.Results.RowHeadersVisible=false;
this.Results.RowNames=((System.Collections.Generic.List<string>)(resources.GetObject("Results.RowNames")));
this.Results.Size=new System.Drawing.Size(315,353);
this.Results.TabIndex=1;
this.Results.TopText="Results";
//
//Parameters
//
this.Parameters.AllowUserToAddRows=false;
this.Parameters.AllowUserToDeleteRows=false;
this.Parameters.ColumnNames=((System.Collections.Generic.List<string>)(resources.GetObject("Parameters.ColumnNames")));
this.Parameters.DisplayTitles=true;
this.Parameters.FirstColumnWidth=64;
this.Parameters.Location=new System.Drawing.Point (61,41);
this.Parameters.Margin=new System.Windows.Forms.Padding(4,3,4,3);
this.Parameters.Name="Parameters";
this.Parameters.ReadOnly =false;
this.Parameters.RowHeadersVisible=false;
this.Parameters.RowNames=((System.Collections.Generic.List<string>)(resources.GetObject("Parameters.RowNames")));
this.Parameters.Size=new System.Drawing.Size(216,127);
this.Parameters.TabIndex=0;
this.Parameters.TopText="Parameters";
//
//Streams
//
this.Streams.Controls.Add(this.Worksheet);
this.Streams.Location=new System.Drawing.Point (4,24);
this.Streams.Margin=new System.Windows.Forms.Padding(2);
this.Streams.Name="Streams";
this.Streams.Padding=new System.Windows.Forms.Padding(2);
this.Streams.Size=new System.Drawing.Size(902,479);
this.Streams.TabIndex=1;
this.Streams.Text="Streams";
this.Streams.UseVisualStyleBackColor=true;
//
//Worksheet
//
this.Worksheet.AutoSize=true;
this.Worksheet.Location=new System.Drawing.Point (2,0);
this.Worksheet.Margin=new System.Windows.Forms.Padding(2);
this.Worksheet.Name="Worksheet";
this.Worksheet.Size=new System.Drawing.Size(1040,538);
this.Worksheet.TabIndex=0;
//
//CoolerDialog
//
this.AutoScaleDimensions=new System.Drawing.SizeF(7F,15F);
this.AutoScaleMode=System.Windows.Forms.AutoScaleMode.Font;
this.ClientSize=new System.Drawing.Size(910,507);
this.Controls.Add(this.tabControl1);
this.Margin=new System.Windows.Forms.Padding(4,2,4,2);
this.Name="CoolerDialog";
this.Text="CoolerDialog";
this.tabControl1.ResumeLayout(false);
this.Summary.ResumeLayout(false);
this.Streams.ResumeLayout(false);
this.Streams.PerformLayout();
this.ResumeLayout(false);

}

#endregion
private TabControl tabControl1;
private TabPage Summary;
private TabPage Streams;
private PortsPropertyWorksheet Worksheet;
private FormControls.UserPropGrid Results;
private FormControls.UserPropGrid Parameters;
//private FlexTextBoxflexTextBoxDT;
}
}