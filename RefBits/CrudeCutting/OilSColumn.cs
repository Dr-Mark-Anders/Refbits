using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization;
using Dialogs;
using System.Drawing.Drawing2D;

namespace Models
{
    /// <summary>
    /// Summary description for UserControl1.
    /// </summary>
    //public  class  SColumn : UserControl

    [Serializable]
    //public  class  SColumn : UserControl
    public class OilSColumn : BaseModelControl, ISerializable
    {
        public new void GetObjectData(SerializationInfo si, StreamingContext ctx)
        {
            si.AddValue("HasConnection", HasConnection);
            si.AddValue("ConnectNo", ConnectNo);
            si.AddValue("X", Left);
            si.AddValue("Y", Top);
            si.AddValue("W", Width);
            si.AddValue("H", Height);
            si.AddValue("Type", Type);
            si.AddValue("Name", Name);
        }

        private OilSColumn(SerializationInfo si, StreamingContext ctx)
        {
            InitializeComponent();
            Type = Model.SCol;
            flanges.Add(Feed);
            flanges.Add(Product1);
            flanges.Add(Product2);
            flanges.Add(Product3);
            flanges.Add(Product4);
            flanges.Add(Product5);
            flanges.Add(Product6);
            flanges.Add(Product7);
            flanges.Add(Product8);
            flanges.Add(Product9);
            flanges.Add(Product10);
            flanges.Transparent();
            HasConnection = si.GetBoolean("HasConnection");
            ConnectNo = si.GetInt32("ConnectNo");
            Left = si.GetInt32("X");
            Top = si.GetInt32("Y");
            Width = si.GetInt32("W");
            Height = si.GetInt32("H");
            Type = (Model)si.GetValue("Type", typeof(Model));
            Name = si.GetString("Name");
        }

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container Components = null;
        public double[] ICP = { -273.15, 36, 80, 170, 240, 300, 350, 370, 450, 580 };
        public Oil[] oils;
        public Flange Feed;
        public Flange Product1;
        public Flange Product2;
        public Flange Product3;
        public Flange Product4;
        public Flange Product5;
        public Flange Product6;
        public Flange Product7;
        public Flange Product8;
        public Flange Product9;
        public Flange Product10;

        public ScolData Data = new ScolData();

        public OilSColumn()
        {
            InitializeComponent();
            Type = Model.OilScol;
        }

        protected override void OnPaint(Paint EventArgs e)
        {
            Graphics g = e.Graphics;
            try
            {
                Color col1, col2;
                col1 = Color.White;
                col2 = Color.Gray;
                Color FrameColor = Color.DarkGray;
                Color LineColor = Color.Black;

                if (FlowSheet != null)
                {
                    col1 = FlowSheet.Options.UnitColor1;
                    col2 = FlowSheet.Options.UnitColor2;
                    BackColor = FlowSheet.Options.BackGroundColor;
                    FrameColor = Color.Black;
                    LineColor = FlowSheet.Options.PipeColor;
                }

                float voff = Convert.ToInt32(0.2 * this.Height);
                float lhoff = Convert.ToInt32(0.32 * this.Width);
                float rhoff = Convert.ToInt32(0.43 * this.Width);
                float r = this.Width - rhoff;
                float b = this.Height - voff;
                float t = voff;
                float l = lhoff;
                float w = r - l;
                float c = (r + l) / 2;
                float ArcHeight = Convert.ToInt32(w / 3);
                float h = (b - ArcHeight) - (t + ArcHeight);
                float Trays = 10;

                Pen p = new Pen(FrameColor, 1);
                LinearGradientBrush BodyBrush = new System.Drawing.Drawing2D.LinearGradientBrush(new PointF(l - 1, 0), new PointF(r, 0), col2, col1);
                BodyBrush.SetBlendTriangularShape(1f, 1f);
                g.SmoothingMode = SmoothingMode.Default;

                GraphicsPath Body = new GraphicsPath();
                RectangleF ColumnBody = new RectangleF(l, t + ArcHeight, w, h);
                Body.AddRectangle(ColumnBody);
                g.DrawPath(p, Body);
                g.FillPath(BodyBrush, Body);

                GraphicsPath Top = new GraphicsPath();
                Top.AddArc(l, t, w, ArcHeight * 2, 180, 180);
                PathGradientBrush TopBrush = new System.Drawing.Drawing2D.PathGradientBrush(Top);
                TopBrush.CenterPoint = new PointF(l, t + ArcHeight);
                TopBrush.CenterColor = col2;
                TopBrush.SurroundColors = new Color[] { col1 };
                g.DrawPath(p, Top);
                g.FillPath(TopBrush, Top);

                GraphicsPath Bottom = new GraphicsPath();
                Bottom.AddArc(l, b - ArcHeight * 2, w, ArcHeight * 2, 0, 180);
                Bottom.CloseAllFigures();
                PathGradientBrush BotBrush = new System.Drawing.Drawing2D.PathGradientBrush(Bottom);
                //BotBrush.SetSigmaBellShape(1f,1f);
                BotBrush.CenterPoint = new PointF(l, b - ArcHeight);
                BotBrush.CenterColor = col2;
                BotBrush.SurroundColors = new Color[] { col1 };
                g.DrawPath(p, Bottom);
                g.FillPath(BotBrush, Bottom);

                GraphicsPath Drum = new GraphicsPath();
                RectangleF D = new RectangleF(r + w / 1.5f, t - h / 4f, w / 3f, h / 4.5f);
                Drum.AddRectangle(D);
                g.DrawPath(p, Drum);
                g.FillPath(BodyBrush, Drum);

                for (int n = 0; n <= Trays; n++)
                {
                    int pos = Convert.ToInt32(t + ArcHeight + h / Trays * n);
                    g.DrawLine(p, l, pos, r, pos);
                }
                p.Width = 2;
                p.Color = LineColor;

                Feed.Top = Convert.ToInt32(t + h * 4 / 5);
                Feed.Left = 0;
                Product1.Top = Convert.ToInt32(t - h / 3) - 3;
                Product2.Top = Convert.ToInt32(t - h / 3f + h / 4.5f + h / 4f - 3);
                Product10.Top = Convert.ToInt32(b + h / 7) - 3;

                int Space = (Feed.Top - Product2.Bottom) / 8;
                Product3.Top = Product2.Bottom + Space;
                Product4.Top = Product3.Top + Space;
                Product5.Top = Product4.Top + Space;
                Product6.Top = Product5.Top + Space;
                Product7.Top = Product6.Top + Space;
                Product8.Top = Product7.Top + Space;
                Product9.Top = Product8.Top + Space;

                Product1.Left = Width - 8;
                Product2.Left = Width - 8;
                Product3.Left = Width - 8;
                Product4.Left = Width - 8;
                Product5.Left = Width - 8;
                Product6.Left = Width - 8;
                Product7.Left = Width - 8;
                Product8.Left = Width - 8;
                Product9.Left = Width - 8;
                Product10.Left = Width - 8;

                //p.EndCap=System.Drawing.Drawing2D.LineCap.ArrowAnchor;
                g.DrawLine(p, 0, Feed.CenterY, l - 1, Feed.CenterY);   // Feed

                //p.EndCap=System.Drawing.Drawing2D.LineCap.NoAnchor;
                g.DrawLine(p, c, t, c, t - h / 7);   // Top
                g.DrawLine(p, c, t - h / 7, r + w / 1.5f - 1, t - h / 7);   // Top

                g.DrawLine(p, (2 * (r + w / 1.5f) + w / 3f) / 2f, t - h / 4f, (2 * (r + w / 1.5f) + w / 3f) / 2f, t - h / 3f);   // Drum Top
                g.DrawLine(p, (2 * (r + w / 1.5f) + w / 3f) / 2f, t - h / 4f + h / 4.5f, (2 * (r + w / 1.5f) + w / 3f) / 2f, t - h / 3f + h / 4.5f + h / 4f);   // Drum Bottom
                //p.EndCap=System.Drawing.Drawing2D.LineCap.ArrowAnchor;
                g.DrawLine(p, (2 * (r + w / 1.5f) + w / 3f) / 2f, Product1.CenterY, this.Width, Product1.CenterY);
                g.DrawLine(p, (2 * (r + w / 1.5f) + w / 3f) / 2f, Product2.CenterY, this.Width, Product2.CenterY);
                //p.EndCap=System.Drawing.Drawing2D.LineCap.NoAnchor;
                g.DrawLine(p, c, b, c, b + h / 7);
                //p.EndCap=System.Drawing.Drawing2D.LineCap.ArrowAnchor;
                g.DrawLine(p, c, Product10.CenterY, this.Width, Product10.CenterY);

                if (Product3.IsConnected)
                {
                    g.DrawLine(p, c + w / 2, Product3.CenterY, this.Width, Product3.CenterY);
                }
                if (Product4.IsConnected)
                {
                    g.DrawLine(p, c + w / 2, Product4.CenterY, this.Width, Product4.CenterY);
                }
                if (Product5.IsConnected)
                {
                    g.DrawLine(p, c + w / 2, Product5.CenterY, this.Width, Product5.CenterY);
                }
                if (Product6.IsConnected)
                {
                    g.DrawLine(p, c + w / 2, Product6.CenterY, this.Width, Product6.CenterY);
                }
                if (Product7.IsConnected)
                {
                    g.DrawLine(p, c + w / 2, Product7.CenterY, this.Width, Product7.CenterY);
                }
                if (Product8.IsConnected)
                {
                    g.DrawLine(p, c + w / 2, Product8.CenterY, this.Width, Product8.CenterY);
                }
                if (Product9.IsConnected)
                {
                    g.DrawLine(p, c + w / 2, Product9.CenterY, this.Width, Product9.CenterY);
                }
                //if(g!=null)
                //	g.Dispose();
            }
            catch
            {
            }
            base.OnPaint(e);
        }

        public override bool Calculate()
        {
            CrudeCutter sc = new CrudeCutter();
            oils = sc.CutStreams(Feed.oil, ICP);
            flanges[1].oil = (Oil)oils[0];
            flanges[2].oil = (Oil)oils[1];
            flanges[3].oil = (Oil)oils[2];
            flanges[4].oil = (Oil)oils[3];
            flanges[5].oil = (Oil)oils[4];
            flanges[6].oil = (Oil)oils[5];
            flanges[7].oil = (Oil)oils[6];
            flanges[8].oil = (Oil)oils[7];
            flanges[9].oil = (Oil)oils[8];
            flanges[10].oil = (Oil)oils[9];
            return true;
        }
        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (Components != null)
                    Components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Feed = new Units.Flange();
            this.Product7 = new Units.Flange();
            this.Product8 = new Units.Flange();
            this.Product9 = new Units.Flange();
            this.Product10 = new Units.Flange();
            this.Product1 = new Units.Flange();
            this.Product2 = new Units.Flange();
            this.Product3 = new Units.Flange();
            this.Product4 = new Units.Flange();
            this.Product5 = new Units.Flange();
            this.Product6 = new Units.Flange();
            this.SuspendLayout();
            //
            // Feed
            //
            this.Feed.AllowDrop = true;
            this.Feed.BackColor = System.Drawing.Color.DarkRed;
            this.Feed.CenterX = 11;
            this.Feed.CenterY = 83;
            this.Feed.IsConnected = false;
            this.Feed.IsData = false;
            this.Feed.IsInput = true;
            this.Feed.IsPlantData = false;
            this.Feed.IsSteam = false;
            this.Feed.IsStream = true;
            this.Feed.IsSyn = false;
            this.Feed.IsVert = false;
            this.Feed.Location = new System.Drawing.Point(7, 79);
            this.Feed.Name = "Feed";
            this.Feed.oil = null;
            this.Feed.ProportionalPosition = 0F;
            this.Feed.Size = new System.Drawing.Size(8, 8);
            this.Feed.TabIndex = 0;
            this.Feed.Visible = false;
            this.Feed.Click += new System.EventHandler(this.Feed_Click);
            //
            // Product7
            //
            this.Product7.AllowDrop = true;
            this.Product7.BackColor = System.Drawing.Color.DarkRed;
            this.Product7.CenterX = 52;
            this.Product7.CenterY = 105;
            this.Product7.IsConnected = false;
            this.Product7.IsData = false;
            this.Product7.IsInput = false;
            this.Product7.IsPlantData = false;
            this.Product7.IsSteam = false;
            this.Product7.IsStream = true;
            this.Product7.IsSyn = false;
            this.Product7.IsVert = false;
            this.Product7.Location = new System.Drawing.Point(48, 101);
            this.Product7.Name = "Product7";
            this.Product7.oil = null;
            this.Product7.ProportionalPosition = 0F;
            this.Product7.Size = new System.Drawing.Size(8, 8);
            this.Product7.TabIndex = 1;
            this.Product7.Visible = false;
            this.Product7.Click += new System.EventHandler(this.Product7_Click);
            //
            // Product8
            //
            this.Product8.AllowDrop = true;
            this.Product8.BackColor = System.Drawing.Color.DarkRed;
            this.Product8.CenterX = 52;
            this.Product8.CenterY = 116;
            this.Product8.IsConnected = false;
            this.Product8.IsData = false;
            this.Product8.IsInput = false;
            this.Product8.IsPlantData = false;
            this.Product8.IsSteam = false;
            this.Product8.IsStream = true;
            this.Product8.IsSyn = false;
            this.Product8.IsVert = false;
            this.Product8.Location = new System.Drawing.Point(48, 112);
            this.Product8.Name = "Product8";
            this.Product8.oil = null;
            this.Product8.ProportionalPosition = 0F;
            this.Product8.Size = new System.Drawing.Size(8, 8);
            this.Product8.TabIndex = 2;
            this.Product8.Visible = false;
            this.Product8.Click += new System.EventHandler(this.Product8_Click);
            //
            // Product9
            //
            this.Product9.AllowDrop = true;
            this.Product9.BackColor = System.Drawing.Color.DarkRed;
            this.Product9.CenterX = 52;
            this.Product9.CenterY = 127;
            this.Product9.IsConnected = false;
            this.Product9.IsData = false;
            this.Product9.IsInput = false;
            this.Product9.IsPlantData = false;
            this.Product9.IsSteam = false;
            this.Product9.IsStream = true;
            this.Product9.IsSyn = false;
            this.Product9.IsVert = false;
            this.Product9.Location = new System.Drawing.Point(48, 123);
            this.Product9.Name = "Product9";
            this.Product9.oil = null;
            this.Product9.ProportionalPosition = 0F;
            this.Product9.Size = new System.Drawing.Size(8, 8);
            this.Product9.TabIndex = 3;
            this.Product9.Visible = false;
            this.Product9.Click += new System.EventHandler(this.Product9_Click);
            //
            // Product10
            //
            this.Product10.AllowDrop = true;
            this.Product10.BackColor = System.Drawing.Color.DarkRed;
            this.Product10.CenterX = 52;
            this.Product10.CenterY = 165;
            this.Product10.IsConnected = false;
            this.Product10.IsData = false;
            this.Product10.IsInput = false;
            this.Product10.IsPlantData = false;
            this.Product10.IsSteam = false;
            this.Product10.IsStream = true;
            this.Product10.IsSyn = false;
            this.Product10.IsVert = false;
            this.Product10.Location = new System.Drawing.Point(48, 161);
            this.Product10.Name = "Product10";
            this.Product10.oil = null;
            this.Product10.ProportionalPosition = 0F;
            this.Product10.Size = new System.Drawing.Size(8, 8);
            this.Product10.TabIndex = 4;
            this.Product10.Visible = false;
            this.Product10.Click += new System.EventHandler(this.Product10_Click);
            //
            // Product1
            //
            this.Product1.AllowDrop = true;
            this.Product1.BackColor = System.Drawing.Color.DarkRed;
            this.Product1.CenterX = 52;
            this.Product1.CenterY = 12;
            this.Product1.IsConnected = false;
            this.Product1.IsData = false;
            this.Product1.IsInput = false;
            this.Product1.IsPlantData = false;
            this.Product1.IsSteam = false;
            this.Product1.IsStream = true;
            this.Product1.IsSyn = false;
            this.Product1.IsVert = false;
            this.Product1.Location = new System.Drawing.Point(48, 8);
            this.Product1.Name = "Product1";
            this.Product1.oil = null;
            this.Product1.ProportionalPosition = 0F;
            this.Product1.Size = new System.Drawing.Size(8, 8);
            this.Product1.TabIndex = 5;
            this.Product1.Visible = false;
            this.Product1.Click += new System.EventHandler(this.Product1_Click);
            //
            // Product2
            //
            this.Product2.AllowDrop = true;
            this.Product2.BackColor = System.Drawing.Color.DarkRed;
            this.Product2.CenterX = 52;
            this.Product2.CenterY = 47;
            this.Product2.IsConnected = false;
            this.Product2.IsData = false;
            this.Product2.IsInput = false;
            this.Product2.IsPlantData = false;
            this.Product2.IsSteam = false;
            this.Product2.IsStream = true;
            this.Product2.IsSyn = false;
            this.Product2.IsVert = false;
            this.Product2.Location = new System.Drawing.Point(48, 43);
            this.Product2.Name = "Product2";
            this.Product2.oil = null;
            this.Product2.ProportionalPosition = 0F;
            this.Product2.Size = new System.Drawing.Size(8, 8);
            this.Product2.TabIndex = 6;
            this.Product2.Visible = false;
            this.Product2.Load += new System.EventHandler(this.Product2_Load);
            this.Product2.Click += new System.EventHandler(this.Product2_Click);
            //
            // Product3
            //
            this.Product3.AllowDrop = true;
            this.Product3.BackColor = System.Drawing.Color.DarkRed;
            this.Product3.CenterX = 52;
            this.Product3.CenterY = 59;
            this.Product3.IsConnected = false;
            this.Product3.IsData = false;
            this.Product3.IsInput = false;
            this.Product3.IsPlantData = false;
            this.Product3.IsSteam = false;
            this.Product3.IsStream = true;
            this.Product3.IsSyn = false;
            this.Product3.IsVert = false;
            this.Product3.Location = new System.Drawing.Point(48, 55);
            this.Product3.Name = "Product3";
            this.Product3.oil = null;
            this.Product3.ProportionalPosition = 0F;
            this.Product3.Size = new System.Drawing.Size(8, 8);
            this.Product3.TabIndex = 7;
            this.Product3.Visible = false;
            this.Product3.Click += new System.EventHandler(this.Product3_Click);
            //
            // Product4
            //
            this.Product4.AllowDrop = true;
            this.Product4.BackColor = System.Drawing.Color.DarkRed;
            this.Product4.CenterX = 52;
            this.Product4.CenterY = 71;
            this.Product4.IsConnected = false;
            this.Product4.IsData = false;
            this.Product4.IsInput = false;
            this.Product4.IsPlantData = false;
            this.Product4.IsSteam = false;
            this.Product4.IsStream = true;
            this.Product4.IsSyn = false;
            this.Product4.IsVert = false;
            this.Product4.Location = new System.Drawing.Point(48, 67);
            this.Product4.Name = "Product4";
            this.Product4.oil = null;
            this.Product4.ProportionalPosition = 0F;
            this.Product4.Size = new System.Drawing.Size(8, 8);
            this.Product4.TabIndex = 8;
            this.Product4.Visible = false;
            this.Product4.Click += new System.EventHandler(this.Product4_Click);
            //
            // Product5
            //
            this.Product5.AllowDrop = true;
            this.Product5.BackColor = System.Drawing.Color.DarkRed;
            this.Product5.CenterX = 52;
            this.Product5.CenterY = 83;
            this.Product5.IsConnected = false;
            this.Product5.IsData = false;
            this.Product5.IsInput = false;
            this.Product5.IsPlantData = false;
            this.Product5.IsSteam = false;
            this.Product5.IsStream = true;
            this.Product5.IsSyn = false;
            this.Product5.IsVert = false;
            this.Product5.Location = new System.Drawing.Point(48, 79);
            this.Product5.Name = "Product5";
            this.Product5.oil = null;
            this.Product5.ProportionalPosition = 0F;
            this.Product5.Size = new System.Drawing.Size(8, 8);
            this.Product5.TabIndex = 9;
            this.Product5.Visible = false;
            this.Product5.Click += new System.EventHandler(this.Product5_Click);
            //
            // Product6
            //
            this.Product6.AllowDrop = true;
            this.Product6.BackColor = System.Drawing.Color.DarkRed;
            this.Product6.CenterX = 52;
            this.Product6.CenterY = 94;
            this.Product6.IsConnected = false;
            this.Product6.IsData = false;
            this.Product6.IsInput = false;
            this.Product6.IsPlantData = false;
            this.Product6.IsSteam = false;
            this.Product6.IsStream = true;
            this.Product6.IsSyn = false;
            this.Product6.IsVert = false;
            this.Product6.Location = new System.Drawing.Point(48, 90);
            this.Product6.Name = "Product6";
            this.Product6.oil = null;
            this.Product6.ProportionalPosition = 0F;
            this.Product6.Size = new System.Drawing.Size(8, 8);
            this.Product6.TabIndex = 10;
            this.Product6.Visible = false;
            this.Product6.Click += new System.EventHandler(this.Product6_Click);
            //
            // OilSColumn
            //
            this.BackColor = System.Drawing.Color.Silver;
            this.Controls.Add(this.Feed);
            this.Controls.Add(this.Product7);
            this.Controls.Add(this.Product8);
            this.Controls.Add(this.Product9);
            this.Controls.Add(this.Product10);
            this.Controls.Add(this.Product1);
            this.Controls.Add(this.Product2);
            this.Controls.Add(this.Product3);
            this.Controls.Add(this.Product4);
            this.Controls.Add(this.Product5);
            this.Controls.Add(this.Product6);
            this.Name = "OilSColumn";
            this.Size = new System.Drawing.Size(64, 176);
            this.double Click += new System.EventHandler(this.SColumn_DClick);
            this.Load += new System.EventHandler(this.SColumn_Load);
            this.Controls.SetChildIndex(this.Product6, 0);
            this.Controls.SetChildIndex(this.Product5, 0);
            this.Controls.SetChildIndex(this.Product4, 0);
            this.Controls.SetChildIndex(this.Product3, 0);
            this.Controls.SetChildIndex(this.Product2, 0);
            this.Controls.SetChildIndex(this.Product1, 0);
            this.Controls.SetChildIndex(this.Product10, 0);
            this.Controls.SetChildIndex(this.Product9, 0);
            this.Controls.SetChildIndex(this.Product8, 0);
            this.Controls.SetChildIndex(this.Product7, 0);
            this.Controls.SetChildIndex(this.Feed, 0);
            this.ResumeLayout(false);
        }

        #endregion Component Designer generated code

        private void SColumn_Load(object sender, System.EventArgs e)
        {
            InitializeComponent();
            flanges.Add(Feed);
            flanges.Add(Product1);
            flanges.Add(Product2);
            flanges.Add(Product3);
            flanges.Add(Product4);
            flanges.Add(Product5);
            flanges.Add(Product6);
            flanges.Add(Product7);
            flanges.Add(Product8);
            flanges.Add(Product9);
            flanges.Add(Product10);
        }

        private void SColumn_DClick(object sender, System.EventArgs e)
        {
            Dialogs.scolDLG d = new Dialogs.scolDLG(this);
            d.ShowDialog();
        }

        private void Product1_Click(object sender, System.EventArgs e)
        {
            ConnectEventArgs ev = new ConnectEventArgs();
            Flange B = ((Flange)sender);
            //ev.X=B.Left+B.Width/2;
            //ev.Y=B.Top+B.Height/2;
            //ev.flange=B;
            //OnConnect(ev);
        }

        private void Product2_Click(object sender, System.EventArgs e)
        {
            ConnectEventArgs ev = new ConnectEventArgs();
            Flange B = ((Flange)sender);
            //ev.X=B.Left+B.Width/2;
            //ev.Y=B.Top+B.Height/2;
            //ev.flange=B;
            //OnConnect(ev);
        }

        private void Product3_Click(object sender, System.EventArgs e)
        {
            ConnectEventArgs ev = new ConnectEventArgs();
            Flange B = ((Flange)sender);
            //ev.X=B.Left+B.Width/2;
            //ev.Y=B.Top+B.Height/2;
            //ev.flange=B;
            //OnConnect(ev);
        }

        private void Product4_Click(object sender, System.EventArgs e)
        {
            ConnectEventArgs ev = new ConnectEventArgs();
            Flange B = ((Flange)sender);
            //ev.X=B.Left+B.Width/2;
            //ev.Y=B.Top+B.Height/2;
            //ev.flange=B;
            //OnConnect(ev);
        }

        private void Product5_Click(object sender, System.EventArgs e)
        {
            ConnectEventArgs ev = new ConnectEventArgs();
            Flange B = ((Flange)sender);
            //ev.X=B.Left+B.Width/2;
            //ev.Y=B.Top+B.Height/2;
            //ev.flange=B;
            //OnConnect(ev);
        }

        private void Product6_Click(object sender, System.EventArgs e)
        {
            ConnectEventArgs ev = new ConnectEventArgs();
            Flange B = ((Flange)sender);
            //ev.X=B.Left+B.Width/2;
            //ev.Y=B.Top+B.Height/2;
            //ev.flange=B;
            //OnConnect(ev);
        }

        private void Product7_Click(object sender, System.EventArgs e)
        {
            ConnectEventArgs ev = new ConnectEventArgs();
            Flange B = ((Flange)sender);
            //ev.X=B.Left+B.Width/2;
            //ev.Y=B.Top+B.Height/2;
            //ev.flange=B;
            //OnConnect(ev);
        }

        private void Product8_Click(object sender, System.EventArgs e)
        {
            ConnectEventArgs ev = new ConnectEventArgs();
            Flange B = ((Flange)sender);
            //ev.X=B.Left+B.Width/2;
            //ev.Y=B.Top+B.Height/2;
            //ev.flange=B;
            //OnConnect(ev);
        }

        private void Product9_Click(object sender, System.EventArgs e)
        {
            ConnectEventArgs ev = new ConnectEventArgs();
            Flange B = ((Flange)sender);
            //ev.X=B.Left+B.Width/2;
            //ev.Y=B.Top+B.Height/2;
            //ev.flange=B;
            //OnConnect(ev);
        }

        private void Product10_Click(object sender, System.EventArgs e)
        {
            ConnectEventArgs ev = new ConnectEventArgs();
            Flange B = ((Flange)sender);
            //ev.X=B.Left+B.Width/2;
            //ev.Y=B.Top+B.Height/2;
            //ev.flange=B;
            //OnConnect(ev);
        }

        private void Feed_Click(object sender, System.EventArgs e)
        {
            ConnectEventArgs ev = new ConnectEventArgs();
            Flange B = ((Flange)sender);
            //ev.X=B.Left+B.Width/2;
            //ev.Y=B.Top+B.Height/2;
            //ev.flange=B;
            //ev.IsInput=true;
            //OnConnect(ev);
        }

        private void Product2_Load(object sender, EventArgs e)
        {
        }
    }
    [Serializable]
    public class ScolData
    {
        public string cut1, cut2, cut3, cut4, cut5, cut6, cut7, cut8, cut9, cut10, basis;
    }
}