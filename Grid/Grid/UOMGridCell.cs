using ModelEngine;
using ModelEngine;
using System.Runtime.Serialization;
using Units;

namespace UOMGrid
{
    // Your class  should look like this:
    [Serializable]
    public class UOMGridCell : DataGridViewTextBoxCell, ISerializable
    {
        private Guid guid = Guid.NewGuid();
        private string? formula = null;
        private Guid UOMGuid = Guid.Empty;
        private int row;
        private int column;
        public StreamProperty? uom;
        private string displayText = "";
        public int CalcOrder = 0;
        private readonly object? SavedValue;

        public override object Clone()
        {
            UOMGridCell newCell = new();
            newCell.formula = formula;
            newCell.column = column;
            newCell.row = row;
            newCell.UOMGuid = UOMGuid;
            newCell.Value = SavedValue;
            return newCell;
        }

        public ePropID Propid
        {
            get
            {
                if (uom != null)
                    return uom.Propid;
                else
                    return ePropID.NullUnits;
            }
        }

        public void Update(StreamProperty? UOM = null, bool isempty = false, bool valueonly = false)
        {
            if (UOM != null)
                if (valueonly && uom != null)
                    this.uom.BaseValue = uom.BaseValue;
                else
                {
                    this.uom = UOM;
                    UOMGuid = uom.guid;
                }
            if (isempty)
                base.Value = "";
            else if (uom != null)
                base.Value = uom.DisplayValueOut().ToString("F4");
        }

        public UOMGridCell(SerializationInfo info, StreamingContext context)
        {
            try
            {
                formula = info.GetString("formula");
                guid = (Guid)info.GetValue("guid", typeof(Guid));
                UOMGuid = (Guid)info.GetValue("UOMGuid", typeof(Guid));
                row = info.GetInt32("Row");
                column = info.GetInt32("Col");
                SavedValue = info.GetValue("Value", typeof(object));
            }
            catch
            {
            }
        }

        public UOMGridCell(StreamProperty q, bool isempty = false)
        {
            string units = GlobalModel.displayunits.UnitsDict[q.Propid];
            q.DisplayUnit = units;
            UOMGuid = q.guid;
            uom = q;
            if (isempty)
                displayText = "";
            else
                displayText = uom.DisplayValueOut().ToString("F4");
            base.Value = displayText;
        }

        public UOMGridCell()
        {
        }

        public UOMGridCell(CellData cell)
        {
            this.formula = cell.Formula;
            this.Value = cell.Value;
            this.row = cell.RowIndex;
            this.column = cell.ColumnIndex;
            this.UOMGuid = cell.UOMGuid;
            this.guid = cell.Guid;
        }

        public new object Value
        {
            get
            {
                if (uom != null)
                    return uom.Value;
                else
                    return base.Value;
            }
            set
            {
                switch (value)
                {
                    case string str:
                        if (double.TryParse(str, out double dd))
                            base.Value = dd;
                        else
                            base.Value = str;
                        break;

                    case double d:
                        base.Value = d;
                        break;

                    case float f:
                        base.Value = f;
                        break;

                    case UOMProperty p:
                        base.Value = p.DisplayValueOut();
                        break;
                }
            }
        }

        public Guid Guid { get => guid; set => guid = value; }
        public string DisplayText { get => displayText; set => displayText = value; }
        public string? Formula { get => formula; set => formula = value; }

        public int SavedRow => row;

        public int SavedColumn => column;

        public void Erase()
        {
            if (uom != null)
                uom.Clear();
            base.Value = double.NaN;
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("fourmula", formula);
            info.AddValue("UOMGuid", UOMGuid);
            info.AddValue("Guid", Guid);
            info.AddValue("Row", RowIndex);
            info.AddValue("Col", ColumnIndex);
            info.AddValue("Value", Value);
        }
    }
}