using System.Windows.Forms;

namespace Units
{
    /// <summary>
    /// Summary description for CustomControl1.
    /// </summary>
    public class FlexTextBox : TextBox
    {
        private void InitializeComponent()
        {
        }

        public FlexTextBox()
        {
        }

        public new string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                if (value != null && value.Trim() != "")
                    base.Text = value;
            }
        }

        public string TextIfBlank
        {
            get
            {
                return base.Text;
            }
            set
            {
                if (value != null && value.Trim() != "")
                    base.Text = value;
            }
        }

        // public  Port_Signal UOM { get; set; }
    }
}