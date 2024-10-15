using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace Dialogs
{
    /// <summary>
    /// Summary description for SColDatadlg.
    /// </summary>
    public class SColDatadlg : Form
    {
        public string ActiveModel = "";
        private System.Data.OleDb.OleDbCommand oleDbSelectCommand1;
        private System.Data.OleDb.OleDbCommand oleDbInsertCommand1;
        private System.Data.OleDb.OleDbCommand oleDbUpdateCommand1;
        private System.Data.OleDb.OleDbCommand oleDbDeleteCommand1;
        private WindowsApplication1.ScolDataSetDlg objScolDataSetDlg;
        private System.Data.OleDb.OleDbConnection oleDbConnection1;
        private System.Data.OleDb.OleDbDataAdapter oleDbDataAdapter1;
        private Button btnLoad;
        private Button btnUpdate;
        private Button btnCancelAll;
        private Label mfp_lblColName;
        private Label mfp_lblICP1;
        private Label mfp_lblICP10;
        private Label mfp_lblICP2;
        private Label mfp_lblICP3;
        private Label mfp_lblICP4;
        private Label mfp_lblICP5;
        private Label mfp_lblICP6;
        private Label mfp_lblICP7;
        private Label mfp_lblICP8;
        private Label mfp_lblICP9;
        private TextBox mfp_editColName;
        private TextBox mfp_editICP1;
        private TextBox mfp_editICP10;
        private TextBox mfp_editICP2;
        private TextBox mfp_editICP3;
        private TextBox mfp_editICP4;
        private TextBox mfp_editICP5;
        private TextBox mfp_editICP6;
        private TextBox mfp_editICP7;
        private TextBox mfp_editICP8;
        private TextBox mfp_editICP9;
        private Label mfp_lblName1;
        private Label mfp_lblName10;
        private Label mfp_lblName2;
        private Label mfp_lblName3;
        private Label mfp_lblName4;
        private Label mfp_lblName5;
        private Label mfp_lblName6;
        private Label mfp_lblName7;
        private Label mfp_lblName8;
        private Label mfp_lblName9;
        private TextBox mfp_editName1;
        private TextBox mfp_editName10;
        private TextBox mfp_editName2;
        private TextBox mfp_editName3;
        private TextBox mfp_editName4;
        private TextBox mfp_editName5;
        private TextBox mfp_editName6;
        private TextBox mfp_editName7;
        private TextBox mfp_editName8;
        private TextBox mfp_editName9;
        private Button mfp_btnNavFirst;
        private Button mfp_btnNavPrev;
        private Label mfp_lblNavLocation;
        private Button mfp_btnNavNext;
        private Button mfp_btnNavLast;
        private Button mfp_btnAdd;
        private Button mfp_btnDelete;
        private Button mfp_btnCancel;
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container Components = null;

        public SColDatadlg()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            //
            // TODO: Add any constructor code after InitializeComponent call
            //
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (Components != null)
                {
                    Components.Dispose();
                }
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
            this.mfp_editICP4 = new TextBox();
            this.objScolDataSetDlg = new WindowsApplication1.ScolDataSetDlg();
            this.mfp_editICP5 = new TextBox();
            this.mfp_editICP6 = new TextBox();
            this.mfp_editICP7 = new TextBox();
            this.mfp_btnNavPrev = new Button();
            this.oleDbSelectCommand1 = new System.Data.OleDb.OleDbCommand();
            this.oleDbConnection1 = new System.Data.OleDb.OleDbConnection();
            this.mfp_btnAdd = new Button();
            this.btnCancelAll = new Button();
            this.mfp_btnNavNext = new Button();
            this.oleDbInsertCommand1 = new System.Data.OleDb.OleDbCommand();
            this.mfp_editName5 = new TextBox();
            this.mfp_lblName9 = new Label();
            this.mfp_lblNavLocation = new Label();
            this.mfp_lblColName = new Label();
            this.mfp_lblName8 = new Label();
            this.mfp_editName4 = new TextBox();
            this.mfp_editName7 = new TextBox();
            this.mfp_lblName3 = new Label();
            this.mfp_lblName1 = new Label();
            this.mfp_lblName7 = new Label();
            this.mfp_lblName6 = new Label();
            this.mfp_lblName5 = new Label();
            this.mfp_lblName4 = new Label();
            this.mfp_editName6 = new TextBox();
            this.mfp_editName1 = new TextBox();
            this.mfp_editName3 = new TextBox();
            this.mfp_editName2 = new TextBox();
            this.btnLoad = new Button();
            this.oleDbDeleteCommand1 = new System.Data.OleDb.OleDbCommand();
            this.mfp_editName9 = new TextBox();
            this.mfp_editName8 = new TextBox();
            this.oleDbDataAdapter1 = new System.Data.OleDb.OleDbDataAdapter();
            this.oleDbUpdateCommand1 = new System.Data.OleDb.OleDbCommand();
            this.mfp_lblICP7 = new Label();
            this.mfp_lblICP4 = new Label();
            this.mfp_lblICP5 = new Label();
            this.mfp_lblICP2 = new Label();
            this.mfp_lblICP3 = new Label();
            this.mfp_lblICP1 = new Label();
            this.mfp_editICP10 = new TextBox();
            this.mfp_lblICP8 = new Label();
            this.mfp_lblICP9 = new Label();
            this.mfp_btnDelete = new Button();
            this.mfp_lblName10 = new Label();
            this.mfp_btnNavFirst = new Button();
            this.btnUpdate = new Button();
            this.mfp_lblName2 = new Label();
            this.mfp_btnNavLast = new Button();
            this.mfp_lblICP10 = new Label();
            this.mfp_btnCancel = new Button();
            this.mfp_editICP8 = new TextBox();
            this.mfp_lblICP6 = new Label();
            this.mfp_editICP9 = new TextBox();
            this.mfp_editColName = new TextBox();
            this.mfp_editName10 = new TextBox();
            this.mfp_editICP1 = new TextBox();
            this.mfp_editICP2 = new TextBox();
            this.mfp_editICP3 = new TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.objScolDataSetDlg)).BeginInit();
            this.SuspendLayout();
            //
            // mfp_editICP4
            //
            this.mfp_editICP4.DataBindings.Add(new Binding("Text", this.objScolDataSetDlg, "SimpleColumns.ICP4"));
            this.mfp_editICP4.Location = new System.Drawing.Point(120, 200);
            this.mfp_editICP4.Name = "mfp_editICP4";
            this.mfp_editICP4.TabIndex = 19;
            this.mfp_editICP4.Text = "";
            //
            // objScolDataSetDlg
            //
            this.objScolDataSetDlg.DataSetName = "ScolDataSetDlg";
            this.objScolDataSetDlg.Locale = new System.Globalization.CultureInfo("en-US");
            this.objScolDataSetDlg.namespace   = "http://www.tempuri.org/ScolDataSetDlg.xsd";
            //
            // mfp_editICP5
            //
            this.mfp_editICP5.DataBindings.Add(new Binding("Text", this.objScolDataSetDlg, "SimpleColumns.ICP5"));

            this.mfp_editICP5.Location = new System.Drawing.Point(120, 232);
			this.mfp_editICP5.Name = "mfp_editICP5";
			this.mfp_editICP5.TabIndex = 20;
			this.mfp_editICP5.Text = "";
			//
			// mfp_editICP6
			//
			this.mfp_editICP6.DataBindings.Add(new Binding("Text", this.objScolDataSetDlg, "SimpleColumns.ICP6"));

            this.mfp_editICP6.Location = new System.Drawing.Point(120, 264);
			this.mfp_editICP6.Name = "mfp_editICP6";
			this.mfp_editICP6.TabIndex = 21;
			this.mfp_editICP6.Text = "";
			//
			// mfp_editICP7
			//
			this.mfp_editICP7.DataBindings.Add(new Binding("Text", this.objScolDataSetDlg, "SimpleColumns.ICP7"));

            this.mfp_editICP7.Location = new System.Drawing.Point(120, 296);
			this.mfp_editICP7.Name = "mfp_editICP7";
			this.mfp_editICP7.TabIndex = 22;
			this.mfp_editICP7.Text = "";
			//
			// mfp_btnNavPrev
			//
			this.mfp_btnNavPrev.Location = new System.Drawing.Point(235, 439);

            this.mfp_btnNavPrev.Name = "mfp_btnNavPrev";
			this.mfp_btnNavPrev.Size = new System.Drawing.Size(35, 23);

            this.mfp_btnNavPrev.TabIndex = 46;
			this.mfp_btnNavPrev.Text = "<";
			this.mfp_btnNavPrev.Click += new System.EventHandler(this.mfp_btnNavPrev_Click);
			//
			// oleDbSelectCommand1
			//
			this.oleDbSelectCommand1.CommandText = "SELECT ColName, ICP1, ICP10, ICP2, ICP3, ICP4, ICP5, ICP6, ICP7, ICP8, ICP9, Name" +
				"1, Name10, Name2, Name3, Name4, Name5, Name6, Name7, Name8, Name9 FROM SimpleCol" +
				"umns";
			this.oleDbSelectCommand1.Connection = this.oleDbConnection1;
			//
			// oleDbConnection1
			//
			this.oleDbConnection1.ConnectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Password="";User ID=Admin;Data Source=C:\Documents and Settings\Administrator\My Documents\Visual Studio Projects\C#Simulation\SimData.mdb;Mode=Share Deny None;Extended Properties="";Jet OLEDB:System database="";Jet OLEDB:Registry Path="";Jet OLEDB:Database Password="";Jet OLEDB:Engine Type=5;Jet OLEDB:Database Locking Mode=1;Jet OLEDB:Global Partial Bulk Ops=2;Jet OLEDB:Global Bulk Transactions=1;Jet OLEDB:new  Database Password="";Jet OLEDB:Create System Database=False;Jet OLEDB:Encrypt Database=False;Jet OLEDB:Don't Copy Locale on Compact=False;Jet OLEDB:Compact Without Replica Repair=False;Jet OLEDB:SFP=False";
			//
			// mfp_btnAdd
			//
			this.mfp_btnAdd.Location = new System.Drawing.Point(195, 472);

            this.mfp_btnAdd.Name = "mfp_btnAdd";
			this.mfp_btnAdd.TabIndex = 50;
			this.mfp_btnAdd.Text = "&Add";
			this.mfp_btnAdd.Click += new System.EventHandler(this.mfp_btnAdd_Click);
			//
			// btnCancelAll
			//
			this.btnCancelAll.Location = new System.Drawing.Point(365, 43);

            this.btnCancelAll.Name = "btnCancelAll";
			this.btnCancelAll.TabIndex = 2;
			this.btnCancelAll.Text = "Ca&ncel All";
			this.btnCancelAll.Click += new System.EventHandler(this.btnCancelAll_Click);
			//
			// mfp_btnNavNext
			//
			this.mfp_btnNavNext.Location = new System.Drawing.Point(365, 439);

            this.mfp_btnNavNext.Name = "mfp_btnNavNext";
			this.mfp_btnNavNext.Size = new System.Drawing.Size(35, 23);

            this.mfp_btnNavNext.TabIndex = 48;
			this.mfp_btnNavNext.Text = ">";
			this.mfp_btnNavNext.Click += new System.EventHandler(this.mfp_btnNavNext_Click);
			//
			// oleDbInsertCommand1
			//
			this.oleDbInsertCommand1.CommandText = "INSERT int O SimpleColumns(ColName, ICP1, ICP10, ICP2, ICP3, ICP4, ICP5, ICP6, ICP" +
				"7, ICP8, ICP9, Name1, Name10, Name2, Name3, Name4, Name5, Name6, Name7, Name8, N" +
				"ame9) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)";
			this.oleDbInsertCommand1.Connection = this.oleDbConnection1;
			this.oleDbInsertCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("ColName", System.Data.OleDb.OleDbType.Char, 50, "ColName"));

            this.oleDbInsertCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("ICP1", System.Data.OleDb.OleDbType.Numeric, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(10)), ((System.Byte)(0)), "ICP1", System.Data.DataRowVersion.Current, null));

            this.oleDbInsertCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("ICP10", System.Data.OleDb.OleDbType.Numeric, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(10)), ((System.Byte)(0)), "ICP10", System.Data.DataRowVersion.Current, null));

            this.oleDbInsertCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("ICP2", System.Data.OleDb.OleDbType.Numeric, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(10)), ((System.Byte)(0)), "ICP2", System.Data.DataRowVersion.Current, null));

            this.oleDbInsertCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("ICP3", System.Data.OleDb.OleDbType.Numeric, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(10)), ((System.Byte)(0)), "ICP3", System.Data.DataRowVersion.Current, null));

            this.oleDbInsertCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("ICP4", System.Data.OleDb.OleDbType.Numeric, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(10)), ((System.Byte)(0)), "ICP4", System.Data.DataRowVersion.Current, null));

            this.oleDbInsertCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("ICP5", System.Data.OleDb.OleDbType.Numeric, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(10)), ((System.Byte)(0)), "ICP5", System.Data.DataRowVersion.Current, null));

            this.oleDbInsertCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("ICP6", System.Data.OleDb.OleDbType.Numeric, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(10)), ((System.Byte)(0)), "ICP6", System.Data.DataRowVersion.Current, null));

            this.oleDbInsertCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("ICP7", System.Data.OleDb.OleDbType.Numeric, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(10)), ((System.Byte)(0)), "ICP7", System.Data.DataRowVersion.Current, null));

            this.oleDbInsertCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("ICP8", System.Data.OleDb.OleDbType.Numeric, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(10)), ((System.Byte)(0)), "ICP8", System.Data.DataRowVersion.Current, null));

            this.oleDbInsertCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("ICP9", System.Data.OleDb.OleDbType.Numeric, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(10)), ((System.Byte)(0)), "ICP9", System.Data.DataRowVersion.Current, null));

            this.oleDbInsertCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("Name1", System.Data.OleDb.OleDbType.Char, 50, "Name1"));

            this.oleDbInsertCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("Name10", System.Data.OleDb.OleDbType.Char, 50, "Name10"));

            this.oleDbInsertCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("Name2", System.Data.OleDb.OleDbType.Char, 50, "Name2"));

            this.oleDbInsertCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("Name3", System.Data.OleDb.OleDbType.Char, 50, "Name3"));

            this.oleDbInsertCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("Name4", System.Data.OleDb.OleDbType.Char, 50, "Name4"));

            this.oleDbInsertCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("Name5", System.Data.OleDb.OleDbType.Char, 50, "Name5"));

            this.oleDbInsertCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("Name6", System.Data.OleDb.OleDbType.Char, 50, "Name6"));

            this.oleDbInsertCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("Name7", System.Data.OleDb.OleDbType.Char, 50, "Name7"));

            this.oleDbInsertCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("Name8", System.Data.OleDb.OleDbType.Char, 50, "Name8"));

            this.oleDbInsertCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("Name9", System.Data.OleDb.OleDbType.Char, 50, "Name9"));
            //
            // mfp_editName5
            //
            this.mfp_editName5.DataBindings.Add(new Binding("Text", this.objScolDataSetDlg, "SimpleColumns.Name5"));

            this.mfp_editName5.Location = new System.Drawing.Point(340, 232);
			this.mfp_editName5.Name = "mfp_editName5";
			this.mfp_editName5.TabIndex = 40;
			this.mfp_editName5.Text = "";
			//
			// mfp_lblName9
			//
			this.mfp_lblName9.Location = new System.Drawing.Point(230, 360);

            this.mfp_lblName9.Name = "mfp_lblName9";
			this.mfp_lblName9.Size = new System.Drawing.Size(58, 23);

            this.mfp_lblName9.TabIndex = 34;
			this.mfp_lblName9.Text = "Name9";
			//
			// mfp_lblNavLocation
			//
			this.mfp_lblNavLocation.BackColor = System.Drawing.Color.White;
			this.mfp_lblNavLocation.Location = new System.Drawing.Point(270, 439);

            this.mfp_lblNavLocation.Name = "mfp_lblNavLocation";
			this.mfp_lblNavLocation.Size = new System.Drawing.Size(95, 23);

            this.mfp_lblNavLocation.TabIndex = 47;
			this.mfp_lblNavLocation.Text = "No Records";
			this.mfp_lblNavLocation.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			//
			// mfp_lblColName
			//
			this.mfp_lblColName.Location = new System.Drawing.Point(10, 64);

            this.mfp_lblColName.Name = "mfp_lblColName";
			this.mfp_lblColName.Size = new System.Drawing.Size(54, 23);

            this.mfp_lblColName.TabIndex = 3;
			this.mfp_lblColName.Text = "ColName";
			//
			// mfp_lblName8
			//
			this.mfp_lblName8.Location = new System.Drawing.Point(230, 328);

            this.mfp_lblName8.Name = "mfp_lblName8";
			this.mfp_lblName8.Size = new System.Drawing.Size(58, 23);

            this.mfp_lblName8.TabIndex = 33;
			this.mfp_lblName8.Text = "Name8";
			//
			// mfp_editName4
			//
			this.mfp_editName4.DataBindings.Add(new Binding("Text", this.objScolDataSetDlg, "SimpleColumns.Name4"));

            this.mfp_editName4.Location = new System.Drawing.Point(340, 200);
			this.mfp_editName4.Name = "mfp_editName4";
			this.mfp_editName4.TabIndex = 39;
			this.mfp_editName4.Text = "";
			//
			// mfp_editName7
			//
			this.mfp_editName7.DataBindings.Add(new Binding("Text", this.objScolDataSetDlg, "SimpleColumns.Name7"));

            this.mfp_editName7.Location = new System.Drawing.Point(340, 296);
			this.mfp_editName7.Name = "mfp_editName7";
			this.mfp_editName7.TabIndex = 42;
			this.mfp_editName7.Text = "";
			//
			// mfp_lblName3
			//
			this.mfp_lblName3.Location = new System.Drawing.Point(230, 168);

            this.mfp_lblName3.Name = "mfp_lblName3";
			this.mfp_lblName3.Size = new System.Drawing.Size(58, 23);

            this.mfp_lblName3.TabIndex = 28;
			this.mfp_lblName3.Text = "Name3";
			//
			// mfp_lblName1
			//
			this.mfp_lblName1.Location = new System.Drawing.Point(230, 104);

            this.mfp_lblName1.Name = "mfp_lblName1";
			this.mfp_lblName1.Size = new System.Drawing.Size(58, 23);

            this.mfp_lblName1.TabIndex = 25;
			this.mfp_lblName1.Text = "Name1";
			//
			// mfp_lblName7
			//
			this.mfp_lblName7.Location = new System.Drawing.Point(230, 296);

            this.mfp_lblName7.Name = "mfp_lblName7";
			this.mfp_lblName7.Size = new System.Drawing.Size(58, 23);

            this.mfp_lblName7.TabIndex = 32;
			this.mfp_lblName7.Text = "Name7";
			//
			// mfp_lblName6
			//
			this.mfp_lblName6.Location = new System.Drawing.Point(230, 264);

            this.mfp_lblName6.Name = "mfp_lblName6";
			this.mfp_lblName6.Size = new System.Drawing.Size(58, 23);

            this.mfp_lblName6.TabIndex = 31;
			this.mfp_lblName6.Text = "Name6";
			//
			// mfp_lblName5
			//
			this.mfp_lblName5.Location = new System.Drawing.Point(230, 232);

            this.mfp_lblName5.Name = "mfp_lblName5";
			this.mfp_lblName5.Size = new System.Drawing.Size(58, 23);

            this.mfp_lblName5.TabIndex = 30;
			this.mfp_lblName5.Text = "Name5";
			//
			// mfp_lblName4
			//
			this.mfp_lblName4.Location = new System.Drawing.Point(230, 200);

            this.mfp_lblName4.Name = "mfp_lblName4";
			this.mfp_lblName4.Size = new System.Drawing.Size(58, 23);

            this.mfp_lblName4.TabIndex = 29;
			this.mfp_lblName4.Text = "Name4";
			//
			// mfp_editName6
			//
			this.mfp_editName6.DataBindings.Add(new Binding("Text", this.objScolDataSetDlg, "SimpleColumns.Name6"));

            this.mfp_editName6.Location = new System.Drawing.Point(340, 264);
			this.mfp_editName6.Name = "mfp_editName6";
			this.mfp_editName6.TabIndex = 41;
			this.mfp_editName6.Text = "";
			//
			// mfp_editName1
			//
			this.mfp_editName1.DataBindings.Add(new Binding("Text", this.objScolDataSetDlg, "SimpleColumns.Name1"));

            this.mfp_editName1.Location = new System.Drawing.Point(340, 104);
			this.mfp_editName1.Name = "mfp_editName1";
			this.mfp_editName1.TabIndex = 35;
			this.mfp_editName1.Text = "";
			//
			// mfp_editName3
			//
			this.mfp_editName3.DataBindings.Add(new Binding("Text", this.objScolDataSetDlg, "SimpleColumns.Name3"));

            this.mfp_editName3.Location = new System.Drawing.Point(340, 168);
			this.mfp_editName3.Name = "mfp_editName3";
			this.mfp_editName3.TabIndex = 38;
			this.mfp_editName3.Text = "";
			//
			// mfp_editName2
			//
			this.mfp_editName2.DataBindings.Add(new Binding("Text", this.objScolDataSetDlg, "SimpleColumns.Name2"));

            this.mfp_editName2.Location = new System.Drawing.Point(340, 136);
			this.mfp_editName2.Name = "mfp_editName2";
			this.mfp_editName2.TabIndex = 37;
			this.mfp_editName2.Text = "";
			//
			// btnLoad
			//
			this.btnLoad.Location = new System.Drawing.Point(10, 10);

            this.btnLoad.Name = "btnLoad";
			this.btnLoad.TabIndex = 0;
			this.btnLoad.Text = "&Load";
			this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
			//
			// oleDbDeleteCommand1
			//
			this.oleDbDeleteCommand1.CommandText = @"DELETE FROM SimpleColumns WHERE (ColName = ?) AND (ICP1 = ?) AND (ICP10 = ?) AND (ICP2 = ?) AND (ICP3 = ?) AND (ICP4 = ?) AND (ICP5 = ?) AND (ICP6 = ?) AND (ICP7 = ?) AND (ICP8 = ?) AND (ICP9 = ?) AND (Name1 = ?) AND (Name10 = ?) AND (Name2 = ?) AND (Name3 = ?) AND (Name4 = ?) AND (Name5 = ?) AND (Name6 = ?) AND (Name7 = ?) AND (Name8 = ?) AND (Name9 = ?)";
			this.oleDbDeleteCommand1.Connection = this.oleDbConnection1;
			this.oleDbDeleteCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("ColName", System.Data.OleDb.OleDbType.Char, 50, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "ColName", System.Data.DataRowVersion.Original, null));

            this.oleDbDeleteCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("ICP1", System.Data.OleDb.OleDbType.Numeric, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(10)), ((System.Byte)(0)), "ICP1", System.Data.DataRowVersion.Original, null));

            this.oleDbDeleteCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("ICP10", System.Data.OleDb.OleDbType.Numeric, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(10)), ((System.Byte)(0)), "ICP10", System.Data.DataRowVersion.Original, null));

            this.oleDbDeleteCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("ICP2", System.Data.OleDb.OleDbType.Numeric, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(10)), ((System.Byte)(0)), "ICP2", System.Data.DataRowVersion.Original, null));

            this.oleDbDeleteCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("ICP3", System.Data.OleDb.OleDbType.Numeric, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(10)), ((System.Byte)(0)), "ICP3", System.Data.DataRowVersion.Original, null));

            this.oleDbDeleteCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("ICP4", System.Data.OleDb.OleDbType.Numeric, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(10)), ((System.Byte)(0)), "ICP4", System.Data.DataRowVersion.Original, null));

            this.oleDbDeleteCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("ICP5", System.Data.OleDb.OleDbType.Numeric, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(10)), ((System.Byte)(0)), "ICP5", System.Data.DataRowVersion.Original, null));

            this.oleDbDeleteCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("ICP6", System.Data.OleDb.OleDbType.Numeric, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(10)), ((System.Byte)(0)), "ICP6", System.Data.DataRowVersion.Original, null));

            this.oleDbDeleteCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("ICP7", System.Data.OleDb.OleDbType.Numeric, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(10)), ((System.Byte)(0)), "ICP7", System.Data.DataRowVersion.Original, null));

            this.oleDbDeleteCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("ICP8", System.Data.OleDb.OleDbType.Numeric, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(10)), ((System.Byte)(0)), "ICP8", System.Data.DataRowVersion.Original, null));

            this.oleDbDeleteCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("ICP9", System.Data.OleDb.OleDbType.Numeric, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(10)), ((System.Byte)(0)), "ICP9", System.Data.DataRowVersion.Original, null));

            this.oleDbDeleteCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("Name1", System.Data.OleDb.OleDbType.Char, 50, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "Name1", System.Data.DataRowVersion.Original, null));

            this.oleDbDeleteCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("Name10", System.Data.OleDb.OleDbType.Char, 50, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "Name10", System.Data.DataRowVersion.Original, null));

            this.oleDbDeleteCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("Name2", System.Data.OleDb.OleDbType.Char, 50, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "Name2", System.Data.DataRowVersion.Original, null));

            this.oleDbDeleteCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("Name3", System.Data.OleDb.OleDbType.Char, 50, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "Name3", System.Data.DataRowVersion.Original, null));

            this.oleDbDeleteCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("Name4", System.Data.OleDb.OleDbType.Char, 50, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "Name4", System.Data.DataRowVersion.Original, null));

            this.oleDbDeleteCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("Name5", System.Data.OleDb.OleDbType.Char, 50, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "Name5", System.Data.DataRowVersion.Original, null));

            this.oleDbDeleteCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("Name6", System.Data.OleDb.OleDbType.Char, 50, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "Name6", System.Data.DataRowVersion.Original, null));

            this.oleDbDeleteCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("Name7", System.Data.OleDb.OleDbType.Char, 50, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "Name7", System.Data.DataRowVersion.Original, null));

            this.oleDbDeleteCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("Name8", System.Data.OleDb.OleDbType.Char, 50, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "Name8", System.Data.DataRowVersion.Original, null));

            this.oleDbDeleteCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("Name9", System.Data.OleDb.OleDbType.Char, 50, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "Name9", System.Data.DataRowVersion.Original, null));
            //
            // mfp_editName9
            //
            this.mfp_editName9.DataBindings.Add(new Binding("Text", this.objScolDataSetDlg, "SimpleColumns.Name9"));

            this.mfp_editName9.Location = new System.Drawing.Point(340, 360);
			this.mfp_editName9.Name = "mfp_editName9";
			this.mfp_editName9.TabIndex = 44;
			this.mfp_editName9.Text = "";
			//
			// mfp_editName8
			//
			this.mfp_editName8.DataBindings.Add(new Binding("Text", this.objScolDataSetDlg, "SimpleColumns.Name8"));

            this.mfp_editName8.Location = new System.Drawing.Point(340, 328);
			this.mfp_editName8.Name = "mfp_editName8";
			this.mfp_editName8.TabIndex = 43;
			this.mfp_editName8.Text = "";
			//
			// oleDbDataAdapter1
			//
			this.oleDbDataAdapter1.DeleteCommand = this.oleDbDeleteCommand1;
			this.oleDbDataAdapter1.InsertCommand = this.oleDbInsertCommand1;
			this.oleDbDataAdapter1.SelectCommand = this.oleDbSelectCommand1;
			this.oleDbDataAdapter1.TableMappings.AddRange(new System.Data.Common.DataTableMapping[] {
																										new System.Data.Common.DataTableMapping("Table", "SimpleColumns", new System.Data.Common.DataColumnMapping[] {
																																																						 new System.Data.Common.DataColumnMapping("ColName", "ColName"),
																																																						 new System.Data.Common.DataColumnMapping("ICP1", "ICP1"),
																																																						 new System.Data.Common.DataColumnMapping("ICP2", "ICP2"),
																																																						 new System.Data.Common.DataColumnMapping("ICP3", "ICP3"),
																																																						 new System.Data.Common.DataColumnMapping("ICP4", "ICP4"),
																																																						 new System.Data.Common.DataColumnMapping("ICP5", "ICP5"),
																																																						 new System.Data.Common.DataColumnMapping("ICP6", "ICP6"),
																																																						 new System.Data.Common.DataColumnMapping("ICP7", "ICP7"),
																																																						 new System.Data.Common.DataColumnMapping("ICP8", "ICP8"),
																																																						 new System.Data.Common.DataColumnMapping("ICP9", "ICP9"),
																																																						 new System.Data.Common.DataColumnMapping("ICP10", "ICP10"),
																																																						 new System.Data.Common.DataColumnMapping("Name1", "Name1"),
																																																						 new System.Data.Common.DataColumnMapping("Name2", "Name2"),
																																																						 new System.Data.Common.DataColumnMapping("Name3", "Name3"),
																																																						 new System.Data.Common.DataColumnMapping("Name4", "Name4"),
																																																						 new System.Data.Common.DataColumnMapping("Name5", "Name5"),
																																																						 new System.Data.Common.DataColumnMapping("Name6", "Name6"),
																																																						 new System.Data.Common.DataColumnMapping("Name7", "Name7"),
																																																						 new System.Data.Common.DataColumnMapping("Name8", "Name8"),
																																																						 new System.Data.Common.DataColumnMapping("Name9", "Name9"),
																																																						 new System.Data.Common.DataColumnMapping("Name10", "Name10")})});
this.oleDbDataAdapter1.UpdateCommand = this.oleDbUpdateCommand1;
//
// oleDbUpdateCommand1
//
this.oleDbUpdateCommand1.CommandText = @"UPDATE SimpleColumns SET ColName = ?, ICP1 = ?, ICP10 = ?, ICP2 = ?, ICP3 = ?, ICP4 = ?, ICP5 = ?, ICP6 = ?, ICP7 = ?, ICP8 = ?, ICP9 = ?, Name1 = ?, Name10 = ?, Name2 = ?, Name3 = ?, Name4 = ?, Name5 = ?, Name6 = ?, Name7 = ?, Name8 = ?, Name9 = ? WHERE (ColName = ?) AND (ICP1 = ?) AND (ICP10 = ?) AND (ICP2 = ?) AND (ICP3 = ?) AND (ICP4 = ?) AND (ICP5 = ?) AND (ICP6 = ?) AND (ICP7 = ?) AND (ICP8 = ?) AND (ICP9 = ?) AND (Name1 = ?) AND (Name10 = ?) AND (Name2 = ?) AND (Name3 = ?) AND (Name4 = ?) AND (Name5 = ?) AND (Name6 = ?) AND (Name7 = ?) AND (Name8 = ?) AND (Name9 = ?)";
this.oleDbUpdateCommand1.Connection = this.oleDbConnection1;
this.oleDbUpdateCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("ColName", System.Data.OleDb.OleDbType.Char, 50, "ColName"));
this.oleDbUpdateCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("ICP1", System.Data.OleDb.OleDbType.Numeric, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(10)), ((System.Byte)(0)), "ICP1", System.Data.DataRowVersion.Current, null));
this.oleDbUpdateCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("ICP10", System.Data.OleDb.OleDbType.Numeric, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(10)), ((System.Byte)(0)), "ICP10", System.Data.DataRowVersion.Current, null));
this.oleDbUpdateCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("ICP2", System.Data.OleDb.OleDbType.Numeric, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(10)), ((System.Byte)(0)), "ICP2", System.Data.DataRowVersion.Current, null));
this.oleDbUpdateCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("ICP3", System.Data.OleDb.OleDbType.Numeric, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(10)), ((System.Byte)(0)), "ICP3", System.Data.DataRowVersion.Current, null));
this.oleDbUpdateCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("ICP4", System.Data.OleDb.OleDbType.Numeric, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(10)), ((System.Byte)(0)), "ICP4", System.Data.DataRowVersion.Current, null));
this.oleDbUpdateCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("ICP5", System.Data.OleDb.OleDbType.Numeric, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(10)), ((System.Byte)(0)), "ICP5", System.Data.DataRowVersion.Current, null));
this.oleDbUpdateCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("ICP6", System.Data.OleDb.OleDbType.Numeric, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(10)), ((System.Byte)(0)), "ICP6", System.Data.DataRowVersion.Current, null));
this.oleDbUpdateCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("ICP7", System.Data.OleDb.OleDbType.Numeric, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(10)), ((System.Byte)(0)), "ICP7", System.Data.DataRowVersion.Current, null));
this.oleDbUpdateCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("ICP8", System.Data.OleDb.OleDbType.Numeric, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(10)), ((System.Byte)(0)), "ICP8", System.Data.DataRowVersion.Current, null));
this.oleDbUpdateCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("ICP9", System.Data.OleDb.OleDbType.Numeric, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(10)), ((System.Byte)(0)), "ICP9", System.Data.DataRowVersion.Current, null));
this.oleDbUpdateCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("Name1", System.Data.OleDb.OleDbType.Char, 50, "Name1"));
this.oleDbUpdateCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("Name10", System.Data.OleDb.OleDbType.Char, 50, "Name10"));
this.oleDbUpdateCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("Name2", System.Data.OleDb.OleDbType.Char, 50, "Name2"));
this.oleDbUpdateCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("Name3", System.Data.OleDb.OleDbType.Char, 50, "Name3"));
this.oleDbUpdateCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("Name4", System.Data.OleDb.OleDbType.Char, 50, "Name4"));
this.oleDbUpdateCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("Name5", System.Data.OleDb.OleDbType.Char, 50, "Name5"));
this.oleDbUpdateCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("Name6", System.Data.OleDb.OleDbType.Char, 50, "Name6"));
this.oleDbUpdateCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("Name7", System.Data.OleDb.OleDbType.Char, 50, "Name7"));
this.oleDbUpdateCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("Name8", System.Data.OleDb.OleDbType.Char, 50, "Name8"));
this.oleDbUpdateCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("Name9", System.Data.OleDb.OleDbType.Char, 50, "Name9"));
this.oleDbUpdateCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_ColName", System.Data.OleDb.OleDbType.Char, 50, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "ColName", System.Data.DataRowVersion.Original, null));
this.oleDbUpdateCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_ICP1", System.Data.OleDb.OleDbType.Numeric, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(10)), ((System.Byte)(0)), "ICP1", System.Data.DataRowVersion.Original, null));
this.oleDbUpdateCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_ICP10", System.Data.OleDb.OleDbType.Numeric, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(10)), ((System.Byte)(0)), "ICP10", System.Data.DataRowVersion.Original, null));
this.oleDbUpdateCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_ICP2", System.Data.OleDb.OleDbType.Numeric, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(10)), ((System.Byte)(0)), "ICP2", System.Data.DataRowVersion.Original, null));
this.oleDbUpdateCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_ICP3", System.Data.OleDb.OleDbType.Numeric, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(10)), ((System.Byte)(0)), "ICP3", System.Data.DataRowVersion.Original, null));
this.oleDbUpdateCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_ICP4", System.Data.OleDb.OleDbType.Numeric, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(10)), ((System.Byte)(0)), "ICP4", System.Data.DataRowVersion.Original, null));
this.oleDbUpdateCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_ICP5", System.Data.OleDb.OleDbType.Numeric, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(10)), ((System.Byte)(0)), "ICP5", System.Data.DataRowVersion.Original, null));
this.oleDbUpdateCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_ICP6", System.Data.OleDb.OleDbType.Numeric, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(10)), ((System.Byte)(0)), "ICP6", System.Data.DataRowVersion.Original, null));
this.oleDbUpdateCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_ICP7", System.Data.OleDb.OleDbType.Numeric, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(10)), ((System.Byte)(0)), "ICP7", System.Data.DataRowVersion.Original, null));
this.oleDbUpdateCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_ICP8", System.Data.OleDb.OleDbType.Numeric, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(10)), ((System.Byte)(0)), "ICP8", System.Data.DataRowVersion.Original, null));
this.oleDbUpdateCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_ICP9", System.Data.OleDb.OleDbType.Numeric, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(10)), ((System.Byte)(0)), "ICP9", System.Data.DataRowVersion.Original, null));
this.oleDbUpdateCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_Name1", System.Data.OleDb.OleDbType.Char, 50, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "Name1", System.Data.DataRowVersion.Original, null));
this.oleDbUpdateCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_Name10", System.Data.OleDb.OleDbType.Char, 50, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "Name10", System.Data.DataRowVersion.Original, null));
this.oleDbUpdateCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_Name2", System.Data.OleDb.OleDbType.Char, 50, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "Name2", System.Data.DataRowVersion.Original, null));
this.oleDbUpdateCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_Name3", System.Data.OleDb.OleDbType.Char, 50, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "Name3", System.Data.DataRowVersion.Original, null));
this.oleDbUpdateCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_Name4", System.Data.OleDb.OleDbType.Char, 50, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "Name4", System.Data.DataRowVersion.Original, null));
this.oleDbUpdateCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_Name5", System.Data.OleDb.OleDbType.Char, 50, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "Name5", System.Data.DataRowVersion.Original, null));
this.oleDbUpdateCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_Name6", System.Data.OleDb.OleDbType.Char, 50, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "Name6", System.Data.DataRowVersion.Original, null));
this.oleDbUpdateCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_Name7", System.Data.OleDb.OleDbType.Char, 50, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "Name7", System.Data.DataRowVersion.Original, null));
this.oleDbUpdateCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_Name8", System.Data.OleDb.OleDbType.Char, 50, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "Name8", System.Data.DataRowVersion.Original, null));
this.oleDbUpdateCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_Name9", System.Data.OleDb.OleDbType.Char, 50, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "Name9", System.Data.DataRowVersion.Original, null));
//
// mfp_lblICP7
//
this.mfp_lblICP7.Location = new System.Drawing.Point(10, 296);
this.mfp_lblICP7.Name = "mfp_lblICP7";
this.mfp_lblICP7.Size = new System.Drawing.Size(54, 23);
this.mfp_lblICP7.TabIndex = 11;
this.mfp_lblICP7.Text = "ICP7";
//
// mfp_lblICP4
//
this.mfp_lblICP4.Location = new System.Drawing.Point(10, 200);
this.mfp_lblICP4.Name = "mfp_lblICP4";
this.mfp_lblICP4.Size = new System.Drawing.Size(54, 23);
this.mfp_lblICP4.TabIndex = 8;
this.mfp_lblICP4.Text = "ICP4";
//
// mfp_lblICP5
//
this.mfp_lblICP5.Location = new System.Drawing.Point(10, 232);
this.mfp_lblICP5.Name = "mfp_lblICP5";
this.mfp_lblICP5.Size = new System.Drawing.Size(54, 23);
this.mfp_lblICP5.TabIndex = 9;
this.mfp_lblICP5.Text = "ICP5";
//
// mfp_lblICP2
//
this.mfp_lblICP2.Location = new System.Drawing.Point(10, 136);
this.mfp_lblICP2.Name = "mfp_lblICP2";
this.mfp_lblICP2.Size = new System.Drawing.Size(54, 23);
this.mfp_lblICP2.TabIndex = 6;
this.mfp_lblICP2.Text = "ICP2";
//
// mfp_lblICP3
//
this.mfp_lblICP3.Location = new System.Drawing.Point(10, 168);
this.mfp_lblICP3.Name = "mfp_lblICP3";
this.mfp_lblICP3.Size = new System.Drawing.Size(54, 23);
this.mfp_lblICP3.TabIndex = 7;
this.mfp_lblICP3.Text = "ICP3";
//
// mfp_lblICP1
//
this.mfp_lblICP1.Location = new System.Drawing.Point(10, 104);
this.mfp_lblICP1.Name = "mfp_lblICP1";
this.mfp_lblICP1.Size = new System.Drawing.Size(54, 23);
this.mfp_lblICP1.TabIndex = 4;
this.mfp_lblICP1.Text = "ICP1";
//
// mfp_editICP10
//
this.mfp_editICP10.DataBindings.Add(new Binding("Text", this.objScolDataSetDlg, "SimpleColumns.ICP10"));
this.mfp_editICP10.Location = new System.Drawing.Point(120, 392);
this.mfp_editICP10.Name = "mfp_editICP10";
this.mfp_editICP10.TabIndex = 16;
this.mfp_editICP10.Text = "";
//
// mfp_lblICP8
//
this.mfp_lblICP8.Location = new System.Drawing.Point(10, 328);
this.mfp_lblICP8.Name = "mfp_lblICP8";
this.mfp_lblICP8.Size = new System.Drawing.Size(54, 23);
this.mfp_lblICP8.TabIndex = 12;
this.mfp_lblICP8.Text = "ICP8";
//
// mfp_lblICP9
//
this.mfp_lblICP9.Location = new System.Drawing.Point(10, 360);
this.mfp_lblICP9.Name = "mfp_lblICP9";
this.mfp_lblICP9.Size = new System.Drawing.Size(54, 23);
this.mfp_lblICP9.TabIndex = 13;
this.mfp_lblICP9.Text = "ICP9";
//
// mfp_btnDelete
//
this.mfp_btnDelete.Location = new System.Drawing.Point(280, 472);
this.mfp_btnDelete.Name = "mfp_btnDelete";
this.mfp_btnDelete.TabIndex = 51;
this.mfp_btnDelete.Text = "&Delete";
this.mfp_btnDelete.Click += new System.EventHandler(this.mfp_btnDelete_Click);
//
// mfp_lblName10
//
this.mfp_lblName10.Location = new System.Drawing.Point(230, 392);
this.mfp_lblName10.Name = "mfp_lblName10";
this.mfp_lblName10.Size = new System.Drawing.Size(58, 23);
this.mfp_lblName10.TabIndex = 26;
this.mfp_lblName10.Text = "Name10";
//
// mfp_btnNavFirst
//
this.mfp_btnNavFirst.Location = new System.Drawing.Point(195, 439);
this.mfp_btnNavFirst.Name = "mfp_btnNavFirst";
this.mfp_btnNavFirst.Size = new System.Drawing.Size(40, 23);
this.mfp_btnNavFirst.TabIndex = 45;
this.mfp_btnNavFirst.Text = "<<";
this.mfp_btnNavFirst.Click += new System.EventHandler(this.mfp_btnNavFirst_Click);
//
// btnUpdate
//
this.btnUpdate.Location = new System.Drawing.Point(365, 10);
this.btnUpdate.Name = "btnUpdate";
this.btnUpdate.TabIndex = 1;
this.btnUpdate.Text = "&Update";
this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
//
// mfp_lblName2
//
this.mfp_lblName2.Location = new System.Drawing.Point(230, 136);
this.mfp_lblName2.Name = "mfp_lblName2";
this.mfp_lblName2.Size = new System.Drawing.Size(58, 23);
this.mfp_lblName2.TabIndex = 27;
this.mfp_lblName2.Text = "Name2";
//
// mfp_btnNavLast
//
this.mfp_btnNavLast.Location = new System.Drawing.Point(400, 439);
this.mfp_btnNavLast.Name = "mfp_btnNavLast";
this.mfp_btnNavLast.Size = new System.Drawing.Size(40, 23);
this.mfp_btnNavLast.TabIndex = 49;
this.mfp_btnNavLast.Text = ">>";
this.mfp_btnNavLast.Click += new System.EventHandler(this.mfp_btnNavLast_Click);
//
// mfp_lblICP10
//
this.mfp_lblICP10.Location = new System.Drawing.Point(10, 392);
this.mfp_lblICP10.Name = "mfp_lblICP10";
this.mfp_lblICP10.Size = new System.Drawing.Size(54, 23);
this.mfp_lblICP10.TabIndex = 5;
this.mfp_lblICP10.Text = "ICP10";
//
// mfp_btnCancel
//
this.mfp_btnCancel.Location = new System.Drawing.Point(365, 472);
this.mfp_btnCancel.Name = "mfp_btnCancel";
this.mfp_btnCancel.TabIndex = 52;
this.mfp_btnCancel.Text = "&Cancel";
this.mfp_btnCancel.Click += new System.EventHandler(this.mfp_btnCancel_Click);
//
// mfp_editICP8
//
this.mfp_editICP8.DataBindings.Add(new Binding("Text", this.objScolDataSetDlg, "SimpleColumns.ICP8"));
this.mfp_editICP8.Location = new System.Drawing.Point(120, 328);
this.mfp_editICP8.Name = "mfp_editICP8";
this.mfp_editICP8.TabIndex = 23;
this.mfp_editICP8.Text = "";
//
// mfp_lblICP6
//
this.mfp_lblICP6.Location = new System.Drawing.Point(10, 264);
this.mfp_lblICP6.Name = "mfp_lblICP6";
this.mfp_lblICP6.Size = new System.Drawing.Size(54, 23);
this.mfp_lblICP6.TabIndex = 10;
this.mfp_lblICP6.Text = "ICP6";
//
// mfp_editICP9
//
this.mfp_editICP9.DataBindings.Add(new Binding("Text", this.objScolDataSetDlg, "SimpleColumns.ICP9"));
this.mfp_editICP9.Location = new System.Drawing.Point(120, 360);
this.mfp_editICP9.Name = "mfp_editICP9";
this.mfp_editICP9.TabIndex = 24;
this.mfp_editICP9.Text = "";
//
// mfp_editColName
//
this.mfp_editColName.DataBindings.Add(new Binding("Text", this.objScolDataSetDlg, "SimpleColumns.ColName"));
this.mfp_editColName.Location = new System.Drawing.Point(120, 64);
this.mfp_editColName.Name = "mfp_editColName";
this.mfp_editColName.TabIndex = 14;
this.mfp_editColName.Text = "";
//
// mfp_editName10
//
this.mfp_editName10.DataBindings.Add(new Binding("Text", this.objScolDataSetDlg, "SimpleColumns.Name10"));
this.mfp_editName10.Location = new System.Drawing.Point(340, 392);
this.mfp_editName10.Name = "mfp_editName10";
this.mfp_editName10.TabIndex = 36;
this.mfp_editName10.Text = "";
//
// mfp_editICP1
//
this.mfp_editICP1.DataBindings.Add(new Binding("Text", this.objScolDataSetDlg, "SimpleColumns.ICP1"));
this.mfp_editICP1.Location = new System.Drawing.Point(120, 104);
this.mfp_editICP1.Name = "mfp_editICP1";
this.mfp_editICP1.TabIndex = 15;
this.mfp_editICP1.Text = "";
this.mfp_editICP1.TextChanged += new System.EventHandler(this.mfp_editICP1_TextChanged);
//
// mfp_editICP2
//
this.mfp_editICP2.DataBindings.Add(new Binding("Text", this.objScolDataSetDlg, "SimpleColumns.ICP2"));
this.mfp_editICP2.Location = new System.Drawing.Point(120, 136);
this.mfp_editICP2.Name = "mfp_editICP2";
this.mfp_editICP2.TabIndex = 17;
this.mfp_editICP2.Text = "";
//
// mfp_editICP3
//
this.mfp_editICP3.DataBindings.Add(new Binding("Text", this.objScolDataSetDlg, "SimpleColumns.ICP3"));
this.mfp_editICP3.Location = new System.Drawing.Point(120, 168);
this.mfp_editICP3.Name = "mfp_editICP3";
this.mfp_editICP3.TabIndex = 18;
this.mfp_editICP3.Text = "";
//
// SColDatadlg
//
this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
this.ClientSize = new System.Drawing.Size(456, 520);
this.Controls.AddRange(new Control[] {
                                                                          this.btnLoad,
                                                                          this.btnUpdate,
                                                                          this.btnCancelAll,
                                                                          this.mfp_lblColName,
                                                                          this.mfp_lblICP1,
                                                                          this.mfp_lblICP10,
                                                                          this.mfp_lblICP2,
                                                                          this.mfp_lblICP3,
                                                                          this.mfp_lblICP4,
                                                                          this.mfp_lblICP5,
                                                                          this.mfp_lblICP6,
                                                                          this.mfp_lblICP7,
                                                                          this.mfp_lblICP8,
                                                                          this.mfp_lblICP9,
                                                                          this.mfp_editColName,
                                                                          this.mfp_editICP1,
                                                                          this.mfp_editICP10,
                                                                          this.mfp_editICP2,
                                                                          this.mfp_editICP3,
                                                                          this.mfp_editICP4,
                                                                          this.mfp_editICP5,
                                                                          this.mfp_editICP6,
                                                                          this.mfp_editICP7,
                                                                          this.mfp_editICP8,
                                                                          this.mfp_editICP9,
                                                                          this.mfp_lblName1,
                                                                          this.mfp_lblName10,
                                                                          this.mfp_lblName2,
                                                                          this.mfp_lblName3,
                                                                          this.mfp_lblName4,
                                                                          this.mfp_lblName5,
                                                                          this.mfp_lblName6,
                                                                          this.mfp_lblName7,
                                                                          this.mfp_lblName8,
                                                                          this.mfp_lblName9,
                                                                          this.mfp_editName1,
                                                                          this.mfp_editName10,
                                                                          this.mfp_editName2,
                                                                          this.mfp_editName3,
                                                                          this.mfp_editName4,
                                                                          this.mfp_editName5,
                                                                          this.mfp_editName6,
                                                                          this.mfp_editName7,
                                                                          this.mfp_editName8,
                                                                          this.mfp_editName9,
                                                                          this.mfp_btnNavFirst,
                                                                          this.mfp_btnNavPrev,
                                                                          this.mfp_lblNavLocation,
                                                                          this.mfp_btnNavNext,
                                                                          this.mfp_btnNavLast,
                                                                          this.mfp_btnAdd,
                                                                          this.mfp_btnDelete,
                                                                          this.mfp_btnCancel});
this.Name = "SColDatadlg";
this.Text = "SColDatadlg";
this.Load += new System.EventHandler(this.SColDatadlg_Load);
((System.ComponentModel.ISupportInitialize)(this.objScolDataSetDlg)).EndInit();
this.ResumeLayout(false);
		}

		#endregion Windows Form Designer generated code

		public void FillDataSet(WindowsApplication1.ScolDataSetDlg dataSet)
{
    this.oleDbConnection1.Open();

    dataSet.EnforceConstraint s = false;
    try
    {
        this.oleDbDataAdapter1.Fill(dataSet);
    }
    catch (System.Exception fillException)
    {
        throw fillException;
    }
    finally
    {
        dataSet.EnforceConstraint s = true;
        this.oleDbConnection1.Close();
    }
}

public int UpdateDataSource(WindowsApplication1.ScolDataSetDlg dataSet)
{
    this.oleDbConnection1.Open();

    System.Data.DataSet UpdatedRows;
    System.Data.DataSet InsertedRows;
    System.Data.DataSet DeletedRows;
    int AffectedRows = 0;

    UpdatedRows = dataSet.GetChanges(System.Data.DataRowState.Modified);
    InsertedRows = dataSet.GetChanges(System.Data.DataRowState.Added);
    DeletedRows = dataSet.GetChanges(System.Data.DataRowState.Deleted);
    try
    {
        if ((UpdatedRows != null))
        {
            AffectedRows = oleDbDataAdapter1.Update(UpdatedRows);
        }
        if ((InsertedRows != null))
        {
            AffectedRows = (AffectedRows + oleDbDataAdapter1.Update(InsertedRows));
        }
        if ((DeletedRows != null))
        {
            AffectedRows = (AffectedRows + oleDbDataAdapter1.Update(DeletedRows));
        }
    }
    catch (System.Exception updateException)
    {
        throw updateException;
    }
    finally
    {
        this.oleDbConnection1.Close();
    }

    return AffectedRows;
}

public void LoadDataSet()
{
    WindowsApplication1.ScolDataSetDlg objDataSetTemp;
    objDataSetTemp = new WindowsApplication1.ScolDataSetDlg();
    try
    {
        // Execute the SelectCommand on the DatasetCommmand and fill the dataset
        this.FillDataSet(objDataSetTemp);
    }
    catch (System.Exception eFillDataSet)
    {
        // Add exception handling code here.
        throw eFillDataSet;
    }
    try
    {
        // Merge the records that were just pulled from the data store int o the main dataset
        objScolDataSetDlg.Merge(objDataSetTemp);
    }
    catch (System.Exception eLoadMerge)
    {
        // Add exception handling code here
        throw eLoadMerge;
    }
}

public void UpdateDataSet()
{
    // Get a new  dataset that holds only the changes that have been made to the main dataset
    WindowsApplication1.ScolDataSetDlg objDataSetChanges = new WindowsApplication1.ScolDataSetDlg();
    System.Data.DataSet objDataSetUpdated = new WindowsApplication1.ScolDataSetDlg();
    // Clear out the current edits
    this.BindingContext[objScolDataSetDlg, "SimpleColumns"].EndCurrentEdit();
    // Get a new  dataset that holds only the changes that have been made to the main dataset
    objDataSetChanges = ((WindowsApplication1.ScolDataSetDlg)(objScolDataSetDlg.GetChanges()));
    // Check to see if the objCustomersDatasetChanges holds any records
    if ((objDataSetChanges != null))
    {
        try
        {
            // Call the update method passing int he dataset and any parameters
            this.UpdateDataSource(objDataSetChanges);
        }
        catch (System.Exception eUpdate)
        {
            // If the update failed and is part of a transaction, this is the place to put your rollback
            throw eUpdate;
        }
        // If the update succeeded and is part of a transaction, this is the place to put your commit
        // Add code to Check the return  ed dataset - objCustomersDataSetUpdate for any errors that may have been
        // pushed int o the row object's error
        // Merge the return  ed changes back int o the main dataset
        try
        {
            objScolDataSetDlg.Merge(objDataSetUpdated);
        }
        catch (System.Exception eUpdateMerge)
        {
            // Add exception handling code here
            throw eUpdateMerge;
        }
        // Commit the changes that were just merged
        // This moves any rows marked as updated, inserted or changed to being marked as original values
        objScolDataSetDlg.AcceptChanges();
    }
}

private void btnCancelAll_Click(object sender, System.EventArgs e)
{
    this.objScolDataSetDlg.RejectChanges();
}

private void objScolDataSetDlg_PositionChanged()
{
    this.mfp_lblNavLocation.Text = ((((this.BindingContext[objScolDataSetDlg, "SimpleColumns"].Position + 1)).ToString() + " of  ")
        + this.BindingContext[objScolDataSetDlg, "SimpleColumns"].Count.ToString());
}

private void mfp_btnNavNext_Click(object sender, System.EventArgs e)
{
    this.BindingContext[objScolDataSetDlg, "SimpleColumns"].Position = (this.BindingContext[objScolDataSetDlg, "SimpleColumns"].Position + 1);
    this.objScolDataSetDlg_PositionChanged();
}

private void mfp_btnNavPrev_Click(object sender, System.EventArgs e)
{
    this.BindingContext[objScolDataSetDlg, "SimpleColumns"].Position = (this.BindingContext[objScolDataSetDlg, "SimpleColumns"].Position - 1);
    this.objScolDataSetDlg_PositionChanged();
}

private void mfp_btnNavLast_Click(object sender, System.EventArgs e)
{
    this.BindingContext[objScolDataSetDlg, "SimpleColumns"].Position = (this.objScolDataSetDlg.Tables["SimpleColumns"].Rows.Count - 1);
    this.objScolDataSetDlg_PositionChanged();
}

private void mfp_btnNavFirst_Click(object sender, System.EventArgs e)
{
    this.BindingContext[objScolDataSetDlg, "SimpleColumns"].Position = 0;
    this.objScolDataSetDlg_PositionChanged();
}

private void btnLoad_Click(object sender, System.EventArgs e)
{
    try
    {
        this.LoadDataSet();
    }
    catch (System.Exception eLoad)
    {
        MessageBox.Show(eLoad.Message);
    }
    this.objScolDataSetDlg_PositionChanged();
}

private void btnUpdate_Click(object sender, System.EventArgs e)
{
    try
    {
        this.UpdateDataSet();
    }
    catch (System.Exception eUpdate)
    {
        MessageBox.Show(eUpdate.Message);
    }
    this.objScolDataSetDlg_PositionChanged();
}

private void mfp_btnAdd_Click(object sender, System.EventArgs e)
{
    try
    {
        // Clear out the current edits
        this.BindingContext[objScolDataSetDlg, "SimpleColumns"].EndCurrentEdit();
        this.BindingContext[objScolDataSetDlg, "SimpleColumns"].Addnew();
    }
    catch (System.Exception eEndEdit)
    {
        MessageBox.Show(eEndEdit.Message);
    }
    this.objScolDataSetDlg_PositionChanged();
}

private void mfp_btnDelete_Click(object sender, System.EventArgs e)
{
    if ((this.BindingContext[objScolDataSetDlg, "SimpleColumns"].Count > 0))
    {
        this.BindingContext[objScolDataSetDlg, "SimpleColumns"].RemoveAt(this.BindingContext[objScolDataSetDlg, "SimpleColumns"].Position);
        this.objScolDataSetDlg_PositionChanged();
    }
}

private void mfp_btnCancel_Click(object sender, System.EventArgs e)
{
    this.BindingContext[objScolDataSetDlg, "SimpleColumns"].CancelCurrentEdit();
    this.objScolDataSetDlg_PositionChanged();
}

private void SColDatadlg_Load(object sender, System.EventArgs e)
{
    LoadDataSet();
}

private void mfp_editICP1_TextChanged(object sender, System.EventArgs e)
{
}
	}
}