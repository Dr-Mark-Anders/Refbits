namespace System.Windows.Forms
{
    // Your class  should look like this:
    public class DataGridViewGuidCell : DataGridViewTextBoxCell
    {
        public Guid guid;
        private string displayText = "";

        public string DisplayText
        {
            get
            {
                return displayText;
            }
            set
            {
                displayText = value;
                base.Value = value;
            }
        }

        public DataGridViewGuidCell()
        {
        }

        public DataGridViewGuidCell(Guid guid)
        {
            this.guid = guid;
            base.Value = displayText;
        }
    }
}