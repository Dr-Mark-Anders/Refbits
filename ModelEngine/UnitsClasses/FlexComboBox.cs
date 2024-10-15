using System;
using System.Collections;
using System.Windows.Forms;

namespace Units
{
    /// <summary>
    /// Summary description for CustomControl1.
    /// </summary>
    public class FlexComboBox : ComboBox
    {
        public FlexComboBox()
        {
            this.MouseLeave += new EventHandler(this.CheckEntry);
        }

        public FlexComboBox(ArrayList Data)
        {
            foreach (object s in Data)
            {
                base.Items.Add(s);
            }
        }

        public new object DataSource
        {
            get
            {
                return base.DataSource;
            }
            set
            {
                string ss = base.Text;
                try
                {
                    foreach (string s in (IList)value)
                    {
                        if (s != null)
                            base.Items.Add(s);
                    }
                }
                catch
                {
                }
                base.Text = ss;
            }
        }

        public new string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                if (value != null && value != "")
                {
                    if (!Items.Contains(value))
                        Items.Add(value);
                    base.Text = value;
                }
            }
        }

        private void CheckEntry(object sender, EventArgs e)
        {
            if (this.Text.Length > 20)
            {
                MessageBox.Show("The entered text is longer than 20 Characters and will be truncated", "Text Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Text = this.Text.Substring(0, 19);
            }
        }
    }
}