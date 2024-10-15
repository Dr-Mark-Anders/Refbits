using ModelEngine;
using System;
using System.Windows.Forms;

namespace Units
{
    public partial class FindGuid : Form
    {
        private DrawArea da;

        public FindGuid(DrawArea da)
        {
            InitializeComponent();
            this.da = da;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (Guid.TryParse(textBox1.Text, out Guid res))
            {
                foreach (DrawObject draw in da.GraphicsList)
                {
                    if (draw.Guid == res)
                    {
                        MessageBox.Show(draw.Name);
                        break;
                    }

                    if (draw is DrawName)
                        break;

                    foreach (Port p in draw.Ports)
                    {
                        if (p.Guid == res)
                        {
                            MessageBox.Show(draw.Name + ":" + p.Name);
                            break;
                        }

                        switch (p)
                        {
                            case Port_Material pm:
                                foreach (StreamProperty sp in pm.Props.Values)
                                {
                                    if (sp.Guid == res)
                                    {
                                        MessageBox.Show(draw.Name + ":" + p.Name + ":" + sp.Name);
                                        break;
                                    }
                                }
                                break;

                            case Port_Energy pe:
                                if (pe.Value.Guid == res)
                                {
                                    MessageBox.Show(draw.Name + ":" + p.Name + ":" + pe.Name);
                                }
                                break;

                            case Port_Signal ps:
                                if (ps.Value.Guid == res)
                                {
                                    MessageBox.Show(draw.Name + ":" + p.Name + ":" + ps.Name);
                                }
                                break;
                        }
                    }
                }
            }
        }
    }
}