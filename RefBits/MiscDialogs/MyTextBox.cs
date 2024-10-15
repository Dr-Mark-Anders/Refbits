using System.ComponentModel;
using System.Windows.Forms;

namespace Units
{
    public class MyTextBox : TextBox, INotifyPropertyChanged
    {
        private string text;

        public new string Name
        {
            get
            {
                return text;
            }
            set
            {
                if (text != value)
                {
                    text = value;
                    // try to remove this line
                    NotifyPropertyChanged("MyProperty");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}