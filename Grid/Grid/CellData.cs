using System.Runtime.Serialization;

namespace UOMGrid
{
    [Serializable]
    public class CellData : ISerializable
    {
        private Guid guid = Guid.NewGuid();

        private string? formula = null;
        private Guid uomGuid = Guid.Empty;
        private int rowIndex;
        private int columnIndex;
        private object? value;

        public CellData(UOMGridCell cell)
        {
            guid = cell.Guid;
            formula = cell.Formula;
            uomGuid = cell.Guid;
            rowIndex = cell.RowIndex;
            columnIndex = cell.ColumnIndex;
            value = cell.Value;
        }

        public CellData(SerializationInfo info, StreamingContext context)
        {
            try
            {
                formula = info.GetString("formula");
                uomGuid = (Guid)info.GetValue("UOMGuid", typeof(Guid));
                guid = (Guid)info.GetValue("Guid", typeof(Guid));
                rowIndex = info.GetInt32("Row");
                columnIndex = info.GetInt32("Col");
                value = info.GetValue("Value", typeof(object));
            }
            catch
            {
            }
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("formula", formula);
            info.AddValue("UOMGuid", uomGuid);
            info.AddValue("Guid", guid);
            info.AddValue("Row", rowIndex);
            info.AddValue("Col", columnIndex);
            info.AddValue("Value", value);
        }

        public Guid Guid { get => guid; set => guid = value; }

        public string? Formula { get => formula; set => formula = value; }
        public Guid UOMGuid { get => uomGuid; set => uomGuid = value; }

        public int RowIndex { get => rowIndex; set => rowIndex = value; }
        public int ColumnIndex { get => columnIndex; set => columnIndex = value; }

        public object Value
        { get => value; set => this.value = value; }
    }
}