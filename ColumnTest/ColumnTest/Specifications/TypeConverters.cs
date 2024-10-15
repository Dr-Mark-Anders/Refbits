using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace UnitsTest
{
    /// <summary>
    /// working text box selection for proeoprty grid
    /// </summary>
    public class SelEditorTest : UITypeEditor
    {
        // this is a container for strings, which can be
        // picked-out
        private ListBox Box1 = new ListBox();

        private IWindowsFormsEditorService edSvc;

        // this is a string array for drop-down list
        public static string[] strList = new string[0];

        public SelEditorTest()
        {
            Box1.BorderStyle = BorderStyle.None;
            // add event handler for drop-down box when item
            // will be selected
            Box1.Click += new EventHandler(Box1_Click);
        }

        public override UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }

        // Displays the UI for value selection.
        public override object EditValue
           (System.ComponentModel.ITypeDescriptorContext
           context, System.IServiceProvider provider,
           object value)
        {
            Box1.Items.Clear();
            Box1.Items.AddRange(strList);
            Box1.Height = Box1.PreferredHeight;
            // Uses the IWindowsFormsEditorService to
            // display a drop-down UI in the Properties
            // window.
            edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));

            if (edSvc != null)
            {
                edSvc.DropDownControl(Box1);
                return Box1.SelectedItem;
            }
            return value;
        }

        private void Box1_Click(object sender, EventArgs e)
        {
            edSvc.CloseDropDown();
        }
    }

    public class StringListConverter : TypeConverter
    {
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true; // display drop
        }

        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return true; // drop-down vs combo
        }

        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            // note you can also look at context etc to build list
            return new StandardValuesCollection(new string[] { "abc", "def", "ghi" });
        }
    }

    public class CustomCollectionEditor : CollectionEditor  //design time only
    {
        public CustomCollectionEditor(Type type)
            : base(type)
        {
        }

        protected override Type CreateCollectionItemType()
        {
            return typeof(string);
        }

        protected override object CreateInstance(Type itemType)
        {
            return "Default Value";
        }
    }

    public class Bar
    {
        public String barvalue;

        public override String ToString()
        {
            return barvalue;
        }
    }

    internal class BarConverter : TypeConverter
    {
        public static List<Bar> barlist = null;

        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            return new StandardValuesCollection(barlist);
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
            {
                return true;
            }
            return base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            if (value is string)
            {
                foreach (Bar b in barlist)
                {
                    if (b.barvalue == (string)value)
                    {
                        return b;
                    }
                }
            }
            return base.ConvertFrom(context, culture, value);
        }
    }

    internal class StringConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
            {
                return true;
            }
            return base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            if (value is string)
            {
                return value;
            }
            return base.ConvertFrom(context, culture, value);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return base.CanConvertTo(context, destinationType);
        }

        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }

    public class ListTypeConverter : TypeConverter
    {
        public ListTypeConverter()
        {
            m_list.Add("Demo.TestClass.Created");
            m_list.Add("Demo.TestClass.Name");
            m_list.Add("Demo.TestClass.Description");
        }

        private List<string> m_list = new List<string>();

        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return true;
        }

        private StandardValuesCollection GetValues()
        {
            return new StandardValuesCollection(m_list);
        }

        protected void SetList(List<string> list)
        {
            m_list = list;
        }

        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            return GetValues();
        }
    }
}