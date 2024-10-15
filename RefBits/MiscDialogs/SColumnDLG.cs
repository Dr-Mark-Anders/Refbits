using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using Models;

namespace Dialogs
{
    /// <summary>
    /// Summary description for Form1.
    /// </summary>
    public class scolDLG : Form
    {
        private Label label1;
        private Label label2;
        private TextBox Stream4;
        private TextBox Stream5;
        private NumericUpDown ICP1;
        private TextBox Stream7;
        private TextBox Stream8;
        private TextBox Stream1;
        private TextBox Stream2;
        private TextBox Stream10;
        private TextBox Stream3;
        private TextBox Stream9;
        private TextBox Stream6;
        private NumericUpDown ICP2;
        private NumericUpDown ICP3;
        private NumericUpDown ICP4;
        private NumericUpDown ICP5;
        private NumericUpDown ICP6;
        private NumericUpDown ICP7;
        private NumericUpDown ICP8;
        private NumericUpDown ICP9;
        private NumericUpDown ICP10;
        private TextBox textBox1;
        private TextBox textBox2;
        private TextBox textBox3;
        private TextBox textBox4;
        private TextBox textBox5;
        private TextBox textBox6;
        private TextBox textBox7;
        private TextBox textBox8;
        private TextBox textBox9;
        private TextBox textBox10;
        private Label label3;
        private TextBox textBox11;
        private TextBox textBox12;
        private TextBox textBox13;
        private TextBox textBox14;
        private TextBox textBox15;
        private TextBox textBox16;
        private TextBox textBox17;
        private TextBox textBox18;
        private TextBox textBox19;
        private TextBox textBox20;
        private Label label4;
        private OilSColumn osc;
        private Button DoChart;
        private CheckBox checkBoxVI;
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container Components = null;

        public scolDLG(OilSColumn sc)
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
            ICP1.BaseValue = (decimal)sc.ICP[0];
            ICP2.BaseValue = (decimal)sc.ICP[1];
            ICP3.BaseValue = (decimal)sc.ICP[2];
            ICP4.BaseValue = (decimal)sc.ICP[3];
            ICP5.BaseValue = (decimal)sc.ICP[4];
            ICP6.BaseValue = (decimal)sc.ICP[5];
            ICP7.BaseValue = (decimal)sc.ICP[6];
            ICP8.BaseValue = (decimal)sc.ICP[7];
            ICP9.BaseValue = (decimal)sc.ICP[8];
            ICP10.BaseValue = (decimal)sc.ICP[9];
            this.osc = sc;
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
            this.Stream8 = new TextBox();
            this.Stream9 = new TextBox();
            this.Stream10 = new TextBox();
            this.ICP10 = new NumericUpDown();
            this.ICP2 = new NumericUpDown();
            this.ICP3 = new NumericUpDown();
            this.ICP4 = new NumericUpDown();
            this.ICP5 = new NumericUpDown();
            this.ICP6 = new NumericUpDown();
            this.ICP8 = new NumericUpDown();
            this.label1 = new Label();
            this.label2 = new Label();
            this.ICP1 = new NumericUpDown();
            this.Stream7 = new TextBox();
            this.Stream4 = new TextBox();
            this.Stream5 = new TextBox();
            this.Stream2 = new TextBox();
            this.Stream3 = new TextBox();
            this.ICP7 = new NumericUpDown();
            this.Stream1 = new TextBox();
            this.ICP9 = new NumericUpDown();
            this.Stream6 = new TextBox();
            this.textBox1 = new TextBox();
            this.textBox2 = new TextBox();
            this.textBox3 = new TextBox();
            this.textBox4 = new TextBox();
            this.textBox5 = new TextBox();
            this.textBox6 = new TextBox();
            this.textBox7 = new TextBox();
            this.textBox8 = new TextBox();
            this.textBox9 = new TextBox();
            this.textBox10 = new TextBox();
            this.label3 = new Label();
            this.textBox11 = new TextBox();
            this.textBox12 = new TextBox();
            this.textBox13 = new TextBox();
            this.textBox14 = new TextBox();
            this.textBox15 = new TextBox();
            this.textBox16 = new TextBox();
            this.textBox17 = new TextBox();
            this.textBox18 = new TextBox();
            this.textBox19 = new TextBox();
            this.textBox20 = new TextBox();
            this.label4 = new Label();
            this.DoChart = new Button();
            this.checkBoxVI = new CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.ICP10)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ICP2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ICP3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ICP4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ICP5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ICP6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ICP8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ICP1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ICP7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ICP9)).BeginInit();
            this.SuspendLayout();
            //
            // Stream8
            //
            this.Stream8.Location = new System.Drawing.Point(32, 216);
            this.Stream8.Name = "Stream8";
            this.Stream8.Size = new System.Drawing.Size(56, 20);
            this.Stream8.TabIndex = 1;
            this.Stream8.Text = "8";
            //
            // Stream9
            //
            this.Stream9.Location = new System.Drawing.Point(32, 240);
            this.Stream9.Name = "Stream9";
            this.Stream9.Size = new System.Drawing.Size(56, 20);
            this.Stream9.TabIndex = 1;
            this.Stream9.Text = "9";
            //
            // Stream10
            //
            this.Stream10.Location = new System.Drawing.Point(32, 264);
            this.Stream10.Name = "Stream10";
            this.Stream10.Size = new System.Drawing.Size(56, 20);
            this.Stream10.TabIndex = 1;
            this.Stream10.Text = "10";
            //
            // ICP10
            //
            this.ICP10.Location = new System.Drawing.Point(128, 264);
            this.ICP10.Maximum = new decimal(new int[] {
            850,
            0,
            0,
            0});
            this.ICP10.Minimum = new decimal(new int[] {
            36,
            0,
            0,
            0});
            this.ICP10.Name = "ICP10";
            this.ICP10.Size = new System.Drawing.Size(56, 20);
            this.ICP10.TabIndex = 2;
            this.ICP10.BaseValue = new decimal(new int[] {
            36,
            0,
            0,
            0});
            //
            // ICP2
            //
            this.ICP2.Location = new System.Drawing.Point(128, 72);
            this.ICP2.Maximum = new decimal(new int[] {
            850,
            0,
            0,
            0});
            this.ICP2.Minimum = new decimal(new int[] {
            36,
            0,
            0,
            0});
            this.ICP2.Name = "ICP2";
            this.ICP2.Size = new System.Drawing.Size(56, 20);
            this.ICP2.TabIndex = 2;
            this.ICP2.BaseValue = new decimal(new int[] {
            36,
            0,
            0,
            0});
            //
            // ICP3
            //
            this.ICP3.Location = new System.Drawing.Point(128, 96);
            this.ICP3.Maximum = new decimal(new int[] {
            850,
            0,
            0,
            0});
            this.ICP3.Minimum = new decimal(new int[] {
            36,
            0,
            0,
            0});
            this.ICP3.Name = "ICP3";
            this.ICP3.Size = new System.Drawing.Size(56, 20);
            this.ICP3.TabIndex = 2;
            this.ICP3.BaseValue = new decimal(new int[] {
            36,
            0,
            0,
            0});
            //
            // ICP4
            //
            this.ICP4.Location = new System.Drawing.Point(128, 120);
            this.ICP4.Maximum = new decimal(new int[] {
            850,
            0,
            0,
            0});
            this.ICP4.Minimum = new decimal(new int[] {
            36,
            0,
            0,
            0});
            this.ICP4.Name = "ICP4";
            this.ICP4.Size = new System.Drawing.Size(56, 20);
            this.ICP4.TabIndex = 2;
            this.ICP4.BaseValue = new decimal(new int[] {
            36,
            0,
            0,
            0});
            //
            // ICP5
            //
            this.ICP5.Location = new System.Drawing.Point(128, 144);
            this.ICP5.Maximum = new decimal(new int[] {
            850,
            0,
            0,
            0});
            this.ICP5.Minimum = new decimal(new int[] {
            36,
            0,
            0,
            0});
            this.ICP5.Name = "ICP5";
            this.ICP5.Size = new System.Drawing.Size(56, 20);
            this.ICP5.TabIndex = 2;
            this.ICP5.BaseValue = new decimal(new int[] {
            36,
            0,
            0,
            0});
            //
            // ICP6
            //
            this.ICP6.Location = new System.Drawing.Point(128, 168);
            this.ICP6.Maximum = new decimal(new int[] {
            850,
            0,
            0,
            0});
            this.ICP6.Minimum = new decimal(new int[] {
            36,
            0,
            0,
            0});
            this.ICP6.Name = "ICP6";
            this.ICP6.Size = new System.Drawing.Size(56, 20);
            this.ICP6.TabIndex = 2;
            this.ICP6.BaseValue = new decimal(new int[] {
            36,
            0,
            0,
            0});
            //
            // ICP8
            //
            this.ICP8.Location = new System.Drawing.Point(128, 216);
            this.ICP8.Maximum = new decimal(new int[] {
            850,
            0,
            0,
            0});
            this.ICP8.Minimum = new decimal(new int[] {
            36,
            0,
            0,
            0});
            this.ICP8.Name = "ICP8";
            this.ICP8.Size = new System.Drawing.Size(56, 20);
            this.ICP8.TabIndex = 2;
            this.ICP8.BaseValue = new decimal(new int[] {
            36,
            0,
            0,
            0});
            //
            // label1
            //
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(32, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 23);
            this.label1.TabIndex = 0;
            this.label1.Text = "Stream";
            //
            // label2
            //
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(128, 24);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 23);
            this.label2.TabIndex = 0;
            this.label2.Text = "ICP";
            //
            // ICP1
            //
            this.ICP1.Location = new System.Drawing.Point(128, 48);
            this.ICP1.Maximum = new decimal(new int[] {
            850,
            0,
            0,
            0});
            this.ICP1.Minimum = new decimal(new int[] {
            273,
            0,
            0,
            -2147483648});
            this.ICP1.Name = "ICP1";
            this.ICP1.Size = new System.Drawing.Size(56, 20);
            this.ICP1.TabIndex = 2;
            this.ICP1.BaseValue = new decimal(new int[] {
            273,
            0,
            0,
            -2147483648});
            //
            // Stream7
            //
            this.Stream7.Location = new System.Drawing.Point(32, 192);
            this.Stream7.Name = "Stream7";
            this.Stream7.Size = new System.Drawing.Size(56, 20);
            this.Stream7.TabIndex = 1;
            this.Stream7.Text = "7";
            //
            // Stream4
            //
            this.Stream4.Location = new System.Drawing.Point(32, 120);
            this.Stream4.Name = "Stream4";
            this.Stream4.Size = new System.Drawing.Size(56, 20);
            this.Stream4.TabIndex = 1;
            this.Stream4.Text = "4";
            //
            // Stream5
            //
            this.Stream5.Location = new System.Drawing.Point(32, 144);
            this.Stream5.Name = "Stream5";
            this.Stream5.Size = new System.Drawing.Size(56, 20);
            this.Stream5.TabIndex = 1;
            this.Stream5.Text = "5";
            //
            // Stream2
            //
            this.Stream2.Location = new System.Drawing.Point(32, 72);
            this.Stream2.Name = "Stream2";
            this.Stream2.Size = new System.Drawing.Size(56, 20);
            this.Stream2.TabIndex = 1;
            this.Stream2.Text = "2";
            //
            // Stream3
            //
            this.Stream3.Location = new System.Drawing.Point(32, 96);
            this.Stream3.Name = "Stream3";
            this.Stream3.Size = new System.Drawing.Size(56, 20);
            this.Stream3.TabIndex = 1;
            this.Stream3.Text = "3";
            //
            // ICP7
            //
            this.ICP7.Location = new System.Drawing.Point(128, 192);
            this.ICP7.Maximum = new decimal(new int[] {
            850,
            0,
            0,
            0});
            this.ICP7.Minimum = new decimal(new int[] {
            36,
            0,
            0,
            0});
            this.ICP7.Name = "ICP7";
            this.ICP7.Size = new System.Drawing.Size(56, 20);
            this.ICP7.TabIndex = 2;
            this.ICP7.BaseValue = new decimal(new int[] {
            36,
            0,
            0,
            0});
            //
            // Stream1
            //
            this.Stream1.Location = new System.Drawing.Point(32, 48);
            this.Stream1.Name = "Stream1";
            this.Stream1.Size = new System.Drawing.Size(56, 20);
            this.Stream1.TabIndex = 1;
            this.Stream1.Text = "1";
            //
            // ICP9
            //
            this.ICP9.Location = new System.Drawing.Point(128, 240);
            this.ICP9.Maximum = new decimal(new int[] {
            850,
            0,
            0,
            0});
            this.ICP9.Minimum = new decimal(new int[] {
            36,
            0,
            0,
            0});
            this.ICP9.Name = "ICP9";
            this.ICP9.Size = new System.Drawing.Size(56, 20);
            this.ICP9.TabIndex = 2;
            this.ICP9.BaseValue = new decimal(new int[] {
            36,
            0,
            0,
            0});
            //
            // Stream6
            //
            this.Stream6.Location = new System.Drawing.Point(32, 168);
            this.Stream6.Name = "Stream6";
            this.Stream6.Size = new System.Drawing.Size(56, 20);
            this.Stream6.TabIndex = 1;
            this.Stream6.Text = "6";
            //
            // textBox1
            //
            this.textBox1.Location = new System.Drawing.Point(213, 264);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(56, 20);
            this.textBox1.TabIndex = 7;
            this.textBox1.Text = "10";
            //
            // textBox2
            //
            this.textBox2.Location = new System.Drawing.Point(213, 240);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(56, 20);
            this.textBox2.TabIndex = 10;
            this.textBox2.Text = "9";
            //
            // textBox3
            //
            this.textBox3.Location = new System.Drawing.Point(213, 216);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(56, 20);
            this.textBox3.TabIndex = 9;
            this.textBox3.Text = "8";
            //
            // textBox4
            //
            this.textBox4.Location = new System.Drawing.Point(213, 192);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(56, 20);
            this.textBox4.TabIndex = 11;
            this.textBox4.Text = "7";
            //
            // textBox5
            //
            this.textBox5.Location = new System.Drawing.Point(213, 168);
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new System.Drawing.Size(56, 20);
            this.textBox5.TabIndex = 13;
            this.textBox5.Text = "6";
            //
            // textBox6
            //
            this.textBox6.Location = new System.Drawing.Point(213, 144);
            this.textBox6.Name = "textBox6";
            this.textBox6.Size = new System.Drawing.Size(56, 20);
            this.textBox6.TabIndex = 12;
            this.textBox6.Text = "5";
            //
            // textBox7
            //
            this.textBox7.Location = new System.Drawing.Point(213, 120);
            this.textBox7.Name = "textBox7";
            this.textBox7.Size = new System.Drawing.Size(56, 20);
            this.textBox7.TabIndex = 5;
            this.textBox7.Text = "4";
            //
            // textBox8
            //
            this.textBox8.Location = new System.Drawing.Point(213, 96);
            this.textBox8.Name = "textBox8";
            this.textBox8.Size = new System.Drawing.Size(56, 20);
            this.textBox8.TabIndex = 4;
            this.textBox8.Text = "3";
            //
            // textBox9
            //
            this.textBox9.Location = new System.Drawing.Point(213, 72);
            this.textBox9.Name = "textBox9";
            this.textBox9.Size = new System.Drawing.Size(56, 20);
            this.textBox9.TabIndex = 6;
            this.textBox9.Text = "2";
            //
            // textBox10
            //
            this.textBox10.Location = new System.Drawing.Point(213, 48);
            this.textBox10.Name = "textBox10";
            this.textBox10.Size = new System.Drawing.Size(56, 20);
            this.textBox10.TabIndex = 8;
            this.textBox10.Text = "1";
            //
            // label3
            //
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(213, 24);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(64, 23);
            this.label3.TabIndex = 3;
            this.label3.Text = "Flow";
            //
            // textBox11
            //
            this.textBox11.Location = new System.Drawing.Point(290, 264);
            this.textBox11.Name = "textBox11";
            this.textBox11.Size = new System.Drawing.Size(56, 20);
            this.textBox11.TabIndex = 18;
            //
            // textBox12
            //
            this.textBox12.Location = new System.Drawing.Point(290, 240);
            this.textBox12.Name = "textBox12";
            this.textBox12.Size = new System.Drawing.Size(56, 20);
            this.textBox12.TabIndex = 21;
            //
            // textBox13
            //
            this.textBox13.Location = new System.Drawing.Point(290, 216);
            this.textBox13.Name = "textBox13";
            this.textBox13.Size = new System.Drawing.Size(56, 20);
            this.textBox13.TabIndex = 20;
            //
            // textBox14
            //
            this.textBox14.Location = new System.Drawing.Point(290, 192);
            this.textBox14.Name = "textBox14";
            this.textBox14.Size = new System.Drawing.Size(56, 20);
            this.textBox14.TabIndex = 22;
            //
            // textBox15
            //
            this.textBox15.Location = new System.Drawing.Point(290, 168);
            this.textBox15.Name = "textBox15";
            this.textBox15.Size = new System.Drawing.Size(56, 20);
            this.textBox15.TabIndex = 24;
            //
            // textBox16
            //
            this.textBox16.Location = new System.Drawing.Point(290, 144);
            this.textBox16.Name = "textBox16";
            this.textBox16.Size = new System.Drawing.Size(56, 20);
            this.textBox16.TabIndex = 23;
            //
            // textBox17
            //
            this.textBox17.Location = new System.Drawing.Point(290, 120);
            this.textBox17.Name = "textBox17";
            this.textBox17.Size = new System.Drawing.Size(56, 20);
            this.textBox17.TabIndex = 16;
            //
            // textBox18
            //
            this.textBox18.Location = new System.Drawing.Point(290, 96);
            this.textBox18.Name = "textBox18";
            this.textBox18.Size = new System.Drawing.Size(56, 20);
            this.textBox18.TabIndex = 15;
            //
            // textBox19
            //
            this.textBox19.Location = new System.Drawing.Point(290, 72);
            this.textBox19.Name = "textBox19";
            this.textBox19.Size = new System.Drawing.Size(56, 20);
            this.textBox19.TabIndex = 17;
            //
            // textBox20
            //
            this.textBox20.Location = new System.Drawing.Point(290, 48);
            this.textBox20.Name = "textBox20";
            this.textBox20.Size = new System.Drawing.Size(56, 20);
            this.textBox20.TabIndex = 19;
            //
            // label4
            //
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(290, 24);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(64, 23);
            this.label4.TabIndex = 14;
            this.label4.Text = "Mass %";
            //
            // DoChart
            //
            this.DoChart.Location = new System.Drawing.Point(553, 13);
            this.DoChart.Name = "DoChart";
            this.DoChart.Size = new System.Drawing.Size(83, 34);
            this.DoChart.TabIndex = 25;
            this.DoChart.Text = "Chart";
            this.DoChart.UseVisualStyleBackColor = true;
            this.DoChart.Click += new System.EventHandler(this.DoChart_Click);
            //
            // checkBoxVI
            //
            this.checkBoxVI.AutoSize = true;
            this.checkBoxVI.Location = new System.Drawing.Point(388, 13);
            this.checkBoxVI.Name = "checkBoxVI";
            this.checkBoxVI.Size = new System.Drawing.Size(149, 17);
            this.checkBoxVI.TabIndex = 26;
            this.checkBoxVI.Text = "ApplyVolumeint erchanges";
            this.checkBoxVI.UseVisualStyleBackColor = true;
            //
            // scolDLG
            //
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(670, 323);
            this.Controls.Add(this.checkBoxVI);
            this.Controls.Add(this.DoChart);
            this.Controls.Add(this.textBox11);
            this.Controls.Add(this.textBox12);
            this.Controls.Add(this.textBox13);
            this.Controls.Add(this.textBox14);
            this.Controls.Add(this.textBox15);
            this.Controls.Add(this.textBox16);
            this.Controls.Add(this.textBox17);
            this.Controls.Add(this.textBox18);
            this.Controls.Add(this.textBox19);
            this.Controls.Add(this.textBox20);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.textBox4);
            this.Controls.Add(this.textBox5);
            this.Controls.Add(this.textBox6);
            this.Controls.Add(this.textBox7);
            this.Controls.Add(this.textBox8);
            this.Controls.Add(this.textBox9);
            this.Controls.Add(this.textBox10);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.ICP10);
            this.Controls.Add(this.ICP9);
            this.Controls.Add(this.ICP8);
            this.Controls.Add(this.ICP7);
            this.Controls.Add(this.ICP6);
            this.Controls.Add(this.ICP5);
            this.Controls.Add(this.ICP4);
            this.Controls.Add(this.ICP3);
            this.Controls.Add(this.ICP2);
            this.Controls.Add(this.ICP1);
            this.Controls.Add(this.Stream10);
            this.Controls.Add(this.Stream9);
            this.Controls.Add(this.Stream8);
            this.Controls.Add(this.Stream7);
            this.Controls.Add(this.Stream6);
            this.Controls.Add(this.Stream5);
            this.Controls.Add(this.Stream4);
            this.Controls.Add(this.Stream3);
            this.Controls.Add(this.Stream2);
            this.Controls.Add(this.Stream1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "scolDLG";
            this.Text = "Set Simple Column ICP or Flows";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.FormClosing += new FormClosingEventHandler(this.scolDLG_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.ICP10)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ICP2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ICP3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ICP4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ICP5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ICP6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ICP8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ICP1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ICP7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ICP9)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion Windows Form Designer generated code

        private void Form1_Load(object sender, System.EventArgs e)
        {
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void scolDLG_FormClosing(object sender, FormClosingEventArgs e)
        {
            osc.ICP[0] = (double)ICP1.BaseValue;
            osc.ICP[1] = (double)ICP2.BaseValue;
            osc.ICP[2] = (double)ICP3.BaseValue;
            osc.ICP[3] = (double)ICP4.BaseValue;
            osc.ICP[4] = (double)ICP5.BaseValue;
            osc.ICP[5] = (double)ICP6.BaseValue;
            osc.ICP[6] = (double)ICP7.BaseValue;
            osc.ICP[7] = (double)ICP8.BaseValue;
            osc.ICP[8] = (double)ICP9.BaseValue;
            osc.ICP[9] = (double)ICP10.BaseValue;
        }

        private double[] offsetcurve(Oil Crude, Oil SideCut)
        {
            double[] LV = new double[Crude.tbp.Length];
            for (int n = 0; n < 11; n++)
            {
                LV[n] = (Crude.GetLVFromShortTBP(SideCut.BRANGEL) * Crude.VF()
                    + SideCut.lv[n] * SideCut.VF()) / Crude.VF();
            }
            return LV;
        }

        private void DoChart_Click(object sender, EventArgs e)
        {
            QuickAssay f = new QuickAssay();
            double[] OldLV = new double[11];
            try
            {
                if (osc.Feed.oil != null)
                    f.AddCurve("Feed", osc.Feed.oil.CulLVpct, Global.BRangeU, Color.Black);
                f.AddCurve("P1", offsetcurve(osc.Feed.oil, osc.oils[0]), osc.oils[0].tbp, Color.Blue);
                f.AddCurve("P2", offsetcurve(osc.Feed.oil, osc.oils[1]), osc.oils[1].tbp, Color.Red);
                f.AddCurve("P3", offsetcurve(osc.Feed.oil, osc.oils[2]), osc.oils[2].tbp, Color.Green);
                f.AddCurve("P4", offsetcurve(osc.Feed.oil, osc.oils[3]), osc.oils[3].tbp, Color.Yellow);
                f.AddCurve("P5", offsetcurve(osc.Feed.oil, osc.oils[4]), osc.oils[4].tbp, Color.Purple);
                f.AddCurve("P6", offsetcurve(osc.Feed.oil, osc.oils[5]), osc.oils[5].tbp, Color.Orange);
                f.AddCurve("P7", offsetcurve(osc.Feed.oil, osc.oils[6]), osc.oils[6].tbp, Color.Red);
                f.AddCurve("P8", offsetcurve(osc.Feed.oil, osc.oils[7]), osc.oils[7].tbp, Color.Green);
                f.AddCurve("P9", offsetcurve(osc.Feed.oil, osc.oils[8]), osc.oils[8].tbp, Color.Yellow);
                f.AddCurve("P10", offsetcurve(osc.Feed.oil, osc.oils[9]), osc.oils[9].tbp, Color.Blue);
                f.Show();
            }
            catch
            {
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            osc.Feed.oil.CreateShortTBPCurveFromLVpct();
            osc.Feed.oil.EraseLVs();
            osc.Feed.oil.CreatLVpctsFromLVandTBPCurves(osc.Feed.oil.lv, osc.Feed.oil.tbp);
            osc.Feed.oil.CreateShortTBPCurveFromLVpct();
        }
    }
}